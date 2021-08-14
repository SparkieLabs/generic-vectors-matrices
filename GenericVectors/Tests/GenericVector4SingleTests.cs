// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Xunit;
using GMatrix4x4 = System.Numerics.Matrix4x4<float>;
using GQuaternion = System.Numerics.Quaternion<float>;
using GScalar = System.Single;
using GVector2 = System.Numerics.Vector2<float>;
using GVector3 = System.Numerics.Vector3<float>;
using GVector4 = System.Numerics.Vector4<float>;

namespace System.Numerics.Tests
{
    [RequiresPreviewFeatures]
    public class GenericVector4SingleTests
    {
        [Fact]
        public void Vector4MarshalSizeTest()
        {
            int scalarSize = sizeof(GScalar);
            Assert.Equal(scalarSize * 4, Marshal.SizeOf(new GVector4()));
        }

        [Fact]
        public void Vector4CopyToTest()
        {
            GVector4 v1 = new GVector4(2.5f, 2.0f, 3.0f, 3.3f);

            GScalar[] a = new GScalar[5];
            GScalar[] b = new GScalar[4];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            AssertExtensions.Throws<ArgumentException>(null, () => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0f, a[0]);
            Assert.Equal(2.5f, a[1]);
            Assert.Equal(2.0f, a[2]);
            Assert.Equal(3.0f, a[3]);
            Assert.Equal(3.3f, a[4]);
            Assert.Equal(2.5f, b[0]);
            Assert.Equal(2.0f, b[1]);
            Assert.Equal(3.0f, b[2]);
            Assert.Equal(3.3f, b[3]);
        }

        [Fact]
        public void Vector4CopyToSpanTest()
        {
            GVector4 vector = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            Span<GScalar> destination = new GScalar[4];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<GScalar>(new GScalar[3])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(3.0f, (GScalar)vector.Z);
            Assert.Equal(4.0f, (GScalar)vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4TryCopyToTest()
        {
            GVector4 vector = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            Span<GScalar> destination = new GScalar[4];

            Assert.False(vector.TryCopyTo(new Span<GScalar>(new GScalar[3])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(3.0f, (GScalar)vector.Z);
            Assert.Equal(4.0f, (GScalar)vector.W);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
            Assert.Equal(vector.W, destination[3]);
        }

        [Fact]
        public void Vector4GetHashCodeTest()
        {
            GVector4 v1 = new GVector4(2.5f, 2.0f, 3.0f, 3.3f);
            GVector4 v2 = new GVector4(2.5f, 2.0f, 3.0f, 3.3f);
            GVector4 v3 = new GVector4(2.5f, 2.0f, 3.0f, 3.3f);
            GVector4 v5 = new GVector4(3.3f, 3.0f, 2.0f, 2.5f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            GVector4 v4 = new GVector4(0.0f, 0.0f, 0.0f, 0.0f);
            GVector4 v6 = new GVector4(1.0f, 0.0f, 0.0f, 0.0f);
            GVector4 v7 = new GVector4(0.0f, 1.0f, 0.0f, 0.0f);
            GVector4 v8 = new GVector4(1.0f, 1.0f, 1.0f, 1.0f);
            GVector4 v9 = new GVector4(1.0f, 1.0f, 0.0f, 0.0f);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v9.GetHashCode(), v7.GetHashCode());
        }

        [Fact]
        public void Vector4ToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            GVector4 v1 = new GVector4((GScalar)2.5, (GScalar)2.0, (GScalar)3.0, (GScalar)3.3);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}{0} {4:G}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
                , separator, 2.5, 2, 3, 3.3);
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for DistanceSquared (Vector4, Vector4)
        [Fact]
        public void Vector4DistanceSquaredTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GScalar expected = 64.0f;
            GScalar actual;

            actual = Vector4S.DistanceSquared(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.DistanceSquared did not return the expected value.");
        }

        // A test for Distance (Vector4, Vector4)
        [Fact]
        public void Vector4DistanceTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GScalar expected = 8.0f;
            GScalar actual;

            actual = Vector4S.Distance(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Distance did not return the expected value.");
        }

        // A test for Distance (Vector4, Vector4)
        // Distance from the same point
        [Fact]
        public void Vector4DistanceTest1()
        {
            GVector4 a = new GVector4(new GVector2(1.051f, 2.05f), 3.478f, 1.0f);
            GVector4 b = new GVector4(new GVector3(1.051f, 2.05f, 3.478f), 0.0f);
            b = b with { W = 1.0f };

            GScalar actual = Vector4S.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for Dot (Vector4, Vector4)
        [Fact]
        public void Vector4DotTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GScalar expected = 70.0f;
            GScalar actual;

            actual = Vector4S.Dot(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Dot did not return the expected value.");
        }

        // A test for Dot (Vector4, Vector4)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector4DotTest1()
        {
            GVector3 a = new GVector3(1.55f, 1.55f, 1);
            GVector3 b = new GVector3(2.5f, 3, 1.5f);
            GVector3 c = Vector3S.Cross(a, b);

            GVector4 d = new GVector4(a, 0);
            GVector4 e = new GVector4(c, 0);

            GScalar actual = Vector4S.Dot(d, e);
            Assert.True(GenericMathHelper.Equal(0.0f, actual), "Vector4.Dot did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector4LengthTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar w = 4.0f;

            GVector4 target = new GVector4(a, w);

            GScalar expected = (GScalar)System.Math.Sqrt(30.0f);
            GScalar actual;

            actual = target.Length();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector4LengthTest1()
        {
            GVector4 target = new GVector4();

            GScalar expected = 0.0f;
            GScalar actual = target.Length();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector4LengthSquaredTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar w = 4.0f;

            GVector4 target = new GVector4(a, w);

            GScalar expected = 30;
            GScalar actual;

            actual = target.LengthSquared();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector4, Vector4)
        [Fact]
        public void Vector4MinTest()
        {
            GVector4 a = new GVector4(-1.0f, 4.0f, -3.0f, 1000.0f);
            GVector4 b = new GVector4(2.0f, 1.0f, -1.0f, 0.0f);

            GVector4 expected = new GVector4(-1.0f, 1.0f, -3.0f, 0.0f);
            GVector4 actual;
            actual = Vector4S.Min(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Min did not return the expected value.");
        }

        // A test for Max (Vector4, Vector4)
        [Fact]
        public void Vector4MaxTest()
        {
            GVector4 a = new GVector4(-1.0f, 4.0f, -3.0f, 1000.0f);
            GVector4 b = new GVector4(2.0f, 1.0f, -1.0f, 0.0f);

            GVector4 expected = new GVector4(2.0f, 4.0f, -1.0f, 1000.0f);
            GVector4 actual;
            actual = Vector4S.Max(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Max did not return the expected value.");
        }

        [Fact]
        public void Vector4MinMaxCodeCoverageTest()
        {
            GVector4 min = GVector4.Zero;
            GVector4 max = GVector4.One;
            GVector4 actual;

            // Min.
            actual = Vector4S.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector4S.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector4S.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector4S.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Clamp (Vector4, Vector4, Vector4)
        [Fact]
        public void Vector4ClampTest()
        {
            GVector4 a = new GVector4(0.5f, 0.3f, 0.33f, 0.44f);
            GVector4 min = new GVector4(0.0f, 0.1f, 0.13f, 0.14f);
            GVector4 max = new GVector4(1.0f, 1.1f, 1.13f, 1.14f);

            // Normal case.
            // Case N1: specified value is in the range.
            GVector4 expected = new GVector4(0.5f, 0.3f, 0.33f, 0.44f);
            GVector4 actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new GVector4(2.0f, 3.0f, 4.0f, 5.0f);
            expected = max;
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new GVector4(-2.0f, -3.0f, -4.0f, -5.0f);
            expected = min;
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new GVector4(-2.0f, 0.5f, 4.0f, -5.0f);
            expected = new GVector4(min.X, a.Y, max.Z, min.W);
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new GVector4(0.0f, 0.1f, 0.13f, 0.14f);
            min = new GVector4(1.0f, 1.1f, 1.13f, 1.14f);

            // Case W1: specified value is in the range.
            a = new GVector4(0.5f, 0.3f, 0.33f, 0.44f);
            expected = max;
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new GVector4(2.0f, 3.0f, 4.0f, 5.0f);
            expected = max;
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new GVector4(-2.0f, -3.0f, -4.0f, -5.0f);
            expected = max;
            actual = Vector4S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        [Fact]
        public void Vector4LerpTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GScalar t = 0.5f;

            GVector4 expected = new GVector4(3.0f, 4.0f, 5.0f, 6.0f);
            GVector4 actual;

            actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with factor zero
        [Fact]
        public void Vector4LerpTest1()
        {
            GVector4 a = new GVector4(new GVector3(1.0f, 2.0f, 3.0f), 4.0f);
            GVector4 b = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);

            GScalar t = 0.0f;
            GVector4 expected = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with factor one
        [Fact]
        public void Vector4LerpTest2()
        {
            GVector4 a = new GVector4(new GVector3(1.0f, 2.0f, 3.0f), 4.0f);
            GVector4 b = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);

            GScalar t = 1.0f;
            GVector4 expected = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with factor > 1
        [Fact]
        public void Vector4LerpTest3()
        {
            GVector4 a = new GVector4(new GVector3(0.0f, 0.0f, 0.0f), 0.0f);
            GVector4 b = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);

            GScalar t = 2.0f;
            GVector4 expected = new GVector4(8.0f, 10.0f, 12.0f, 14.0f);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with factor < 0
        [Fact]
        public void Vector4LerpTest4()
        {
            GVector4 a = new GVector4(new GVector3(0.0f, 0.0f, 0.0f), 0.0f);
            GVector4 b = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);

            GScalar t = -2.0f;
            GVector4 expected = -(b * 2);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with special GWrapper value
        [Fact]
        public void Vector4LerpTest5()
        {
            GVector4 a = new GVector4(45.67f, 90.0f, 0, 0);
            GVector4 b = new GVector4(GScalar.PositiveInfinity, GScalar.NegativeInfinity, 0, 0);

            GScalar t = 0.408f;
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GScalar.IsPositiveInfinity(actual.X), "Vector4.Lerp did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Y), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test from the same point
        [Fact]
        public void Vector4LerpTest6()
        {
            GVector4 a = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);
            GVector4 b = new GVector4(4.0f, 5.0f, 6.0f, 7.0f);

            GScalar t = 0.85f;
            GVector4 expected = a;
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        [Fact]
        public void Vector4LerpTest7()
        {
            GVector4 a = new GVector4(0.44728136f);
            GVector4 b = new GVector4(0.46345946f);

            GScalar t = 0.26402435f;

            GVector4 expected = new GVector4(0.45155275f);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector4, Vector4, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector4LerpTest8()
        {
            GVector4 a = new GVector4(-100);
            GVector4 b = new GVector4(0.33333334f);

            GScalar t = 1f;

            GVector4 expected = new GVector4(0.33333334f);
            GVector4 actual = Vector4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Lerp did not return the expected value.");
        }

        // A test for Reflect (Vector4, Vector4)
        [Fact]
        public void Vector4ReflectTest()
        {
            GVector4 a = Vector4S.Normalize(new GVector4(1.0f, 1.0f, 1.0f, 1.0f));

            // Reflect on XZ plane.
            GVector4 n = new GVector4(0.0f, 1.0f, 0.0f, 0.0f);
            GVector4 expected = new GVector4(a.X, -a.Y, a.Z, a.W);
            GVector4 actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new GVector4(0.0f, 0.0f, 1.0f, 0.0f);
            expected = new GVector4(a.X, a.Y, -a.Z, a.W);
            actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new GVector4(1.0f, 0.0f, 0.0f, 0.0f);
            expected = new GVector4(-a.X, a.Y, a.Z, a.W);
            actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector4, Vector4)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector4ReflectTest1()
        {
            GVector4 n = new GVector4(0.45f, 1.28f, 0.86f, 1.0f);
            n = Vector4S.Normalize(n);
            GVector4 a = n;

            GVector4 expected = -n;
            GVector4 actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector4, Vector4)
        // Reflection when normal and source are negation
        [Fact]
        public void Vector4ReflectTest2()
        {
            GVector4 n = new GVector4(0.45f, 1.28f, 0.86f, 1.0f);
            n = Vector4S.Normalize(n);
            GVector4 a = -n;

            GVector4 expected = n;
            GVector4 actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector4, Vector4)
        // Reflection when normal and source are perpendicular (a dot n = 0)
        [Fact]
        public void Vector4ReflectTest3()
        {
            GVector4 n = new GVector4(0.45f, 1.28f, 0.86f, 0.0f);
            GVector4 temp = new GVector4(1.28f, 0.45f, 0.01f, 0.0f);
            // find a perpendicular vector of n
            GVector3 a3 = Vector3S.Cross(new GVector3(temp.X, temp.Y, temp.Z), new GVector3(n.X, n.Y, n.Z));
            GVector4 a = new(a3.X, a3.Y, a3.Z, 0.0f);

            double dot = Vector4S.Dot(a, n);
            Assert.Equal(0, dot, 5);

            GVector4 expected = a;
            GVector4 actual = Vector4S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Reflect did not return the expected value.");
        }

        // A test for Transform (Vector2, Matrix4x4)
        [Fact]
        public void Vector4TransformTest1()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = new GVector4(10.316987f, 22.183012f, 30.3660259f, 1.0f);
            GVector4 actual;

            actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Matrix4x4)
        [Fact]
        public void Vector4TransformTest2()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = new GVector4(12.19198728f, 21.53349376f, 32.61602545f, 1.0f);
            GVector4 actual;

            actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Matrix4x4)
        [Fact]
        public void Vector4TransformVector4Test()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = new GVector4(2.19198728f, 1.53349376f, 2.61602545f, 0.0f);
            GVector4 actual;

            actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");

            //
            v = v with { W = 1.0f };

            expected = new GVector4(12.19198728f, 21.53349376f, 32.61602545f, 1.0f);
            actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Matrix4x4)
        // Transform vector4 with zero matrix
        [Fact]
        public void Vector4TransformVector4Test1()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);
            GMatrix4x4 m = new GMatrix4x4();
            GVector4 expected = new GVector4(0, 0, 0, 0);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Matrix4x4)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4TransformVector4Test2()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);
            GMatrix4x4 m = GMatrix4x4.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Matrix4x4)
        // Transform Vector3 test
        [Fact]
        public void Vector4TransformVector3Test()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = Vector4S.Transform(new GVector4(v, 1.0f), m);
            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Matrix4x4)
        // Transform vector3 with zero matrix
        [Fact]
        public void Vector4TransformVector3Test1()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GMatrix4x4 m = new GMatrix4x4();
            GVector4 expected = new GVector4(0, 0, 0, 0);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Matrix4x4)
        // Transform vector3 with identity matrix
        [Fact]
        public void Vector4TransformVector3Test2()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GMatrix4x4 m = GMatrix4x4.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 3.0f, 1.0f);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Matrix4x4)
        // Transform Vector2 test
        [Fact]
        public void Vector4TransformVector2Test()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = Vector4S.Transform(new GVector4(v, 0.0f, 1.0f), m);
            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Matrix4x4)
        // Transform Vector2 with zero matrix
        [Fact]
        public void Vector4TransformVector2Test1()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GMatrix4x4 m = new GMatrix4x4();
            GVector4 expected = new GVector4(0, 0, 0, 0);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Matrix4x4)
        // Transform vector2 with identity matrix
        [Fact]
        public void Vector4TransformVector2Test2()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GMatrix4x4 m = GMatrix4x4.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 0, 1.0f);

            GVector4 actual = Vector4S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector3, Matrix4x4)
        [Fact]
        public void Vector4TransformNormalTest()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector4 expected = new GVector4(2.19198728f, 1.53349364f, 2.61602545f, 4.0f);
            GVector4 actual;

            actual = Vector4S.TransformNormal(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.TransformNormal did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        [Fact]
        public void Vector4TransformVector2QuatanionTest()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));

            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector4 expected = Vector4S.Transform(v, m);
            GVector4 actual;

            actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        [Fact]
        public void Vector4TransformVector3Quaternion()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector4 expected = Vector4S.Transform(v, m);
            GVector4 actual;

            actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Quaternion)
        [Fact]
        public void Vector4TransformVector4QuaternionTest()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector4 expected = Vector4S.Transform(v, m);
            GVector4 actual;

            actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");

            //
            v = v with { W = 1.0f };
            expected = expected with { W = 1.0f };
            actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Quaternion)
        // Transform vector4 with zero quaternion
        [Fact]
        public void Vector4TransformVector4QuaternionTest1()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);
            GQuaternion q = new GQuaternion();
            GVector4 expected = v;

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector4, Quaternion)
        // Transform vector4 with identity matrix
        [Fact]
        public void Vector4TransformVector4QuaternionTest2()
        {
            GVector4 v = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);
            GQuaternion q = GQuaternion.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 3.0f, 0.0f);

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        // Transform Vector3 test
        [Fact]
        public void Vector4TransformVector3QuaternionTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector4 expected = Vector4S.Transform(v, m);
            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        // Transform vector3 with zero quaternion
        [Fact]
        public void Vector4TransformVector3QuaternionTest1()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GQuaternion q = new GQuaternion();
            GVector4 expected = new GVector4(v, 1.0f);

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        // Transform vector3 with identity quaternion
        [Fact]
        public void Vector4TransformVector3QuaternionTest2()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GQuaternion q = GQuaternion.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 3.0f, 1.0f);

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        // Transform Vector2 by quaternion test
        [Fact]
        public void Vector4TransformVector2QuaternionTest()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector4 expected = Vector4S.Transform(v, m);
            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        // Transform Vector2 with zero quaternion
        [Fact]
        public void Vector4TransformVector2QuaternionTest1()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GQuaternion q = new GQuaternion();
            GVector4 expected = new GVector4(1.0f, 2.0f, 0, 1.0f);

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Matrix4x4)
        // Transform vector2 with identity Quaternion
        [Fact]
        public void Vector4TransformVector2QuaternionTest2()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GQuaternion q = GQuaternion.Identity;
            GVector4 expected = new GVector4(1.0f, 2.0f, 0, 1.0f);

            GVector4 actual = Vector4S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector4)
        [Fact]
        public void Vector4NormalizeTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GVector4 expected = new GVector4(
                0.1825741858350553711523232609336f,
                0.3651483716701107423046465218672f,
                0.5477225575051661134569697828008f,
                0.7302967433402214846092930437344f);
            GVector4 actual;

            actual = Vector4S.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4)
        // Normalize vector of length one
        [Fact]
        public void Vector4NormalizeTest1()
        {
            GVector4 a = new GVector4(1.0f, 0.0f, 0.0f, 0.0f);

            GVector4 expected = new GVector4(1.0f, 0.0f, 0.0f, 0.0f);
            GVector4 actual = Vector4S.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector4)
        // Normalize vector of length zero
        [Fact]
        public void Vector4NormalizeTest2()
        {
            GVector4 a = new GVector4(0.0f, 0.0f, 0.0f, 0.0f);

            GVector4 expected = new GVector4(0.0f, 0.0f, 0.0f, 0.0f);
            GVector4 actual = Vector4S.Normalize(a);
            Assert.True(GScalar.IsNaN(actual.X) && GScalar.IsNaN(actual.Y) && GScalar.IsNaN(actual.Z) && GScalar.IsNaN(actual.W), "Vector4.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector4)
        [Fact]
        public void Vector4UnaryNegationTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GVector4 expected = new GVector4(-1.0f, -2.0f, -3.0f, -4.0f);
            GVector4 actual;

            actual = -a;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator - did not return the expected value.");
        }

        // A test for operator - (Vector4, Vector4)
        [Fact]
        public void Vector4SubtractionTest()
        {
            GVector4 a = new GVector4(1.0f, 6.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 2.0f, 3.0f, 9.0f);

            GVector4 expected = new GVector4(-4.0f, 4.0f, 0.0f, -5.0f);
            GVector4 actual;

            actual = a - b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator - did not return the expected value.");
        }

        // A test for operator * (Vector4, scalar)
        [Fact]
        public void Vector4MultiplyOperatorTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GScalar factor = 2.0f;

            GVector4 expected = new GVector4(2.0f, 4.0f, 6.0f, 8.0f);
            GVector4 actual;

            actual = a * factor;
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator * did not return the expected value.");
        }

        // A test for operator * (scalar, Vector4)
        [Fact]
        public void Vector4MultiplyOperatorTest2()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GScalar factor = 2.0f;
            GVector4 expected = new GVector4(2.0f, 4.0f, 6.0f, 8.0f);
            GVector4 actual;

            actual = factor * a;
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator * did not return the expected value.");
        }

        // A test for operator * (Vector4, Vector4)
        [Fact]
        public void Vector4MultiplyOperatorTest3()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GVector4 expected = new GVector4(5.0f, 12.0f, 21.0f, 32.0f);
            GVector4 actual;

            actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator * did not return the expected value.");
        }

        // A test for operator / (Vector4, scalar)
        [Fact]
        public void Vector4DivisionTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GScalar div = 2.0f;

            GVector4 expected = new GVector4(0.5f, 1.0f, 1.5f, 2.0f);
            GVector4 actual;

            actual = a / div;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4, Vector4)
        [Fact]
        public void Vector4DivisionTest1()
        {
            GVector4 a = new GVector4(1.0f, 6.0f, 7.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 2.0f, 3.0f, 8.0f);

            GVector4 expected = new GVector4(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f, 4.0f / 8.0f);
            GVector4 actual;

            actual = a / b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4, Vector4)
        // Divide by zero
        [Fact]
        public void Vector4DivisionTest2()
        {
            GVector4 a = new GVector4(-2.0f, 3.0f, GScalar.MaxValue, GScalar.NaN);

            GScalar div = 0.0f;

            GVector4 actual = a / div;

            Assert.True(GScalar.IsNegativeInfinity(actual.X), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Y), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Z), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsNaN(actual.W), "Vector4.operator / did not return the expected value.");
        }

        // A test for operator / (Vector4, Vector4)
        // Divide by zero
        [Fact]
        public void Vector4DivisionTest3()
        {
            GVector4 a = new GVector4(0.047f, -3.0f, GScalar.NegativeInfinity, GScalar.MinValue);
            GVector4 b = new GVector4();

            GVector4 actual = a / b;

            Assert.True(GScalar.IsPositiveInfinity(actual.X), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Y), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Z), "Vector4.operator / did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.W), "Vector4.operator / did not return the expected value.");
        }

        // A test for operator + (Vector4, Vector4)
        [Fact]
        public void Vector4AdditionTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GVector4 expected = new GVector4(6.0f, 8.0f, 10.0f, 12.0f);
            GVector4 actual;

            actual = a + b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector4.operator + did not return the expected value.");
        }

        [Fact]
        public void OperatorAddTest()
        {
            GVector4 v1 = new GVector4((GScalar)2.5, (GScalar)2.0, (GScalar)3.0, (GScalar)3.3);
            GVector4 v2 = new GVector4((GScalar)5.5, (GScalar)4.5, (GScalar)6.5, (GScalar)7.5);

            GVector4 v3 = v1 + v2;
            GVector4 v5 = new GVector4(-1, 0, 0, GScalar.NaN);
            GVector4 v4 = v1 + v5;
            Assert.Equal((GScalar)8.0, v3.X);
            Assert.Equal((GScalar)6.5, v3.Y);
            Assert.Equal((GScalar)9.5, v3.Z);
            Assert.Equal((GScalar)10.8, v3.W);
            Assert.Equal((GScalar)1.5, v4.X);
            Assert.Equal((GScalar)2.0, v4.Y);
            Assert.Equal((GScalar)3.0, v4.Z);
            Assert.Equal(GScalar.NaN, v4.W);
        }

        // A test for Vector4 (scalar, scalar, scalar, scalar)
        [Fact]
        public void Vector4ConstructorTest()
        {
            GScalar x = 1.0f;
            GScalar y = 2.0f;
            GScalar z = 3.0f;
            GScalar w = 4.0f;

            GVector4 target = new GVector4(x, y, z, w);

            Assert.True(GenericMathHelper.Equal(target.X, x) && GenericMathHelper.Equal(target.Y, y) && GenericMathHelper.Equal(target.Z, z) && GenericMathHelper.Equal(target.W, w),
                "Vector4 constructor(x,y,z,w) did not return the expected value.");
        }

        // A test for Vector4 (Vector2, scalar, scalar)
        [Fact]
        public void Vector4ConstructorTest1()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GScalar z = 3.0f;
            GScalar w = 4.0f;

            GVector4 target = new GVector4(a, z, w);
            Assert.True(GenericMathHelper.Equal(target.X, a.X) && GenericMathHelper.Equal(target.Y, a.Y) && GenericMathHelper.Equal(target.Z, z) && GenericMathHelper.Equal(target.W, w),
                "Vector4 constructor(Vector2,z,w) did not return the expected value.");
        }

        // A test for Vector4 (Vector3, scalar)
        [Fact]
        public void Vector4ConstructorTest2()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar w = 4.0f;

            GVector4 target = new GVector4(a, w);

            Assert.True(GenericMathHelper.Equal(target.X, a.X) && GenericMathHelper.Equal(target.Y, a.Y) && GenericMathHelper.Equal(target.Z, a.Z) && GenericMathHelper.Equal(target.W, w),
                "Vector4 constructor(Vector3,w) did not return the expected value.");
        }

        // A test for Vector4 ()
        // Constructor with no parameter
        [Fact]
        public void Vector4ConstructorTest4()
        {
            GVector4 a = new GVector4();

            Assert.Equal(0.0f, a.X);
            Assert.Equal(0.0f, a.Y);
            Assert.Equal(0.0f, a.Z);
            Assert.Equal(0.0f, a.W);
        }

        // A test for Vector4 ()
        // Constructor with special floating values
        [Fact]
        public void Vector4ConstructorTest5()
        {
            GVector4 target = new GVector4(GScalar.NaN, GScalar.MaxValue, GScalar.PositiveInfinity, GScalar.Epsilon);

            Assert.True(GScalar.IsNaN(target.X), "Vector4.constructor (GWrapper, GWrapper, GWrapper, GWrapper) did not return the expected value.");
            Assert.True(GScalar.Equals(GScalar.MaxValue, target.Y), "Vector4.constructor (GWrapper, GWrapper, GWrapper, GWrapper) did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(target.Z), "Vector4.constructor (GWrapper, GWrapper, GWrapper, GWrapper) did not return the expected value.");
            Assert.True(GScalar.Equals(GScalar.Epsilon, target.W), "Vector4.constructor (GWrapper, GWrapper, GWrapper, GWrapper) did not return the expected value.");
        }

        // A test for Vector4 (ReadOnlySpan<scalar>)
        [Fact]
        public void Vector4ConstructorTest7()
        {
            GScalar value = 1.0f;
            GVector4 target = new GVector4(new[] { value, value, value, value });
            GVector4 expected = new GVector4(value);

            Assert.Equal(expected, target);
            Assert.Throws<IndexOutOfRangeException>(() => new GVector4(new GScalar[3]));
        }

        // A test for Add (Vector4, Vector4)
        [Fact]
        public void Vector4AddTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GVector4 expected = new GVector4(6.0f, 8.0f, 10.0f, 12.0f);
            GVector4 actual;

            actual = Vector4S.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector4, scalar)
        [Fact]
        public void Vector4DivideTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GScalar div = 2.0f;
            GVector4 expected = new GVector4(0.5f, 1.0f, 1.5f, 2.0f);
            GVector4 actual;
            actual = Vector4S.Divide(a, div);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Divide (Vector4, Vector4)
        [Fact]
        public void Vector4DivideTest1()
        {
            GVector4 a = new GVector4(1.0f, 6.0f, 7.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 2.0f, 3.0f, 8.0f);

            GVector4 expected = new GVector4(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f, 4.0f / 8.0f);
            GVector4 actual;

            actual = Vector4S.Divide(a, b);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector4EqualsTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            object? obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10.0f };
            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new GQuaternion();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (scalar, Vector4)
        [Fact]
        public void Vector4MultiplyTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GScalar factor = 2.0f;
            GVector4 expected = new GVector4(2.0f, 4.0f, 6.0f, 8.0f);
            GVector4 actual = Vector4S.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4, scalar)
        [Fact]
        public void Vector4MultiplyTest2()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GScalar factor = 2.0f;
            GVector4 expected = new GVector4(2.0f, 4.0f, 6.0f, 8.0f);
            GVector4 actual = Vector4S.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector4, Vector4)
        [Fact]
        public void Vector4MultiplyTest3()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 6.0f, 7.0f, 8.0f);

            GVector4 expected = new GVector4(5.0f, 12.0f, 21.0f, 32.0f);
            GVector4 actual;

            actual = Vector4S.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Plus (Vector4)
        [Fact]
        public void Vector4PlusTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GVector4 expected = a;
            GVector4 actual;

            actual = Vector4S.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector4)
        [Fact]
        public void Vector4NegateTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            GVector4 expected = new GVector4(-1.0f, -2.0f, -3.0f, -4.0f);
            GVector4 actual;

            actual = Vector4S.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector4, Vector4)
        [Fact]
        public void Vector4InequalityTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10.0f };
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Vector4, Vector4)
        [Fact]
        public void Vector4EqualityTest()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10.0f };
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Vector4, Vector4)
        [Fact]
        public void Vector4SubtractTest()
        {
            GVector4 a = new GVector4(1.0f, 6.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(5.0f, 2.0f, 3.0f, 9.0f);

            GVector4 expected = new GVector4(-4.0f, 4.0f, 0.0f, -5.0f);
            GVector4 actual;

            actual = Vector4S.Subtract(a, b);

            Assert.Equal(expected, actual);
        }

        // A test for UnitW
        [Fact]
        public void Vector4UnitWTest()
        {
            GVector4 val = new GVector4(0.0f, 0.0f, 0.0f, 1.0f);
            Assert.Equal(val, GVector4.UnitW);
        }

        // A test for UnitX
        [Fact]
        public void Vector4UnitXTest()
        {
            GVector4 val = new GVector4(1.0f, 0.0f, 0.0f, 0.0f);
            Assert.Equal(val, GVector4.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector4UnitYTest()
        {
            GVector4 val = new GVector4(0.0f, 1.0f, 0.0f, 0.0f);
            Assert.Equal(val, GVector4.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector4UnitZTest()
        {
            GVector4 val = new GVector4(0.0f, 0.0f, 1.0f, 0.0f);
            Assert.Equal(val, GVector4.UnitZ);
        }

        // A test for One
        [Fact]
        public void Vector4OneTest()
        {
            GVector4 val = new GVector4(1.0f, 1.0f, 1.0f, 1.0f);
            Assert.Equal(val, GVector4.One);
        }

        // A test for Zero
        [Fact]
        public void Vector4ZeroTest()
        {
            GVector4 val = new GVector4(0.0f, 0.0f, 0.0f, 0.0f);
            Assert.Equal(val, GVector4.Zero);
        }

        // A test for Equals (Vector4)
        [Fact]
        public void Vector4EqualsTest1()
        {
            GVector4 a = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);
            GVector4 b = new GVector4(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            Assert.True(a.Equals(b));

            // case 2: compare between different values
            b = b with { X = 10.0f };
            Assert.False(a.Equals(b));
        }

        // A test for Vector4 (scalar)
        [Fact]
        public void Vector4ConstructorTest6()
        {
            GScalar value = 1.0f;
            GVector4 target = new GVector4(value);

            GVector4 expected = new GVector4(value, value, value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new GVector4(value);
            expected = new GVector4(value, value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector4 comparison involving NaN values
        [Fact]
        public void Vector4EqualsNanTest()
        {
            GVector4 a = new GVector4(GScalar.NaN, 0, 0, 0);
            GVector4 b = new GVector4(0, GScalar.NaN, 0, 0);
            GVector4 c = new GVector4(0, 0, GScalar.NaN, 0);
            GVector4 d = new GVector4(0, 0, 0, GScalar.NaN);

            Assert.False(a == GVector4.Zero);
            Assert.False(b == GVector4.Zero);
            Assert.False(c == GVector4.Zero);
            Assert.False(d == GVector4.Zero);

            Assert.True(a != GVector4.Zero);
            Assert.True(b != GVector4.Zero);
            Assert.True(c != GVector4.Zero);
            Assert.True(d != GVector4.Zero);

            Assert.False(a.Equals(GVector4.Zero));
            Assert.False(b.Equals(GVector4.Zero));
            Assert.False(c.Equals(GVector4.Zero));
            Assert.False(d.Equals(GVector4.Zero));

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
            Assert.False(d.Equals(d));
        }

        [Fact]
        public void Vector4AbsTest()
        {
            GVector4 v1 = new GVector4(-2.5f, 2.0f, 3.0f, 3.3f);
            GVector4 v3 = Vector4S.Abs(new GVector4(GScalar.PositiveInfinity, 0.0f, GScalar.NegativeInfinity, GScalar.NaN));
            GVector4 v = Vector4S.Abs(v1);
            Assert.Equal(2.5f, v.X);
            Assert.Equal(2.0f, v.Y);
            Assert.Equal(3.0f, v.Z);
            Assert.Equal(3.3f, v.W);
            Assert.Equal(GScalar.PositiveInfinity, v3.X);
            Assert.Equal(0.0f, v3.Y);
            Assert.Equal(GScalar.PositiveInfinity, v3.Z);
            Assert.Equal(GScalar.NaN, v3.W);
        }

        [Fact]
        public void Vector4SqrtTest()
        {
            GVector4 v1 = new GVector4(-2.5f, 2.0f, 3.0f, 3.3f);
            GVector4 v2 = new GVector4(5.5f, 4.5f, 6.5f, 7.5f);
            Assert.Equal(2, (int)Vector4S.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector4S.SquareRoot(v2).Y);
            Assert.Equal(2, (int)Vector4S.SquareRoot(v2).Z);
            Assert.Equal(2, (int)Vector4S.SquareRoot(v2).W);
            Assert.Equal(GScalar.NaN, Vector4S.SquareRoot(v1).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector4SizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 4, sizeof(GVector4));
            Assert.Equal(scalarSize * 8, sizeof(Vector4_2x));
            Assert.Equal(scalarSize * 5, sizeof(Vector4PlusScalar));
            Assert.Equal(scalarSize * 10, sizeof(Vector4PlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4_2x
        {
            private GVector4 _a;
            private GVector4 _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4PlusScalar
        {
            private GVector4 _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector4PlusScalar_2x
        {
            private Vector4PlusScalar _a;
            private Vector4PlusScalar _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            GVector4 v3 = new GVector4(4f, 5f, 6f, 7f);
            v3 = v3 with { X = 1.0f, Y = 2.0f, Z = 3.0f, W = 4.0f };
            Assert.Equal(1.0f, v3.X);
            Assert.Equal(2.0f, v3.Y);
            Assert.Equal(3.0f, v3.Z);
            Assert.Equal(4.0f, v3.W);
            GVector4 v4 = v3;
            v4 = v4 with { Y = 0.5f, Z = 2.2f, W = 3.5f };
            Assert.Equal(1.0f, v4.X);
            Assert.Equal(0.5f, v4.Y);
            Assert.Equal(2.2f, v4.Z);
            Assert.Equal(3.5f, v4.W);
            Assert.Equal(2.0f, v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector = evo.FieldVector with { X = 5.0f, Y = 5.0f, Z = 5.0f, W = 5.0f };
            Assert.Equal(5.0f, evo.FieldVector.X);
            Assert.Equal(5.0f, evo.FieldVector.Y);
            Assert.Equal(5.0f, evo.FieldVector.Z);
            Assert.Equal(5.0f, evo.FieldVector.W);
        }

        private class EmbeddedVectorObject
        {
            public GVector4 FieldVector;
        }
    }
}
