# CTween - zero allocation tweening library for Unity3D.  
A lightweight, thread safe, small & compact tweening library for Unity3D engine. 

![ctween-shattered-gff-nf-hn](https://github.com/breadnone/CTween/assets/64100867/35bef2fe-876b-4604-a7eb-d92e8685c181)

## Features : 

- Move, Rotate, Scale, Value types interpolation (float, Vectors, Matrix4x4, Quaternion, etc).
- UIToolkit support.
- Fluent API : Makes syntax clean and simple with less boilerplates.
- Curves (Quadratic curves, Bezier curves movement).
- Easing functions.
- Support for speed based interpolations.
- Shader property.
- Event callbacks & dispatching.
- 2D & 3D workflow.

## Get Started:
- Download the latest released [package](https://github.com/breadnone/CTween/releases) then import to your project.
- Add the namespace `using CompactTween;`
- Minimum Unity3D version 2022.x and above.   

## Syntaxes :
```cs

//.move : Transform : param (Transform, Vector3:destination, float:duration)
//description : Moves to certain point in the scene.
CTween.move(go.transform, new Vector3(200, 150, 100), 5f);
CTween.moveLocal(go.transform, new Vector3(200, 150, 100), 5f);  //Moves in localSpace

//.moveX : Transform : param (Transform, float:destination, float:duration)
//description : Moves along X axis.
CTween.moveX(go.transform, 100f, 1f);

//.moveY : Transform : param (Transform, float:destination, float:duration)
//description : Moves along Y axis.
CTween.moveY(go.transform, 100f, 2f);

//.moveZ : Transform : param (Transform, float:destination, float:duration)
//description : Move along Z axis.
CTween.moveZ(go.transform, 100f, 3f);

//.moveLocalX : Transform : param (Transform, float:destination, float:duration)
//description : Move along X axis in localSpace.
CTween.moveLocalX(go.transform, 100f, 2f);

//.moveLocalY : Transform : param (Transform, float:destination, float:duration)
//description : Move along Y axis in localSpace.
CTween.moveLocalY(go.transform, 200f, 4f);

//.moveLocalZ : Transform : param (Transform, float:destination, float:duration)
//description : Move along Z axis in localSpace.
CTween.moveLocalZ(go.transform, 150f, 5f);

//.move : RectTransform : param (RectTransform, Vector3:destination, float:duration)
//description : Move a RectTransform to a certain point in the scene.  
CTween.move(go.GetComponent<RectTransform>(), new Vector3(122, 22, 0f), 5f)

//.move : VisualElement : param (VisualElement, Vector3:destination, float:duration)
//description : Moves to certain point in the scene.
CTween.move(visualElement, new Vector3(200, 150, 100), 5f);

//.moveX : Transform : param (VisualElement, float:destination, float:duration)
//description : Moves along X axis.
CTween.moveX(visualElement, 100f, 1f);

//.moveY : Transform : param (VisualElement, float:destination, float:duration)
//description : Moves along Y axis.
CTween.moveX(visualElement, 100f, 1f);

//.scale : Transform : param (Transform, Vector3:target, float:duration)
//description : Scale a Transform to target value.
CTween.scale(go.transform, new Vector3(2f, 2f, 2f), 3f);

//.sizeDelta : RectTransform : param (RectTransform, Vector2:width & height, float:duration)
//description : Smoothly resizes a rectTransform's deltaSize.
CTween.sizeDelta(go.GetComponent<RectTransform>(), new Vector2(111f, 102f), 1f);
 
//sizeAnchored : RectTransform : param (RectTransform, Vector2:destination, float:duration)
//description : Smoothly resizes a rectTransform relative to it's anchor/pivot point.
CTween.sizeAnchored(go.GetComponent<RectTransform>(), new Vector2(5f, 5f), float:duration);

//.rotate : Transform : param (RectTransform, Vector3:axis, float:angle, float:duration)
//description : Rotates a transform.
CTween.rotate(go.transform, Vector3.forward, 90f, 5f);

//.rotateLocal : Transform : param (RectTransform, Vector3:axis, float:angle, float:duration)
//description : rotates a transform in localSpace.
CTween.rotateLocal(go.transform, Vector3.forward, 45f, 2f); //Moves in localSpace

//.rotateX : Transform : param (Transform, float:angle, float:duration)
//description : Equals to Vector3.right.
CTween.rotateX(go.transform, 90f, 2f);

//.rotateY : Transform : param (Transform, float:angle, float:duration)
//description : Equals to Vector3.up.
CTween.rotateY(go.transform, 45f, 3f);

//.rotateZ : Transform : param (RectTransform, Vector3:axis, float:angle, float:duration)
//description : Equals to Vector3.forward.
CTween.rotateZ(go.transform, 90f, 2f);

//.rotateLocalX : Transform : param (Transform, float:angle, float:duration)
//description : Equals to Vector3.right in localSpace.
CTween.rotateLocalX(go.transform, 180f, 2f);

//.rotateLocalY : Transform : param (Transform, float:angle, float:duration)
//description : Equals to Vector3.up in localSpace.
CTween.rotateLocalY(go.transform, 30f, 7f);

//.rotateLocalZ : Transform : param (Transform, float:angle, float:duration)
//description : Equals to Vector3.forward in localSpace. 
CTween.rotateLocalZ(go.transform, 50, 1f);

//.translate : Transform : param (Transform, Vector3:target, float:duration, bool:localSpace)
//description : Moves the transform in the direction and distance of translation.
CTween.translate(go.transform, new Vector3(122f, 233f, 31f), 2f);

//.value : float : param (int id, float:from, float:to, float:duration, Action<float> callback)
//description : Interpolates a float type.
CTween.value(id : 4, 0f, 10f, 2f, x=> {Debug.Log("print value : " + x);});

//.value : int : param (int id, int:from, int:to, float:duration, Action<int> callback)
//description : Interpolates a integer type.
CTween.value(id : 2, 0, 10, 2f, x=> {Debug.Log("print value : " + x);});

//.value : Vector3 : param (int id, Vector3:from, Vector3:to, float:duration, Action<Vector3> callback)
//description : Interpolates a Vector3 type.
CTween.value(id : 1, vector3.zero, new Vector3(10f, 19f, 11f), 2f, x=> {Debug.Log("print value : " + x);});

//.value : Quaternion : param (int id, Quaternion:from, Quaternion:to, float:duration, Action<Quaternion> callback)
//description : Interpolates a Quaternion type.
CTween.value(id: 6, 0, 10, 2f, x=> {Debug.Log("print value : " + x);});

//.value : Color : param (int id, Color:from, Color:to, float:duration, callback:Action<color>)
//description : Hue shift a colors.
CTween.value(id : 8, 0, 10, 2f, x=> {Debug.Log("print value : " + x);});

//.alpha : alpha : param (CanvasGroup, float:from, float:to, float:duration)
//description : Fades in/out alpha value of a canvasGroyup component.
CTween.alpha(canvas.GetComponent<CanvasGroup>(), 1f, 0f, 3f);

//.alpha : alpha : param (VisualElement, float:from, float:to, float:duration)
//description : Fades in/out the opacity of a VisualElement.
CTween.alpha(visualElement, 1f, 0f, 3f);

//.color : Color : param (UnityEngine.UI.Image : Image component, Color:from, Color:to, float:duration)
//description : Hue shift a color of Image component.
CTween.color(gameObject.GetComponent<UnityEngine.UI.Image>(), Color.white, Color.blue, 3f);

//.color : Color : param (VisualElement, Color:from, Color:to, float:duration)
//description : Hue shift a color of a visualElement.
CTween.color(visualElement, Color.white, Color.blue, 3f);

//.shaderFloat : float : param (Material, float:from, float:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderFloat(material, 0f, 1f, 3f);

//.shaderInt : int : param (Material, int:from, int:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderInt(material, 0, 10, 3f);

//.shaderColor : float : param (Material, Color:from, Color:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderFloat(material, Color.white, Color.red, 3f);

//.shaderVector2 : Vector2 : param (Material, Vector2:from, Vector2:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderVector2(material, Vector2.zero, new Vector2(20f, 20f), 3f);

//.shaderVector3 : Vector3 : param (Material, Vector3:from, Vector3:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderVector3(material, Vector3.zero, new Vector3(0f, 20f, 77f), 3f);

//.shaderVector4 : Vector4 : param (Material, Vector4:from, Vector4:to, float:duration)
//description : Smoothly interpolates a shader property.
CTween.shaderFloat(material, Vector4.zero, new Vector4(12f, 23f, 90f, `11f), 3f);

//.next : CompactTween : param (Transform, CompactTween:previousTween, CompactTween:nextTween)
//description : Queues or chain a sequence of tweens.
CTween.moveX(go.transform, 100f, 5f).next(CTween.moveY(go.transform, 90f, 2f));
```
## Properties : Chainable properties
```
//.onLoopCount(int loopCount);
//description : Sets how many loop cycle for the tween to complete.

//.onPingPong(bool state);
//description : Pingpong loop cycle instead of linear.

//.onSpeed();
//description : Speed based tweening.

//.onLoopType(LoopType type);
//description : Clamp or pingPong loop mode.

//.onComplete(Action callback);
//description : Invoked once the tween has completed.

//.onCompleteRepeat(bool state);
//description : Triggers the onComplete callback on each loop cycle completion.

//.onEase(Ease ease);
//description : Easing function type.

//.onUnscaledTime(bool state);
//description : Unscaledtime state. Default is false.

//.onUpdate(Action callback);
//description : Invoked every frame while tweening.

//.onLookAt(Transform transform, bool instant, float speed)
//description : Focus on a transform while tweening.

//.onDestroyOnComplete(bool state);
//description : Destroys the gameObject once finished tweening.

//.onCompleteActive(bool state)
//description : Active state of a gameObject once finished tweening.

//.onSetFrom(Vector3 direction, float angle);
//description : Sets the initial rotation of a tween instance.

///onSetFrom(Vector3 from)
//description : Sets the inital point of a tween instance.

//.onInfinite(bool state);
//description : Infinitely tweening.

//.onEndPlayAudio(AudioSource auSource);
//description : Plays an audio once done tweening.

//.onDelay(float delayInSeconds);
//description : Delays the execution of a tween.

```
## Curves
```
//Moves along quadratic curves in world space.
CTween.curve(go.transform, new Vector3(100f, 200f, 250f), new Vector3(150f, 350f, 250f), duration: 5f, lookAtDirection: true);

//Moves along quadratic curves in localSpace.
CTween.curveLocal(go.transform, new Vector3(100f, 200f, 250f), new Vector3(150f, 350f, 250f), duration: 5f, lookAtDirection: true);

//Moves along bezier curves in world space.
CTween.bezier(go.transform, MyPointList, 5f);

//Moves along bezier curves in localSpace.
CTween.bezierLocal(go.transform, MyPointList, 5f);
```

## Asynchronous Tweening
```
IEnumerator WaitTween()
{
   yield return CTween.move(go.transform, new Vector3(120f, 23f, 50f), 5f).AsCoroutine();
}

async Task WaitTaskTween()
{
   await CTween.move(go.transform, new Vector3(120f, 23f, 50f), 5f).AsTask();
}

```
## Pause, Resume, Cancel
```
CTween.moveLocalX(go.transform, 100f, 2f);

//Pause
CTween.Pause(go);

//Resume
CTween.Resume(go);

//Cancel
CTween.Cancel(go);

//Bulk Pauses, Resumes, Cancels
CTween.PauseAll();
CTween.ResumeAll();
CTween.CancelAll();

//Cancels 1st found tween instance.
CTween.CancelFirst(go);

//Cancels multiple tweens that's tied to the same gameObject.
CTween.CancelMany(go);

//Canceling queues. Queues are special and must be cancelled via it's built-in method (TryCancelQueues())
//cache the instance
var ct = CTween.move(mov.transform, target.transform.position, duration).next(CTween.move(mov.transform, defPos, duration)).onEase(ease);
//Cancel with the built-in method.
ct.TryCancelQueues();

```

## Combine  
To combine you must use curves based tweening for move commands (the rest can be combined as is).   
Reason being it's more performant than combining multiple interpolations at once and due to this library is meant to be fast and lightweight.   

## STween vs CTween  
STween is packed with lots of features such as edit-mode support, built-in mono components, pooling, etc and more robust APIs and highly extensible, while CTween is meant to be fast and compact and with less bloats and to avoid the preallocated objects in memory (no pooling, just structs).  
