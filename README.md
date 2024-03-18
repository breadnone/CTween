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
```
//Move : Transform : param (Transform, Vector3:destination, float:duration)
CTween.move(go.transform, new Vector3(200, 150, 100), 5f);
CTween.moveLocal(go.transform, new Vector3(200, 150, 100), 5f);  //Moves in localSpace

////Move X axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveX(go.transform, 100f, 1f);

////Move Y axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveY(go.transform, 100f, 2f);

////Move Z axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveZ(go.transform, 100f, 3f);

////Move local X axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveLocalX(go.transform, 100f, 2f);

////Move local Y axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveLocalX(go.transform, 200f, 4f);

////Move local Z axis : Transform : param (Transform, float:destination, float:duration)
CTween.moveLocalX(go.transform, 150f, 5f);

//Move : RectTransform : param (RectTransform, Vector3:destination, float:duration)
CTween.move(go.GetComponent<RectTransform>(), new Vector3(122, 22, 0f), 5f)

//Scale : Transform : param (Transform, Vector3:target, float:duration)
CTween.scale(go.transform, new Vector3(2f, 2f, 2f), 3f);

//Size : RectTransform : param (RectTransform, Vector2:width & height, float:duration)
CTween.sizeDelta(go.GetComponent<RectTransform>(), new Vector2(111f, 102f), 1f)

//Anchored Size : RectTransform : param (RectTransform, Vector2:destination, float:duration)
CTween.sizeAnchored(go.GetComponent<RectTransform>(), new Vector2(5f, 5f), float:duration);

//Rotate : Transform : param (RectTransform, Vector3:axis, float:angle, float:duration)
CTween.rotate(go.transform, Vector3.forward, 90f, 5f);
CTween.rotateLocal(go.transform, Vector3.forward, 45f, 2f); //Moves in localSpace

```

## Combine  
To combine you must use curves based tweening for move commands (the rest can be combined as is).   
Reason being it's more performant than combining multiple interpolations at once and due to this library is meant to be fast and lightweight.   

## STween vs CTween  
STween is packed with lots of features such as edit-mode support, built-in mono components, pooling, etc and more robust APIs and highly extensible, while CTween is meant to be fast and compact and with less bloats.  
