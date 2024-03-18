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

using System.Collections;
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
        public static CTcore[] fcore;
        /// <summary>UnityObjects.</summary>
        public static CTobject[] ctobjects;
        /// <summary>CTcore pool.</summary>
        static ArrayPool<CTcore> pool = ArrayPool<CTcore>.Shared;
        /// <summary>Object pool.</summary>
        static ArrayPool<CTobject> poolObjects = ArrayPool<CTobject>.Shared;
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
        /// <summary>Finds the non-main transforms that are related to a tween.</summary>
        /// <param name="index"></param>
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
        public static void Init()
        {
            fcore = pool.Rent(maxPoolLength);
            ctobjects = poolObjects.Rent(maxPoolLength);
            CPlayerLoop.InitStaticArray();

            for (int i = 0; i < CPlayerLoop.activeCores.Length; i++)
            {
                CPlayerLoop.activeCores[i].set(-1, -1);
                fcore[i].setDefault(false);
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
        /// <summary>Register a callback.</summary>
        static void RegisterCalls(int type, int index, Action callback, Action<float> updateCallback = null)
        {
            fcore[index].selectDelegate();

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
        }
        /// <summary>Resizes the array length.</summary>
        /// <param name="setdefault">Set default state.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Resize(bool setdefault)
        {
            if (!setdefault)
            {
                var count = fcore.Length;
                var newarr = pool.Rent(count * 2);
                var newobjs = poolObjects.Rent(count * 2);

                for (int i = 0; i < newarr.Length; i++)
                {
                    if (i < fcore.Length)
                    {
                        newarr[i] = fcore[i];
                        newobjs[i].AssignObjects(ctobjects[i].GetObjects.transform, ctobjects[i].GetObjects.invoke);
                        ctobjects[i].Reset();
                    }
                    else
                    {
                        newarr[i]._index = -1;
                    }
                }

                pool.Return(fcore);
                poolObjects.Return(ctobjects);
                fcore = newarr;
                ctobjects = newobjs;
                CPlayerLoop.Resize(false);
            }
            else
            {
                pool.Return(fcore);
                poolObjects.Return(ctobjects);
                fcore = pool.Rent(maxPoolLength);
                ctobjects = poolObjects.Rent(maxPoolLength);
                CPlayerLoop.Resize(true);
            }
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateVector(Transform transform, Vector3 from, Vector3 to, float duration, LerpCoreType mode, bool isLocal, out int index)
        {
            var core = new CTcore
            {
                _id = transform.gameObject.GetInstanceID(),
                _duration = duration,
                _mode = mode
            };

            core.set(from, to);
            core.selectIslocal(isLocal);
            index = GetArraySlot(ref core, transform);
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateFollow(Transform transform, Transform to, float smoothTime, Vector3 velocity, out int index)
        {
            var core = new CTcore
            {
                _id = transform.gameObject.GetInstanceID(),
                _duration = float.PositiveInfinity,
                _mode = LerpCoreType.Follow
            };

            core.set(new Vector3(smoothTime, 0f, 0f), velocity);
            index = GetArraySlot(ref core, transform);
            exttransforms.Add((to, index));
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateRotateAround(Transform transform, Vector3 target, Vector3 direction, float angle, float duration, bool isLocal, out int index)
        {
            var core = new CTcore
            {
                _id = transform.gameObject.GetInstanceID(),
                _duration = duration,
                _mode = LerpCoreType.RotateAround
            };

            core.set(target, direction);
            core.setFromRotation(new Quaternion(angle, transform.position.x, transform.position.y, transform.position.z));
            core.selectIslocal(isLocal);
            index = GetArraySlot(ref core, transform);
        }
        /// <summary>Instantiates new interpolator.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateQuat(Transform transform, Vector3 direction, float angle, float duration, bool isLocal, out int index)
        {
            var core = new CTcore
            {
                _id = transform.gameObject.GetInstanceID(),
                _duration = duration,
                _mode = LerpCoreType.Rotation
            };

            core.selectIslocal(isLocal);
            core.set(new Vector3(angle, 0f, 0f), direction);
            core.setFromRotation(!isLocal ? transform.rotation : transform.localRotation);
            index = GetArraySlot(ref core, transform);
        }
        /// <summary>Floats interpolation.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateFloat(int id, float from, float to, float duration, Action<float> func, out int index, bool specialCase = false)
        {
            var core = new CTcore
            {
                _id = id,
                _duration = duration,
                _mode = LerpCoreType.Float
            };

            core.setFrom(new Vector3(from, to, 0f));
            index = GetArraySlot(ref core, null);

            if (func != null)
            {
                fcore[index].selectDelegate();

                fcore[index].assignCallback((x, value) =>
                {
                    if (x == 0)
                    {
                        func.Invoke(value);
                    }
                });
            }
            else
            {
                if (!specialCase)
                {
                    throw new Exception("CTween error : Can't be empty/null.");
                }
            }
        }
        /// <summary>Gets/sets array pool slot.</summary>
        public static int GetArraySlot(ref CTcore core, Transform transform)
        {
            var idx = -1;

            void Insert(ref CTcore core, Transform transform)
            {
                for (int i = 0; i < fcore.Length; i++)
                {
                    if (fcore[i].index != -1)
                        continue;

                    core._index = (short)i;
                    fcore[i] = core;
                    idx = i;
                    ctobjects[i].AssignObjects(transform, null);
                    CPlayerLoop.InsertToActiveTweens(i);
                    return;
                }
            }

            Insert(ref core, transform);

            if (idx == -1)
            {
                Resize(false);
                Insert(ref core, transform);
            }

            return idx;
        }

        ////////////////////////////////////////////////////////////
        /////////////////// NON STATICS HERE ///////////////////////
        ////////////////////////////////////////////////////////////
        /// <summary>The property</summary>
        Quaternion _initRotation;
        /// <summary>The transform.</summary>
        float x, a, y, b, z, c;
        /// <summary>The duration/time for the tween to complete.</summary>
        float _duration;
        /// <summary>The elapsed time in seconds based on the duration.</summary>
        float _runningTime;
        /// <summary>The gameObject's instance id. Equal to gameObject.GetInstanceID().</summary>
        int _id;
        /// <summary>The index in the array.</summary>
        short _index;
        /// <summary>Loop count.</summary>
        byte _loopCount;
        /// <summary>Elapsed loop counter.</summary>
        byte _loopCounter;
        /// <summary>Easing functions.</summary>
        byte _ease;
        /// <summary>Speed</summary>
        byte _speed;
        byte _jump;
        /// <summary>Tween mode.</summary>
        LerpCoreType _mode;
        /// <summary>Boolean states.</summary>
        BoolProperty _states;
        /// <summary>Initial value.</summary>
        public Vector3 from => new Vector3(a, b, c);
        /// <summary>Target value.</summary>
        public Vector3 to => new Vector3(x, y, z);
        /// <summary>Initial rotation.</summary>
        public Quaternion fromRotation => _initRotation;
        /// <summary>Interpolation type.</summary>
        public LerpCoreType mode => _mode;
        /// <summary>Speed based.</summary>
        public void setSpeed(byte value) => _speed = value;
        /// <summary>Easing function type.</summary>
        public void setEase(byte value) => _ease = value;
        /// <summary>Loop count.</summary>
        public void setLoopCount(byte value) => _loopCount = value;
        /// <summary>Loop counter.</summary>
        public void setLoopCounter(byte value) => _loopCounter = value;
        /// <summary>Instance id.</summary>
        public void setId(int value) => _id = value;
        /// <summary>Elapsed time.</summary>
        public float runningTime => _runningTime;
        /// <summary>Id. Default is the gameObjects/unityObject's id otherwise customs.</summary>
        public int id => _id;
        /// <summary>Index.</summary>
        public int index => _index;
        /// <summary>Loop count.</summary>
        public byte loopCount => _loopCount;
        /// <summary>Tween duration.</summary>
        public float duration => _duration;
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
        float tick
        {
            get
            {
                if (_speed == 0)
                {
                    return CTEasing.Easing(_ease, _runningTime / _duration);
                }
                else
                {
                    _runningTime = Mathf.MoveTowards(_runningTime, !isFlipTick ? 1f : 0f, _speed / 3f * (!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime));
                    return _runningTime;
                }
            }
        }
        /// <summary>Sets default property.</summary>
        void setDefault(bool resetCtObjects)
        {
            if (resetCtObjects)
            {
                ctobjects[_index].Reset();
            }

            _id = -1;
            _index = -1;
            _ease = (byte)Ease.EaseLinear;
            _runningTime = 0f;
            _duration = 0f;
            _loopCount = 1;
            _loopCounter = 0;
            _mode = LerpCoreType.Position;
            _initRotation = Quaternion.identity;

            ClearFlags();
        }
        /// <summary>Sets from rotation.</summary>
        /// <param name="quaternion">Quaternion.</param>
        public void setFromRotation(Quaternion quaternion) => _initRotation = quaternion;
        /// <summary>Retrieves the initial position value of a transform.</summary>
        public Vector3 initPosition => new Vector3(_initRotation.y, _initRotation.z, _initRotation.w);
        /// <summary>Sets from.</summary>
        public void setFrom(Vector3 from)
        {
            a = from.x; b = from.y; c = from.z;
        }
        /// <summary>Sets target value.</summary>
        public void setTo(Vector3 to)
        {
            x = to.x; y = to.y; z = to.z;
        }
        /// <summary>Sets the init and target values.</summary>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        public void set(Vector3 from, Vector3 to)
        {
            setFrom(from);
            setTo(to);
        }
        /// <summary>Invoked on completion.</summary>
        void OnComplete()
        {
            OnLoop();
            Clear();
        }
        /// <summary>Invoked during loop cycle.</summary>
        void OnLoop()
        {
            switch (_mode)
            {
                case LerpCoreType.Position:
                case LerpCoreType.Translate:

                    if(!isLocal)
                    {
                        transform.position = lerpVector3(!isFlipTick ? 1f : 0f);
                    }
                    else
                    {
                        transform.localPosition = lerpVector3(!isFlipTick ? 1f : 0f);
                    }

                    break;
                case LerpCoreType.Rotation:

                    if (!isLocal)
                    {
                        transform.rotation = lerpQuat(!isFlipTick ? 1f : 0f);
                    }
                    else
                    {
                        transform.localRotation = lerpQuat(!isFlipTick ? 1f : 0f);
                    }

                    break;
                case LerpCoreType.Scale:
                    transform.localScale = lerpVector3(!isFlipTick ? 1f : 0f);
                    break;
                case LerpCoreType.Float:

                    if (isDelegateAssigned)
                    {
                        callback(0, lerpFloat(!isFlipTick ? 1f : 0f));
                    }

                    break;
                case LerpCoreType.AnchoredPosition:
                    if (!isLocal)
                    {
                        rectTransform.anchoredPosition3D = lerpVector3(!isFlipTick ? 1f : 0f);
                    }
                    else
                    {
                        rectTransform.anchoredPosition = lerpVector3(!isFlipTick ? 1f : 0f);
                    }
                    break;
                case LerpCoreType.SizeDelta:
                    rectTransform.sizeDelta = lerpVector3(!isFlipTick ? 1f : 0f);
                    break;
                case LerpCoreType.RotateAround:
                    if (!isLocal)
                    {
                        transform.rotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, to.normalized);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, to.normalized);
                    }
                    break;
            }
        }
        /// <summary>Interpolates. Must be called every frame.</summary>
        public void Run()
        {
            if (isPaused)
            {
                return;
            }

            updateTime();
            bool wasComplete = false;

            switch (_mode)
            {
                case LerpCoreType.Position:
                case LerpCoreType.Translate:
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
                    wasComplete = interpScale2D();
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
            if (isPaused)
            {
                return;
            }

            selectPause(true);
        }
        /// <summary>Resumes paused tween.</summary>
        /// <param name="updateTransform">Updates the transform.</param>
        public void Resume(bool updateTransform = false)
        {
            if (!isPaused)
            {
                return;
            }

            if (updateTransform)
            {
                UpdateTransform();
            }

            selectPause(false);
        }
        /// <summary>Cancels the running tween.</summary>
        public void Cancel(bool invokeOnComplete = false, bool invokeLastComplete = true) => Clear(invokeOnComplete, invokeLastComplete);
        /// <summary>Update the delta tick.</summary>
        void updateTime() => _runningTime += !isFlipTick ? (!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime) : -(!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime);
        /// <summary>Updates the transform properties (from and to values or rotation).</summary>
        public void UpdateTransform()
        {
            switch (_mode)
            {
                case LerpCoreType.Position:
                case LerpCoreType.Translate:
                    setFrom(!isLocal ? transform.position : transform.localPosition);
                    break;
                case LerpCoreType.Scale:
                    setFrom(transform.localScale);
                    break;
                case LerpCoreType.Rotation:
                    setFromRotation(!isLocal ? transform.rotation : transform.localRotation);
                    break;
                case LerpCoreType.AnchoredPosition:
                    setFrom(!isLocal ? rectTransform.anchoredPosition3D : rectTransform.anchoredPosition);
                    break;
            }
        }
        /// <summary>Interpolates the position.</summary>
        bool interpPosition()
        {
            if (!onRepeat())
            {
                if(!isLocal)
                {
                    transform.position = lerpVector3(tick);
                }
                else
                {
                    transform.localPosition = lerpVector3(tick);
                }

                return false;
            }

            return true;
        }
        /// <summary>Interpolates the position.</summary>
        bool interpPosition2D()
        {
            if (!onRepeat())
            {
                if(isLocal)
                {
                    rectTransform.anchoredPosition3D = lerpVector3(tick);
                }
                else
                {
                    rectTransform.anchoredPosition = lerpVector3(tick);
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
                if (!isLocal)
                {
                    transform.rotation = lerpQuat(tick);
                }
                else
                {
                    transform.localRotation = lerpQuat(tick);
                }

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
                if (!isLocal)
                {
                    var angle = tick * fromRotation.x; // 360 degrees in the specified duration
                    Quaternion rotation = Quaternion.AngleAxis(angle, to);
                    Vector3 rotatedDirection = rotation * (initPosition - from);
                    transform.SetPositionAndRotation(from + rotatedDirection, rotation);
                }
                else
                {
                    var angle = tick * fromRotation.x; // 360 degrees in the specified duration
                    Quaternion rotation = Quaternion.AngleAxis(angle, to);
                    Vector3 rotatedDirection = rotation * (initPosition - from);
                    transform.SetLocalPositionAndRotation(from + rotatedDirection, rotation);
                }

                return false;
            }

            return true;
        }
        /// <summary>Interpolates the scale.</summary>
        bool interpScale()
        {
            if (!onRepeat())
            {
                transform.localScale = lerpVector3(tick); //redirect the tick to easing function here.
                return false;
            }

            return true;
        }
        /// <summary>Interpolates the scale.</summary>
        bool interpScale2D()
        {
            if (!onRepeat())
            {
                rectTransform.sizeDelta = lerpVector3(tick); //redirect the tick to easing function here.
                return false;
            }

            return true;
        }
        /// <summary>Interpolates the float value.</summary>
        bool interpFloat()
        {
            if (!onRepeat())
            {
                callback(0, lerpFloat(tick));
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
            fcore[_index].setDefault(true);
        }
        /// <summary>Invoked at the end of loop cycle.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool onRepeat()
        {
            if (!isFlipTick)
            {
                if (_speed == 0)
                {
                    if (_runningTime < _duration)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_runningTime < 1)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (_speed == 0)
                {
                    if (_runningTime > 0f)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_runningTime > 0f)
                    {
                        return false;
                    }
                }
            }

            OnLoop();

            if (_loopCount > 0)
            {
                _loopCounter++;

                if (!isPingpong)
                {
                    _runningTime = 0f;

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
                        if (_loopCounter == _loopCount)
                        {
                            _loopCounter = 0;
                            return false;
                        }
                    }

                    return _loopCounter < _loopCount ? false : true;
                }
                else
                {
                    if (isCompleterepeat && (_loopCounter & 1) == 0)
                    {
                        //1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).
                        if (isDelegateAssigned)
                        {
                            callback(2, tick);
                        }
                    }

                    if (_loopCounter < _loopCount * 2)
                    {
                        selectFliptick(!isFlipTick);
                        return false;
                    }

                    if (isInfiniteLoop)
                    {
                        _loopCounter = 0;
                        selectFliptick(!isFlipTick);
                        return false;
                    }
                }
            }

            return true;
        }
        /// <summary>Interpolates Vector3s.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Vector3 lerpVector3(float tick) => new Vector3(a + (x - a) * tick, b + (y - b) * tick, c + (z - c) * tick);
        /// <summary>Slerps two quaternions.</summary>
        Quaternion lerpQuat(float tick) => Quaternion.SlerpUnclamped(fromRotation, fromRotation * Quaternion.AngleAxis(a, isLocal ? transform.InverseTransformDirection(to).normalized : to.normalized), tick);
        /// <summary>Interpolates floating points.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float lerpFloat(float tick) => a + (b - a) * tick;
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
        public void selectIslocal(bool islocal)
        {
            if (!islocal)
            {
                return;
            }

            _states |= BoolProperty.Islocal;
        }
        /// <summary>Flipping the loop cycle state.</summary>
        public void selectFliptick(bool state)
        {
            if (!state)
                _states &= ~BoolProperty.Fliptick;
            else
                _states |= BoolProperty.Fliptick;
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
        public void selectDelegate() => _states |= BoolProperty.Delegatewasassigned;
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
    public enum LoopType : int
    {
        Clamp = 0,
        PingPong = 1
    }
}
