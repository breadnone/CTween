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
using CompactTween.Extension;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.UIElements;
using System.Threading.Tasks;

namespace CompactTween
{
    /// <summary>Static extension class.</summary>
    public static class CTween
    {
        static short shortCounter = short.MinValue;
        /// <summary>Quick ids.</summary>
        public static short quickShortId
        {
            get
            {
                shortCounter++;
                return shortCounter;
            }
        }
        /// <summary>Moves the visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween move(VisualElement visualElement, Vector3 to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                var from = visualElement.resolvedStyle.translate;
                var val = Vector3.LerpUnclamped(from, to, tick);
                visualElement.style.translate = new Translate(val.x, val.y, val.z);

            }, out int index);

            dummy.index = index;
            return dummy;
        }

        /// <summary>Moves a visualElement along x axis.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveX(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();
            var from = visualElement.resolvedStyle.translate;

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                var pos = new Vector3(visualElement.resolvedStyle.translate.x + to, visualElement.resolvedStyle.translate.y, visualElement.resolvedStyle.translate.z);
                var val = Vector3.LerpUnclamped(from, pos, tick);
                visualElement.style.translate = new Translate(val.x, val.y, val.z);
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves visualElement along y axis.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveY(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();
            var from = visualElement.resolvedStyle.translate;

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                var pos = new Vector3(visualElement.resolvedStyle.translate.x, visualElement.resolvedStyle.translate.y + to, visualElement.resolvedStyle.translate.z);
                var val = Vector3.LerpUnclamped(from, pos, tick);
                visualElement.style.translate = new Translate(val.x, val.y, val.z);
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the opacity of a visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween alpha(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateFloat(visualElement.GetHashCode(), visualElement.resolvedStyle.opacity, to, duration, value =>
            {
                visualElement.style.opacity = value;
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales the visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scale(VisualElement visualElement, Vector2 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                visualElement.style.scale = new Scale(Vector2.LerpUnclamped(visualElement.resolvedStyle.scale.value, to, tick));
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the width based on the percentage.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween width(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();
            var from = Length.Percent(visualElement.resolvedStyle.width);
            CTcore.InstantiateFloat(visualElement.GetHashCode(), from.value, to, duration, value =>
            {
                visualElement.style.width = Length.Percent(value);
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the height based on the percentage.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween height(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CoreTween();
            var heg = Length.Percent(visualElement.resolvedStyle.height);

            CTcore.InstantiateFloat(visualElement.GetHashCode(), heg.value, to, duration, value =>
            {
                visualElement.style.height = value;
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween move(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();

            if(transform.parent != null)
            {
                to = transform.parent.InverseTransformPoint(to);
            }
            
            CTcore.InstantiateVector(transform, transform.localPosition, to, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the gameObject in world space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween move(GameObject gameObject, Vector3 to, float duration)
        {
            var dummy = new CoreTween();

            if(gameObject.transform.parent != null)
            {
                to = gameObject.transform.parent.InverseTransformPoint(to);
            }
            
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localPosition, to, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform along x axis in world space.</summary>
        /// <param name="transform">The transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveX(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            var rto = new Vector3(to, pos.y, pos.z);

            if(transform.parent != null)
            {
                rto = transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the gameObject along x axis in world space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveX(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;
            var rto = new Vector3(to, pos.y, pos.z);

            if(gameObject.transform.parent != null)
            {
                rto = gameObject.transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(gameObject.transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along y axis in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveY(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            var rto = new Vector3(pos.x, to, pos.z);

            if(transform.parent != null)
            {
                rto = transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves gameObject along y axis in world space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveY(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;
            var rto = new Vector3(pos.x, to, pos.z);

            if(gameObject.transform.parent != null)
            {
                rto = gameObject.transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(gameObject.transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along z axis in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveZ(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            var rto = new Vector3(pos.x, pos.y, to);

            if(transform.parent != null)
            {
                rto = transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves gameObject along z axis in world space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveZ(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;
            var rto = new Vector3(pos.x, pos.y, to);

            if(gameObject.transform.parent != null)
            {
                rto = gameObject.transform.parent.InverseTransformPoint(rto);
            }
            
            CTcore.InstantiateVector(gameObject.transform, pos, rto, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocal(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, transform.localPosition, to, duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the gameObject in local space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocal(GameObject gameObject, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localPosition, to, duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along x axis in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Target destinatijon.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalX(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(to, pos.y, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves gameobject's x axis in local space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Target destinatijon.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalX(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;            
            CTcore.InstantiateVector(gameObject.transform, pos, new Vector3(to, pos.y, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along y axis in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalY(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, to, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves gameObject along y axis in local space.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalY(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;
            CTcore.InstantiateVector(gameObject.transform, pos, new Vector3(pos.x, to, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along z axis in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">To target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalZ(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, pos.y, to), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves gameObject along z axis.</summary>
        /// <param name="gameObject">GameObject to move.</param>
        /// <param name="to">To target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalZ(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = gameObject.transform.localPosition;
            CTcore.InstantiateVector(gameObject.transform, pos, new Vector3(pos.x, pos.y, to), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween move(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, to, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's x axis in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveX(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = new Vector3(to, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's y axis in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveY(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = new Vector3(rectTransform.anchoredPosition3D.x, to, rectTransform.anchoredPosition3D.z);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's z axis in world spcae.</summary>
        /// <param name="rectTransform">RectTransform to move.[</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveZ(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, to);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's x axis in local space.[]</summary>
        /// <param name="rectTransform">RectTranform top move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duhration.</param>
        public static CoreTween moveLocalX(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = new Vector2(to, rectTransform.anchoredPosition.y);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, pos, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's y axis in local space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocalY(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CoreTween();
            var pos = new Vector2(rectTransform.anchoredPosition.x, to);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, pos, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in local space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween moveLocal(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, to, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAround(Transform transform, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, direction.normalized, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around right direction the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundX(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.right, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around up direction the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundY(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.up, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around forward direction the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundZ(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.forward, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAround(GameObject gameObject, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, direction.normalized, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around right direction the transform in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundX(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.right, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around up direction the transform in world space.</summary>
        /// <param name="gameObject">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundY(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.up, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around right direction the transform in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundZ(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.forward, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis the transform in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocal(Transform transform, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, direction.normalized, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around right direction in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalX(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.right, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around up direction in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalY(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.up, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around forward direction in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalZ(Transform transform, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(transform, target, Vector3.forward, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis in local space.</summary>
        /// <param name="GameObject">GameObject to rotate..</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocal(GameObject gameObject, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, direction.normalized, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around right direction in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalX(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.right, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around up direction in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalY(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.up, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around forward direction in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="target">Pivot point.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateAroundLocalZ(GameObject gameObject, Vector3 target, float angle, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateRotateAround(gameObject.transform, target, Vector3.forward, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotate(VisualElement visualElement, float angle, float duration)
        {
            var dummy = new CoreTween();
            var from = visualElement.resolvedStyle.rotate.angle.value;

            CTcore.InstantiateFloat(visualElement.GetHashCode(), from, angle, duration, tick =>
            {
                visualElement.style.rotate = new Rotate(Angle.Degrees(tick));

            }, out int index);

            dummy.index = index;
            return dummy;
        }

        /// <summary>Rotates a transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotate(Transform transform, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, transform.parent != null ? transform.parent.InverseTransformDirection(direction).normalized : direction.normalized);
            CTcore.InstantiateQuat(transform, transform.localRotation * (Quaternion.Inverse(transform.parent != null ? transform.parent.rotation : transform.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a transform along x axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateX(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, transform.parent != null ? transform.parent.InverseTransformDirection(Vector3.right).normalized : Vector3.right);
            CTcore.InstantiateQuat(transform, transform.localRotation * (Quaternion.Inverse(transform.parent != null ? transform.parent.rotation : transform.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        
        /// <summary>Rotates a transform along y axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateY(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, transform.parent != null ? transform.parent.InverseTransformDirection(Vector3.up).normalized : Vector3.up);
            CTcore.InstantiateQuat(transform, transform.localRotation * (Quaternion.Inverse(transform.parent != null ? transform.parent.rotation : transform.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }

        
        /// <summary>Rotates a transform along z axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateZ(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, transform.parent != null ? transform.parent.InverseTransformDirection(Vector3.forward).normalized : Vector3.forward);
            CTcore.InstantiateQuat(transform, transform.localRotation * (Quaternion.Inverse(transform.parent != null ? transform.parent.rotation : transform.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotate(GameObject gameObject, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, gameObject.transform.parent != null ? gameObject.transform.parent.InverseTransformDirection(direction).normalized : direction.normalized);
            CTcore.InstantiateQuat(gameObject.transform, gameObject.transform.localRotation * (Quaternion.Inverse(gameObject.transform.parent.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along x axis in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateX(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, gameObject.transform.parent != null ? gameObject.transform.parent.InverseTransformDirection(Vector3.right).normalized : Vector3.right);
            CTcore.InstantiateQuat(gameObject.transform, gameObject.transform.localRotation * (Quaternion.Inverse(gameObject.transform.parent.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along y axis in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateY(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, gameObject.transform.parent != null ? gameObject.transform.parent.InverseTransformDirection(Vector3.up).normalized : Vector3.up);
            CTcore.InstantiateQuat(gameObject.transform, gameObject.transform.localRotation * (Quaternion.Inverse(gameObject.transform.parent.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along x axis in world space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateZ(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, gameObject.transform.parent != null ? gameObject.transform.parent.InverseTransformDirection(Vector3.forward).normalized : Vector3.forward);
            CTcore.InstantiateQuat(gameObject.transform, gameObject.transform.localRotation * (Quaternion.Inverse(gameObject.transform.parent.rotation) * quat), duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates the transform in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocal(Transform transform, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, direction.normalized);
            CTcore.InstantiateQuat(transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotate transform along x axis in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalX(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.right);
            CTcore.InstantiateQuat(transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates along y axis in local space.</summary>
        /// <param name="transform">Transform to move.[</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalY(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.up);
            CTcore.InstantiateQuat(transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotate transform along z axis in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalZ(Transform transform, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.forward);
            CTcore.InstantiateQuat(transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocal(GameObject gameObject, Vector3 direction, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, direction.normalized);
            CTcore.InstantiateQuat(gameObject.transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along x axis in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalX(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.right);
            CTcore.InstantiateQuat(gameObject.transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along y axis in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalY(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.up);
            CTcore.InstantiateQuat(gameObject.transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a gameObject along x axis in local space.</summary>
        /// <param name="gameObject">GameObject to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween rotateLocalZ(GameObject gameObject, float angle, float duration)
        {
            var dummy = new CoreTween();
            var quat = Quaternion.AngleAxis(angle, Vector3.forward);
            CTcore.InstantiateQuat(gameObject.transform, quat, duration, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scale(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, transform.localScale, to, duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a gameObject.</summary>
        /// <param name="gameObject">GameObject to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scale(GameObject gameObject, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localScale, to, duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleX(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(to, transform.localScale.y, transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleY(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(transform.localScale.x, to, transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleZ(Transform transform, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, to), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a gameObject's x axis.</summary>
        /// <param name="gameObject">GameObject to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleX(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localScale, new Vector3(to, gameObject.transform.localScale.y, gameObject.transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a gameObject's y axis.</summary>
        /// <param name="gameObject">GameObject to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleY(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localScale, new Vector3(gameObject.transform.localScale.x, to, gameObject.transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a gameObject's z axis.</summary>
        /// <param name="gameObject">GameObject to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween scaleZ(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.localScale, new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, to), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates float value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(float from, float to, float duration, Action<float> callback)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateFloat(dummy.GetHashCode(), from, to, duration, callback, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Unlike ".value", this is the normalized 0-1 range version. The input parameter will output 0 - 1 float value. \nWhen chained it will use the duration/time from the first tween instance.</summary>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        /// <returns></returns>
        public static CoreTween qvalue(float duration, Action<float> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(value);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Chaining multiple values at once (not queueing). Duration/time will be taken from the 1st tween instance in this sequence.</summary>
        /// <param name="callback">Callback.</param>
        /// <returns></returns>
        public static CoreTween qvalue(this CoreTween ct, Action<float> callback)
        {
            if(ct.getTween.mode != LerpCoreType.Float)
            {
                throw new Exception("CTween error : Qvalue can't be chained with different instances that's not instantiated via CTween.qvalue.");
            }

            CTcore.RegisterOnUpdate(ct.index, callback);
            return ct;
        }
        /// <summary>Interpolates Matrix4x4 value.</summary>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Matrix4x4 from, Matrix4x4 to, float duration, Action<Matrix4x4> callback)
        {
            var dummy = new CoreTween();
            Matrix4x4 result = new Matrix4x4();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, tick =>
            {
                for (int i = 0; i < 16; i++)
                {
                    result[i] = Mathf.LerpUnclamped(from[i], to[i], tick);
                }

                callback.Invoke(result);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates quaternion value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Quaternion from, Quaternion to, float duration, Action<Quaternion> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, tick =>
            {
                callback.Invoke(Quaternion.SlerpUnclamped(from, to, tick));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector3 value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Vector3 from, Vector3 to, float duration, Action<Vector3> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector3.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector4 value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Vector4 from, Vector4 to, float duration, Action<Vector4> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector4.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector2 value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Vector2 from, Vector2 to, float duration, Action<Vector2> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector2.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates integer value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(int from, int to, float duration, Action<int> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), (float)from, (float)to, duration, (float value) =>
            {
                callback.Invoke((int)value);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates color value.</summary>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CoreTween value(Color from, Color to, float duration, Action<Color> callback)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(dummy.GetHashCode(), 0, 1f, duration, value =>
            {
                var color = ColorShift(from, to);
                var col = Vector3.LerpUnclamped(color.start, color.end, value);
                callback.Invoke(new Color(col.x, col.y, col.z));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates to the direction of translation.</summary>
        /// <param name="transform">Transform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="isLocal">Locality.</param>
        public static CoreTween translate(Transform transform, Vector3 to, float duration, bool isLocal)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(transform, !isLocal ? transform.position : transform.localPosition, to, duration, LerpCoreType.Translate, isLocal, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates to the direction of translation.</summary>
        /// <param name="gameObject">Transform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="isLocal">Locality.</param>
        public static CoreTween translate(GameObject gameObject, Vector3 to, float duration, bool isLocal)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(gameObject.transform, gameObject.transform.position, to, duration, LerpCoreType.Translate, isLocal, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Resizes the deltaSize of a rectTransform.</summary>
        /// <param name="rectTransform">RectTransform to resize.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween sizeDelta(RectTransform rectTransform, Vector2 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.sizeDelta, to, duration, LerpCoreType.SizeDelta, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the size relative to the anchored point.</summary>
        /// <param name="rectTransform">RectTransform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween anchored(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, to, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the size relative to the anchored point.</summary>
        /// <param name="rectTransform">RectTransform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween anchored3D(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, to, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the alpha value of a CanvasGroup.</summary>
        /// <param name="canvasGroup">The canvasGroup component.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween alpha(CanvasGroup canvasGroup, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(canvasGroup.gameObject.GetInstanceID(), canvasGroup.alpha, to, duration, (float value) =>
            {
                canvasGroup.alpha = value;
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Fades in/out an alpha value of a ui.</summary>
        /// <param name="rectTransform">RectTransform that contains image component.</param>
        /// <param name="to">Target value</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween alpha(RectTransform rectTransform, float to, float duration)
        {
            var img = rectTransform.gameObject.GetComponent<UnityEngine.UI.Image>();
            var dummy = new CoreTween();

            if (img == null)
            {
                throw new Exception("CTween error : No ui image component can be found in the rectTransform");
            }

            CTcore.InstantiateFloat(img.gameObject.GetInstanceID(), img.color.a, to, duration, (float value) =>
            {
                var col = img.color;
                col.a = value;
                img.color = col;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Fades in/out an alpha value of a ui.</summary>
        /// <param name="image">Image component.</param>
        /// <param name="to">Target value</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween alpha(UnityEngine.UI.Image image, float to, float duration)
        {
            var dummy = new CoreTween();

            if (image == null)
            {
                throw new Exception("CTween error : No ui image component can be found in the rectTransform");
            }

            CTcore.InstantiateFloat(image.gameObject.GetInstanceID(), image.color.a, to, duration, (float value) =>
            {
                var col = image.color;
                col.a = value;
                image.color = col;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Shifts between two colors.</summary>
        /// <param name="image">Image component.</param>
        /// <param name="from">Initial color.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween color(UnityEngine.UI.Image image, Color to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(image.gameObject.GetInstanceID(), 0, 1f, duration, value =>
            {
                var color = ColorShift(image.color, to);
                var col = Vector3.LerpUnclamped(color.start, color.end, value);
                image.color = new Color(col.x, col.y, col.z);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Shifts between two colors.</summary>
        /// <param name="rectTransform">RectTransform that containes image component.</param>
        /// <param name="from">Initial color.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween color(RectTransform rectTransform, Color to, float duration)
        {
            var img = rectTransform.GetComponent<UnityEngine.UI.Image>();

            var dummy = new CoreTween();

            if (img == null)
            {
                throw new Exception("CTween error : No ui image component can be found in the rectTransform.");
            }

            CTcore.InstantiateFloat(rectTransform.gameObject.GetInstanceID(), 0, 1f, duration, value =>
            {
                var color = ColorShift(img.color, to);
                var col = Vector3.LerpUnclamped(color.start, color.end, value);
                img.color = new Color(col.x, col.y, col.z);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Shifts between two colors.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="from">Initial color.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween color(VisualElement visualElement, Color to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1f, duration, value =>
            {
                var color = ColorShift(visualElement.resolvedStyle.backgroundColor, to);
                var col = Vector3.LerpUnclamped(color.start, color.end, value);
                visualElement.style.backgroundColor = new Color(col.x, col.y, col.z);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Vector3 start, Vector3 end) ColorShift(Color a, Color targetColor)
        {
            Color.RGBToHSV(a, out var h, out var s, out var v);
            Color.RGBToHSV(targetColor, out var hh, out var ss, out var vv);
            var vecStart = new Vector3(h, s, v);
            var vecEnd = new Vector3(hh, ss, vv);
            return (vecStart, vecEnd);
        }

        /// <summary>Interpolates AudioSource volume value.</summary>
        /// <param name="audioSource">The audioSource component.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween volume(AudioSource audioSource, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(audioSource.gameObject.GetInstanceID(), audioSource.volume, to, duration, (float value) =>
            {
                audioSource.volume = value;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates AudioListener value.</summary>
        /// <param name="gameObject">The gameObject that contains audioListener component in it.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween globalAudio(GameObject gameObject, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(gameObject.GetInstanceID(), AudioListener.volume, to, duration, (float value) =>
            {
                AudioListener.volume = value;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates floating point value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderFloat(Material material, string propertyName, float to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(material.GetInstanceID(), material.GetFloat(propertyName), to, duration, tick =>
            {
                material.SetFloat(propertyName, tick);
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates integer value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderInt(Material material, string propertyName, int to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(material.GetInstanceID(), material.GetInt(propertyName), to, duration, tick =>
            {
                material.SetFloat(propertyName, (int)tick);
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector2 value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderVector2(Material material, string propertyName, Vector2 to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(material.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                var tmp = material.GetVector(propertyName);
                material.SetVector(propertyName, ((Vector2)tmp + (Vector2)to) * tick);
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector3 value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderVector3(Material material, string propertyName, Vector3 to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(material.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                var tmp = material.GetVector(propertyName);
                material.SetVector(propertyName, ((Vector3)tmp + (Vector3)to) * tick);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector4 value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderVector4(Material material, string propertyName, Vector4 to, float duration)
        {
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(material.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                var tmp = material.GetVector(propertyName);
                material.SetVector(propertyName, ((Vector4)tmp + (Vector4)to) * tick);
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates material Color value.</summary>
        /// <param name="material">Material.</param>
        /// <param name="propertyName">Exposed shader property name.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CoreTween shaderColor(Material material, string propertyName, Color to, float duration)
        {
            var dummy = new CoreTween();
            var tmp = material.GetColor(propertyName);
            var col = ColorShift(tmp, to);

            CTcore.InstantiateFloat(material.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                material.SetColor(propertyName, Color.LerpUnclamped(new Color(col.start.x, col.start.y, col.start.z), new Color(col.end.x, col.end.y, col.end.z), tick));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Object following.</summary>
        /// <param name="transform">Transform.</param>
        /// <param name="target">Target object to follow.</param>
        /// <param name="smoothTime">Ease time.</param>
        /// <param name="velocity"></param>
        public static CoreTween follow(Transform transform, Transform target, float smoothTime, Vector3 velocity)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateFollow(transform, target, smoothTime, velocity, out int index);

            dummy.index = index;
            return dummy;
        }

        ///////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////// EXTENSIONS HERE ////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the max loop cycle for the tween to finish.</summary>
        /// <param name="loopCount">Loop count.</param>
        public static CoreTween onLoopCount(in this CoreTween ct, int loopCount)
        {
            if (loopCount > 255 || loopCount < 0)
            {
                throw new Exception("CTween error : Loop count max is 0 - 255.");
            }

            ct.getTween.setLoopCount((byte)loopCount);
            return ct;
        }
        /// <summary>Smooth pinpong like loop cycle.</summary>
        /// <param name="state">Enable state.</param>
        public static CoreTween onPingPong(in this CoreTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween.selectPingpong();
            return ct;
        }
        /// <summary>Loop type. Clamp = linear loop cycle, pingpong is non-linear one.</summary>
        /// <param name="loopType"></param>
        public static CoreTween onLoopType(in this CoreTween ct, LoopType loopType)
        {
            if (loopType == LoopType.PingPong)
            {
                ct.getTween.selectPingpong();
                ct.getTween.setLoopCount(1);
            }

            return ct;
        }
        /// <summary>Callback upon completion.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween onComplete(in this CoreTween ct, Action callback)
        {
            CTcore.RegisterOnComplete(ct.index, callback);
            return ct;
        }
        /// <summary>Easing function for smooth interpolation between values.</summary>
        /// <param name="ease">Easing function type. Default is liner.</param>
        public static CoreTween onEase(in this CoreTween ct, Ease ease)
        {
            if (ease == Ease.PingPong)
            {
                ct.getTween.selectPingpong();
                return ct;
            }
            else if (ease == Ease.Shake)
            {
                ct.getTransform.gameObject.ctShake(2f, 25f, ct.getTween.duration);
                return ct;
            }
            else if (ease == Ease.Punch)
            {
                ct.getTransform.gameObject.ctPunch2D(2f, ct.getTween.duration);
                return ct;
            }

            ct.getTween.setEase((byte)ease);
            return ct;
        }
        /// <summary>Sets the unscaledTime of tween. True = would not be affected by timeScale.</summary>
        /// <param name="state">Enable state.</param>
        public static CoreTween onUnscaledTime(in this CoreTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween.selectUnscaledtime();
            return ct;
        }
        /// <summary>Callback that will be invoked every frame during tweening.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween onUpdate(in this CoreTween ct, System.Action callback)
        {
            CTcore.RegisterOnUpdate(ct.index, value => callback.Invoke());
            return ct;
        }
        /// <summary>Callback that will be invoked every frame during tweening.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween onUpdate(in this CoreTween ct, System.Action<float> callback)
        {
            if(ct.getTween.mode is LerpCoreType.Float)
            {
                CTcore.RegisterOnUpdate(ct.index, value => callback.Invoke(value));
            }

            return ct;
        }
        /// <summary>Callback that will be invoked every frame during tweening.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween onUpdate(this CoreTween ct, System.Action<Vector3> callback)
        {
            if(ct.getTween.mode is LerpCoreType.Position or LerpCoreType.Scale)
            {
                CTcore.RegisterOnUpdate(ct.index, value => callback.Invoke(ct.getTween.getVectorProgress));
            }

            return ct;
        }
        /// <summary>Callback that will be invoked every frame during tweening.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween onUpdate(this CoreTween ct, System.Action<Vector2> callback)
        {
            CTcore.RegisterOnUpdate(ct.index, x => callback.Invoke(ct.getTween.getVectorProgress));
            return ct;
        }
        
        /// <summary>Sets the tween to be speed-based rather than duration/time based.</summary>
        /// <param name="speed">Speed value.</param>
        public static CoreTween onSpeed(in this CoreTween ct, float speed)
        {
            if (speed > 255)
            {
                speed = 255;
            }

            if (speed < 0)
            {
                speed = 0;
            }

            ct.getTween.setSpeed(speed);
            return ct;
        }
        /// <summary>Will be invoked on each completion of a loop cycle.</summary>
        /// <param name="state">Enable state.</param>
        public static CoreTween onCompleteRepeat(in this CoreTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween.selectOncompleterepeat();
            return ct;
        }
        /// <summary>Focus on target transform while tweening.</summary>
        /// <param name="transform">Target transform.</param>
        /// <param name="instant">False = no interpolation.</param>
        /// <param name="speed">Speed</param>
        public static CoreTween onLookAt(this CoreTween ct, Transform transform, bool instant, float speed)
        {
            var defrot = ct.getTransform.rotation;

            CTcore.RegisterOnUpdate(ct.index, value =>
            {
                if (!instant)
                {
                    ct.getTransform.rotation = Quaternion.SlerpUnclamped(defrot, defrot * transform.rotation, Mathf.Clamp01(value * Time.deltaTime * speed));
                }
                else
                {
                    ct.getTransform.LookAt(transform);
                }
            });

            return ct;
        }
        /// <summary>Destroy the gameObject on completion.</summary>
        /// <param name="state">Enable state.</param>
        public static CoreTween onDestroyOnComplete(this CoreTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                GameObject.Destroy(ct.getTransform.gameObject);
            });

            return ct;
        }
        /// <summary>Sets active state of a gameObject on complete.</summary>
        /// <param name="state">Active state.</param>
        public static CoreTween onCompleteActive(this CoreTween ct, bool state)
        {
            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                ct.getTransform.gameObject.SetActive(state);
            });

            return ct;
        }
        /// <summary>Sets new point for when tweening.</summary>
        /// <param name="from">Inition point</param>
        public static CoreTween onSetFrom(in this CoreTween ct, Vector3 from)
        {
            if (ct.getTween.mode == LerpCoreType.Position || ct.getTween.mode == LerpCoreType.Translate)
            {
                if (!ct.getTween.isLocal)
                {
                    ct.getTransform.position = from;
                }
                else
                {
                    ct.getTransform.localPosition = from;
                }
            }

            return ct;
        }
        /// <summary>Sets initial point of tweening.</summary>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Degree angle</param>
        public static CoreTween onSetFrom(in this CoreTween ct, Vector3 direction, float angle)
        {
            if (ct.getTween.mode != LerpCoreType.Rotation)
            {
                return ct;
            }

            if (!ct.getTween.isLocal)
            {
                ct.getTransform.rotation = Quaternion.AngleAxis(angle, direction);
            }
            else
            {
                ct.getTransform.localRotation = Quaternion.AngleAxis(angle, direction);
            }

            return ct;
        }
        /// <summary>Halts the tweening. Similar to pause but can be chained.</summary>
        /// <param name="state">Halt state.</param>
        public static CoreTween halt(in this CoreTween ct, bool state)
        {
            if (state)
                ct.getTween.Pause();
            else
                ct.getTween.Resume();

            return ct;
        }

        /// <summary>Sets custom id.</summary>
        /// <param name="id">Custom id.</param>
        public static CoreTween onSetId(in this CoreTween ct, int id)
        {
            if (id == 0)
            {
                throw new Exception("CTween error : Custom id can't be 0.");
            }

            ct.getTween.setId(id);
            return ct;
        }
        /// <summary>Sequentially execute tween instances.</summary>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CoreTween next(this CoreTween ct, CoreTween nextTween)
        {
            nextTween.getTween.Pause();
            ct.queue(ref ct);
            nextTween.queue(ref ct);

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                nextTween.getTween.Resume(true);
                ct.RemoveFromQueueList();
            });

            return nextTween;
        }
        /// <summary>Sequentially execute tween instances.</summary>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CoreTween next(this CoreTween ct, Action callback)
        {
            ct.queue(ref ct);

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                callback.Invoke();
                ct.RemoveFromQueueList();
            });

            return ct;
        }
        /// <summary>Queues an isntance.</summary>
        /// <param name="callback">Callback.</param>
        public static CoreTween next(Action callback)
        {
            var dummy = new CoreTween();
            dummy.queue(ref dummy);

            CTcore.RegisterLastOnComplete(dummy.index, () =>
            {
                callback.Invoke();
                dummy.RemoveFromQueueList();
            });

            return dummy;
        }

        /// <summary>Delays a queue block.</summary>
        /// <param name="duration">Duration.</param>
        public static CoreTween nextDelay(this CoreTween ct, float duration)
        {
            var dummy = new CoreTween();
            CTcore.InstantiateFloat(ct.getTransform.gameObject.GetInstanceID(), 0f, 1f, duration, x=>{}, out int index);
            dummy.index = index;
            dummy.halt(true);

            CTcore.RegisterLastOnComplete(index, () =>
            {
                dummy.halt(false);
                ct.RemoveFromQueueList();
            });

            ct.queue(ref ct);
            dummy.queue(ref ct);
            return dummy;
        }

        /// <summary>Sets infinite loop.</summary>
        /// <param name="state">Infinite loop enable state.</param>
        public static CoreTween onInfinite(in this CoreTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween.selectInfiniteloop();
            return ct;
        }

        /// <summary>Queues tween instances.</summary>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CoreTween onEndPlayAudio(in this CoreTween ct, AudioSource audioSource)
        {
            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                audioSource.Play();
            });

            return ct;
        }

        /// <summary>Delaying the running tween. This will allocate at least 20+ bytes.</summary>
        /// <param name="delay">Delay time in seconds.</param>
        public static CoreTween onDelay(this CoreTween ct, float delay)
        {
            ct.halt(true);

            CPlayerLoop.mono.PassSecondCoroutine(new WaitForSeconds(delay), () => ct.getTween.Resume(true));

            return ct;
        }

        /// <summary>Quadratic curves movement in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="point1">Middle point.</param>
        /// <param name="point2">End point.</param>
        /// <param name="time">Duration.</param>
        /// <param name="lookAtDirection">Focus direction.</param>
        /// <param name="is2d">2D look at axis.</param>
        public static CoreTween curve(Transform transform, Vector3 point1, Vector3 point2, float time, bool lookAtDirection)
        {
            var start = transform.position;
            // Calculate control points for cubic Bezier curve
            Vector3 controlStart = start + 2f * (point1 - start) / 3f;
            Vector3 controlEnd = point2 + 2f * (point1 - point2) / 3f;

            var dummy = new CoreTween();

            CTcore.InstantiateFloat(transform.gameObject.GetInstanceID(), 0f, 1f, time, tick =>
            {
                float t = tick;
                float t2 = t * t;
                float t3 = t2 * t;
                float oneMinusT = 1f - t;
                float oneMinusT2 = oneMinusT * oneMinusT;
                float oneMinusT3 = oneMinusT2 * oneMinusT;

                Vector3 position =
                    oneMinusT3 * start +
                    3f * oneMinusT2 * t * controlStart +
                    3f * oneMinusT * t2 * controlEnd +
                    t3 * point2;

                var dir = position;
                var opos = transform.position;
                transform.position = position;

                if (lookAtDirection)
                {
                    Vector3 direction = (dir - opos).normalized;

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tick);
                    }
                }
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Quadratic curves movement.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="point1">Middle point.</param>
        /// <param name="point2">End point.</param>
        /// <param name="time">Duration.</param>
        /// <param name="lookAtDirection">Focus direction.</param>
        /// <param name="is2d">2D look at axis.</param>
        public static CoreTween curveLocal(Transform transform, Vector3 point1, Vector3 point2, float time, bool lookAtDirection)
        {
            var start = transform.localPosition;
            Vector3 controlStart = start + 2f * (point1 - start) / 3f;
            Vector3 controlEnd = point2 + 2f * (point1 - point2) / 3f;

            var dummy = new CoreTween();

            CTcore.InstantiateFloat(transform.gameObject.GetInstanceID(), 0f, 1f, time, tick =>
            {
                // Calculate position on the Bezier curve using cubic formula
                float t = tick;
                float t2 = t * t;
                float t3 = t2 * t;
                float oneMinusT = 1f - t;
                float oneMinusT2 = oneMinusT * oneMinusT;
                float oneMinusT3 = oneMinusT2 * oneMinusT;

                Vector3 position =
                    oneMinusT3 * start +
                    3f * oneMinusT2 * t * controlStart +
                    3f * oneMinusT * t2 * controlEnd +
                    t3 * point2;

                var dir = position;
                var opos = transform.localPosition;
                transform.localPosition = position;

                // Calculate rotation to face the direction of movement
                if (lookAtDirection)
                {
                    Vector3 direction = (dir - opos).normalized;

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, tick);
                    }
                }
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves along bezier curves in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="points">List of points to form a bezier path.</param>
        /// <param name="time">Duration.</param>
        public static CoreTween bezier(Transform transform, List<Vector3> points, float time)
        {
            points.Insert(0, transform.position);
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(transform.gameObject.GetInstanceID(), 0f, 1f, time, tick =>
            {
                if (tick <= 1f)
                {
                    Vector3 position = CalculateBezierPoint(tick, points);
                    transform.position = position;
                }
                else
                {
                    transform.position = points[points.Count - 1]; // Set final position to the last control point
                }
            }, out int index);

            dummy.index = index;

            Vector3 CalculateBezierPoint(float t, List<Vector3> points)
            {
                int numPoints = points.Count;
                Vector3 position = Vector3.zero;

                for (int i = 0; i < numPoints; i++)
                {
                    position += BinomialCoefficient(numPoints - 1, i) * Mathf.Pow(1 - t, numPoints - 1 - i) * Mathf.Pow(t, i) * points[i];
                }

                return position;
            }

            int BinomialCoefficient(int n, int k)
            {
                int result = 1;
                for (int i = 1; i <= k; i++)
                {
                    result *= (n - (k - i));
                    result /= i;
                }
                return result;
            }

            return dummy;
        }
        /// <summary>Moves along bezier curves in localSpace.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="points">List of points to form a bezier path.</param>
        /// <param name="time">Duration.</param>
        public static CoreTween bezierLocal(Transform transform, List<Vector3> points, float time)
        {
            points.Insert(0, transform.localPosition);
            var dummy = new CoreTween();

            CTcore.InstantiateFloat(transform.gameObject.GetInstanceID(), 0f, 1f, time, tick =>
            {
                if (tick <= 1f)
                {
                    Vector3 position = CalculateBezierPoint(tick, points);
                    transform.localPosition = position;
                }
                else
                {
                    transform.localPosition = points[points.Count - 1]; // Set final position to the last control point
                }
            }, out int index);

            dummy.index = index;

            Vector3 CalculateBezierPoint(float t, List<Vector3> points)
            {
                int numPoints = points.Count;
                Vector3 position = Vector3.zero;

                for (int i = 0; i < numPoints; i++)
                {
                    position += BinomialCoefficient(numPoints - 1, i) * Mathf.Pow(1 - t, numPoints - 1 - i) * Mathf.Pow(t, i) * points[i];
                }

                return position;
            }

            int BinomialCoefficient(int n, int k)
            {
                int result = 1;
                for (int i = 1; i <= k; i++)
                {
                    result *= (n - (k - i));
                    result /= i;
                }
                return result;
            }

            return dummy;
        }

        /// <summary>Frame-by-frame sprite animation.</summary>
        /// <param name="rectTransform">RectTransform.</param>
        /// <param name="sprites">Sprites.</param>
        /// <param name="frameRatePerSecond">Frame per second.</param>
        public static CoreTween play(RectTransform rectTransform, Sprite[] sprites, int frameRatePerSecond)
        {
            return ct.ctAnim(rectTransform, sprites, frameRatePerSecond);
        }

        ///////////////////////////////////////////////////////////////////////////////////
        /////////////////////////// EXTENDED EXTENSIONS HERE //////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>Awaits a tween in a similar manner it awaits a regular Task.</summary>
        /// <returns>Task.</returns>
        public static Task AsTask(this CoreTween ct)
        {
            var tcs = new TaskCompletionSource<bool>();

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        /// <summary>Yield a tween in an IEnumerator or coroutine.</summary>
        /// <returns>Yield instruction.</returns>
        public static WaitUntil AsCoroutine(this CoreTween ct)
        {
            bool complete = false;

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                complete = true;
            });

            return new WaitUntil(() => complete);
        }
        /// <summary>Cancels the tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Cancel(this CoreTween ct, GameObject gameObject, bool invokeOnComplete = false) => CTcore.getTween(gameObject.GetInstanceID()).Cancel(invokeOnComplete);
        /// <summary>Cancels the tween.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void Cancel(this CoreTween ct, VisualElement gameObject, bool invokeOnComplete = false) => CTcore.getTween(gameObject.GetHashCode()).Cancel(invokeOnComplete);
        /// <summary>Cancels the tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Cancel(GameObject gameObject, bool invokeOnComplete = false)
        {
            if(!CoreTween.QueueContains(CTcore.getTween(gameObject.GetInstanceID()).index))
            {
                CTcore.getTween(gameObject.GetInstanceID()).Cancel(invokeOnComplete);
            }
            else
            {
                CoreTween.TryCancelQueues(CTcore.getTween(gameObject.GetInstanceID()).index);
            }
        }
        /// <summary>Cancels the tween.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void Cancel(VisualElement visualElement, bool invokeOnComplete = false)
        {
            if(!CoreTween.QueueContains(CTcore.getTween(visualElement.GetHashCode()).index))
            {
                CTcore.getTween(visualElement.GetHashCode()).Cancel(invokeOnComplete);
            }
            else
            {
                CoreTween.TryCancelQueues(CTcore.getTween(visualElement.GetHashCode()).index);
            }
        }
        /// <summary>Cancels the tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Cancel(int id, bool invokeOnComplete = false) => CTcore.getTween(id).Cancel(invokeOnComplete);
        /// <summary>Cancels the tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Cancel(this CoreTween ct, bool invokeOnComplete = false)
        {
            if(ct.queueId == -1)
            {
                CTcore.getTweenViaIndex(ct.index).Cancel(invokeOnComplete);
            }
            else
            {
                ct.TryCancelQueues();
            }
        }
        /// <summary>Resumes paused tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Resume(this CoreTween ct, GameObject gameObject)=> CTcore.getTween(gameObject.GetInstanceID()).Resume();
        /// <summary>Resumes paused tween.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void Resume(this CoreTween ct, VisualElement visualElement)=> CTcore.getTween(visualElement.GetHashCode()).Resume();
        /// <summary>Pauses the running tween.</summary>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static void Pause(this CoreTween ct, GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).Pause();
        /// <summary>Checks if still tweening.</summary>
        /// <param name="id">Transform instance id.</param>
        public static bool IsTweening(int id) => CTcore.getTween(id).id > -1;
        /// <summary>Checks if still tweening.</summary>
        /// <param name="id">GameObject.</param>
        public static bool IsTweening(GameObject gameObject) => CTcore.getTween(gameObject.transform.GetInstanceID()).id > -1;
        /// <summary>Checks if still tweening.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static bool IsTweening(VisualElement visualElement) => CTcore.getTween(visualElement.GetHashCode()).id > -1;
        /// <summary>Checks if a tween instance is paused.</summary>
        /// <param name="id">Transform instance id.</param>
        public static bool IsPaused(int id) => CTcore.getTween(id).isPaused;
        /// <summary>Checks if a tween instance is paused.</summary>
        /// <param name="gameObject">GameObject instance id.</param>
        public static bool IsPaused(GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).isPaused;
        /// <summary>Checks if a tween is paused.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static bool IsPaused(VisualElement visualElement) => CTcore.getTween(visualElement.GetHashCode()).isPaused;
        /// <summary>Pause a tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void Pause(int id) => CTcore.getTween(id).Pause();
        /// <summary>Pauses the running tween instance.</summary>
        public static void PauseAll() => EnumerateAllCores(false, true, false);
        /// <summary>Enumerates all CTcores.</summary>
        /// <param name="cancel">Cancels.</param>
        /// <param name="pause">Pauses.</param>
        /// <param name="resume">Resumes.</param>
        /// <param name="id">Optional id for single operation.</param>
        static void EnumerateAllCores(bool cancel, bool pause, bool resume, int id = -1)
        {
            void local(ref CTcore core)
            {
                if (pause)
                {
                    if (!core.isPaused)
                        core.Pause();
                }
                else if (cancel)
                {
                    core.Cancel();
                }
                else if (resume)
                {
                    if (core.isPaused)
                        core.Resume();
                }
            }

            for (int i = 0; i < CTcore.fcore.Length; i++)
            {
                if (CTcore.fcore[i].id == -1 || CTcore.fcore[i].index == -1)
                    continue;

                if (id > -1)
                {
                    if (CTcore.fcore[i].id == id)
                        local(ref CTcore.fcore[i]);
                }
                else
                {
                    local(ref CTcore.fcore[i]);
                }
            }
        }
        /// <summary>Cancels all queues if any. If successful it will allocate.</summary>
        public static void CancellAllQueue() => CoreTween.TryCancelAllQueues();
        /// <summary>Canels queue.</summary>
        public static void CancelQueue(in CoreTween ct) => CoreTween.TryCancelQueues(ct.queueId);
        /// <summary>Cancels queue.</summary>
        /// <param name="queueid">Queue id.</param>
        public static void CancelQueue(int queueid) => CoreTween.TryCancelQueues(queueid);
        /// <summary>Resumes a paused tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void Resume(int id) => CTcore.getTween(id).Resume();
        /// <summary>Resumes all paused tweens.</summary>
        public static void ResumeAll() => EnumerateAllCores(false, false, true);
        /// <summary>Cancels an active tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void CancelFirst(int id, bool invokeOnComplete = false) => CTcore.getTween(id).Cancel(invokeOnComplete);
        /// <summary>Cancels all active tweens.</summary>
        public static void CancelAll() => EnumerateAllCores(true, false, false);
        /// <summary>Cancels any instances found with the same instance id. Usually tweens the are tweening the same transform/gameobject at once.</summary>
        /// <param name="id">instance id</param>
        public static void CancelMany(int id) => EnumerateAllCores(true, false, false, id);
        /// <summary>Pauses an active tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned.</param>
        public static void Pause(GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).Pause();
        /// <summary>Pauses an active tween instance.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void Pause(VisualElement visualElement) => CTcore.getTween(visualElement.GetHashCode()).Pause();
        /// <summary>Cancels any instances found with the same instance id. Usually tweens the are tweening the same transform/gameobject at once.</summary>
        /// <param name="gameObject">GameObject.</param>
        public static void PauseMany(GameObject gameObject) => EnumerateAllCores(false, true, false, gameObject.GetInstanceID());
        /// <summary>Cancels any instances found with the same instance id. Usually tweens the are tweening the same visualElement at once.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void PauseMany(VisualElement visualElement) => EnumerateAllCores(false, true, false, visualElement.GetHashCode());
        /// <summary>Resumes a paused tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned</param>
        public static void Resume(GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).Resume();
        /// <summary>Resumes a paused tween instance.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void Resume(VisualElement visualElement) => CTcore.getTween(visualElement.GetHashCode()).Resume();
        /// <summary>Resumes any instances found with the same instance id.</summary>
        /// <param name="gameObject">GameObject</param>
        public static void ResumeMany(GameObject gameObject) => EnumerateAllCores(false, false, true, gameObject.GetInstanceID());
        /// <summary>Resume any instances found with the same instance id.</summary>
        /// <param name="visualElement">GameObject</param>
        public static void ResumeMany(VisualElement visualElement) => EnumerateAllCores(false, false, true, visualElement.GetHashCode());
        /// <summary>Cancels an active tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned.</param>
        /// <param name="invokeOnComplete">Invokes on complete callback.</param>
        public static void CancelFirst(GameObject gameObject, bool invokeOnComplete = false) => CTcore.getTween(gameObject.GetInstanceID()).Cancel(invokeOnComplete);
        /// <summary>Cancels an active tween instance 1st found in the scene.</summary>
        /// <param name="gameObject">VisualElement.</param>
        /// <param name="invokeOnComplete">Invokes on complete callback.</param>
        public static void CancelFirst(VisualElement visualElement, bool invokeOnComplete = false) => CTcore.getTween(visualElement.GetHashCode()).Cancel(invokeOnComplete);
        /// <summary>Cancels any instances found with the same instance id. Usually tweens the are tweening the same transform/gameobject at once.</summary>
        /// <param name="gameObject">GameObject.</param>
        public static void CancelMany(GameObject gameObject) => EnumerateAllCores(true, false, false, gameObject.GetInstanceID());
        /// <summary>Cancels any instances found with the same instance id.</summary>
        /// <param name="visualElement">VisualElement.</param>
        public static void CancelMany(VisualElement visualElement) => EnumerateAllCores(true, false, false, visualElement.GetHashCode());
        /// <summary>Re-initialization of the array length. Useful for when there lots of tween instances at the same time by less often resizing the internal arrays. Put this on Awake.</summary>
        /// <param name="defaultPoolLength">Length.</param>
        public static void Init(int defaultPoolLength)=> CTcore.Init(defaultPoolLength);
        /// <summary>Scales a gameObject based on pivot point.</summary>
        static void ScaleAround(Transform transform, Vector3 pivot, Vector3 newScale)
        {
            Vector3 A = transform.localPosition;
            Vector3 B = pivot;
        
            Vector3 C = A - B; // diff from object pivot to desired pivot/origin
            float RS = newScale.x / transform.localScale.x; // relative scale factor
        
            // calc final position post-scale
            Vector3 FP = B + C * RS;
        
            // finally, actually perform the scale/translation
            transform.localScale = newScale;
            transform.localPosition = FP;
        }
    }

    public struct CoreTween
    {
        /// <summary>Index.</summary>
        public int index;
        /// <summary>Queue id.</summary>
        public int queueId;
        /// <summary>Queue list.</summary>
        static List<(int index, int id, int queueId, int loopCount, int loopCounter, bool pingpong)> queues = new(8);
        static List<(byte counter, Vector3 laspos, List<int> indexes, Transform transform)> combines = new(8);
        static short counter = 0;
        public CoreTween(int i = 0)
        {
            index = -1;
            queueId = -1;
            setQueueId();
        }
        public static void updateCombine(int instanceid)
        {
            for(int i = 0; i < combines.Count; i++)
            {
                if(combines[i].transform.GetInstanceID() == instanceid)
                {
                    var copy = combines[i];
                    combines[i] = (copy.counter, copy.transform.position, copy.indexes, copy.transform);
                    return;
                }
            }
        }
        /// <summary>Adds tween instance to combine list.</summary>
        /// <param name="id">Id.</param>
        static void addCombine(ref CTcore core)
        {
            var trans = core.transform;

            if(combines.Exists(x=> x.transform == trans))
            {
                for(int i = 0; i < combines.Count; i++)
                {
                    if(combines[i].transform != trans)
                        continue;
                    var copy = combines[i];
                    var num = copy.counter;
                    copy.indexes.Add(core.index);
                    combines[i] = ((byte)(copy.counter + 1), trans.localPosition, copy.indexes, copy.transform);
                    return;   
                }
            }

            var comid = counter++;
            counter += 1;
            var lis = new List<int>();
            lis.Add(core.id);
            combines.Add(((byte)1, trans.localPosition, lis, core.transform));
        }
        /// <summary>Removes tween instance from combine list.</summary>
        /// <param name="id">Id.</param>
        public static void removeCombine(int instanceid)
        {
            for(int i = 0; i < combines.Count; i++)
            {
                if(combines[i].transform.GetInstanceID() == instanceid)
                {
                    var copy = combines[i];

                    if(copy.counter == 0)
                    {
                        combines.RemoveAt(i);
                    }
                    else
                    {
                        for(int j = 0; j < copy.indexes.Count; j++)
                        {
                            if(copy.indexes[j] == CTcore.getTween(instanceid).index)
                            {
                                copy.indexes.RemoveAt(j);
                                break;
                            }
                        }

                        combines[i] = ((byte)(copy.counter - 1), copy.laspos, copy.indexes, copy.transform);
                    }

                    return;
                }
            }
        }
        /// <summary>Sets the queue id.</summary>
        public void setQueueId() => queueId = GetHashCode();
        /// <summary>Checks if available in queue list.</summary>
        public static bool QueueContains(int index) => queues.Count > 0 && queues.Exists(x => x.index == index);
        /// <summary>Gets active tween.</summary>
        public ref CTcore getTween => ref CTcore.fcore[index];
        /// <summary>Gets transform.</summary>
        public ref Transform getTransform => ref CTcore.ctobjects[index].transform;
        /// <summary>Add to queue list.</summary>
        /// <param name="ct"></param>
        public void queue(ref CoreTween ct)
        {
            queues.Add((index, getTween.id, ct.queueId, 1, 0, false));
        }
        /// <summary>Process queue list.</summary>
        public static void ProcessQueueList(int queueid, bool pause, bool resume, bool cancel)
        {
            if (queueid == -1)
            {
                return;
            }

            for (int i = queues.Count; i-- > 0;)
            {
                var tmp = queues[i];

                if (tmp.queueId == queueid && CTcore.fcore[tmp.index].id > -1)
                {
                    if (cancel)
                    {
                        CTcore.fcore[tmp.index].Cancel(invokeLastComplete: false);
                        queues.RemoveAt(i);
                    }
                    else if (pause)
                    {
                        CTcore.fcore[tmp.index].Pause();
                    }
                    else if (resume)
                    {
                        CTcore.fcore[tmp.index].Resume();
                    }
                }
            }
        }
        /// <summary>Removes from queue.</summary>
        public void RemoveFromQueueList()
        {
            if (queueId == -1)
            {
                return;
            }

            for (int i = queues.Count; i-- > 0;)
            {
                var tmp = queues[i];

                if (tmp.index == index && tmp.queueId == queueId)
                {
                    queues.RemoveAt(i);
                    break;
                }
            }
        }
        /// <summary>Try cancels queue.</summary>
        public void TryCancelQueues() => ProcessQueueList(queueId, false, false, true);
        /// <summary>Try pauses queues.</summary>
        public void TryPauseQueues() => ProcessQueueList(queueId, true, false, false);
        /// <summary>Try resumes queues.</summary>
        public void TryResumeQueues() => ProcessQueueList(queueId, false, true, false);
        public static int GetQueueIndex
        {
            get
            {
                for(int i = 0; i < queues.Count; i++)
                {

                }

                return -1;
            }
        }
        public static void TryCancelQueues(int queueid) => ProcessQueueList(queueid, false, false, true);
        public static void TryPauseQueues(int queueid) => ProcessQueueList(queueid, true, false, false);
        public static void TryResumeQueues(int queueid) => ProcessQueueList(queueid, false, true, false);
        public static void TryCancelAllQueues()
        {
            if(queues.Count == 0)
            {
                return;
            }

            for(int i = 0; i < queues.Count; i++)
            {
                if(queues[i].id > -1)
                {
                    CTcore.getTweenViaIndex(queues[i].index).Cancel();
                }
            }

            queues.Clear();
        }
    }
    public enum LoopType : int
    {
        Clamp = 0,
        PingPong = 1
    }
}