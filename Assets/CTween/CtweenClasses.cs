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

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CompactTween.Extension
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CTcore
    {
        static CTcore dummy = new CTcore { _id = -1, _index = -1 };
        /// <summary>Tween instances.</summary>
        public static CTcore[] fcore { get; private set; }
        /// <summary>UnityObjects.</summary>
        public static CTobject[] ctobjects { get; private set; }
        /// <summary>Common properties.</summary>
        static CTdelta[] ctdeltas;
        /// <summary>Vector objects.</summary>
        static CTvector[] ctvectors;
        /// <summary>CTcore pool.</summary>
        static ArrayPool<CTcore> pool = ArrayPool<CTcore>.Shared;
        /// <summary>Object pool.</summary>
        static ArrayPool<CTobject> poolObjects = ArrayPool<CTobject>.Shared;
        /// <summary>Vector objects.</summary>
        static ArrayPool<CTvector> poolVectors = ArrayPool<CTvector>.Shared;
        /// <summary>Property objects.</summary>
        static ArrayPool<CTdelta> poolDeltas = ArrayPool<CTdelta>.Shared;
        /// <summary>The max limit for the pool. Default is 16 and will be resized automatically as needed.</summary>
        public static int maxPoolLength { get; set; } = 16;
        /// <summary>External transforms.</summary>
        public static List<(Transform transform, int index)> exttransforms = new List<(Transform, int)>(8);
        /// <summary>Gets tween the tween.</summary>
        /// <param name="instanceId">The instanceId or id.</param>
        public static ref CTcore getTween(int instanceId)
        {
            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i]._id != instanceId)
                    continue;

                return ref fcore[i];
            }

            return ref dummy;
        }
        public static ref CTcore getTweenViaIndex(int index)
        {
            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index != index)
                    continue;

                return ref fcore[i];
            }

            return ref dummy;
        }
        /// <summary>Finds the non-main transforms that are related to a tween.</summary>
        /// <param name="index">Array index.</param>
        static Transform FindExternalTransform(int index)
        {
            for (int i = 0; i < exttransforms.Count; i++)
            {
                if (index != exttransforms[i].index)
                    continue;

                return exttransforms[i].transform;
            }

            return null;
        }
        /// <summary>Initialization.</summary>
        public static void Init(int defaultLength = 16)
        {
            if (maxPoolLength < defaultLength)
            {
                maxPoolLength = defaultLength;
            }
            else if (maxPoolLength == 0)
            {
                maxPoolLength = defaultLength;
            }
            else if (maxPoolLength < 16)
            {
                maxPoolLength = 16;
            }

            fcore = pool.Rent(maxPoolLength);
            ctobjects = poolObjects.Rent(maxPoolLength);
            ctdeltas = poolDeltas.Rent(maxPoolLength);
            ctvectors = poolVectors.Rent(maxPoolLength);
            CPlayerLoop.InitStaticArray();

            for (int i = 0; i < CPlayerLoop.activeCores.Length; i++)
            {
                CPlayerLoop.activeCores[i].set(-1, -1);
                fcore[i].setDefault(false);
                ctdeltas[i].setDefault();
            }
        }
        /// <summary>Clears the pool.</summary>
        public static void ClearPools(bool clean = false)
        {
            if (fcore != null)
            {
                if (clean)
                {
                    for (int i = 0; i < ctobjects.Length; i++)
                    {
                        ctobjects[i].Reset();
                    }
                }

                pool.Return(fcore);
                poolObjects.Return(ctobjects);
                poolDeltas.Return(ctdeltas);
                poolVectors.Return(ctvectors);
                CPlayerLoop.DefaultPool();

                fcore = null;
                ctobjects = null;
                ctdeltas = null;
                ctvectors = null;
                CPlayerLoop.ClearPool();
            }
        }
        /// <summary>Tweening status.</summary>
        public static bool IsTweening(int index) => fcore[index]._id > -1;

        /// <summary>Registers on update.</summary>
        /// <param name="index">The index.</param>
        /// <param name="callback">The callback.</param>
        public static void RegisterOnUpdate(int index, Action<float> callback) => RegisterCalls(1, index, null, callback);
        /// <summary>Callback invocation on completion.</summary>
        /// <param name="index">Index.</param>
        /// <param name="callback">Callback.</param>
        public static void RegisterOnComplete(int index, Action callback) => RegisterCalls(2, index, callback);
        /// <summary>Very last callback invocation, whether the tween completed or cancelled.</summary>
        /// <param name="index">Index.</param>
        /// <param name="callback">Callback.</param>
        public static void RegisterLastOnComplete(int index, Action callback) => RegisterCalls(4, index, callback);
        /// <summary>Callback on every loop cycle routine.</summary>
        /// <param name="index"></param>
        /// <param name="callback"></param>
        public static void RegisterLoopCycle(int index, Action callback) => RegisterCalls(12, index, callback);
        /// <summary>Register a callback.</summary>
        static void RegisterCalls(int type, int index, Action callback, Action<float> updateCallback = null)
        {
            fcore[index].selectDelegate();

            //Last invoked.
            if (type == 4)
            {
                fcore[index].assignCallback((x, y) =>
                {
                    if (x == 4)
                    {
                        callback.Invoke();
                    }
                });
            }
            //Invoked every frame.
            else if (type == 1)
            {
                fcore[index].assignCallback((x, y) =>
                {
                    if (x == 1)
                    {
                        updateCallback.Invoke(y);
                    }
                });
            }
            //Invoked on complete.
            else if (type == 2)
            {
                fcore[index].assignCallback((x, y) =>
                {
                    if (x == 2)
                    {
                        callback.Invoke();
                    }
                });
            }
            //Invoked on every loop cycle.
            else if(type == 12)
            {
                fcore[index].assignCallback((x, y) => 
                {
                    if(x == 12)
                    {
                        callback.Invoke();
                    }
                });           
            }
        }

        /// <summary>Resizes the array length.</summary>
        /// <param name="setdefault">Set default state.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Resize()
        {
            var count = fcore.Length;
            var newarr = pool.Rent(count * 2);
            var newobjs = poolObjects.Rent(count * 2);
            var newvectors = poolVectors.Rent(count * 2);
            var newdeltas = poolDeltas.Rent(count * 2);

            var len = newarr.Length;

            for (int i = 0; i < len; i++)
            {
                if (i < count)
                {
                    newarr[i] = fcore[i];
                    newobjs[i].AssignObjects(ctobjects[i].GetObjects.transform, ctobjects[i].GetObjects.invoke);
                    ctobjects[i].Reset();
                    newvectors[i] = ctvectors[i];
                    newdeltas[i] = ctdeltas[i];
                }
                else
                {
                    newarr[i].setDefault(false);
                    newdeltas[i].setDefault();
                }
            }

            pool.Return(fcore);
            poolObjects.Return(ctobjects);
            poolVectors.Return(ctvectors);
            poolDeltas.Return(ctdeltas);

            fcore = newarr;
            ctobjects = newobjs;
            ctvectors = newvectors;
            ctdeltas = newdeltas;
            CPlayerLoop.Resize(false);
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateVector(Transform transform, Vector3 from, Vector3 to, float duration, LerpCoreType mode, bool isLocal, out int index)
        {
            var core = new CTcore();
            core.setInit(transform.gameObject.GetInstanceID(), mode, isLocal);
            index = GetArraySlot(ref core, transform);
            ctdeltas[index]._duration = duration;
            ctvectors[index].set(from, to);

            if(mode == LerpCoreType.Position)
            {
                fcore[index].setInitPosition();
            }
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateFollow(Transform transform, Transform to, float smoothTime, Vector3 velocity, out int index)
        {
            var core = new CTcore();
            core.setInit(transform.gameObject.GetInstanceID(), LerpCoreType.Follow, false);
            index = GetArraySlot(ref core, transform);
            ctdeltas[index]._duration = float.PositiveInfinity;
            exttransforms.Add((to, index));
            ctvectors[index].set(new Vector3(smoothTime, 0f, 0f), velocity);
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateRotateAround(Transform transform, Vector3 target, Vector3 direction, float angle, float duration, bool isLocal, out int index)
        {
            var core = new CTcore();
            core.setInit(transform.gameObject.GetInstanceID(), LerpCoreType.RotateAround, new Quaternion(angle, transform.position.x, transform.position.y, transform.position.z), isLocal);
            index = GetArraySlot(ref core, transform);
            ctdeltas[index]._duration = duration;
            ctvectors[index].set(target, direction);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateQuat(Transform transform, Quaternion to, float duration, out int index)
        {
            var core = new CTcore();
            core.setInit(transform.gameObject.GetInstanceID(), LerpCoreType.Rotation, transform.localRotation, true);
            index = GetArraySlot(ref core, transform);
            ctdeltas[index]._duration = duration;
            ctvectors[index].set(new Vector3(to.x, to.y, to.z), new Vector3(to.w, 0f, 0f));
        }
        /// <summary>Floats interpolation.</summary>
        public static void InstantiateFloat(int id, float from, float to, float duration, Action<float> func, out int index)
        {
            var core = new CTcore();
            core.setInit(id, LerpCoreType.Float, false);

            index = GetArraySlot(ref core, null);
            ctdeltas[index]._duration = duration;
            ctvectors[index].from = new Vector3(from, to, 0f);

            fcore[index].selectDelegate();

            fcore[index].assignCallback((x, value) =>
            {
                if (x == 0)
                {
                    func.Invoke(value);
                }
            });
        }
        /// <summary>Gets/sets array pool slot.</summary>
        public static int GetArraySlot(ref CTcore core, Transform transform)
        {
            if (fcore == null) Init();

            var idx = -1;

            int Insert(ref CTcore core, Transform transform)
            {
                for (int i = 0; i < fcore.Length; i++)
                {
                    if (fcore[i].index != -1)
                        continue;

                    core.setIndex((short)i);

                    fcore[i] = core;
                    ctobjects[i].AssignObjects(transform, null);
                    CPlayerLoop.InsertToActiveTweens(i);
                    return i;
                }

                return -1;
            }

            idx = Insert(ref core, transform);

            if (idx == -1)
            {
                Resize();
                idx = Insert(ref core, transform);
            }

            return idx;
        }

        ////////////////////////////////////////////////////////////
        /////////////////// NON STATICS HERE ///////////////////////
        ////////////////////////////////////////////////////////////
        /// <summary>The property</summary>
        Quaternion _initRotation;
        /// <summary>The gameObject's instance id. Equal to gameObject.GetInstanceID().</summary>
        int _id;
        /// <summary>The index in the array.</summary>
        short _index;
        /// <summary>Boolean states.</summary>
        BoolProperty _states;
        /// <summary>Tween mode.</summary>
        LerpCoreType _mode;
        public void setInit(int id, LerpCoreType mode, bool isLocal)
        {
            _mode = mode;
            _id = id;
            selectIslocal(isLocal);
        }
        public void setInit(int id, LerpCoreType mode, Quaternion fromrot, bool isLocal)
        {
            _mode = mode;
            _id = id;
            selectIslocal(isLocal);
            _initRotation = fromrot;
        }
        public Vector3 getVectorProgress => Vector3.LerpUnclamped(vector.from, vector.to, tick);
        public void setIndex(short index) => _index = index;
        public ref CTcore ctcore => ref fcore[_index];
        public ref CTdelta delta => ref ctdeltas[_index];
        public ref CTvector vector => ref ctvectors[_index];
        /// <summary>Initial value.</summary>
        public ref Vector3 from => ref ctvectors[_index].from;
        /// <summary>Target value.</summary>
        public ref Vector3 to => ref ctvectors[_index].to;
        /// <summary>Initial rotation.</summary>
        public Quaternion fromRotation => _initRotation;
        /// <summary>Interpolation type.</summary>
        public LerpCoreType mode => _mode;
        /// <summary>Speed based.</summary>
        public void setSpeed(float value) => ctdeltas[_index]._speed = value;
        /// <summary>Easing function type.</summary>
        public void setEase(byte value) => ctdeltas[_index]._ease = value;
        /// <summary>Loop count.</summary>
        public void setLoopCount(byte value) => ctdeltas[_index]._loopCount = value;
        /// <summary>Instance id.</summary>
        public void setId(int value) => _id = value;
        /// <summary>Id. Default is the gameObjects/unityObject's id otherwise customs.</summary>
        public int id => _id;
        /// <summary>Index.</summary>
        public int index => _index;
        /// <summary>Loop count.</summary>
        public byte loopCount => ctdeltas[_index]._loopCount;
        /// <summary>Tween duration.</summary>
        public float duration => ctdeltas[_index]._duration;
        /// <summary>RectTransform.</summary>
        public RectTransform rectTransform => ctobjects[_index].transform as RectTransform;
        /// <summary>The transform.</summary>
        public Transform transform => ctobjects[_index].transform;
        /// <summary>Delegate.</summary>
        /// <param name="calltype">Type of calls, e.g : oncomplete, update, lastoncomplete.</param>
        /// <param name="ticks">The delta ticks.</param>
        public void callback(int calltype, float ticks) => ctobjects[_index].invoke.Invoke(calltype, ticks);
        /// <summary>Register callbacks.</summary>
        public void assignCallback(Action<int, float> callback) => ctobjects[_index].invoke += callback;
        /// <summary>Delta tick.</summary>
        float tick => ctdeltas[_index].tick;
        /// <summary>Sets default property.</summary>
        void setDefault(bool resetCtObjects)
        {
            if (_index > -1)
            {
                if (resetCtObjects)
                {
                    ctobjects[_index].Reset();
                }

                ctdeltas[_index].setDefault();
            }

            _id = -1;
            _index = -1;
            _mode = LerpCoreType.Position;
            ClearFlags();
        }
        /// <summary>Sets from rotation.</summary>
        /// <param name="quaternion">Quaternion.</param>
        public void setFromRotation(Quaternion quaternion) => _initRotation = quaternion;
        /// <summary>Retrieves the initial position value of a transform.</summary>
        public Vector3 initPosition => new Vector3(_initRotation.y, _initRotation.z, _initRotation.w);
        public void setInitPosition() => _initRotation = new Quaternion(from.x, from.y, from.z, 0f); 
        /// <summary>Sets from.</summary>
        public void setFrom(Vector3 from) => vector.from = from;
        /// <summary>Invoked on completion.</summary>
        void OnComplete()
        {
            OnLoop();
            Clear();
        }
        /// <summary>Invoked during loop cycle.</summary>
        void OnLoop()
        {
            var zeroone = !isFlipTick ? 1f : 0f;

            switch (_mode)
            {
                case LerpCoreType.Position:
                    transform.localPosition = vector.lerp(zeroone);
                    break;
                case LerpCoreType.Rotation:
                    transform.localRotation = lerpQuat(zeroone);
                    break;
                case LerpCoreType.Scale:
                    transform.localScale = vector.lerp(zeroone);
                    break;
                case LerpCoreType.Float:
                    callback(0, vector.lerpFloat(zeroone));
                    break;
                case LerpCoreType.AnchoredPosition:
                    interpPosition2D();
                    break;
                case LerpCoreType.SizeDelta:
                    rectTransform.sizeDelta = vector.lerp(zeroone);
                    break;
                case LerpCoreType.RotateAround:
                    rotateAround(zeroone);
                    break;
                case LerpCoreType.Translate:
                    translate(tick);
                    break;
            }
        }
        /// <summary>Interpolates. Must be called every frame.</summary>
        public void Run()
        {
            if (isPaused) return;

            updateTime();
            bool wasComplete = false;

            switch (_mode)
            {
                case LerpCoreType.Position:
                    wasComplete = interpPosition();
                    break;
                case LerpCoreType.Rotation:
                    wasComplete = interpRotation();
                    break;
                case LerpCoreType.Scale:
                    wasComplete = interpScale();
                    break;
                case LerpCoreType.Float:
                    wasComplete = interpFloat();
                    break;
                case LerpCoreType.SizeDelta:
                    wasComplete = interpSizeDelta();
                    break;
                case LerpCoreType.AnchoredPosition:
                    wasComplete = interpPosition2D();
                    break;
                case LerpCoreType.RotateAround:
                    wasComplete = interpRotateAround();
                    break;
                case LerpCoreType.Follow:
                    follow();
                    break;
                case LerpCoreType.Translate:
                    wasComplete = interpTranslate();
                    break;
            }

            if (!wasComplete)
            {
                //1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).
                if (isDelegateAssigned)
                {
                    callback(1, tick);
                }
            }
            else
            {
                OnComplete();
            }
        }
        /// <summary>Pauses the tween.</summary>
        public void Pause()
        {
            if (isPaused) return;

            selectPause(true);
        }
        /// <summary>Resumes paused tween.</summary>
        /// <param name="updateTransform">Updates the transform.</param>
        public void Resume(bool updateTransform = false)
        {
            if (!isPaused) return;

            if (updateTransform)
            {
                UpdateTransform();
            }

            selectPause(false);
        }
        /// <summary>Cancels the running tween.</summary>
        public void Cancel(bool invokeOnComplete = false, bool invokeLastComplete = true) => Clear(invokeOnComplete, invokeLastComplete);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Update the delta tick.</summary>
        void updateTime() => delta._runningTime += !isFlipTick ? (!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime) : -(!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime);
        /// <summary>Updates the transform properties (from and to values or rotation).</summary>
        public void UpdateTransform()
        {
            switch (_mode)
            {
                case LerpCoreType.Position:
                    setFrom(transform.localPosition);
                    break;
                case LerpCoreType.Scale:
                    setFrom(transform.localScale);
                    break;
                case LerpCoreType.Rotation:
                    setFromRotation(transform.localRotation);
                    break;
                case LerpCoreType.AnchoredPosition:
                    setFrom(!isLocal ? rectTransform.anchoredPosition3D : rectTransform.anchoredPosition);
                    break;
                case LerpCoreType.Translate:
                    setFrom(!isLocal ? transform.position : transform.localPosition);
                    break;
                case LerpCoreType.SizeDelta:
                    setFrom(rectTransform.sizeDelta);
                    break;
            }
        }
        /// <summary>Interpolates the position.</summary>
        bool interpPosition()
        {
            if (!onRepeat())
            {
                transform.localPosition = vector.lerp(tick);
                return false;
            }

            return true;
        }
        bool interpTranslate()
        {
            if (!onRepeat())
            {
                translate(tick);
                return false;
            }

            return true;
        }
        void translate(float tick)
        {
            var tmp = ctvectors[_index].get;
            Vector3 targetDelta = Vector3.LerpUnclamped(tmp.from, tmp.to, tick) - (!isLocal ? transform.position : transform.localPosition);
            transform.Translate(targetDelta, !isLocal ? Space.World : Space.Self);
        }
        /// <summary>Interpolates the position.</summary>
        bool interpPosition2D()
        {
            if (!onRepeat())
            {
                if (!isLocal)
                {
                    rectTransform.anchoredPosition3D = vector.lerp(tick);
                }
                else
                {
                    rectTransform.anchoredPosition = vector.lerp(tick);
                }

                return false;
            }

            return true;
        }
        /// <summary>Interpolates the rotation.</summary>
        bool interpRotation()
        {
            if (!onRepeat())
            {
                transform.localRotation = lerpQuat(tick);
                return false;
            }

            return true;
        }
        /// <summary>Rotates around target point.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool interpRotateAround()
        {
            if (!onRepeat())
            {
                rotateAround(tick);
                return false;
            }

            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void rotateAround(float tick)
        {
            Quaternion rotation = Quaternion.AngleAxis(tick * fromRotation.x, to);
            Vector3 rotatedDirection = rotation * (initPosition - from);
            rotatedDirection = from + rotatedDirection;

            if (!isLocal)
            {
                transform.SetPositionAndRotation(rotatedDirection, rotation);
            }
            else
            {
                transform.SetLocalPositionAndRotation(rotatedDirection, rotation);
            }
        }
        /// <summary>Interpolates the scale.</summary>
        bool interpScale()
        {
            if (!onRepeat())
            {
                transform.localScale = vector.lerp(tick); //redirect the tick to easing function here.
                return false;
            }

            return true;
        }
        /// <summary>Interpolates the scale.</summary>
        bool interpSizeDelta()
        {
            if (!onRepeat())
            {
                rectTransform.sizeDelta = vector.lerp(tick); //redirect the tick to easing function here.
                return false;
            }

            return true;
        }
        /// <summary>Interpolates the float value.</summary>
        bool interpFloat()
        {
            if (!onRepeat())
            {
                callback(0, vector.lerpFloat(tick));
                return false;
            }

            return true;
        }
        /// <summary>Finalizes the tween cycle.</summary>
        void Clear(bool invokeOnComplete = false, bool invokeLastComplete = true)
        {
            if (isDelegateAssigned)
            {
                //1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).
                if (invokeOnComplete)
                {
                    callback(2, tick);
                }
                else
                {
                    if (!isCompleterepeat)
                    {
                        callback(2, tick);
                    }
                }

                if (invokeLastComplete)
                {
                    callback(4, tick);
                }
            }

            CPlayerLoop.RemoveFromActiveTweens(index);
            ctcore.setDefault(true);
        }
        /// <summary>Invoked at the end of loop cycle.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool onRepeat()
        {
            var tmp = delta.getTime;

            if (!isFlipTick)
            {
                if (!tmp.speed)
                {
                    if (tmp.runningTime < tmp.duration)
                    {
                        return false;
                    }
                }
                else
                {
                    if (tmp.runningTime < 1f)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (!tmp.speed)
                {
                    if (tmp.runningTime > 0f)
                    {
                        return false;
                    }
                }
                else
                {
                    if (tmp.runningTime > 0f)
                    {
                        return false;
                    }
                }
            }

            OnLoop();
            var loops = delta.getLoops;

            if (loops.loopCount > 0)
            {
                delta.incLoopCounter();
                loops = delta.getLoops;

                if (!isPingpong)
                {
                    delta._runningTime = 0f;

                    if (isCompleterepeat)
                    {
                        //1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).
                        if (isDelegateAssigned)
                        {
                            callback(2, tick);
                        }
                    }

                    if (isInfiniteLoop)
                    {
                        if (loops.loopCount == loops.loopCounter)
                        {
                            delta._loopCounter = 0;
                            return false;
                        }
                    }

                    return loops.loopCounter < loops.loopCount ? false : true;
                }
                else
                {
                    if (isCompleterepeat && (loops.loopCounter & 1) == 0)
                    {
                        //1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).
                        if (isDelegateAssigned)
                        {
                            callback(2, tick);
                        }
                    }

                    if (loops.loopCounter < loops.loopCount * 2)
                    {
                        selectFliptick(!isFlipTick);
                        return false;
                    }

                    if (isInfiniteLoop)
                    {
                        delta.resetCounter();
                        selectFliptick(!isFlipTick);
                        return false;
                    }
                }
            }

            return true;
        }
        /// <summary>Slerps two quaternions.</summary>
        Quaternion lerpQuat(float tick) => Quaternion.SlerpUnclamped(fromRotation, vector.getQuat, tick);
        /// <summary>Follows at far distance range.</summary>
        public void follow()
        {
            Vector3 velocity = to;
            transform.position = Vector3.SmoothDamp(transform.position, FindExternalTransform(index).position, ref velocity, from.x);
        }

        ////////////////////////////////////////////////////////////////////////
        //////////////// TWEEN STATES BIT WISE OPERATIONS HERE /////////////////
        ////////////////////////////////////////////////////////////////////////
        /// <summary>Unscaled time state.</summary>
        public bool isUnscaled => _states.HasFlag(BoolProperty.Unscaledtime);
        /// <summary>Paused state.</summary>
        public bool isPaused => _states.HasFlag(BoolProperty.Pause);
        /// <summary>Pingpong state.</summary>
        public bool isPingpong => _states.HasFlag(BoolProperty.Pingpong);
        /// <summary>Locality.</summary>
        public bool isLocal => _states.HasFlag(BoolProperty.Islocal);
        /// <summary>Whether the oncomplete callback should be invoked on every loop cycle.</summary>
        public bool isCompleterepeat => _states.HasFlag(BoolProperty.Completerepeat);
        /// <summary>Loops the tween endlessly.</summary>
        public bool isInfiniteLoop => _states.HasFlag(BoolProperty.Infiniteloop);
        /// <summary>Loop cycle state, whether it's linear or non-linear direction.</summary>
        public bool isFlipTick => _states.HasFlag(BoolProperty.Fliptick);
        /// <summary>Delegate assignment state.</summary>
        public bool isDelegateAssigned => _states.HasFlag(BoolProperty.Delegatewasassigned);
        /////////////////// FLAG SETTER HERE ////////////////////
        /// <summary>Trigger unscaledTime.</summary>
        public void selectUnscaledtime() => _states |= BoolProperty.Unscaledtime;
        /// <summary>Triggers pingpong state.</summary>
        public void selectPingpong() => _states |= BoolProperty.Pingpong;
        /// <summary>Sets the locality.</summary>
        void selectIslocal(bool islocal)
        {
            if (!islocal) return;

            _states |= BoolProperty.Islocal;
        }
        /// <summary>Flipping the loop cycle state.</summary>
        void selectFliptick(bool state)
        {
            if (!state)
                _states &= ~BoolProperty.Fliptick;
            else
                _states |= BoolProperty.Fliptick;

            delta.FlipSwitch(isFlipTick);
        }
        /// <summary>Sets the callback repeat flag.</summary>
        public void selectOncompleterepeat() => _states |= BoolProperty.Completerepeat;
        /// <summary>Changes the pause state.</summary>
        public void selectPause(bool state)
        {
            if (!state)
            {
                _states &= ~BoolProperty.Pause;
            }
            else
            {
                _states |= BoolProperty.Pause;
            }
        }
        /// <summary>Sets the infinite loop flag.</summary>
        public void selectInfiniteloop() => _states |= BoolProperty.Infiniteloop;
        /// <summary>Sets the delegate assignment flag.</summary>
        void selectDelegate() => _states |= BoolProperty.Delegatewasassigned;
        /// <summary>Clears the flags.</summary>
        void ClearFlags()
        {
            BoolProperty flagsToClear = BoolProperty.Pingpong | BoolProperty.Islocal | BoolProperty.Fliptick | BoolProperty.Completerepeat
            | BoolProperty.Pause | BoolProperty.Unscaledtime | BoolProperty.Infiniteloop | BoolProperty.Delegatewasassigned;

            _states &= ~flagsToClear;
        }
    }

    [Flags]
    public enum BoolProperty : byte
    {
        Pingpong = 1,
        Islocal = 1 << 1,
        Fliptick = 1 << 2,
        Completerepeat = 1 << 3,
        Pause = 1 << 4,
        Unscaledtime = 1 << 5,
        Infiniteloop = 1 << 6,
        Delegatewasassigned = 1 << 7
    }
    /// <summary>UnityObject wrapper.</summary>
    [SerializeField]
    public struct CTobject
    {
        /// <summary>Transform.</summary>
        public Transform transform;
        /// <summary>Callback.</summary>
        public Action<int, float> invoke;
        public (Transform transform, Action<int, float> invoke) GetObjects => (transform, invoke);
        public void AssignObjects(Transform transforms, Action<int, float> invokes)
        {
            transform = transforms;
            invoke = invokes;
        }
        public void Reset()
        {
            transform = null;
            invoke = null;
        }
    }
    [Serializable]
    public struct CTvector
    {
        public Vector3 from;
        public Vector3 to;
        public (Vector3 from, Vector3 to) get => (from, to);
        public void set(Vector3 froms, Vector3 tos)
        {
            from = froms;
            to = tos;
        }
        public Quaternion getQuat => new Quaternion(from.x, from.y, from.z, to.x);
        /// <summary>Interpolates Vector3s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 lerp(float tick) => new Vector3(from.x + (to.x - from.x) * tick, from.y + (to.y - from.y) * tick, from.z + (to.z - to.z) * tick);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float lerpFloat(float tick) => from.x + (from.y - from.x) * tick;
    }
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CTdelta
    {
        public float _duration;
        public float _runningTime;
        public float _speed;
        public float _flipTick;
        public byte _loopCount;
        public byte _loopCounter;
        public byte _ease;
        public bool _unscaledTime;
        public float tick
        {
            get
            {
                if (_speed < 0f)
                {
                    return CTEasing.Easing(_ease, _runningTime / _duration);
                }
                else
                {
                    _runningTime = Mathf.MoveTowards(_runningTime, _flipTick, _speed * 0.25f * (!_unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime));
                    return _runningTime;
                }
            }
        }
        public void FlipSwitch(bool flip) => _flipTick = !flip ? 1f : 0f;
        public void setDefault()
        {
            _loopCount = 1;
            _ease = (byte)Ease.EaseLinear;
            _speed = -1f;
            _runningTime = 0f;
            _loopCounter = 0;
            _unscaledTime = false;
            _flipTick = 0f;
        }
        public void resetCounter() => _loopCounter = 0;
        public void incLoopCounter() => _loopCounter++;
        public (float duration, float runningTime, bool speed) getTime => (_duration, _runningTime, _speed > 0);
        public (byte loopCount, byte loopCounter) getLoops => (_loopCount, _loopCounter);
    }
    /// <summary>Interpolation types.</summary>
    public enum LerpCoreType : byte
    {
        Position = 0,
        Rotation = 1,
        Float = 2,
        Scale = 3,
        Translate = 4,
        Follow = 5,
        AnchoredPosition = 6,
        SizeDelta = 7,
        RotateAround = 8
    }
}
