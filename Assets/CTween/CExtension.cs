using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CTween.Extension;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.UIElements;
using System.Threading;
using System.Threading.Tasks;

namespace CTween
{
    public interface ICTween { }
    public struct CompactTween
    {
        public int index;
        public CompactTween(int i = 0)
        {
            index = -1;
        }
        public ref CTcore getTween()
        {
            return ref CTcore.fcore[index];
        }
    }

    public static class Ctween
    {
        /// <summary>Moves the visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween move(VisualElement visualElement, Vector3 to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween moveX(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                var from = visualElement.resolvedStyle.translate;
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
        public static CompactTween moveY(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                var from = visualElement.resolvedStyle.translate;
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
        public static CompactTween alpha(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                visualElement.style.opacity = tick * to;
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales the visualElement.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween scale(VisualElement visualElement, Vector2 to, float duration)
        {
            var dummy = new CompactTween();
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
        public static CompactTween width(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CompactTween();
            var wid = Length.Percent(visualElement.resolvedStyle.width);
            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                visualElement.style.width = to * tick;
            }, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the height based on the percentage.</summary>
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween height(VisualElement visualElement, float to, float duration)
        {
            var dummy = new CompactTween();
            var heg = Length.Percent(visualElement.resolvedStyle.height);

            CTcore.InstantiateFloat(visualElement.GetHashCode(), 0, 1, duration, tick =>
            {
                visualElement.style.height = to * tick;
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween move(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.position, to, duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform along x axis in world space.</summary>
        /// <param name="transform">The transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveX(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.position;
            CTcore.InstantiateVector(transform, pos, new Vector3(to, pos.y, pos.z), duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along y axis in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveY(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.position;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, to, pos.z), duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform along z axis in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveZ(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.position;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, pos.y, to), duration, LerpCoreType.Position, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocal(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.position, to, duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform's x axis in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Target destinatijon.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocalX(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(to, pos.y, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform's y axis in local space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocalY(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, to, pos.z), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves transform's z</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="to">To target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocalZ(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = transform.localPosition;
            CTcore.InstantiateVector(transform, pos, new Vector3(pos.x, pos.y, to), duration, LerpCoreType.Position, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween move(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, to, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's x axis in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveX(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = new Vector3(to, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's y axis in world space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveY(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = new Vector3(rectTransform.anchoredPosition3D.x, to, rectTransform.anchoredPosition3D.z);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's z axis in world spcae.</summary>
        /// <param name="rectTransform">RectTransform to move.[</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveZ(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, to);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition3D, pos, duration, LerpCoreType.AnchoredPosition, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's x axis in local space.[]</summary>
        /// <param name="rectTransform">RectTranform top move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duhration.</param>
        public static CompactTween moveLocalX(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = new Vector2(to, rectTransform.anchoredPosition.y);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, pos, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves rectTransform's y axis in local space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocalY(RectTransform rectTransform, float to, float duration)
        {
            var dummy = new CompactTween();
            var pos = new Vector2(rectTransform.anchoredPosition3D.x, to);
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, pos, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Moves the transform in local space.</summary>
        /// <param name="rectTransform">RectTransform to move.</param>
        /// <param name="to">Destination.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween moveLocal(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.anchoredPosition, to, duration, LerpCoreType.AnchoredPosition, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Direction of rotation.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateAround(Transform transform, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateRotateAround(transform, target, direction.normalized, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates around axis the transform in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="target">Direction of rotation.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateAroundLocal(Transform transform, Vector3 target, Vector3 direction, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateRotateAround(transform, target, direction.normalized, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotate(VisualElement visualElement, float angle, float duration)
        {
            var dummy = new CompactTween();
            
            var from = visualElement.resolvedStyle.rotate.angle.value;

            CTcore.InstantiateFloat(visualElement.GetHashCode(), from, angle, duration, tick=>
            {
                visualElement.style.rotate = new Rotate(Angle.Degrees(tick));

            }, out int index);

            dummy.index = index;
            return dummy;
        }

        /// <summary>Rotates the transform in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotate(Transform transform, Vector3 direction, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, direction, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a transform along x axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateX(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.right, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a transform along y axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateY(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.up, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates a transform along z axis in world space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Target degree angle.</param>
        /// <param name="duration">Duration.param>
        public static CompactTween rotateZ(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.forward, angle, duration, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates the transform in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="angle">Angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateLocal(Transform transform, Vector3 direction, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, direction, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotate transform along x axis in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateLocalX(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.right, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotates along y axis in local space.</summary>
        /// <param name="transform">Transform to move.[</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateLocalY(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.up, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Rotate transform along z axis in local space.</summary>
        /// <param name="transform">Transform to rotate.</param>
        /// <param name="angle">Degree angle.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween rotateLocalZ(Transform transform, float angle, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateQuat(transform, Vector3.forward, angle, duration, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween scale(Transform transform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.localScale, to, duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween scaleX(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(to, transform.localScale.y, transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween scaleY(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(transform.localScale.x, to, transform.localScale.z), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Scales a transform.</summary>
        /// <param name="transform">Transform to be scaled.</param>
        /// <param name="to">Target scale value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween scaleZ(Transform transform, float to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, to), duration, LerpCoreType.Scale, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolate alpha value of a CanvasGroup.</summary>
        /// <param name="id">Id to be assigned.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">To target value.</param>
        /// <param name="duration">Duratiob.</param>
        public static CompactTween value(CanvasGroup canvasGroup, float to, float duration)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(canvasGroup.gameObject.GetInstanceID(), canvasGroup.alpha, to, duration, (float value) =>
            {
                canvasGroup.alpha = value;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates float value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, float from, float to, float duration, Action<float> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, from, to, duration, (float value) =>
            {
                callback.Invoke(value);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Matrix4x4 value.</summary>
        /// <param name="id">Id.</param>
        /// <param name="from">Initial value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, Matrix4x4 from, Matrix4x4 to, float duration, Action<Matrix4x4> callback)
        {
            var dummy = new CompactTween();
            Matrix4x4 result = new Matrix4x4();

            CTcore.InstantiateFloat(id, 0f, 1f, duration, tick =>
            {    
                for (int i = 0; i < 16; i++)
                {
                    result[i] = Mathf.Lerp(from[i], to[i], tick);
                }

                callback.Invoke(result);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        public static CompactTween value(int id, Quaternion from, Quaternion to, float duration, Action<Quaternion> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, 0f, 1f, duration, tick =>
            {    
                callback.Invoke(Quaternion.SlerpUnclamped(from, to, tick));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector3 value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, Vector3 from, Vector3 to, float duration, Action<Vector3> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector3.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector4 value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, Vector4 from, Vector4 to, float duration, Action<Vector4> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector4.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates Vector2 value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, Vector2 from, Vector2 to, float duration, Action<Vector2> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, 0f, 1f, duration, (float value) =>
            {
                callback.Invoke(Vector2.LerpUnclamped(from, to, value));

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates integer value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween value(int id, int from, int to, float duration, Action<int> callback)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(id, (float)from, (float)to, duration, (float value) =>
            {
                callback.Invoke((int)value);

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates to the direction of translation.</summary>
        /// <param name="transform">Transform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="isLocal">Locality.</param>
        public static CompactTween translate(Transform transform, Vector3 to, float duration, bool isLocal)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(transform, transform.position, to, duration, LerpCoreType.Translate, isLocal, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Resizes the deltaSize of a rectTransform.</summary>
        /// <param name="rectTransform">RectTransform to resize.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween sizeDelta(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.sizeDelta, to, duration, LerpCoreType.SizeDelta, false, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the size relative to the anchored point.</summary>
        /// <param name="rectTransform">RectTransform.</param>
        /// <param name="to">Target.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween sizeAnchored(RectTransform rectTransform, Vector3 to, float duration)
        {
            var dummy = new CompactTween();
            CTcore.InstantiateVector(rectTransform, rectTransform.position, to, duration, LerpCoreType.SizeDelta, true, out int index);
            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates the alpha value of a CanvasGroup.</summary>
        /// <param name="canvasGroup">The canvasGroup component.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween alpha(CanvasGroup canvasGroup, float to, float duration)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(canvasGroup.gameObject.GetInstanceID(), canvasGroup.alpha, to, duration, (float value) =>
            {
                canvasGroup.alpha = value;
            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Shifts between two colors.</summary>
        /// <param name="image">Image component.</param>
        /// <param name="from">Initial color.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween color(UnityEngine.UI.Image image, Color to, float duration)
        {
            var dummy = new CompactTween();

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
        /// <param name="visualElement">VisualElement.</param>
        /// <param name="from">Initial color.</param>
        /// <param name="to">Target color.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween color(VisualElement visualElement, Color to, float duration)
        {
            var dummy = new CompactTween();

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

        /// <summary>Interpolates AudioSource value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween volume(AudioSource audioSource, float to, float duration)
        {
            var dummy = new CompactTween();

            CTcore.InstantiateFloat(audioSource.gameObject.GetInstanceID(), audioSource.volume, to, duration, (float value) =>
            {
                audioSource.volume = value;

            }, out int index);

            dummy.index = index;
            return dummy;
        }
        /// <summary>Interpolates AudioListener value.</summary>
        /// <param name="id">Assigned id used for cancelling or pausing.</param>
        /// <param name="from">Init value.</param>
        /// <param name="to">Target value.</param>
        /// <param name="duration">Duration.</param>
        public static CompactTween globalAudio(GameObject gameObject, float to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween shaderFloat(Material material, string propertyName, float to, float duration)
        {
            var dummy = new CompactTween();
            
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
        public static CompactTween shaderInt(Material material, string propertyName, int to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween shaderVector2(Material material, string propertyName, Vector2 to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween shaderVector3(Material material, string propertyName, Vector3 to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween shaderVector4(Material material, string propertyName, Vector4 to, float duration)
        {
            var dummy = new CompactTween();

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
        public static CompactTween shaderColor(Material material, string propertyName, Color to, float duration)
        {
            var dummy = new CompactTween();
            var tmp = material.GetColor(propertyName);
            var col = ColorShift(tmp, to);

            CTcore.InstantiateFloat(material.GetInstanceID(), 0f, 1f, duration, tick =>
            {
                material.SetColor(propertyName, Color.LerpUnclamped(new Color(col.start.x, col.start.y, col.start.z), new Color(col.end.x, col.end.y, col.end.z), tick));
            
            }, out int index);
            
            dummy.index = index;
            return dummy;
        }
        ///////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////// EXTENSIONS HERE ////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////

        /// <summary>Sets the max loop cycle for the tween to finish.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="loopCount">Loop count.</param>
        public static CompactTween onLoopCount(this CompactTween ct, int loopCount)
        {
            if (loopCount > 255 || loopCount < 0)
            {
                throw new Exception("CTween error : Loop count max is 0 - 255.");
            }

            ct.getTween().loopCount = (byte)loopCount;
            return ct;
        }
        /// <summary>Smooth pinpong like loop cycle.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="state">Enable state.</param>
        public static CompactTween onPingPong(this CompactTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween().selectPingpong();
            return ct;
        }
        /// <summary>Callback upon completion.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween onComplete(this CompactTween ct, System.Action callback)
        {
            CTcore.RegisterOnComplete(ct.index, callback);
            return ct;
        }
        /// <summary>
        /// Easing function for smooth interpolation between values.
        /// </summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="ease">Easing function type. Default is liner.</param>
        public static CompactTween onEase(this CompactTween ct, Ease ease)
        {
            ct.getTween().ease = (byte)(int)ease;
            return ct;
        }
        /// <summary>Sets the unscaledTime of tween. True = would not be affected by timeScale.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="state">Enable state.</param>
        public static CompactTween onUnscaledTime(this CompactTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween().selectUnscaledtime();
            return ct;
        }
        /// <summary>Callback that will be invoked every frame during tweening.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="callback">Callback.</param>
        public static CompactTween onUpdate(this CompactTween ct, System.Action callback)
        {
            CTcore.RegisterOnUpdate(ct.index, (x) => callback.Invoke());
            return ct;
        }
        /// <summary>Sets the tween to be speed-based rather than duration/time based.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="speed">Speed value.</param>
        public static CompactTween onSpeed(this CompactTween ct, int speed)
        {
            if (speed > 255 || speed < 0)
            {
                throw new Exception("CTween error : Loop count max is 0 - 255.");
            }

            ct.getTween().speed = (byte)speed;
            return ct;
        }
        /// <summary>Will be invoked on each completion of a loop cycle.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="state">Enable state.</param>
        public static CompactTween onCompleteRepeat(this CompactTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween().selectOncompleterepeat();
            return ct;
        }
        /// <summary>Focus on target transform while tweening.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="transform">Target transform.</param>
        /// <param name="instant">False = no interpolation.</param>
        /// <param name="speed">Speed</param>
        public static CompactTween onLookAt(this CompactTween ct, Transform transform, bool instant, float speed)
        {
            var defrot = ct.getTween().transform.rotation;

            CTcore.RegisterOnUpdate(ct.index, (x) =>
            {
                if (!instant)
                {
                    ct.getTween().transform.rotation = Quaternion.SlerpUnclamped(defrot, defrot * transform.rotation, Mathf.Clamp01(x * Time.deltaTime * speed));
                }
                else
                {
                    ct.getTween().transform.LookAt(transform);
                }
            });

            return ct;
        }
        /// <summary>Destroy the gameObject on completion.</summary>
        /// <param name="ct">Tween instance</param>
        /// <param name="state">Enable state.</param>
        public static CompactTween onDestroyOnComplete(this CompactTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                GameObject.Destroy(ct.getTween().transform.gameObject);
            });

            return ct;
        }
        /// <summary>Sets active state of a gameObject on complete.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="state">Active state.</param>
        public static CompactTween onCompleteActive(this CompactTween ct, bool state)
        {
            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                ct.getTween().transform.gameObject.SetActive(state);
            });

            return ct;
        }
        /// <summary>Sets new point for when tweening.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="from">Inition point</param>
        public static CompactTween onSetFrom(this CompactTween ct, Vector3 from)
        {
            if (CTcore.fcore[ct.index].mode == LerpCoreType.Position || CTcore.fcore[ct.index].mode == LerpCoreType.Translate)
            {
                if (!ct.getTween().isLocal)
                {
                    ct.getTween().transform.position = from;
                }
                else
                {
                    ct.getTween().transform.localPosition = from;
                }
            }

            return ct;
        }
        /// <summary>Sets initial point of tweening.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="direction">Direction of rotation.</param>
        /// <param name="angle">Degree angle</param>
        public static CompactTween onSetFrom(this CompactTween ct, Vector3 direction, float angle)
        {
            if (ct.getTween().mode != LerpCoreType.Rotation)
            {
                return ct;
            }

            if (!ct.getTween().isLocal)
            {
                ct.getTween().transform.rotation = Quaternion.AngleAxis(angle, direction);
            }
            else
            {
                ct.getTween().transform.localRotation = Quaternion.AngleAxis(angle, direction);
            }

            return ct;
        }
        /// <summary>Halts the tweening. Similar to pause but can be chained.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="state">Halt state.</param>
        public static CompactTween halt(this CompactTween ct, bool state)
        {
            if (state)
                ct.getTween().Pause();
            else
                ct.getTween().Resume();
            return ct;
        }
        /// <summary>Sequentially execute tween instances.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CompactTween next(this CompactTween ct, CompactTween nextTween)
        {
            nextTween.getTween().Pause();

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                nextTween.getTween().Resume(true);
            });

            return ct;
        }
        /// <summary>Sets infinite loop.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="state">Infinite loop enable state.</param>
        public static CompactTween onInfinite(this CompactTween ct, bool state)
        {
            if (!state)
            {
                return ct;
            }

            ct.getTween().selectInfiniteloop();
            return ct;
        }
        /// <summary>Queues tween instances.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CompactTween onEndPlayAudio(this CompactTween ct, AudioSource audioSource)
        {
            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                audioSource.Play();
            });

            return ct;
        }
        /// <summary>Queues tween instances.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="nextTween">Next tween to be queued.</param>
        public static CompactTween onStartPlayAudio(this CompactTween ct, AudioSource audioSource)
        {
            bool init = false;

            CTcore.RegisterOnUpdate(ct.index, (x) =>
            {
                if (!init)
                {
                    init = true;
                    audioSource.Play();
                }
            });

            return ct;
        }
        /// <summary>Delaying the running tween.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="delay">Delay time in seconds.</param>
        public static CompactTween onDelay(this CompactTween ct, float delay)
        {
            ct.halt(true);

            CPlayerLoop.mono.PassSecondCoroutine(new WaitForSeconds(delay), () =>
            {
                ct.getTween().Resume(true);
            });

            return ct;
        }
        /// <summary>Quadratic curves movement in world space.</summary>
        /// <param name="transform">Transform to move.</param>
        /// <param name="point1">Middle point.</param>
        /// <param name="point2">End point.</param>
        /// <param name="time">Duration.</param>
        /// <param name="lookAtDirection">Focus direction.</param>
        /// <param name="is2d">2D look at axis.</param>
        public static CompactTween curve(Transform transform, Vector3 point1, Vector3 point2, float time, bool lookAtDirection, bool is2d)
        {
            var start = transform.position;
            // Calculate control points for cubic Bezier curve
            Vector3 controlStart = start + 2f * (point1 - start) / 3f;
            Vector3 controlEnd = point2 + 2f * (point1 - point2) / 3f;

            var dummy = new CompactTween();

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
                        if (is2d)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tick);
                        }
                        else
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tick);
                        }
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
        public static CompactTween curveLocal(Transform transform, Vector3 point1, Vector3 point2, float time, bool lookAtDirection, bool is2d)
        {
            var start = transform.localPosition;
            Vector3 controlStart = start + 2f * (point1 - start) / 3f;
            Vector3 controlEnd = point2 + 2f * (point1 - point2) / 3f;

            var dummy = new CompactTween();

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
                        if (is2d)
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, tick);
                        }
                        else
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, tick);
                        }
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
        public static CompactTween bezier(Transform transform, List<Vector3> points, float time)
        {
            Vector3 from = transform.position;
            points.Insert(0, transform.position);
            var dummy = new CompactTween();

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
        public static CompactTween bezierLocal(Transform transform, List<Vector3> points, float time)
        {
            Vector3 from = transform.localPosition;
            points.Insert(0, transform.localPosition);
            var dummy = new CompactTween();

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
        ///////////////////////////////////////////////////////////////////////////////////
        /////////////////////////// EXTENDED EXTENSIONS HERE //////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Awaits a tween in a similar manner it awaits a regular Task.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <returns>Returns Task.</returns>
        public static Task AsTask(this CompactTween ct)
        {
            var tcs = new TaskCompletionSource<bool>();

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                CPlayerLoop.mono.PassTaskCoroutine(tcs);
            });

            return tcs.Task;
        }
        /// <summary>Yield a tween in an IEnumerator or coroutine.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <returns>Yield instruction.</returns>
        public static WaitUntil AsCoroutine(this CompactTween ct)
        {
            bool complete = false;

            CTcore.RegisterLastOnComplete(ct.index, () =>
            {
                complete = true;
            });

            return new WaitUntil(() => complete);
        }
        /// <summary>Cancels the tween.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static CompactTween Cancel(this CompactTween ct, GameObject gameObject, bool invokeOnComplete = false)
        {
            CTcore.getTween(gameObject.GetInstanceID()).Cancel(invokeOnComplete);
            return ct;
        }
        /// <summary>Resumes paused tween.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static CompactTween Resume(this CompactTween ct, GameObject gameObject)
        {
            CTcore.getTween(gameObject.GetInstanceID()).Resume();
            return ct;
        }
        /// <summary>Pauses the running tween.</summary>
        /// <param name="ct">Tween instance.</param>
        /// <param name="gameObject">GameObject that owns the transform.</param>
        public static CompactTween Pause(this CompactTween ct, GameObject gameObject)
        {
            CTcore.getTween(gameObject.GetInstanceID()).Pause();
            return ct;
        }
        /// <summary>Checks if still tweening.</summary>
        /// <param name="id">Transform instance id.</param>
        public static bool IsTweening(int id)
        {
            var t = CTcore.getTween(id);
            return t.id > -1;
        }
        /// <summary>Checks if a tween instance is paused.</summary>
        /// <param name="id">Transform instance id.</param>
        public static bool IsPaused(int id)
        {
            return CTcore.getTween(id).isPaused;
        }
        /// <summary>Pause a tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void Pause(int id) => CTcore.getTween(id).Pause();
        /// <summary>Pauses the running tween instance.</summary>
        public static void PauseAll()
        {
            for (int i = 0; i < CTcore.fcore.Length; i++)
            {
                if (CTcore.fcore[i].id > -1 && !CTcore.fcore[i].isPaused)
                {
                    CTcore.fcore[i].Pause();
                }
            }
        }
        /// <summary>Resumes a paused tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void Resume(int id) => CTcore.getTween(id).Resume();
        /// <summary>Resumes all paused tween.</summary>
        public static void ResumeAll()
        {
            for (int i = 0; i < CTcore.fcore.Length; i++)
            {
                if (CTcore.fcore[i].isPaused)
                {
                    CTcore.fcore[i].Resume();
                }
            }
        }
        /// <summary>Cancels an active tween instance.</summary>
        /// <param name="id">Transform instance id.</param>
        public static void Cancel(int id, bool invokeOnComplete = false) => CTcore.getTween(id).Cancel(invokeOnComplete);
        /// <summary>Pauses an active tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned.</param>
        public static void Pause(GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).Pause();
        /// <summary>Resumes a paused tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned</param>
        public static void Resume(GameObject gameObject) => CTcore.getTween(gameObject.GetInstanceID()).Resume();
        /// <summary>Cancels an active tween instance.</summary>
        /// <param name="gameObject">GameObject that contains the transform that was assigned</param>
        public static void Cancel(GameObject gameObject, bool invokeOnComplete = false) => CTcore.getTween(gameObject.GetInstanceID()).Cancel(invokeOnComplete);
    }
}