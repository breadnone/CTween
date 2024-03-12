using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CTween.Extension
{
    public enum LerpCoreType : byte
    {
        Position = 0,
        Rotation = 1,
        Float = 2,
        Scale = 4,
        Translate = 8,
        Combine = 16,
        AnchoredPosition = 32,
        SizeDelta = 64,
        RotateAround = 128
    }
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct CTcore
    {
        static CTcore dummy = new CTcore { id = -1, index = -1 };
        public static CTcore[] fcore;
        public static Action<int, float>[] delegates;
        static ArrayPool<CTcore> pool = ArrayPool<CTcore>.Shared;
        /// <summary>The max limit for the pool. Default is 16 and will be resized automatically as needed.</summary>
        public static int maxPoolLength { get; set; } = 6;
        /// <summary>Gets tween the tween.</summary>
        /// <param name="instanceId">The instanceId or id.</param>
        public static ref CTcore getTween(int instanceId)
        {
            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].id == instanceId)
                {
                    return ref fcore[i];
                }
            }

            return ref dummy;
        }
        /// <summary>Removes the tween.</summary>
        /// <param name="index">Index.</param>
        public static void Remove(int index)
        {
            fcore[index].setDefault();
            delegates[index] = null;
            CPlayerLoop.RemoveFromActiveTweens(index);
        }
        /// <summary>Initialization.</summary>
        public static void Init()
        {
            fcore = pool.Rent(maxPoolLength);
            delegates = new Action<int, float>[400];
            CPlayerLoop.InitStaticArray();

            for (int i = 0; i < CPlayerLoop.activeCores.Length; i++)
            {
                CPlayerLoop.activeCores[i].set(-1, -1);
            }

            for (int i = 0; i < fcore.Length; i++)
            {
                fcore[i].index = -1;
                fcore[i].setDefault();
            }
        }
        /// <summary>Checks if tween instance is active in the pool.</summary>
        /// <param name="instanceId">Id or instanceId</param>
        public static bool Contains(int instanceId)
        {
            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index > -1 && fcore[i].id == instanceId)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>Tweening status.</summary>
        public static bool IsTweening(int index)
        {
            return fcore[index].id > -1;
        }
        /// <summary>Registers on update.</summary>
        /// <param name="index">The index.</param>
        /// <param name="callback">The callback.</param>
        public static void RegisterOnUpdate(int index, Action<float> callback)
        {
            fcore[index].selectAssigneddelegate();

            delegates[index] += (x, y) =>
            {
                if (x == 1)
                {
                    callback.Invoke(y);
                }
            };
        }
        /// <summary>Callback invocation on completion.</summary>
        /// <param name="index">Index.</param>
        /// <param name="callback">Callback.</param>
        public static void RegisterOnComplete(int index, Action callback)
        {
            fcore[index].selectAssigneddelegate();

            delegates[index] += (x, y) =>
            {
                if (x == 2)
                {
                    callback.Invoke();
                }
            };
        }
        /// <summary>Very last callback invocation, whether the tween completed or cancelled.</summary>
        /// <param name="index">Index.</param>
        /// <param name="callback">Callback.</param>
        public static void RegisterLastOnComplete(int index, Action callback)
        {
            fcore[index].selectAssigneddelegate();

            delegates[index] += (x, y) =>
            {
                if (x == 4)
                {
                    callback.Invoke();
                }
            };
        }
        /// <summary>Invokes the delegate based on the index in the pool. The index should be matched with the tween instance.</summary>
        /// <param name="type">1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last).</param>
        void InvokeCallback(int type)
        {
            if (isDelegateassigned)
            {
                delegates[index].Invoke(type, tick);
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
                var newArr = pool.Rent(count * 2);

                for (int i = 0; i < newArr.Length; i++)
                {
                    if (i < fcore.Length)
                    {
                        newArr[i] = fcore[i];
                    }
                    else
                    {
                        newArr[i].index = -1;
                    }
                }

                pool.Return(fcore);
                fcore = newArr;
                CPlayerLoop.Resize(false);
            }
            else
            {
                pool.Return(fcore);
                fcore = pool.Rent(maxPoolLength);
                CPlayerLoop.Resize(true);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Instantiates new interpolator.</summary>
        /// <param name="transform">Transform component.</param>
        public static void InstantiateVector(Transform transform, Vector3 from, Vector3 to, float duration, LerpCoreType mode, bool isLocal, out int index)
        {
        repeat:

            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index == -1)
                {
                    var core = new CTcore
                    {
                        id = transform.gameObject.GetInstanceID(),
                        index = (short)i,
                        duration = duration,
                        transform = transform,
                    };

                    index = i;

                    core.setFrom(from);
                    core.setTo(to);
                    core.mode = mode;
                    core.selectIslocal(isLocal);

                    fcore[i] = core;
                    CPlayerLoop.InsertToActiveTweens(i);
                    return;
                }
            }

            Resize(false);
            goto repeat;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Instantiates new interpolator.</summary>
        /// <param name="transform">Transform component.</param>
        public static void InstantiateRotateAround(Transform transform, Vector3 target, Vector3 direction, float angle, float duration, bool isLocal, out int index)
        {
        repeat:

            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index == -1)
                {
                    var core = new CTcore
                    {
                        id = transform.gameObject.GetInstanceID(),
                        index = (short)i,
                        duration = duration,
                        transform = transform,
                    };

                    index = i;

                    var pos = transform.position;

                    core.setFrom(target);
                    core.setTo(direction);
                    core.setFromRotation(new Quaternion(angle, pos.x, pos.y, pos.z));
                    core.mode = LerpCoreType.RotateAround;
                    core.selectIslocal(isLocal);

                    fcore[i] = core;
                    CPlayerLoop.InsertToActiveTweens(i);
                    return;
                }
            }

            Resize(false);
            goto repeat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Instantiates new interpolator.</summary>
        /// <param name="transform">Transform component.</param>
        public static void InstantiateQuat(Transform transform, Vector3 direction, float angle, float duration, bool isLocal, out int index)
        {
        repeat:

            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index == -1)
                {
                    var core = new CTcore
                    {
                        id = transform.gameObject.GetInstanceID(),
                        index = (short)i,
                        duration = duration,
                        transform = transform,
                    };

                    index = i;

                    core.selectIslocal(isLocal);
                    core.setTo(direction);
                    core.setFrom(new Vector3(angle, 0f, 0f));
                    core.mode = LerpCoreType.Rotation;
                    core.setFromRotation(!isLocal ? transform.rotation : transform.localRotation);
                    fcore[i] = core;

                    CPlayerLoop.InsertToActiveTweens(i);
                    return;
                }
            }

            Resize(false);
            goto repeat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InstantiateFloat(int id, float from, float to, float duration, Action<float> func, out int index)
        {
        repeat:

            for (int i = 0; i < fcore.Length; i++)
            {
                if (fcore[i].index == -1)
                {
                    var core = new CTcore
                    {
                        id = id,
                        index = (short)i,
                        duration = duration
                    };

                    index = i;
                    core.setFrom(new Vector3(from, to, 0f));
                    core.mode = LerpCoreType.Float;

                    delegates[i] += (x, tick) =>
                    {
                        if (x == 1)
                        {
                            var idx = i;
                            func.Invoke(Mathf.LerpUnclamped(from, to, tick));
                        }
                    };

                    fcore[i] = core;
                    CPlayerLoop.InsertToActiveTweens(i);
                    return;
                }
            }

            Resize(false);
            goto repeat;
        }

        ////////////////////////////////////////////////////////////
        /////////////////// NON STATICS HERE ///////////////////////
        ////////////////////////////////////////////////////////////

        /// <summary>The property</summary>
        Quaternion _defaultFromRotation;
        public Transform transform;
        /// <summary>The gameObject's instance id. Equal to gameObject.GetInstanceID().</summary>
        public int id;
        /// <summary>Easing functions.</summary>
        public byte ease;
        /// <summary>Speed</summary>
        public byte speed;
        /// <summary>Elapsed speed.</summary>
        public float runningSpeed;
        /// <summary>The transform.</summary>
        float x;
        float a;
        float y;
        float b;
        float z;
        float c;
        /// <summary>The duration/time for the tween to complete.</summary>
        public float duration;
        /// <summary>The elapsed time in seconds based on the duration.</summary>
        float runningTime;
        /// <summary>The index in the array.</summary>
        short index;
        /// <summary>Tween mode.</summary>
        public LerpCoreType mode;
        /// <summary>Loop count.</summary>
        public byte loopCount;
        /// <summary>Elapsed loop counter.</summary>
        public byte loopCounter;
        /// <summary>Initial value.</summary>
        public Vector3 from => new Vector3(a, b, c);
        /// <summary>Target value.</summary>
        public Vector3 to => new Vector3(x, y, z);
        /// <summary>Initial rotation.</summary>
        public Quaternion fromRotation => _defaultFromRotation;
        /// <summary>RectTransform.</summary>
        public RectTransform rectTransform => transform as RectTransform;
        /// <summary>Boolean states.</summary>
        public BoolProperty states;
        /// <summary>Delta tick.</summary>
        public float tick
        {
            get
            {
                if (speed == 0)
                {
                    return CTEasing.Easing(ease, runningTime / duration);
                }
                else
                {
                    var to = 1f;

                    if (isFlipTick)
                    {
                        to = 0f;
                    }

                    runningSpeed = Mathf.MoveTowards(runningSpeed, to, speed / 3f * (!isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime));
                    return runningSpeed;
                }
            }
        }
        /// <summary>Sets default property.</summary>
        public void setDefault()
        {
            id = -1;
            ease = (byte)Ease.EaseLinear;
            runningTime = 0f;
            runningSpeed = 0f;
            duration = 0f;
            transform = null;
            loopCount = 0;
            loopCounter = 0;
            index = -1;
            mode = LerpCoreType.Position;
            _defaultFromRotation = Quaternion.identity;

            ClearFlags();
        }
        /// <summary>Sets from rotation.</summary>
        /// <param name="quaternion">Quaternion.</param>
        public void setFromRotation(Quaternion quaternion)
        {
            _defaultFromRotation = quaternion;
        }
        public Vector3 getInitialPosition => new Vector3(_defaultFromRotation.y, _defaultFromRotation.z, _defaultFromRotation.w);
        /// <summary>Sets from.</summary>
        public void setFrom(Vector3 from)
        {
            a = from.x;
            b = from.y;
            c = from.z;
        }
        /// <summary>Sets target value.</summary>
        public void setTo(Vector3 to)
        {
            x = to.x;
            y = to.y;
            z = to.z;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void OnComplete()
        {
            if (mode == LerpCoreType.Position || mode == LerpCoreType.Translate)
            {
                if (!isLocal)
                    transform.position = lerpVector3(!isFlipTick ? 1f : 0f);
                else
                    transform.localPosition = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Rotation)
            {
                if (!isLocal)
                    transform.rotation = lerpQuat(!isFlipTick ? 1f : 0f);
                else
                    transform.localRotation = lerpQuat(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Scale)
            {
                transform.localScale = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.SizeDelta)
            {
                rectTransform.sizeDelta = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Float)
            {
                delegates[index].Invoke(1, !isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.AnchoredPosition)
            {
                if (!isLocal)
                {
                    rectTransform.anchoredPosition = lerpVector3(!isFlipTick ? 1f : 0f);
                }
                else
                {
                    rectTransform.anchoredPosition3D = lerpVector3(!isFlipTick ? 1f : 0f);
                }
            }
            else if (mode == LerpCoreType.RotateAround)
            {
                if (!isLocal)
                {
                    transform.rotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, transform.TransformDirection(to).normalized);

                    if (isFlipTick)
                    {
                        transform.position = new Vector3(fromRotation.y, fromRotation.z, fromRotation.w);
                    }
                }
                else
                {
                    transform.rotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, transform.TransformDirection(to).normalized);
                }
            }
        }
        /// <summary>Invoked during loop cycle.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void OnLoop()
        {
            if (mode == LerpCoreType.Position || mode == LerpCoreType.Translate)
            {
                if (!isLocal)
                    transform.position = lerpVector3(!isFlipTick ? 1f : 0f);
                else
                    transform.localPosition = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Rotation)
            {
                if (!isLocal)
                    transform.rotation = lerpQuat(!isFlipTick ? 1f : 0f);
                else
                    transform.localRotation = lerpQuat(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Scale)
            {
                transform.localScale = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.Float)
            {
                delegates[index].Invoke(1, !isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.AnchoredPosition)
            {
                if (!isLocal)
                {
                    rectTransform.anchoredPosition = lerpVector3(!isFlipTick ? 1f : 0f);
                }
                else
                {
                    rectTransform.anchoredPosition3D = lerpVector3(!isFlipTick ? 1f : 0f);
                }
            }
            else if (mode == LerpCoreType.SizeDelta)
            {
                rectTransform.sizeDelta = lerpVector3(!isFlipTick ? 1f : 0f);
            }
            else if (mode == LerpCoreType.RotateAround)
            {
                if (!isLocal)
                {
                    transform.rotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, to.normalized);
                }
                else
                {
                    transform.localRotation = Quaternion.AngleAxis(!isFlipTick ? fromRotation.x : -fromRotation.x, to.normalized);
                }
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

            switch (mode)
            {
                case LerpCoreType.Position:
                    interpPosition();
                    break;
                case LerpCoreType.Rotation:
                    interpRotation();
                    break;
                case LerpCoreType.Scale:
                    interpScale();
                    break;
                case LerpCoreType.Float:
                    interpFloat();
                    break;
                case LerpCoreType.Translate:
                    interpPosition();
                    break;
                case LerpCoreType.SizeDelta:
                    interpScale2D();
                    break;
                case LerpCoreType.AnchoredPosition:
                    interpPosition2D();
                    break;
                case LerpCoreType.RotateAround:
                    interpRotateAround();
                    break;
            }

            //0 = tweening, 1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last)
            InvokeCallback(1);
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
        public void Cancel(bool invokeOnComplete = false)
        {
            Clear(invokeOnComplete);
        }
        /// <summary>Update the delta tick.</summary>
        void updateTime()
        {
            if (!isFlipTick)
            {
                runningTime += !isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime;
            }
            else
            {
                runningTime -= !isUnscaled ? Time.deltaTime : Time.unscaledDeltaTime;
            }
        }
        /// <summary>Updates the transform properties (from and to values or rotation).</summary>
        public void UpdateTransform()
        {
            if (mode == LerpCoreType.Position)
            {
                setFrom(!isLocal ? transform.position : transform.localPosition);
            }
            else if (mode == LerpCoreType.Scale)
            {
                setFrom(transform.localScale);
            }
        }
        /// <summary>Interpolates the position.</summary>
        public void interpPosition()
        {
            if (!onRepeat())
            {
                if (!isLocal)
                {
                    transform.position = lerpVector3(tick); //redirect the tick to easing function here.
                }
                else
                {
                    transform.localPosition = lerpVector3(tick);
                }
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Interpolates the position.</summary>
        public void interpPosition2D()
        {
            if (!onRepeat())
            {
                if (!isLocal)
                {
                    rectTransform.anchoredPosition = lerpVector3(tick); //redirect the tick to easing function here.
                }
                else
                {
                    rectTransform.anchoredPosition3D = lerpVector3(tick);
                }
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Interpolates the rotation.</summary>
        public void interpRotation()
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
            }
            else
            {
                Clear();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void interpRotateAround()
        {
            if (!onRepeat())
            {
                if (!isLocal)
                {

                    var angle = tick * fromRotation.x; // 360 degrees in the specified duration

                    Quaternion rotation = Quaternion.AngleAxis(angle, to);
                    Vector3 rotatedDirection = rotation * (getInitialPosition - from);
                    Vector3 newPosition = from + rotatedDirection;
                    transform.position = newPosition;
                    transform.rotation = rotation;

                }
                else
                {
                    var angle = tick * fromRotation.x; // 360 degrees in the specified duration

                    Quaternion rotation = Quaternion.AngleAxis(angle, to);
                    Vector3 rotatedDirection = rotation * (getInitialPosition - from);
                    Vector3 newPosition = from + rotatedDirection;
                    transform.localPosition = newPosition;
                    transform.localRotation = rotation;
                    /*
                    var agl = fromRotation.x;
                    float angle = agl / duration * Time.deltaTime * (!isFlipTick ? 1f : -1f);
                    float calc = angle * tick * 2f;

                    transform.RotateAround(transform.TransformPoint(from), transform.TransformDirection(to).normalized, calc);
                    */
                }
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Interpolates the scale.</summary>
        public void interpScale()
        {
            if (!onRepeat())
            {
                transform.localScale = lerpVector3(tick); //redirect the tick to easing function here.
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Interpolates the scale.</summary>
        public void interpScale2D()
        {
            if (!onRepeat())
            {
                rectTransform.sizeDelta = lerpVector3(tick); //redirect the tick to easing function here.
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Interpolates the float value.</summary>
        public void interpFloat()
        {
            if (!onRepeat())
            {
                //0 = tweening, 1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last)
                delegates[index].Invoke(1, lerpFloat(tick));
            }
            else
            {
                Clear();
            }
        }
        /// <summary>Finalizes the tween cycle.</summary>
        public void Clear(bool invokeOnComplete = false)
        {
            OnComplete();

            if (invokeOnComplete || !isCompleterepeat)
            {
                InvokeCallback(2);
            }

            //0 = tweening, 1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last)
            InvokeCallback(4);
            Remove(index);
        }
        /// <summary>Invoked at the end of loop cycle.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool onRepeat()
        {
            if (!isFlipTick)
            {
                if (speed == 0)
                {
                    if (runningTime < duration)
                    {
                        return false;
                    }
                }
                else
                {
                    if (runningSpeed < 1)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (speed == 0)
                {
                    if (runningTime > 0f)
                    {
                        return false;
                    }
                }
                else
                {
                    if (runningSpeed > 0f)
                    {
                        return false;
                    }
                }
            }

            OnLoop();

            if (loopCount > 0)
            {
                loopCounter++;

                if (!isPingpong)
                {
                    runningTime = 0f;
                    runningSpeed = 0f;

                    if (isCompleterepeat)
                    {
                        InvokeCallback(2);
                    }

                    if (isInfiniteLoop)
                    {
                        if (loopCounter == loopCount)
                        {
                            loopCounter = 0;
                            return false;
                        }
                    }

                    return loopCounter < loopCount ? false : true;
                }
                else
                {
                    if (isCompleterepeat && (loopCounter & 1) == 0)
                    {
                        //0 = tweening, 1 = update, 2 = oncomplete, 3 = on loop repeat cycle, 4 = on last complete (very last)
                        InvokeCallback(2);
                    }

                    if (loopCounter < loopCount * 2)
                    {
                        selectFliptick(!isFlipTick);
                        return false;
                    }

                    if (isInfiniteLoop)
                    {
                        loopCounter = 0;
                        selectFliptick(!isFlipTick);
                        return false;
                    }
                }
            }

            return true;
        }
        /// <summary>Interpolates Vector3s.</summary>
        /// <param name="tick">Delta tick.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 lerpVector3(float tick)
        {
            return new Vector3(a + (x - a) * tick, b + (y - b) * tick, c + (z - c) * tick);
        }
        /// <summary>Slerps two quaternions.</summary>
        /// <param name="tick">Delta tick.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion lerpQuat(float tick)
        {
            return Quaternion.SlerpUnclamped(fromRotation, fromRotation * Quaternion.AngleAxis(a, isLocal ? transform.InverseTransformDirection(to).normalized : to.normalized), tick);
        }
        /// <summary>Interpolates floating points.</summary>
        /// <param name="tick">Delta tick.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float lerpFloat(float tick) => a + (b - a) * tick;

        ////////////////////////////////////////////////////////////////////////
        //////////////// TWEEN STATES BIT WISE OPERATIONS HERE /////////////////
        ////////////////////////////////////////////////////////////////////////

        public bool isUnscaled => states.HasFlag(BoolProperty.Unscaledtime);
        public bool isPaused => states.HasFlag(BoolProperty.Pause);
        public bool isPingpong => states.HasFlag(BoolProperty.Pingpong);
        public bool isLocal => states.HasFlag(BoolProperty.Islocal);
        public bool isCompleterepeat => states.HasFlag(BoolProperty.Completerepeat);
        public bool isDelegateassigned => states.HasFlag(BoolProperty.Delegateassigned);
        public bool isInfiniteLoop => states.HasFlag(BoolProperty.Infiniteloop);
        public bool isFlipTick => states.HasFlag(BoolProperty.Fliptick);

        /////////////////// FLAG SETTER HERE ////////////////////
        public void selectUnscaledtime() => states |= BoolProperty.Unscaledtime;
        public void selectPingpong() => states |= BoolProperty.Pingpong;
        public void selectIslocal(bool islocal)
        {
            if (!islocal)
            {
                return;
            }

            states |= BoolProperty.Islocal;
        }
        public void selectFliptick(bool state)
        {
            if (!state)
                states &= ~BoolProperty.Fliptick;
            else
                states |= BoolProperty.Fliptick;
        }
        public void selectOncompleterepeat() => states |= BoolProperty.Completerepeat;
        public void selectPause(bool state)
        {
            if (!state)
            {
                states &= ~BoolProperty.Pause;
            }
            else
            {
                states |= BoolProperty.Pause;
                
            }
        }
        public void selectAssigneddelegate() => states |= BoolProperty.Delegateassigned;
        public void selectInfiniteloop() => states |= BoolProperty.Infiniteloop;

        void ClearFlags()
        {
            BoolProperty flagsToClear = BoolProperty.Pingpong | BoolProperty.Islocal | BoolProperty.Fliptick | BoolProperty.Completerepeat
            | BoolProperty.Pause | BoolProperty.Unscaledtime | BoolProperty.Delegateassigned | BoolProperty.Infiniteloop;

            states &= ~flagsToClear;
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
        Delegateassigned = 1 << 6,
        Infiniteloop = 1 << 7
    }
}
