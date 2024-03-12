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

## Combine  
To combine you must use curves based tweening for move commands (the rest can be combined as is).   
Reason being it's more performant than combining multiple interpolations at once and due to this library is meant to be fast and lightweight.   

## STween vs CTween  
STween is packed with lots of features such as edit-mode support, built-in mono components, pooling, etc and more robust APIs and highly extensible, while CTween is meant to be fast and compact and with less bloats.  
