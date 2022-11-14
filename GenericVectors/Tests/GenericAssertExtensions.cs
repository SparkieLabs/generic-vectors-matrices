// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Numerics;
using Xunit;

namespace System
{
    public static class GenericAssertExtensions
    {
        private const int Precision = 5;

        public static void Equal(Vector2<Half> expected, Vector2<Half> actual)
        {
            Assert.Equal((float)expected.X, (float)actual.X, Precision);
            Assert.Equal((float)expected.Y, (float)actual.Y, Precision);
        }

        public static void Equal(Vector2<float> expected, Vector2<float> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
        }

        public static void Equal(Vector2<double> expected, Vector2<double> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
        }

        public static void Equal(Vector3<Half> expected, Vector3<Half> actual)
        {
            Assert.Equal((float)expected.X, (float)actual.X, Precision);
            Assert.Equal((float)expected.Y, (float)actual.Y, Precision);
            Assert.Equal((float)expected.Z, (float)actual.Z, Precision);
        }

        public static void Equal(Vector3<float> expected, Vector3<float> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
        }

        public static void Equal(Vector3<double> expected, Vector3<double> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
        }

        public static void Equal(Vector4<Half> expected, Vector4<Half> actual)
        {
            Assert.Equal((float)expected.X, (float)actual.X, Precision);
            Assert.Equal((float)expected.Y, (float)actual.Y, Precision);
            Assert.Equal((float)expected.Z, (float)actual.Z, Precision);
            Assert.Equal((float)expected.W, (float)actual.W, Precision);
        }

        public static void Equal(Vector4<float> expected, Vector4<float> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
            Assert.Equal(expected.W, actual.W, Precision);
        }

        public static void Equal(Vector4<double> expected, Vector4<double> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
            Assert.Equal(expected.W, actual.W, Precision);
        }

        public static void Equal(Quaternion<Half> expected, Quaternion<Half> actual)
        {
            Assert.Equal((float)expected.X, (float)actual.X, Precision);
            Assert.Equal((float)expected.Y, (float)actual.Y, Precision);
            Assert.Equal((float)expected.Z, (float)actual.Z, Precision);
            Assert.Equal((float)expected.W, (float)actual.W, Precision);
        }

        public static void Equal(Quaternion<float> expected, Quaternion<float> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
            Assert.Equal(expected.W, actual.W, Precision);
        }

        public static void Equal(Quaternion<double> expected, Quaternion<double> actual)
        {
            Assert.Equal(expected.X, actual.X, Precision);
            Assert.Equal(expected.Y, actual.Y, Precision);
            Assert.Equal(expected.Z, actual.Z, Precision);
            Assert.Equal(expected.W, actual.W, Precision);
        }
    }
}
