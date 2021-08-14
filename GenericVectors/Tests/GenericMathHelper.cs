// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Versioning;

namespace System.Numerics.Tests
{
    [RequiresPreviewFeatures]
    static class GenericMathHelper
    {
        public const float Pi = MathF.PI;
        public const float PiOver2 = Pi / 2f;
        public const float PiOver4 = Pi / 4f;

        // Angle conversion helper.
        public static T ToRadians<T>(T degrees)
            where T : struct, IFloatingPoint<T>
        {
            return degrees * T.Create(Math.PI / 180.0);
        }

        // Comparison helpers with small tolerance to allow for floating point rounding during computations.
        public static bool Equal<T>(T a, T b)
            where T : struct, IFloatingPoint<T>
        {
            return (T.Abs(a - b) < T.Create(1e-5));
        }

        public static bool Equal<T>(Vector2<T> a, Vector2<T> b)
            where T : struct, IFloatingPoint<T>
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y);
        }

        public static bool Equal<T>(Vector3<T> a, Vector3<T> b)
            where T : struct, IFloatingPoint<T>
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y) && Equal(a.Z, b.Z);
        }

        public static bool Equal<T>(Vector4<T> a, Vector4<T> b)
            where T : struct, IFloatingPoint<T>
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y) && Equal(a.Z, b.Z) && Equal(a.W, b.W); ;
        }

        public static bool Equal<T>(Matrix4x4<T> a, Matrix4x4<T> b)
             where T : struct, IFloatingPoint<T>
        {
            return
                Equal(a.M11, b.M11) && Equal(a.M12, b.M12) && Equal(a.M13, b.M13) && Equal(a.M14, b.M14) &&
                Equal(a.M21, b.M21) && Equal(a.M22, b.M22) && Equal(a.M23, b.M23) && Equal(a.M24, b.M24) &&
                Equal(a.M31, b.M31) && Equal(a.M32, b.M32) && Equal(a.M33, b.M33) && Equal(a.M34, b.M34) &&
                Equal(a.M41, b.M41) && Equal(a.M42, b.M42) && Equal(a.M43, b.M43) && Equal(a.M44, b.M44);
        }

        public static bool Equal<T>(Matrix3x2<T> a, Matrix3x2<T> b)
             where T : struct, IFloatingPoint<T>
        {
            return
                Equal(a.M11, b.M11) && Equal(a.M12, b.M12) &&
                Equal(a.M21, b.M21) && Equal(a.M22, b.M22) &&
                Equal(a.M31, b.M31) && Equal(a.M32, b.M32);
        }

        public static bool Equal<T>(Plane<T> a, Plane<T> b)
            where T : struct, IFloatingPoint<T>
        {
            return Equal(a.Normal, b.Normal) && Equal(a.Distance, b.Distance);
        }

        public static bool Equal<T>(Quaternion<T> a, Quaternion<T> b)
            where T : struct, IFloatingPoint<T>
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y) && Equal(a.Z, b.Z) && Equal(a.W, b.W);
        }

        public static bool EqualRotation<T>(Quaternion<T> a, Quaternion<T> b)
              where T : struct, IFloatingPoint<T>
        {
            return Equal(a, b) || Equal(a, -b);
        }
    }
}
