// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Xunit;
using GMatrix4x4 = System.Numerics.Matrix4x4<double>;
using GQuaternion = System.Numerics.Quaternion<double>;
using GScalar = System.Double;
using GVector2 = System.Numerics.Vector2<double>;
using GVector3 = System.Numerics.Vector3<double>;

namespace System.Numerics.Tests
{
    [RequiresPreviewFeatures]
    public class Vector3DoubleTests
    {
        [Fact]
        public void Vector3MarshalSizeTest()
        {
            int scalarSize = sizeof(GScalar);
            Assert.Equal(scalarSize * 3, Marshal.SizeOf(new GVector3()));
        }

        [Fact]
        public void Vector3CopyToTest()
        {
            GVector3 v1 = new GVector3(2.0f, 3.0f, 3.3f);

            GScalar[] a = new GScalar[4];
            GScalar[] b = new GScalar[3];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            AssertExtensions.Throws<ArgumentException>(null, () => v1.CopyTo(a, a.Length - 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0f, a[0]);
            Assert.Equal(2.0f, a[1]);
            Assert.Equal(3.0f, a[2]);
            Assert.Equal(3.3f, a[3]);
            Assert.Equal(2.0f, b[0]);
            Assert.Equal(3.0f, b[1]);
            Assert.Equal(3.3f, b[2]);
        }

        [Fact]
        public void Vector3CopyToSpanTest()
        {
            GVector3 vector = new GVector3(1.0f, 2.0f, 3.0f);
            Span<GScalar> destination = new GScalar[3];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<GScalar>(new GScalar[2])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(3.0f, (GScalar)vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3TryCopyToTest()
        {
            GVector3 vector = new GVector3(1.0f, 2.0f, 3.0f);
            Span<GScalar> destination = new GScalar[3];

            Assert.False(vector.TryCopyTo(new Span<GScalar>(new GScalar[2])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(3.0f, (GScalar)vector.Z);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
            Assert.Equal(vector.Z, destination[2]);
        }

        [Fact]
        public void Vector3GetHashCodeTest()
        {
            GVector3 v1 = new GVector3(2.0f, 3.0f, 3.3f);
            GVector3 v2 = new GVector3(2.0f, 3.0f, 3.3f);
            GVector3 v3 = new GVector3(2.0f, 3.0f, 3.3f);
            GVector3 v5 = new GVector3(3.0f, 2.0f, 3.3f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v5.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v3.GetHashCode());
            GVector3 v4 = new GVector3(0.0f, 0.0f, 0.0f);
            GVector3 v6 = new GVector3(1.0f, 0.0f, 0.0f);
            GVector3 v7 = new GVector3(0.0f, 1.0f, 0.0f);
            GVector3 v8 = new GVector3(1.0f, 1.0f, 1.0f);
            GVector3 v9 = new GVector3(1.0f, 1.0f, 0.0f);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v9.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v9.GetHashCode());
        }

        [Fact]
        public void Vector3ToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            GVector3 v1 = new GVector3((GScalar)2.0, (GScalar)3.0, (GScalar)3.3);
            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}{0} {3:G}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3, 3.3);
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}{0} {3:c}>"
                , separator, 2, 3, 3.3);
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for Cross (Vector3, Vector3)
        [Fact]
        public void Vector3CrossTest()
        {
            GVector3 a = new GVector3(1.0f, 0.0f, 0.0f);
            GVector3 b = new GVector3(0.0f, 1.0f, 0.0f);

            GVector3 expected = new GVector3(0.0f, 0.0f, 1.0f);
            GVector3 actual;

            actual = Vector3S.Cross(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Cross did not return the expected value.");
        }

        // A test for Cross (Vector3, Vector3)
        // Cross test of the same vector
        [Fact]
        public void Vector3CrossTest1()
        {
            GVector3 a = new GVector3(0.0f, 1.0f, 0.0f);
            GVector3 b = new GVector3(0.0f, 1.0f, 0.0f);

            GVector3 expected = new GVector3(0.0f, 0.0f, 0.0f);
            GVector3 actual = Vector3S.Cross(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Cross did not return the expected value.");
        }

        // A test for Distance (Vector3, Vector3)
        [Fact]
        public void Vector3DistanceTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar expected = (GScalar)Math.Sqrt(27);
            GScalar actual;

            actual = Vector3S.Distance(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Distance did not return the expected value.");
        }

        // A test for Distance (Vector3, Vector3)
        // Distance from the same point
        [Fact]
        public void Vector3DistanceTest1()
        {
            GVector3 a = new GVector3(1.051f, 2.05f, 3.478f);
            GVector3 b = new GVector3(new GVector2(1.051f, 0.0f), 1);
            b = b with { Y = 2.05f, Z = 3.478f };

            GScalar actual = Vector3S.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for DistanceSquared (Vector3, Vector3)
        [Fact]
        public void Vector3DistanceSquaredTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar expected = 27.0f;
            GScalar actual;

            actual = Vector3S.DistanceSquared(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector3, Vector3)
        [Fact]
        public void Vector3DotTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar expected = 32.0f;
            GScalar actual;

            actual = Vector3S.Dot(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Dot did not return the expected value.");
        }

        // A test for Dot (Vector3, Vector3)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector3DotTest1()
        {
            GVector3 a = new GVector3(1.55f, 1.55f, 1);
            GVector3 b = new GVector3(2.5f, 3, 1.5f);
            GVector3 c = Vector3S.Cross(a, b);

            GScalar expected = 0.0f;
            GScalar actual1 = Vector3S.Dot(a, c);
            GScalar actual2 = Vector3S.Dot(b, c);
            Assert.True(GenericMathHelper.Equal(expected, actual1), "Vector3.Dot did not return the expected value.");
            Assert.True(GenericMathHelper.Equal(expected, actual2), "Vector3.Dot did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector3LengthTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GScalar z = 3.0f;

            GVector3 target = new GVector3(a, z);

            GScalar expected = (GScalar)Math.Sqrt(14.0f);
            GScalar actual;

            actual = target.Length();
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector3LengthTest1()
        {
            GVector3 target = new GVector3();

            GScalar expected = 0.0f;
            GScalar actual = target.Length();
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector3LengthSquaredTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GScalar z = 3.0f;

            GVector3 target = new GVector3(a, z);

            GScalar expected = 14.0f;
            GScalar actual;

            actual = target.LengthSquared();
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.LengthSquared did not return the expected value.");
        }

        // A test for Min (Vector3, Vector3)
        [Fact]
        public void Vector3MinTest()
        {
            GVector3 a = new GVector3(-1.0f, 4.0f, -3.0f);
            GVector3 b = new GVector3(2.0f, 1.0f, -1.0f);

            GVector3 expected = new GVector3(-1.0f, 1.0f, -3.0f);
            GVector3 actual;
            actual = Vector3S.Min(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Min did not return the expected value.");
        }

        // A test for Max (Vector3, Vector3)
        [Fact]
        public void Vector3MaxTest()
        {
            GVector3 a = new GVector3(-1.0f, 4.0f, -3.0f);
            GVector3 b = new GVector3(2.0f, 1.0f, -1.0f);

            GVector3 expected = new GVector3(2.0f, 4.0f, -1.0f);
            GVector3 actual;
            actual = Vector3S.Max(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "vector3.Max did not return the expected value.");
        }

        [Fact]
        public void Vector3MinMaxCodeCoverageTest()
        {
            GVector3 min = GVector3.Zero;
            GVector3 max = GVector3.One;
            GVector3 actual;

            // Min.
            actual = Vector3S.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector3S.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector3S.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector3S.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        [Fact]
        public void Vector3LerpTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar t = 0.5f;

            GVector3 expected = new GVector3(2.5f, 3.5f, 4.5f);
            GVector3 actual;

            actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with factor zero
        [Fact]
        public void Vector3LerpTest1()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar t = 0.0f;
            GVector3 expected = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with factor one
        [Fact]
        public void Vector3LerpTest2()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar t = 1.0f;
            GVector3 expected = new GVector3(4.0f, 5.0f, 6.0f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with factor > 1
        [Fact]
        public void Vector3LerpTest3()
        {
            GVector3 a = new GVector3(0.0f, 0.0f, 0.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar t = 2.0f;
            GVector3 expected = new GVector3(8.0f, 10.0f, 12.0f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with factor < 0
        [Fact]
        public void Vector3LerpTest4()
        {
            GVector3 a = new GVector3(0.0f, 0.0f, 0.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GScalar t = -2.0f;
            GVector3 expected = new GVector3(-8.0f, -10.0f, -12.0f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with special scalar value
        [Fact]
        public void Vector3LerpTest5()
        {
            GVector3 a = new GVector3(45.67f, 90.0f, 0f);
            GVector3 b = new GVector3(GScalar.PositiveInfinity, GScalar.NegativeInfinity, 0);

            GScalar t = 0.408f;
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GScalar.IsPositiveInfinity(actual.X), "Vector3.Lerp did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Y), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test from the same point
        [Fact]
        public void Vector3LerpTest6()
        {
            GVector3 a = new GVector3(1.68f, 2.34f, 5.43f);
            GVector3 b = a;

            GScalar t = 0.18f;
            GVector3 expected = new GVector3(1.68f, 2.34f, 5.43f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        [Fact]
        public void Vector3LerpTest7()
        {
            GVector3 a = new GVector3(0.44728136f);
            GVector3 b = new GVector3(0.46345946f);

            GScalar t = 0.26402435f;

            GVector3 expected = new GVector3(0.45155275f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector3, Vector3, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector3LerpTest8()
        {
            GVector3 a = new GVector3(-100);
            GVector3 b = new GVector3(0.33333334f);

            GScalar t = 1f;

            GVector3 expected = new GVector3(0.33333334f);
            GVector3 actual = Vector3S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Lerp did not return the expected value.");
        }

        // A test for Reflect (Vector3, Vector3)
        [Fact]
        public void Vector3ReflectTest()
        {
            GVector3 a = Vector3S.Normalize(new GVector3(1.0f, 1.0f, 1.0f));

            // Reflect on XZ plane.
            GVector3 n = new GVector3(0.0f, 1.0f, 0.0f);
            GVector3 expected = new GVector3(a.X, -a.Y, a.Z);
            GVector3 actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new GVector3(0.0f, 0.0f, 1.0f);
            expected = new GVector3(a.X, a.Y, -a.Z);
            actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new GVector3(1.0f, 0.0f, 0.0f);
            expected = new GVector3(-a.X, a.Y, a.Z);
            actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3, Vector3)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector3ReflectTest1()
        {
            GVector3 n = new GVector3(0.45f, 1.28f, 0.86f);
            n = Vector3S.Normalize(n);
            GVector3 a = n;

            GVector3 expected = -n;
            GVector3 actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3, Vector3)
        // Reflection when normal and source are negation
        [Fact]
        public void Vector3ReflectTest2()
        {
            GVector3 n = new GVector3(0.45f, 1.28f, 0.86f);
            n = Vector3S.Normalize(n);
            GVector3 a = -n;

            GVector3 expected = n;
            GVector3 actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector3, Vector3)
        // Reflection when normal and source are perpendicular (a dot n = 0)
        [Fact]
        public void Vector3ReflectTest3()
        {
            GVector3 n = new GVector3(0.45f, 1.28f, 0.86f);
            GVector3 temp = new GVector3(1.28f, 0.45f, 0.01f);
            // find a perpendicular vector of n
            GVector3 a = Vector3S.Cross(temp, n);

            GVector3 expected = a;
            GVector3 actual = Vector3S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Reflect did not return the expected value.");
        }

        // A test for Transform(Vector3, Matrix4x4)
        [Fact]
        public void Vector3TransformTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector3 expected = new GVector3(12.191987f, 21.533493f, 32.616024f);
            GVector3 actual;

            actual = Vector3S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Transform did not return the expected value.");
        }

        // A test for Clamp (Vector3, Vector3, Vector3)
        [Fact]
        public void Vector3ClampTest()
        {
            GVector3 a = new GVector3(0.5f, 0.3f, 0.33f);
            GVector3 min = new GVector3(0.0f, 0.1f, 0.13f);
            GVector3 max = new GVector3(1.0f, 1.1f, 1.13f);

            // Normal case.
            // Case N1: specified value is in the range.
            GVector3 expected = new GVector3(0.5f, 0.3f, 0.33f);
            GVector3 actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new GVector3(2.0f, 3.0f, 4.0f);
            expected = max;
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // Case N3: specified value is smaller than max value.
            a = new GVector3(-2.0f, -3.0f, -4.0f);
            expected = min;
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // Case N4: combination case.
            a = new GVector3(-2.0f, 0.5f, 4.0f);
            expected = new GVector3(min.X, a.Y, max.Z);
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // User specified min value is bigger than max value.
            max = new GVector3(0.0f, 0.1f, 0.13f);
            min = new GVector3(1.0f, 1.1f, 1.13f);

            // Case W1: specified value is in the range.
            a = new GVector3(0.5f, 0.3f, 0.33f);
            expected = max;
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new GVector3(2.0f, 3.0f, 4.0f);
            expected = max;
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new GVector3(-2.0f, -3.0f, -4.0f);
            expected = max;
            actual = Vector3S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Clamp did not return the expected value.");
        }

        // A test for TransformNormal (Vector3, Matrix4x4)
        [Fact]
        public void Vector3TransformNormalTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector3 expected = new GVector3(2.19198728f, 1.53349364f, 2.61602545f);
            GVector3 actual;

            actual = Vector3S.TransformNormal(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.TransformNormal did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        [Fact]
        public void Vector3TransformByQuaternionTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector3 expected = Vector3S.Transform(v, m);
            GVector3 actual = Vector3S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        // Transform vector3 with zero quaternion
        [Fact]
        public void Vector3TransformByQuaternionTest1()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GQuaternion q = new GQuaternion();
            GVector3 expected = v;

            GVector3 actual = Vector3S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Transform did not return the expected value.");
        }

        // A test for Transform (Vector3, Quaternion)
        // Transform vector3 with identity quaternion
        [Fact]
        public void Vector3TransformByQuaternionTest2()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GQuaternion q = GQuaternion.Identity;
            GVector3 expected = v;

            GVector3 actual = Vector3S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector3)
        [Fact]
        public void Vector3NormalizeTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GVector3 expected = new GVector3(
                0.26726124191242438468455348087975f,
                0.53452248382484876936910696175951f,
                0.80178372573727315405366044263926f);
            GVector3 actual;

            actual = Vector3S.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3)
        // Normalize vector of length one
        [Fact]
        public void Vector3NormalizeTest1()
        {
            GVector3 a = new GVector3(1.0f, 0.0f, 0.0f);

            GVector3 expected = new GVector3(1.0f, 0.0f, 0.0f);
            GVector3 actual = Vector3S.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector3)
        // Normalize vector of length zero
        [Fact]
        public void Vector3NormalizeTest2()
        {
            GVector3 a = new GVector3(0.0f, 0.0f, 0.0f);

            GVector3 expected = new GVector3(0.0f, 0.0f, 0.0f);
            GVector3 actual = Vector3S.Normalize(a);
            Assert.True(GScalar.IsNaN(actual.X) && GScalar.IsNaN(actual.Y) && GScalar.IsNaN(actual.Z), "Vector3.Normalize did not return the expected value.");
        }

        // A test for operator - (Vector3)
        [Fact]
        public void Vector3UnaryNegationTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GVector3 expected = new GVector3(-1.0f, -2.0f, -3.0f);
            GVector3 actual;

            actual = -a;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator - did not return the expected value.");
        }

        [Fact]
        public void Vector3UnaryNegationTest1()
        {
            GVector3 a = -new GVector3(GScalar.NaN, GScalar.PositiveInfinity, GScalar.NegativeInfinity);
            GVector3 b = -new GVector3(0.0f, 0.0f, 0.0f);
            Assert.Equal(GScalar.NaN, (GScalar)a.X);
            Assert.Equal(GScalar.NegativeInfinity, (GScalar)a.Y);
            Assert.Equal(GScalar.PositiveInfinity, (GScalar)a.Z);
            Assert.Equal(0.0f, (GScalar)b.X);
            Assert.Equal(0.0f, (GScalar)b.Y);
            Assert.Equal(0.0f, (GScalar)b.Z);
        }

        // A test for operator - (Vector3, Vector3)
        [Fact]
        public void Vector3SubtractionTest()
        {
            GVector3 a = new GVector3(4.0f, 2.0f, 3.0f);

            GVector3 b = new GVector3(1.0f, 5.0f, 7.0f);

            GVector3 expected = new GVector3(3.0f, -3.0f, -4.0f);
            GVector3 actual;

            actual = a - b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator - did not return the expected value.");
        }

        // A test for operator * (Vector3, scalar)
        [Fact]
        public void Vector3MultiplyOperatorTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GScalar factor = 2.0f;

            GVector3 expected = new GVector3(2.0f, 4.0f, 6.0f);
            GVector3 actual;

            actual = a * factor;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator * did not return the expected value.");
        }

        // A test for operator * (scalar, Vector3)
        [Fact]
        public void Vector3MultiplyOperatorTest2()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            const GScalar factor = 2.0f;

            GVector3 expected = new GVector3(2.0f, 4.0f, 6.0f);
            GVector3 actual;

            actual = factor * a;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator * did not return the expected value.");
        }

        // A test for operator * (Vector3, Vector3)
        [Fact]
        public void Vector3MultiplyOperatorTest3()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GVector3 expected = new GVector3(4.0f, 10.0f, 18.0f);
            GVector3 actual;

            actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator * did not return the expected value.");
        }

        // A test for operator / (Vector3, scalar)
        [Fact]
        public void Vector3DivisionTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GScalar div = 2.0f;

            GVector3 expected = new GVector3(0.5f, 1.0f, 1.5f);
            GVector3 actual;

            actual = a / div;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3, Vector3)
        [Fact]
        public void Vector3DivisionTest1()
        {
            GVector3 a = new GVector3(4.0f, 2.0f, 3.0f);

            GVector3 b = new GVector3(1.0f, 5.0f, 6.0f);

            GVector3 expected = new GVector3(4.0f, 0.4f, 0.5f);
            GVector3 actual;

            actual = a / b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3, Vector3)
        // Divide by zero
        [Fact]
        public void Vector3DivisionTest2()
        {
            GVector3 a = new GVector3(-2.0f, 3.0f, GScalar.MaxValue);

            GScalar div = 0.0f;

            GVector3 actual = a / div;

            Assert.True(GScalar.IsNegativeInfinity(actual.X), "Vector3.operator / did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Y), "Vector3.operator / did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Z), "Vector3.operator / did not return the expected value.");
        }

        // A test for operator / (Vector3, Vector3)
        // Divide by zero
        [Fact]
        public void Vector3DivisionTest3()
        {
            GVector3 a = new GVector3(0.047f, -3.0f, GScalar.NegativeInfinity);
            GVector3 b = new GVector3();

            GVector3 actual = a / b;

            Assert.True(GScalar.IsPositiveInfinity(actual.X), "Vector3.operator / did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Y), "Vector3.operator / did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Z), "Vector3.operator / did not return the expected value.");
        }

        // A test for operator + (Vector3, Vector3)
        [Fact]
        public void Vector3AdditionTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(4.0f, 5.0f, 6.0f);

            GVector3 expected = new GVector3(5.0f, 7.0f, 9.0f);
            GVector3 actual;

            actual = a + b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector3.operator + did not return the expected value.");
        }

        // A test for Vector3 (scalar, scalar, scalar)
        [Fact]
        public void Vector3ConstructorTest()
        {
            GScalar x = 1.0f;
            GScalar y = 2.0f;
            GScalar z = 3.0f;

            GVector3 target = new GVector3(x, y, z);
            Assert.True(GenericMathHelper.Equal(target.X, x) && GenericMathHelper.Equal(target.Y, y) && GenericMathHelper.Equal(target.Z, z), "Vector3.constructor (x,y,z) did not return the expected value.");
        }

        // A test for Vector3 (Vector2, scalar)
        [Fact]
        public void Vector3ConstructorTest1()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GScalar z = 3.0f;

            GVector3 target = new GVector3(a, z);
            Assert.True(GenericMathHelper.Equal(target.X, a.X) && GenericMathHelper.Equal(target.Y, a.Y) && GenericMathHelper.Equal(target.Z, z), "Vector3.constructor (Vector2,z) did not return the expected value.");
        }

        // A test for Vector3 ()
        // Constructor with no parameter
        [Fact]
        public void Vector3ConstructorTest3()
        {
            GVector3 a = new GVector3();

            Assert.Equal(0.0f, (GScalar)a.X);
            Assert.Equal(0.0f, (GScalar)a.Y);
            Assert.Equal(0.0f, (GScalar)a.Z);
        }

        // A test for Vector2 (scalar, scalar)
        // Constructor with special floating values
        [Fact]
        public void Vector3ConstructorTest4()
        {
            GVector3 target = new GVector3(GScalar.NaN, GScalar.MaxValue, GScalar.PositiveInfinity);

            Assert.True(GScalar.IsNaN(target.X), "Vector3.constructor (Vector3) did not return the expected value.");
            Assert.True(Equals(GScalar.MaxValue, (GScalar)target.Y), "Vector3.constructor (Vector3) did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(target.Z), "Vector3.constructor (Vector3) did not return the expected value.");
        }

        // A test for Vector3 (ReadOnlySpan<scalar>)
        [Fact]
        public void Vector3ConstructorTest6()
        {
            GScalar value = 1.0f;
            GVector3 target = new GVector3(new[] { value, value, value });
            GVector3 expected = new GVector3(value);

            Assert.Equal(expected, target);
            Assert.Throws<IndexOutOfRangeException>(() => new GVector3(new GScalar[2]));
        }

        // A test for Add (Vector3, Vector3)
        [Fact]
        public void Vector3AddTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(5.0f, 6.0f, 7.0f);

            GVector3 expected = new GVector3(6.0f, 8.0f, 10.0f);
            GVector3 actual;

            actual = Vector3S.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector3, scalar)
        [Fact]
        public void Vector3DivideTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar div = 2.0f;
            GVector3 expected = new GVector3(0.5f, 1.0f, 1.5f);
            GVector3 actual;
            actual = Vector3S.Divide(a, div);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Divide (Vector3, Vector3)
        [Fact]
        public void Vector3DivideTest1()
        {
            GVector3 a = new GVector3(1.0f, 6.0f, 7.0f);
            GVector3 b = new GVector3(5.0f, 2.0f, 3.0f);

            GVector3 expected = new GVector3(1.0f / 5.0f, 6.0f / 2.0f, 7.0f / 3.0f);
            GVector3 actual;

            actual = Vector3S.Divide(a, b);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector3EqualsTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(1.0f, 2.0f, 3.0f);

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

        // A test for Multiply (Vector3, scalar)
        [Fact]
        public void Vector3MultiplyTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            const GScalar factor = 2.0f;
            GVector3 expected = new GVector3(2.0f, 4.0f, 6.0f);
            GVector3 actual = Vector3S.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (scalar, Vector3)
        [Fact]
        public static void Vector3MultiplyTest2()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            const GScalar factor = 2.0f;
            GVector3 expected = new GVector3(2.0f, 4.0f, 6.0f);
            GVector3 actual = Vector3S.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector3, Vector3)
        [Fact]
        public void Vector3MultiplyTest3()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(5.0f, 6.0f, 7.0f);

            GVector3 expected = new GVector3(5.0f, 12.0f, 21.0f);
            GVector3 actual;

            actual = Vector3S.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Plus (Vector3)
        [Fact]
        public void Vector3PlusTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GVector3 expected = a;
            GVector3 actual;

            actual = Vector3S.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector3)
        [Fact]
        public void Vector3NegateTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);

            GVector3 expected = new GVector3(-1.0f, -2.0f, -3.0f);
            GVector3 actual;

            actual = Vector3S.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector3, Vector3)
        [Fact]
        public void Vector3InequalityTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(1.0f, 2.0f, 3.0f);

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

        // A test for operator == (Vector3, Vector3)
        [Fact]
        public void Vector3EqualityTest()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(1.0f, 2.0f, 3.0f);

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

        // A test for Subtract (Vector3, Vector3)
        [Fact]
        public void Vector3SubtractTest()
        {
            GVector3 a = new GVector3(1.0f, 6.0f, 3.0f);
            GVector3 b = new GVector3(5.0f, 2.0f, 3.0f);

            GVector3 expected = new GVector3(-4.0f, 4.0f, 0.0f);
            GVector3 actual;

            actual = Vector3S.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for One
        [Fact]
        public void Vector3OneTest()
        {
            GVector3 val = new GVector3(1.0f, 1.0f, 1.0f);
            Assert.Equal(val, GVector3.One);
        }

        // A test for UnitX
        [Fact]
        public void Vector3UnitXTest()
        {
            GVector3 val = new GVector3(1.0f, 0.0f, 0.0f);
            Assert.Equal(val, GVector3.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector3UnitYTest()
        {
            GVector3 val = new GVector3(0.0f, 1.0f, 0.0f);
            Assert.Equal(val, GVector3.UnitY);
        }

        // A test for UnitZ
        [Fact]
        public void Vector3UnitZTest()
        {
            GVector3 val = new GVector3(0.0f, 0.0f, 1.0f);
            Assert.Equal(val, GVector3.UnitZ);
        }

        // A test for Zero
        [Fact]
        public void Vector3ZeroTest()
        {
            GVector3 val = new GVector3(0.0f, 0.0f, 0.0f);
            Assert.Equal(val, GVector3.Zero);
        }

        // A test for Equals (Vector3)
        [Fact]
        public void Vector3EqualsTest1()
        {
            GVector3 a = new GVector3(1.0f, 2.0f, 3.0f);
            GVector3 b = new GVector3(1.0f, 2.0f, 3.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10.0f };
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Vector3 (scalar)
        [Fact]
        public void Vector3ConstructorTest5()
        {
            GScalar value = 1.0f;
            GVector3 target = new GVector3(value);

            GVector3 expected = new GVector3(value, value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new GVector3(value);
            expected = new GVector3(value, value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector3 comparison involving NaN values
        [Fact]
        public void Vector3EqualsNanTest()
        {
            GVector3 a = new GVector3(GScalar.NaN, 0, 0);
            GVector3 b = new GVector3(0, GScalar.NaN, 0);
            GVector3 c = new GVector3(0, 0, GScalar.NaN);

            Assert.False(a == GVector3.Zero);
            Assert.False(b == GVector3.Zero);
            Assert.False(c == GVector3.Zero);

            Assert.True(a != GVector3.Zero);
            Assert.True(b != GVector3.Zero);
            Assert.True(c != GVector3.Zero);

            Assert.False(a.Equals(GVector3.Zero));
            Assert.False(b.Equals(GVector3.Zero));
            Assert.False(c.Equals(GVector3.Zero));

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
        }

        [Fact]
        public void Vector3AbsTest()
        {
            GVector3 v1 = new GVector3(-2.5f, 2.0f, 0.5f);
            GVector3 v3 = Vector3S.Abs(new GVector3(0.0f, GScalar.NegativeInfinity, GScalar.NaN));
            GVector3 v = Vector3S.Abs(v1);
            Assert.Equal(2.5f, (GScalar)v.X);
            Assert.Equal(2.0f, (GScalar)v.Y);
            Assert.Equal(0.5f, (GScalar)v.Z);
            Assert.Equal(0.0f, (GScalar)v3.X);
            Assert.Equal(GScalar.PositiveInfinity, (GScalar)v3.Y);
            Assert.Equal(GScalar.NaN, (GScalar)v3.Z);
        }

        [Fact]
        public void Vector3SqrtTest()
        {
            GVector3 a = new GVector3(-2.5f, 2.0f, 0.5f);
            GVector3 b = new GVector3(5.5f, 4.5f, 16.5f);
            Assert.Equal(2, (int)Vector3S.SquareRoot(b).X);
            Assert.Equal(2, (int)Vector3S.SquareRoot(b).Y);
            Assert.Equal(4, (int)Vector3S.SquareRoot(b).Z);
            Assert.Equal(GScalar.NaN, (GScalar)Vector3S.SquareRoot(a).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector3SizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 3, sizeof(GVector3));
            Assert.Equal(scalarSize * 6, sizeof(Vector3_2x));
            Assert.Equal(scalarSize * 4, sizeof(Vector3PlusScalar));
            Assert.Equal(scalarSize * 8, sizeof(Vector3PlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3_2x
        {
            private GVector3 _a;
            private GVector3 _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3PlusScalar
        {
            private GVector3 _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Vector3PlusScalar_2x
        {
            private Vector3PlusScalar _a;
            private Vector3PlusScalar _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            GVector3 v3 = new GVector3(4f, 5f, 6f);
            v3 = v3 with { X = 1.0f, Y = 2.0f, Z = 3.0f };
            Assert.Equal(1.0f, (GScalar)v3.X);
            Assert.Equal(2.0f, (GScalar)v3.Y);
            Assert.Equal(3.0f, (GScalar)v3.Z);
            GVector3 v4 = v3 with { Y = 0.5f, Z = 2.2f };
            Assert.Equal(1.0f, (GScalar)v4.X);
            Assert.Equal(0.5f, (GScalar)v4.Y);
            Assert.Equal(2.2f, (GScalar)v4.Z);
            Assert.Equal(2.0f, (GScalar)v3.Y);

            GVector3 before = new GVector3(1f, 2f, 3f);
            GVector3 after = before;
            after = after with { X = 500.0f };
            Assert.NotEqual(before, after);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector = evo.FieldVector with { X = 5.0f, Y = 5.0f, Z = 5.0f };
            Assert.Equal(5.0f, (GScalar)evo.FieldVector.X);
            Assert.Equal(5.0f, (GScalar)evo.FieldVector.Y);
            Assert.Equal(5.0f, (GScalar)evo.FieldVector.Z);
        }

        private class EmbeddedVectorObject
        {
            public GVector3 FieldVector;
        }
    }
}
