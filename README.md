# .NET Generic Vectors and Matrices 

THIS REPO IS EXPERIMENTAL AND SUBJECT TO BREAKING CHANGES!

[![.NET](https://github.com/sparkie108/generic-vectors-matrices/actions/workflows/tests.yml/badge.svg)](https://github.com/sparkie108/generic-vectors-matrices/actions/workflows/tests.yml)

This repo contains an implementation of generic versions of the following .NET types using the new generic math feature added as preview in .NET 6:

| Existing type | Generic implementation (this repo) |
|---------------------------------|---------------------------------|
System.Numerics.Matrix3x2.cs | System.Numerics.Matrix3x2\<T\>.cs
System.Numerics.Matrix4x4.cs | System.Numerics.Matrix4x4\<T\>.cs
System.Numerics.Plane.cs | System.Numerics.Plane\<T\>.cs
System.Numerics.Quaternion.cs | System.Numerics.Quaternion\<T\>.cs
System.Numerics.Vector2.cs | System.Numerics.Vector2\<T\>.cs
System.Numerics.Vector3.cs | System.Numerics.Vector3\<T\>.cs
System.Numerics.Vector4.cs | System.Numerics.Vector4\<T\>.cs

For further details of .NET Generic Math see:
[Preview Features in .NET 6 – Generic Math](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/).

The generic argument is constrained to types implementing System.IFloatingPoint<T> which, as of .NET 6 Preview 7, are:
* System.Half
* System.Single (float)
* System.Double (double)

The code is based on the [non-generic implementations](https://github.com/dotnet/runtime/tree/main/src/libraries/System.Private.CoreLib/src/System/Numerics).

The added generic vector and matrix types are designed to satisfy this existing .NET issue:
[Add support System.Numerics.Vectors types with double precision](https://github.com/dotnet/runtime/issues/24168).

As the Vector and Matrix classes themselves implement some of the generic math interfaces they can be used with generic math functions:
```c#
Vector3<double>[] data = { new(1.0, 2.0, 3.0), new(4.0, 5.0, 6.0) };
var sum = Sum(data);

public static T Sum<T>(IEnumerable<T> data)
    where T : IAdditiveIdentity<T, T>, IAdditionOperators<T, T, T>
{
    T sum = T.AdditiveIdentity;
    foreach (var item in data)
        sum += item;
    return sum;
} 
```
## Using in other projects
    
To use outside this repo copy the files in the src directory to your own project and follow the [directions in the blog post](https://devblogs.microsoft.com/dotnet/preview-features-in-net-6-generic-math/#trying-out-the-features) to enable generic math.
    
Note that in order to get the code to compile you must be using at least .NET 6 Preview 7.
    
## Static methods
    
The ideal is to add the static methods to the existing non-generic classes, for example:
```c#
    Matrix3x2<double> transform = Matrix3x2.CreateTranslation(-100.0, -100.0);
```
 However that is not possible in this repo, so they have been added to a class with an added 'S' on the end, for example:
```c#
    Matrix3x2<double> transform = Matrix3x2S.CreateTranslation(-100.0, -100.0);  
``` 
