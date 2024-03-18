# CTween - zero allocation tweening library for Unity3D.  
A lightweight, thread safe, small & compact tweening library for Unity3D engine. 

## Performance
CTween is a compact version of STween which is another zero allocation tweening lib for Unity3D.  
Internally CTween is 100% struct based to avoid heap allocation and get much better performance compared to regular classes.  

## Features :  
- Move, Rotate, Scale, Value types interpolation (float, Vectors, Matrix4x4, Quaternion, etc).
- UIToolkit support.
- Fluent API : Makes syntax clean and simple with less boilerplates.
- Curves (Quadratic curves, Bezier curves movement).
- Easing functions.
- Support for speed based interpolations.
- Event callbacks & dispatching.
- 2D & 3D workflow.

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

//.value : color : param (int id, color:from, color:to, float:duration, Action<color> callback)
//description : Hue shift a colors.
CTween.value(id : 8, 0, 10, 2f, x=> {Debug.Log("print value : " + x);});

```

## Combine  
To combine you must use curves based tweening for move commands (the rest can be combined as is).   
Reason being it's more performant than combining multiple interpolations at once and due to this library is meant to be fast and lightweight.   

## STween vs CTween  
STween is packed with lots of features such as edit-mode support, built-in mono components, pooling, etc and more robust APIs and highly extensible, while CTween is meant to be fast and compact and with less bloats.  
