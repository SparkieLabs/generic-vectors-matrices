using System;
using System.Numerics;

var transform = Matrix3x2S.CreateTranslation(-100.0, -100.0);
transform *= Matrix3x2S.CreateScale(2.0, 2.0);
transform *= Matrix3x2S.CreateTranslation(100.0, 100.0);

var point = new Vector2<double>(105, 110);
var transformed = Vector2S.Transform(point, transform);

// <105, 110> => <110, 120>
Console.Write($"{point} => {transformed}");
