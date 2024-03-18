/*
MIT License

created by : Stvp Ric

Copyright(c) 2023

Permission is hereby granted, free of charge, to any person obtaining a copy of this
software and associated documentation files (the "Software"), to deal in the Software
without restriction, including without limitation the rights to use, copy, modify,
merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using System;
using System.Linq;
using System.Buffers;
using CompactTween.Extension;
using System.Collections;
using System.Collections.Generic;

namespace CompactTween
{
    /// <summary>PlayerLoop class</summary>
    public static class CPlayerLoop
    {
        /// <summary>MonoB singleton.</summary>
        public static CTmono mono { get; set; }
        public static TimeStruct[] activeCores;
        /// <summary>Struct pool.</summary>
        static ArrayPool<TimeStruct> staticPool = ArrayPool<TimeStruct>.Shared;
        /// <summary>Active tween instances.</summary>
        public static int activecount = 0;
        public static void InitStaticArray()
        {
            activeCores = staticPool.Rent(CTcore.maxPoolLength);
        }
        public static void WorkerUpdate()
        {
            if (activecount == 0)
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

            if (activecount == 0 && CTcore.fcore.Length > CTcore.maxPoolLength)
            {
                CTcore.Resize(true);
            }
        }
        public static void InsertToActiveTweens(int index)
        {
            activecount++;
            activeCores[index] = new TimeStruct(index, Time.frameCount + 1);
        }
        public static void RemoveFromActiveTweens(int index)
        {
            if (index < 0)
            {
                return;
            }

            activeCores[index].set(-1, -1);
            activecount--;
        }

        public static void Resize(bool setdefault)
        {
            if (!setdefault)
            {
                var pool = staticPool.Rent(activeCores.Length * 2);

                for (int i = 0; i < pool.Length; i++)
                {
                    if (i < activeCores.Length)
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

                for (int i = 0; i < activeCores.Length; i++)
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