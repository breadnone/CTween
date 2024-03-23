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
using System.Runtime.CompilerServices;
using CompactTween.Extension;
using UnityEngine.UIElements;
using TMPro;

namespace CompactTween
{
    // Clamping the lower bounds of easing functions sort of unnecessary when the value is 0 - 1 range.
    // instead we clamp the upperbound, so it won't miss the target or overshoot.

    // Example to use :
    // float runningTime = 0f;
    // float duration = 5f; //5 seconds to complete.
    // 
    // void Update()
    // {
    //      runningTime += Time.deltaTime;
    //      var normalizedTime = runningTime / duration;
    //      gameObject.transform.position = Vector.Lerp(from, to, STEasing.Easing(STEasing.Ease.EaseInOutQuad, normallizedTime));
    // }

    public static class CTEasing
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //All easings are expecting normalized (0 - 1) value as it's inputs.
        static float Linear(float val)
        {
            return 0 + (1 - 0) * clamp1(val);
        }
        static float EaseInSine(float val)
        {
            return 1 - Mathf.Cos(val * Mathf.PI * 0.5f);
        }
        static float EaseOutSine(float val)
        {
            return clamp1(Mathf.Sin(val * Mathf.PI * 0.5f));
        }
        static float EaseInOutSine(float val)
        {
            return clamp1(0.5f * (1 - Mathf.Cos(Mathf.PI * val)));
        }
        static float EaseInQuad(float val)
        {
            return val * val;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutQuad(float val)
        {
            float p0 = 1f - val;
            return clamp1(1f - p0 * p0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutQuad(float val)
        {
            return val < 0.5001f ? 2f * val * val : clamp1(1f - pow(-2f * val + 2f, 2) * 0.5f);
        }
        static float EaseInCubic(float val)
        {
            return val * val * val;
        }
        static float EaseOutCubic(float val)
        {
            val = 1f - val;
            return clamp1(1 - (val * val * val));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutCubic(float val)
        {
            return val < 0.5001f ? 4f * val * val * val : clamp1(1f - pow(-2f * val + 2f, 3) * 0.5f);
        }
        static float EaseInQuart(float val)
        {
            return val * val * val * val;
        }
        static float EaseOutQuart(float val)
        {
            val = 1f - val;
            return clamp1(1f - (val * val * val * val));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutQuart(float val)
        {
            return val < 0.5001f ? 8f * val * val * val * val : clamp1(1f - pow(-2f * val + 2f, 4) * 0.5f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInQuint(float val)
        {
            return val * val * val * val * val;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutQuint(float val)
        {
            val = 1f - val;
            return 1f - clamp1(val * val * val * val * val);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutQuint(float val)
        {
            return val < 0.5001f ? 16f * val * val * val * val * val : clamp1(1f - pow(-2f * val + 2f, 5) * 0.5f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInExpo(float val)
        {
            return val > 0f ? Mathf.Pow(2f, 10f * val - 10f) : 0f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutExpo(float val)
        {
            return val > 1f ? 1f : 1f - Mathf.Pow(2f, -10f * val);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutExpo(float val)
        {
            return val < 0f
            ? 0f
            : val > 1f
            ? 1f
            : val < 0.5001 ? Mathf.Pow(2f, 20f * val - 10f) * 0.5f
            : clamp1((2f - Mathf.Pow(2f, -20f * val + 10f)) * 0.5f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInCirc(float val)
        {
            return 1f - clamp1(Mathf.Sqrt(1f - (val * val)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutCirc(float val)
        {
            val = val - 1f;
            return clamp1(Mathf.Sqrt(1f - (val * val - 1f)));
        }
        //TODO : Surely, this is not accurate.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutCirc(float val)
        {
            val = val * 2f;

            if (val < 1f)
            {
                return 0.5f * (1f - Mathf.Sqrt(1f - val * val));
            }

            val -= 2f;
            return clamp1(0.5f * (Mathf.Sqrt(1f - val * val) + 1f));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInBack(float val)
        {
            return 2.70158f * val * val * val - 1.70158f * val * val;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutBack(float val)
        {
            float p0 = val - 1f;
            float p1 = p0 * p0;
            return 1f + 2.70158f * (p0 * p0 * p0) + 1.70158f * p1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutBack(float val)
        {
            const float c2 = 2.594909f;
            float p0 = val * 2f;
            float p1 = p0 - 2f;

            return val < 0.5001f
            ? p0 * p0 * ((c2 + 1f) * 2f * val - c2) * 0.5f
            : (p1 * p1 * ((c2 + 1f) * (p0 - 2f) + c2) + 2f) * 0.5f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInElastic(float val)
        {
            return val < 0f
            ? 0f
            : val > 1f
            ? 1f
            : -Mathf.Pow(2f, 10f * val - 10f) * Mathf.Sin((val * 10f - 10.75f) * 2.0943951023932f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutElastic(float val)
        {
            return val < 0f
            ? 0f
            : val > 1f
            ? 1f
            : Mathf.Pow(2f, -10f * val) * Mathf.Sin((val * 10f - 0.75f) * 2.0943951023932f) + 1f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutElastic(float val)
        {
            const float c5 = 1.39626340159546f;

            return val < 0f
            ? 0f
            : val > 1f
            ? 1f
            : val < 0.5001f
            ? -(Mathf.Pow(2f, 20f * val - 10f) * Mathf.Sin((20f * val - 11.125f) * c5)) * 0.5f
            : Mathf.Pow(2f, -20f * val + 10f) * Mathf.Sin((20f * val - 11.125f) * c5) * 0.5f + 1f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInBounce(float val)
        {
            return 1f - EaseOutBounce(1f - val);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseOutBounce(float val)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (val < 1f / d1)
            {
                return n1 * val * val;
            }
            else if (val < 2f / d1)
            {
                val -= 1.5f / d1;
                return n1 * val * val + 0.75f;
            }
            else if (val < 2.5f / d1)
            {
                val -= 2.25f / d1;
                return n1 * val * val + 0.9375f;
            }
            else
            {
                val -= 2.625f / d1;
                return n1 * val * val + 0.984375f;
            }
        }
        //TODO: This is very wrong.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInOutBounce(float val)
        {
            return val < 0.5001f
            ? (1f - EaseOutBounce(1f - 2f * val)) * 0.5f
            : (1f + EaseOutBounce(2f * val - 1f)) * 0.5f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float SpringIn(float val)
        {
            return val < 1.0001f ? val * val * ((1.70158f + 1) * val - 1.70158f) : 1f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float SpringOut(float val)
        {
            float p0 = val - 1f;
            return val < 1.0001f ? p0 * p0 * ((1.70158f + 1f) * p0 + 1.70158f) + 1f : 1f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float SpringInOut(float val)
        {
            const float c = 1.70158f;

            if (val < 0.5001)
            {
                float p0 = val * 2f;
                return p0 * p0 * ((c + 1f) * p0 - c);
            }
            else
            {
                val = (1f - val) / 2f;
                float p0 = val * 2f;
                return 1f - (p0 * p0 * ((c + 1f) * p0 - c));
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInWeightedOut(float val)
        {
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;
            return val < 0.5f ? 8f * val * val * val : (1f - 8f * (val - 1f) * (val - 1f) * 1.70158f * ((val - 0.5f) * (val - 0.5f)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float EaseInWeightedReboundOut(float val)
        {
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;
            return val < 0.5f ? 8f * val * val * val : (1f - 8f * (val - 1f) * (val - 1f) * 1.70158f * ((val - 0.5f) * (val - 0.75f)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float Bezier1D(float val)
        {
            float min = 0f;
            float max = 1f;
            float c = Mathf.Sin(0.5f * val);

            return clamp1(((1 - val) * (1 - val) * min) + (2 * val * (1 - val) * c) + (val * val * max));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float Bezier2DEaseFloatIn(float val)
        {
            float start = 0f;
            float end = 1f;
            float startMid = 0.25f * val;
            float endMid = 0.75f * -val;
            return clamp1(((1 - val) * (1 - val) * (1 - val) * start) + (3 * (1 - val) * (1 - val) * val * startMid) + (3 * (1 - val) * val * val * endMid) + (val * val * val * end));
        }
        static float Bezier2DEaseFloatOut(float val)
        {
            return clamp1(1f - Bezier2DEaseFloatIn(1f - val));
        }
        static float Bezier2DEaseFloatInOut(float val)
        {
            return val < 0.5001f
            ? (1f - Bezier2DEaseFloatOut(1f - 2f * val)) * 0.5f
            : (1f + Bezier2DEaseFloatOut(2f * val - 1f)) * 0.5f;
        }
        static float Bezier2DEaseBrakeInOut(float val)
        {
            return val < 0.5001f
            ? (1f - Bezier2DEaseFloatIn(1f - 2f * val)) * 0.5f
            : (1f + Bezier2DEaseFloatIn(2f * val - 1f)) * 0.5f;
        }
        /// <summary>
        /// Upperbound clamping.
        /// </summary>
        /// <param name="upperbound"></param>
        /// <returns></returns>
        static float clamp1(float upperbound)
        {
            return upperbound < 1 ? upperbound : 1f;
        }
        //less than 10 this is faster
        static float pow(float input, int length)
        {
            float result = input;
            var len = length - 1;
            for (int i = 0; i < len; i++) result *= input;
            return result;
        }
        public static float Easing(byte ease, float tick)
        {
            switch (ease)
            {
                case 0:
                    return Linear(tick);
                case 1:
                    return EaseInQuad(tick);
                case 2:
                    return EaseOutQuad(tick);
                case 3:
                    return EaseInOutQuad(tick);
                case 4:
                    return EaseInCubic(tick);
                case 5:
                    return EaseOutCubic(tick);
                case 6:
                    return EaseInOutCubic(tick);
                case 7:
                    return EaseInQuart(tick);
                case 8:
                    return EaseOutQuart(tick);
                case 9:
                    return EaseInOutQuart(tick);
                case 10:
                    return EaseInQuint(tick);
                case 11:
                    return EaseOutQuint(tick);
                case 12:
                    return EaseInOutQuint(tick);
                case 13:
                    return EaseInSine(tick);
                case 14:
                    return EaseOutSine(tick);
                case 15:
                    return EaseInOutSine(tick);
                case 16:
                    return EaseInExpo(tick);
                case 17:
                    return EaseOutExpo(tick);
                case 18:
                    return EaseInOutExpo(tick);
                case 19:
                    return EaseInCirc(tick);
                case 20:
                    return EaseOutCirc(tick);
                case 21:
                    return EaseInOutCirc(tick);
                case 22:
                    return SpringIn(tick);
                case 23:
                    return SpringOut(tick);
                case 24:
                    return SpringInOut(tick);
                case 25:
                    return EaseInBounce(tick);
                case 26:
                    return EaseOutBounce(tick);
                case 27:
                    return EaseInOutBounce(tick);
                case 28:
                    return EaseInBack(tick);
                case 29:
                    return EaseOutBack(tick);
                case 30:
                    return EaseInOutBack(tick);
                case 31:
                    return EaseInElastic(tick);
                case 32:
                    return EaseOutElastic(tick);
                case 33:
                    return EaseInOutElastic(tick);
                case 34:
                    return EaseInWeightedOut(tick);
                case 35:
                    return EaseInWeightedReboundOut(tick);
                case 36:
                    return Bezier1D(tick);
                case 37:
                    return Bezier2DEaseFloatIn(tick);
                case 38:
                    return Bezier2DEaseFloatOut(tick);
                case 39:
                    return Bezier2DEaseFloatInOut(tick);
                case 40:
                    return Bezier2DEaseBrakeInOut(tick);
            }
            return 0;
        }

    }
    /// <summary>Easing functions.</summary>
    public enum Ease
    {
        EaseLinear = 0,
        EaseInQuad = 1,
        EaseOutQuad = 2,
        EaseInOutQuad = 3,
        EaseInCubic = 4,
        EaseOutCubic = 5,
        EaseInOutCubic = 6,
        EaseInQuart = 7,
        EaseOutQuart = 8,
        EaseInOutQuart = 9,
        EaseInQuint = 10,
        EaseOutQuint = 11,
        EaseInOutQuint = 12,
        EaseInSine = 13,
        EaseOutSine = 14,
        EaseInOutSine = 15,
        EaseInExpo = 16,
        EaseOutExpo = 17,
        EaseInOutExpo = 18,
        EaseInCirc = 19,
        EaseOutCirc = 20,
        EaseInOutCirc = 21,
        SpringIn = 22,
        SpringOut = 23,
        SpringInOut = 24,
        EaseInBounce = 25,
        EaseOutBounce = 26,
        EaseInOutBounce = 27,
        EaseInBack = 28,
        EaseOutBack = 29,
        EaseInOutBack = 30,
        EaseInElastic = 31,
        EaseOutElastic = 32,
        EaseInOutElastic = 33,
        EaseInWeightedOut = 34,
        EaseInWeightedReboundOut = 35,
        Bezier1D = 36,
        Bezier2DEaseFloatIn = 37,
        Bezier2DEaseFloatOut = 38,
        Bezier2DEaseFloatInOut = 39,
        Bezier2DEaseBrakeInOut = 40,
        PingPong = 41,
        Punch,
        Shake
    }
    /// <summary>Main static class.</summary>
    public static class ct
    {
        /// <summary>Moves gameObject.</summary>
        /// <param name="gameObject">GameObject.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctMove(this GameObject gameObject, Vector3 to, float duration)
        {
            return CTween.move(gameObject.transform, to, duration);
        }
        /// <summary>Rotates a gameObject.</summary>
        /// <param name="gameObject">GameObject.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctRotate(this GameObject gameObject, Vector3 direction, float angle, float duration)
        {
            return CTween.rotate(gameObject.transform, direction, angle, duration);
        }
        /// <summary>Scales the gameObject.</summary>
        /// <param name="gameObject">GameObject.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctScale(this GameObject gameObject, Vector3 to, float duration)
        {
            return CTween.scale(gameObject.transform, to, duration);
        }
        /// <summary>Moves a visualElement.[</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctMove(this VisualElement visualElement, Vector3 to, float duration)
        {
            return CTween.move(visualElement, to, duration);
        }
        /// <summary>Scales a VisualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctScale(this VisualElement visualElement, Vector3 to, float duration)
        {
            return CTween.scale(visualElement, to, duration);
        }
        /// <summary>Rotates a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctRotate(this VisualElement visualElement, float to, float duration)
        {
            return CTween.rotate(visualElement, to, duration);
        }
        /// <summary>Interpolates a float value.</summary>
        /// <param name="text">TMP_Text component.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctFloat(this TMP_Text text, float from, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(text.GetInstanceID(), from, to, duration, tick =>
            {
                text.SetText(tick.ToString());

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates float.</summary>
        /// <param name="text">TextField element.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctFloat(this TextField text, float from, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(text.GetHashCode(), from, to, duration, tick =>
            {
                text.value = tick.ToString();

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates interger.</summary>
        /// <param name="text">TMP_Text component.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctInt(this TMP_Text text, float from, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(text.GetInstanceID(), from, to, duration, tick =>
            {
                var ints = (int)tick;
                text.SetText(ints.ToString());

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates integer.</summary>
        /// <param name="text">TextField component.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctInt(this TextField text, float from, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(text.GetHashCode(), from, to, duration, tick =>
            {
                var ints = (int)tick;
                text.value = ints.ToString();

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Resizes the width of a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="percent">Percent based value. 100 is the max value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctWidth(this VisualElement visualElement, float percent, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), visualElement.resolvedStyle.width, percent, duration, tick =>
            {
                visualElement.style.width = Length.Percent(tick);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Resizes the height of a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="percent">Percent based value. 100 is the max value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctHeight(this VisualElement visualElement, float percent, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), visualElement.resolvedStyle.width, percent, duration, tick =>
            {
                visualElement.style.height = Length.Percent(tick);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Fades in/out the opacity of a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="percent">Percent based value. 100 is the max value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctAlpha(this VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), visualElement.resolvedStyle.opacity, to, duration, tick =>
            {
                visualElement.style.opacity = tick;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Color shifts the backgroundColor of a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween ctColor(this VisualElement visualElement, Color to, float duration)
        {
            return CTween.color(visualElement, to, duration);
        }
        /// <summary>Punch effects.</summary>
        /// <param name="gameObject">GameObject.</param>
        /// <param name="power">Power.</param>
        /// <param name="duration">Duration.</param>
        /// <returns></returns>
        public static CoreTween ctPunch2D(this GameObject gameObject, float power, float duration)
        {
            var dummy = new CoreTween();

            var defpos = gameObject.transform.localPosition;
            var defrot = gameObject.transform.localRotation;

            var pos = new Vector3(defpos.x, defpos.y + Mathf.Pow(6f, power), defpos.z);

            var sca = CTween.scale(gameObject.transform, new Vector3(gameObject.transform.localScale.x * (power * 0.65f), gameObject.transform.localScale.y * (power * 0.65f), gameObject.transform.localScale.z * (power * 0.65f)), duration * 0.3f).onLoopCount(1).onPingPong(true).onEase(Ease.EaseInOutBounce);
            var rotinit = CTween.rotateLocal(gameObject.transform, Vector3.forward, -15f, duration * 0.3f).onPingPong(true).onLoopCount(1).onEase(Ease.EaseInOutQuad);
            var rot = CTween.rotateLocal(gameObject.transform, Vector3.forward, 15f, duration * 0.3f).onLoopCount(1).onPingPong(true).onEase(Ease.EaseInOutQuad).halt(true);
            var mov = CTween.moveLocal(gameObject.transform, pos, duration * 0.35f).onPingPong(true).onLoopCount(1);

            CTcore.RegisterLastOnComplete(rotinit.index, () =>
            {
                rot.halt(false);
            });

            mov.getTween.setFrom(defpos);
            dummy.index = mov.index;
            return dummy;
        }
        public static CoreTween ctShake(this GameObject gameObject, float power, float magnitude, float duration)
        {
            var dummy = new CoreTween();
            var transform = gameObject.transform;
            var defpos = transform.localPosition;
            var defrot = transform.localRotation;

            Vector3 maximumTranslationShake = Vector3.one;
            Vector3 maximumAngularShake = Vector3.one * 25;
            float frequency = magnitude;
            float traumaExponent = 1;
            float recoverySpeed = 1;
            float trauma = power;
            float seed = Random.value;

            CTcore.InstantiateFloat(transform.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                float shake = Mathf.Pow(trauma, traumaExponent);

                var posOffset = new Vector3(
                    maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
                    maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
                    maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
                ) * shake;

                var rotOffset = Quaternion.Euler(new Vector3(
                    maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
                    maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
                    maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
                ) * shake);

                // Apply the offset relative to the original position and rotation
                //transform.localPosition = defpos + posOffset;
                //transform.localRotation = defrot * rotOffset;
                transform.SetLocalPositionAndRotation(defpos + posOffset, defrot * rotOffset);
                trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);

            }, out int index);

            CTcore.RegisterLastOnComplete(index, () => transform.SetLocalPositionAndRotation(defpos, defrot));
            dummy.index = index;
            return dummy;
        }
        public static CoreTween ctAnim(this RectTransform rectTransform, Sprite[] sprites, int framePerSecond)
        {
            var dummy = new CoreTween();
            var fps = 1 / 60f * framePerSecond;

            CTcore.InstantiateFloat(rectTransform.gameObject.GetInstanceID(), 0f, 1000f, float.PositiveInfinity, x=>{}, out int index);
            dummy.index = index;

            var img = rectTransform.gameObject.GetComponent<UnityEngine.UI.Image>();
            float lastFrame = 0f;
            bool flip = false;
            int loopCounter = 0;
            int lastIndex = 0;
            bool complete = false;

            CTcore.RegisterOnUpdate(dummy.index, (tick =>
            {
                if(complete)
                {
                    dummy.Cancel();
                    return;
                }

                int start = 0;
                int end = sprites.Length;

                if (dummy.getTween.isFlipTick)
                {
                    start = sprites.Length;
                    end = 0;
                }

                bool last = false;

                if (!flip)
                {
                    var t = Time.realtimeSinceStartup;

                    if (lastFrame + fps < t)
                    {
                        img.sprite = sprites[lastIndex];
                        lastFrame = t;

                        if (lastIndex == sprites.Length - 1)
                        {
                            last = true;
                        }
                        else
                        {
                            lastIndex++;
                        }
                    }

                }
                else
                {
                    var t = Time.realtimeSinceStartup;
                    
                    if (lastFrame + fps < t)
                    {
                        img.sprite = sprites[lastIndex];
                        lastFrame = t;

                        if (lastIndex == 0)
                        {
                            last = true;
                        }
                        else
                        {
                            lastIndex--;
                        }
                    }
                }

                bool ispingpong = dummy.getTween.isPingpong;

                if (dummy.getTween.loopCount > 0 && last)
                {
                    loopCounter++;

                    if (ispingpong)
                    {
                        flip = !flip;

                        if(loopCounter == dummy.getTween.loopCount * 2)
                        {
                            complete = true;
                            return;
                        }
                        else
                        {
                            if(dummy.getTween.isCompleterepeat && loopCounter % 2 == 0)
                            {
                                CTcore.ctobjects[index].invoke.Invoke(2, 0f);
                            }
                        }
                    }
                    else
                    {
                        lastIndex = 0;

                        if(loopCounter == dummy.getTween.loopCount)
                        {
                            complete = true;
                            return;
                        }
                        else
                        {
                            if(dummy.getTween.isCompleterepeat && loopCounter % 2 == 0)
                            {
                                CTcore.ctobjects[index].invoke.Invoke(2, 0f);
                            }
                        }
                    }
                }
            }));

            return dummy;
        }
        
    }
}