using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using System;
using System.Linq;
using System.Buffers;
using CTween.Extension;
using System.Collections;
using System.Collections.Generic;

namespace CTween
{
    /// <summary>PlayerLoop class</summary>
    public static class CPlayerLoop
    {
        public static CTmono mono {get;set;}
        public static TimeStruct[] activeCores;
        static ArrayPool<TimeStruct> staticPool = ArrayPool<TimeStruct>.Shared;
        static int activecount = 0;
        static TimeStruct t = new TimeStruct(-1, -1);

        public static void InitStaticArray()
        {
            activeCores = staticPool.Rent(CTcore.maxPoolLength);
        }
        public static void WorkerUpdate()
        {
            if(activecount == 0)
            {
                return;
            }

            for (int i = 0; i < activeCores.Length; i++)
            {
                var t = activeCores[i];

                if (t.isValid)
                {
                    CTcore.fcore[t.index].Run();
                }
            }

            if(activecount == 0 && CTcore.fcore.Length > CTcore.maxPoolLength)
            {
                CTcore.Resize(true);
            }
        }
        public static void InsertToActiveTweens(int index)
        {
            for (int i = 0; i < activeCores.Length; i++)
            {
                if (activeCores[i].index == -1)
                {
                    activecount++;
                    activeCores[i] = new TimeStruct(index, Time.frameCount + 1);
                    break;
                }
            }
        }
        public static void RemoveFromActiveTweens(int index)
        {
            GetActiveTween(index).set(-1, -1);
            activecount--;
        }
        
        public static ref TimeStruct GetActiveTween(int index)
        {
            for (int i = 0; i < activeCores.Length; i++)
            {
                if (activeCores[i].index == index)
                {
                    return ref activeCores[i];
                }
            }

            return ref t;
        }
        public static void Resize(bool setdefault)
        {
            if(!setdefault)
            {
                var pool = staticPool.Rent(activeCores.Length * 2);

                for(int i = 0; i < pool.Length; i++)
                {
                    if(i < activeCores.Length)
                    {
                        pool[i] = activeCores[i];
                    }
                    else
                    {
                        pool[i] = new TimeStruct(-1, -1);
                    }
                }

                staticPool.Return(activeCores);
                activeCores = pool;
            }
            else
            {
                staticPool.Return(activeCores);
                activeCores = staticPool.Rent(CTcore.maxPoolLength);
                
                for(int i = 0; i < activeCores.Length; i++)
                {
                    activeCores[i].set(-1, -1);
                }
            }
        }
    }
    /// <summary>Tween validator struct.</summary>
    [Serializable]
    public struct TimeStruct
    {
        int _index;
        int _frameIn;
        public TimeStruct(int index, int frameIn)
        {
            _index = index;
            _frameIn = frameIn;
        }
        public void set(int index, int frameIn)
        {
            _index = index;
            _frameIn = frameIn;
        }
        public bool isValid => _index > -1 && _frameIn < Time.frameCount;
        public int index => _index;
    }

    //Update dummy class
    public struct CAwaitUpdate { }
    //Update dummy class
    public sealed class CtweenLoop
    {
        static CtweenLoop tweenLoop;

        public CtweenLoop()
        {
            InitUpdate();
        }
        void InitUpdate()
        {
            Application.wantsToQuit += OnQuit;
            AssignPlayerLoop(true);
        }
        void AssignPlayerLoop(bool addElseRemove)
        {
            if (addElseRemove)
            {
                PlayerLoop.SetPlayerLoop(InjectCustomUpdate(PlayerLoop.GetCurrentPlayerLoop(), true));
            }
            else
            {
                PlayerLoop.SetPlayerLoop(InjectCustomUpdate(PlayerLoop.GetCurrentPlayerLoop(), false));
            }
        }
        bool OnQuit()
        {
            AssignPlayerLoop(false);
            tweenLoop = null;
            Application.wantsToQuit -= OnQuit;
            return true;
        }

        PlayerLoopSystem InjectCustomUpdate(PlayerLoopSystem root, bool addCustomUpdateElseClear)
        {
            var lis = root.subSystemList.ToList();
            var index = 0;

            for (int i = lis.Count; i-- > 0;)
            {
                if (lis[i].type == typeof(Update))
                {
                    index = i;
                }
            }

            var tmp = root.subSystemList[index].subSystemList.ToList();

            for (int i = tmp.Count; i-- > 0;)
            {
                if (tmp[i].type == typeof(CAwaitUpdate))
                {
                    tmp.Remove(tmp[i]);
                }
            }

            if (addCustomUpdateElseClear)
            {
                int idx = 0;
                idx = tmp.FindIndex(x => x.type == typeof(Update.ScriptRunBehaviourUpdate));
                var beforeIndex = idx--;
                var afterIndex = idx++;

                if (idx == 0)
                {
                    beforeIndex = 0;
                    afterIndex = 2;
                }

                tmp.Insert(beforeIndex, new PlayerLoopSystem
                {
                    updateDelegate = CPlayerLoop.WorkerUpdate,
                    type = typeof(CAwaitUpdate)
                });
            }

            root.subSystemList[index].subSystemList = tmp.ToArray();
            return root;
        }
        static void InstantiateMono()
        {
            var go = new GameObject();
            go.name = "ctween-hn-nf-sr";
            go.AddComponent<CTmono>();
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            CTcore.Init();
            tweenLoop = new CtweenLoop();
            InstantiateMono();
        }
    }
}