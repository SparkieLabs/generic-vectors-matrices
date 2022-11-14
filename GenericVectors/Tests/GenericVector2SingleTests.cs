// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using GMatrix3x2 = System.Numerics.Matrix3x2<float>;
using GMatrix4x4 = System.Numerics.Matrix4x4<float>;
using GQuaternion = System.Numerics.Quaternion<float>;
using GScalar = System.Single;
using GVector2 = System.Numerics.Vector2<float>;

namespace System.Numerics.Tests
{
    public class GenericVector2SingleTests
    {
        [Fact]
        public void Vector2MarshalSizeTest()
        {
            int scalarSize = sizeof(GScalar);
            Assert.Equal(scalarSize * 2, Marshal.SizeOf(new GVector2()));
        }

        [Fact]
        public void Vector2CopyToTest()
        {
            GVector2 v1 = new GVector2(2.0f, 3.0f);

            GScalar[] a = new GScalar[3];
            GScalar[] b = new GScalar[2];

            Assert.Throws<NullReferenceException>(() => v1.CopyTo(null!, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
            AssertExtensions.Throws<ArgumentException>(null, () => v1.CopyTo(a, 2));

            v1.CopyTo(a, 1);
            v1.CopyTo(b);
            Assert.Equal(0.0, a[0]);
            Assert.Equal(2.0, a[1]);
            Assert.Equal(3.0, a[2]);
            Assert.Equal(2.0, b[0]);
            Assert.Equal(3.0, b[1]);
        }

        [Fact]
        public void Vector2CopyToSpanTest()
        {
            GVector2 vector = new GVector2(1.0f, 2.0f);
            Span<GScalar> destination = new GScalar[2];

            Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<GScalar>(new GScalar[1])));
            vector.CopyTo(destination);

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2TryCopyToTest()
        {
            GVector2 vector = new GVector2(1.0f, 2.0f);
            Span<GScalar> destination = new GScalar[2];

            Assert.False(vector.TryCopyTo(new Span<GScalar>(new GScalar[1])));
            Assert.True(vector.TryCopyTo(destination));

            Assert.Equal(1.0f, (GScalar)vector.X);
            Assert.Equal(2.0f, (GScalar)vector.Y);
            Assert.Equal(vector.X, destination[0]);
            Assert.Equal(vector.Y, destination[1]);
        }

        [Fact]
        public void Vector2GetHashCodeTest()
        {
            GVector2 v1 = new GVector2(2.0f, 3.0f);
            GVector2 v2 = new GVector2(2.0f, 3.0f);
            GVector2 v3 = new GVector2(3.0f, 2.0f);
            Assert.Equal(v1.GetHashCode(), v1.GetHashCode());
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
            Assert.NotEqual(v1.GetHashCode(), v3.GetHashCode());
            GVector2 v4 = new GVector2(0.0f, 0.0f);
            GVector2 v6 = new GVector2(1.0f, 0.0f);
            GVector2 v7 = new GVector2(0.0f, 1.0f);
            GVector2 v8 = new GVector2(1.0f, 1.0f);
            Assert.NotEqual(v4.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v7.GetHashCode());
            Assert.NotEqual(v4.GetHashCode(), v8.GetHashCode());
            Assert.NotEqual(v7.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v6.GetHashCode());
            Assert.NotEqual(v8.GetHashCode(), v7.GetHashCode());
        }

        [Fact]
        public void Vector2ToStringTest()
        {
            string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            CultureInfo enUsCultureInfo = new CultureInfo("en-US");

            GVector2 v1 = new GVector2(2.0f, 3.0f);

            string v1str = v1.ToString();
            string expectedv1 = string.Format(CultureInfo.CurrentCulture
                , "<{1:G}{0} {2:G}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1, v1str);

            string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
            string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv1formatted, v1strformatted);

            string v2strformatted = v1.ToString("c", enUsCultureInfo);
            string expectedv2formatted = string.Format(enUsCultureInfo
                , "<{1:c}{0} {2:c}>"
                , new object[] { enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3 });
            Assert.Equal(expectedv2formatted, v2strformatted);

            string v3strformatted = v1.ToString("c");
            string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
                , "<{1:c}{0} {2:c}>"
                , new object[] { separator, 2, 3 });
            Assert.Equal(expectedv3formatted, v3strformatted);
        }

        // A test for Distance (Vector2, Vector2)
        [Fact]
        public void Vector2DistanceTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(3.0f, 4.0f);

            GScalar expected = (GScalar)Math.Sqrt(8);
            GScalar actual;

            actual = Vector2S.Distance(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Distance did not return the expected value.");
        }

        // A test for Distance (Vector2, Vector2)
        // Distance from the same point
        [Fact]
        public void Vector2DistanceTest2()
        {
            GVector2 a = new GVector2(1.051f, 2.05f);
            GVector2 b = new GVector2(1.051f, 2.05f);

            GScalar actual = Vector2S.Distance(a, b);
            Assert.Equal(0.0f, actual);
        }

        // A test for DistanceSquared (Vector2, Vector2)
        [Fact]
        public void Vector2DistanceSquaredTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(3.0f, 4.0f);

            GScalar expected = 8.0f;
            GScalar actual;

            actual = Vector2S.DistanceSquared(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.DistanceSquared did not return the expected value.");
        }

        // A test for Dot (Vector2, Vector2)
        [Fact]
        public void Vector2DotTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(3.0f, 4.0f);

            GScalar expected = 11.0f;
            GScalar actual;

            actual = Vector2S.Dot(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Dot did not return the expected value.");
        }

        // A test for Dot (Vector2, Vector2)
        // Dot test for perpendicular vector
        [Fact]
        public void Vector2DotTest1()
        {
            GVector2 a = new GVector2(1.55f, 1.55f);
            GVector2 b = new GVector2(-1.55f, 1.55f);

            GScalar expected = 0.0f;
            GScalar actual = Vector2S.Dot(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Dot (Vector2, Vector2)
        // Dot test with specail scalar values
        [Fact]
        public void Vector2DotTest2()
        {
            GVector2 a = new GVector2(GScalar.MinValue, GScalar.MinValue);
            GVector2 b = new GVector2(GScalar.MaxValue, GScalar.MaxValue);

            GScalar actual = Vector2S.Dot(a, b);
            Assert.True(GScalar.IsNegativeInfinity(actual), "Vector2.Dot did not return the expected value.");
        }

        // A test for Length ()
        [Fact]
        public void Vector2LengthTest()
        {
            GVector2 a = new GVector2(2.0f, 4.0f);

            GVector2 target = a;

            GScalar expected = (GScalar)Math.Sqrt(20);
            GScalar actual;

            actual = target.Length();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Length did not return the expected value.");
        }

        // A test for Length ()
        // Length test where length is zero
        [Fact]
        public void Vector2LengthTest1()
        {
            GVector2 target = new GVector2();
            target = target with { X = 0.0f, Y = 0.0f };

            GScalar expected = 0.0f;
            GScalar actual;

            actual = target.Length();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Length did not return the expected value.");
        }

        // A test for LengthSquared ()
        [Fact]
        public void Vector2LengthSquaredTest()
        {
            GVector2 a = new GVector2(2.0f, 4.0f);

            GVector2 target = a;

            GScalar expected = 20.0f;
            GScalar actual;

            actual = target.LengthSquared();

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.LengthSquared did not return the expected value.");
        }

        // A test for LengthSquared ()
        // LengthSquared test where the result is zero
        [Fact]
        public void Vector2LengthSquaredTest1()
        {
            GVector2 a = new GVector2(0.0f, 0.0f);

            GScalar expected = 0.0f;
            GScalar actual = a.LengthSquared();

            Assert.Equal(expected, actual);
        }

        // A test for Min (Vector2, Vector2)
        [Fact]
        public void Vector2MinTest()
        {
            GVector2 a = new GVector2(-1.0f, 4.0f);
            GVector2 b = new GVector2(2.0f, 1.0f);

            GVector2 expected = new GVector2(-1.0f, 1.0f);
            GVector2 actual;
            actual = Vector2S.Min(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Min did not return the expected value.");
        }

        [Fact]
        public void Vector2MinMaxCodeCoverageTest()
        {
            GVector2 min = new GVector2(0, 0);
            GVector2 max = new GVector2(1, 1);
            GVector2 actual;

            // Min.
            actual = Vector2S.Min(min, max);
            Assert.Equal(actual, min);

            actual = Vector2S.Min(max, min);
            Assert.Equal(actual, min);

            // Max.
            actual = Vector2S.Max(min, max);
            Assert.Equal(actual, max);

            actual = Vector2S.Max(max, min);
            Assert.Equal(actual, max);
        }

        // A test for Max (Vector2, Vector2)
        [Fact]
        public void Vector2MaxTest()
        {
            GVector2 a = new GVector2(-1.0f, 4.0f);
            GVector2 b = new GVector2(2.0f, 1.0f);

            GVector2 expected = new GVector2(2.0f, 4.0f);
            GVector2 actual;
            actual = Vector2S.Max(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Max did not return the expected value.");
        }

        // A test for Clamp (Vector2, Vector2, Vector2)
        [Fact]
        public void Vector2ClampTest()
        {
            GVector2 a = new GVector2(0.5f, 0.3f);
            GVector2 min = new GVector2(0.0f, 0.1f);
            GVector2 max = new GVector2(1.0f, 1.1f);

            // Normal case.
            // Case N1: specified value is in the range.
            GVector2 expected = new GVector2(0.5f, 0.3f);
            GVector2 actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");
            // Normal case.
            // Case N2: specified value is bigger than max value.
            a = new GVector2(2.0f, 3.0f);
            expected = max;
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");
            // Case N3: specified value is smaller than max value.
            a = new GVector2(-1.0f, -2.0f);
            expected = min;
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");
            // Case N4: combination case.
            a = new GVector2(-2.0f, 4.0f);
            expected = new GVector2(min.X, max.Y);
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");
            // User specified min value is bigger than max value.
            max = new GVector2(0.0f, 0.1f);
            min = new GVector2(1.0f, 1.1f);

            // Case W1: specified value is in the range.
            a = new GVector2(0.5f, 0.3f);
            expected = max;
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");

            // Normal case.
            // Case W2: specified value is bigger than max and min value.
            a = new GVector2(2.0f, 3.0f);
            expected = max;
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");

            // Case W3: specified value is smaller than min and max value.
            a = new GVector2(-1.0f, -2.0f);
            expected = max;
            actual = Vector2S.Clamp(a, min, max);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Clamp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        [Fact]
        public void Vector2LerpTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(3.0f, 4.0f);

            GScalar t = 0.5f;

            GVector2 expected = new GVector2(2.0f, 3.0f);
            GVector2 actual;
            actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with factor zero
        [Fact]
        public void Vector2LerpTest1()
        {
            GVector2 a = new GVector2(0.0f, 0.0f);
            GVector2 b = new GVector2(3.18f, 4.25f);

            GScalar t = 0.0f;
            GVector2 expected = GVector2.Zero;
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with factor one
        [Fact]
        public void Vector2LerpTest2()
        {
            GVector2 a = new GVector2(0.0f, 0.0f);
            GVector2 b = new GVector2(3.18f, 4.25f);

            GScalar t = 1.0f;
            GVector2 expected = new GVector2(3.18f, 4.25f);
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with factor > 1
        [Fact]
        public void Vector2LerpTest3()
        {
            GVector2 a = new GVector2(0.0f, 0.0f);
            GVector2 b = new GVector2(3.18f, 4.25f);

            GScalar t = 2.0f;
            GVector2 expected = b * 2.0f;
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with factor < 0
        [Fact]
        public void Vector2LerpTest4()
        {
            GVector2 a = new GVector2(0.0f, 0.0f);
            GVector2 b = new GVector2(3.18f, 4.25f);

            GScalar t = -2.0f;
            GVector2 expected = -(b * 2.0f);
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with special scalar value
        [Fact]
        public void Vector2LerpTest5()
        {
            GVector2 a = new GVector2(45.67f, 90.0f);
            GVector2 b = new GVector2(GScalar.PositiveInfinity, GScalar.NegativeInfinity);

            GScalar t = 0.408f;
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GScalar.IsPositiveInfinity(actual.X), "Vector2.Lerp did not return the expected value.");
            Assert.True(GScalar.IsNegativeInfinity(actual.Y), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test from the same point
        [Fact]
        public void Vector2LerpTest6()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(1.0f, 2.0f);

            GScalar t = 0.5f;

            GVector2 expected = new GVector2(1.0f, 2.0f);
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        [Fact]
        public void Vector2LerpTest7()
        {
            GVector2 a = new GVector2(0.44728136f);
            GVector2 b = new GVector2(0.46345946f);

            GScalar t = 0.26402435f;

            GVector2 expected = new GVector2(0.45155275f);
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Lerp (Vector2, Vector2, scalar)
        // Lerp test with values known to be innacurate with the old lerp impl
        // (Old code incorrectly gets 0.33333588)
        [Fact]
        public void Vector2LerpTest8()
        {
            GVector2 a = new GVector2(-100);
            GVector2 b = new GVector2(0.33333334f);

            GScalar t = 1f;

            GVector2 expected = new GVector2(0.33333334f);
            GVector2 actual = Vector2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Lerp did not return the expected value.");
        }

        // A test for Transform(Vector2, Matrix4x4)
        [Fact]
        public void Vector2TransformTest()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector2 expected = new GVector2(10.316987f, 22.183012f);
            GVector2 actual;

            actual = Vector2S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for Transform(Vector2, Matrix3x2)
        [Fact]
        public void Vector2Transform3x2Test()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GMatrix3x2 m = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30.0f));
            m = m with { M31 = 10.0f, M32 = 20.0f };

            GVector2 expected = new GVector2(9.866025f, 22.23205f);
            GVector2 actual;

            actual = Vector2S.Transform(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2, Matrix4x4)
        [Fact]
        public void Vector2TransformNormalTest()
        {
            GVector2 v = new GVector2(1.0f, 2.0f);
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10.0f, M42 = 20.0f, M43 = 30.0f };

            GVector2 expected = new GVector2(0.3169873f, 2.18301272f);
            GVector2 actual;

            actual = Vector2S.TransformNormal(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Tranform did not return the expected value.");
        }

        // A test for TransformNormal (Vector2, Matrix3x2)
        [Fact]
        public void Vector2TransformNormal3x2Test()
        {
            GVector2 v = new GVector2(1, 2);
            GMatrix3x2 m = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30.0f));
            m = m with { M31 = 10.0f, M32 = 20.0f };

            GVector2 expected = new GVector2(-0.133974612f, 2.232051f);
            GVector2 actual;

            actual = Vector2S.TransformNormal(v, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        [Fact]
        public void Vector2TransformByQuaternionTest()
        {
            GVector2 v = new GVector2(1, 2);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GVector2 expected = Vector2S.Transform(v, m);
            GVector2 actual = Vector2S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        // Transform Vector2 with zero quaternion
        [Fact]
        public void Vector2TransformByQuaternionTest1()
        {
            GVector2 v = new GVector2(1, 2);
            GQuaternion q = new GQuaternion();
            GVector2 expected = v;

            GVector2 actual = Vector2S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for Transform (Vector2, Quaternion)
        // Transform Vector2 with identity quaternion
        [Fact]
        public void Vector2TransformByQuaternionTest2()
        {
            GVector2 v = new GVector2(1, 2);
            GQuaternion q = GQuaternion.Identity;
            GVector2 expected = v;

            GVector2 actual = Vector2S.Transform(v, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Transform did not return the expected value.");
        }

        // A test for Normalize (Vector2)
        [Fact]
        public void Vector2NormalizeTest()
        {
            GVector2 a = new GVector2(2, 3);
            GVector2 expected = new GVector2(0.554700196225229122018341733457f, 0.8320502943378436830275126001855f);
            GVector2 actual;

            actual = Vector2S.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2)
        // Normalize zero length vector
        [Fact]
        public void Vector2NormalizeTest1()
        {
            GVector2 a = new GVector2(); // no parameter, default to 0.0f
            GVector2 actual = Vector2S.Normalize(a);
            Assert.True(GScalar.IsNaN(actual.X) && GScalar.IsNaN(actual.Y), "Vector2.Normalize did not return the expected value.");
        }

        // A test for Normalize (Vector2)
        // Normalize infinite length vector
        [Fact]
        public void Vector2NormalizeTest2()
        {
            GVector2 a = new GVector2(GScalar.MaxValue, GScalar.MaxValue);
            GVector2 actual = Vector2S.Normalize(a);
            GVector2 expected = new GVector2(0, 0);
            Assert.Equal(expected, actual);
        }

        // A test for operator - (Vector2)
        [Fact]
        public void Vector2UnaryNegationTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GVector2 expected = new GVector2(-1.0f, -2.0f);
            GVector2 actual;

            actual = -a;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator - did not return the expected value.");
        }



        // A test for operator - (Vector2)
        // Negate test with special scalar value
        [Fact]
        public void Vector2UnaryNegationTest1()
        {
            GVector2 a = new GVector2(GScalar.PositiveInfinity, GScalar.NegativeInfinity);

            GVector2 actual = -a;

            Assert.True(GScalar.IsNegativeInfinity(actual.X), "Vector2.operator - did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Y), "Vector2.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2)
        // Negate test with special scalar value
        [Fact]
        public void Vector2UnaryNegationTest2()
        {
            GVector2 a = new GVector2(GScalar.NaN, 0.0f);
            GVector2 actual = -a;

            Assert.True(GScalar.IsNaN(actual.X), "Vector2.operator - did not return the expected value.");
            Assert.True(GScalar.Equals((GScalar)0, (GScalar)actual.Y), "Vector2.operator - did not return the expected value.");
        }

        // A test for operator - (Vector2, Vector2)
        [Fact]
        public void Vector2SubtractionTest()
        {
            GVector2 a = new GVector2(1.0f, 3.0f);
            GVector2 b = new GVector2(2.0f, 1.5f);

            GVector2 expected = new GVector2(-1.0f, 1.5f);
            GVector2 actual;

            actual = a - b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator - did not return the expected value.");
        }

        // A test for operator * (Vector2, scalar)
        [Fact]
        public void Vector2MultiplyOperatorTest()
        {
            GVector2 a = new GVector2(2.0f, 3.0f);
            const GScalar factor = 2.0f;

            GVector2 expected = new GVector2(4.0f, 6.0f);
            GVector2 actual;

            actual = a * factor;
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator * did not return the expected value.");
        }

        // A test for operator * (scalar, Vector2)
        [Fact]
        public void Vector2MultiplyOperatorTest2()
        {
            GVector2 a = new GVector2(2.0f, 3.0f);
            const GScalar factor = 2.0f;

            GVector2 expected = new GVector2(4.0f, 6.0f);
            GVector2 actual;

            actual = factor * a;
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator * did not return the expected value.");
        }

        // A test for operator * (Vector2, Vector2)
        [Fact]
        public void Vector2MultiplyOperatorTest3()
        {
            GVector2 a = new GVector2(2.0f, 3.0f);
            GVector2 b = new GVector2(4.0f, 5.0f);

            GVector2 expected = new GVector2(8.0f, 15.0f);
            GVector2 actual;

            actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator * did not return the expected value.");
        }

        // A test for operator / (Vector2, scalar)
        [Fact]
        public void Vector2DivisionTest()
        {
            GVector2 a = new GVector2(2.0f, 3.0f);

            GScalar div = 2.0f;

            GVector2 expected = new GVector2(1.0f, 1.5f);
            GVector2 actual;

            actual = a / div;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2, Vector2)
        [Fact]
        public void Vector2DivisionTest1()
        {
            GVector2 a = new GVector2(2.0f, 3.0f);
            GVector2 b = new GVector2(4.0f, 5.0f);

            GVector2 expected = new GVector2(2.0f / 4.0f, 3.0f / 5.0f);
            GVector2 actual;

            actual = a / b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2, scalar)
        // Divide by zero
        [Fact]
        public void Vector2DivisionTest2()
        {
            GVector2 a = new GVector2(-2.0f, 3.0f);

            GScalar div = 0.0f;

            GVector2 actual = a / div;

            Assert.True(GScalar.IsNegativeInfinity(actual.X), "Vector2.operator / did not return the expected value.");
            Assert.True(GScalar.IsPositiveInfinity(actual.Y), "Vector2.operator / did not return the expected value.");
        }

        // A test for operator / (Vector2, Vector2)
        // Divide by zero
        [Fact]
        public void Vector2DivisionTest3()
        {
            GVector2 a = new GVector2(0.047f, -3.0f);
            GVector2 b = new GVector2();

            GVector2 actual = a / b;

            Assert.True(GScalar.IsInfinity(actual.X), "Vector2.operator / did not return the expected value.");
            Assert.True(GScalar.IsInfinity(actual.Y), "Vector2.operator / did not return the expected value.");
        }

        // A test for Vector2 (scalar, scalar)
        [Fact]
        public void Vector2ConstructorTest()
        {
            GScalar x = 1;
            GScalar y = 2;

            GVector2 target = new GVector2(x, y);
            Assert.True(GenericMathHelper.Equal(target.X, x) && GenericMathHelper.Equal(target.Y, y), "Vector2(x,y) constructor did not return the expected value.");
        }

        // A test for Vector2 ()
        // Constructor with no parameter
        [Fact]
        public void Vector2ConstructorTest2()
        {
            GVector2 target = new GVector2();
            Assert.Equal(0.0f, (GScalar)target.X);
            Assert.Equal(0.0f, (GScalar)target.Y);
        }

        // A test for Vector2 (scalar, scalar)
        // Constructor with special floating values
        [Fact]
        public void Vector2ConstructorTest3()
        {
            GVector2 target = new GVector2(GScalar.NaN, GScalar.MaxValue);
            Assert.Equal((GScalar)target.X, GScalar.NaN);
            Assert.Equal((GScalar)target.Y, GScalar.MaxValue);
        }

        // A test for Vector2 (scalar)
        [Fact]
        public void Vector2ConstructorTest4()
        {
            GScalar value = 1;
            GVector2 target = new GVector2(value);

            GVector2 expected = new GVector2(value, value);
            Assert.Equal(expected, target);

            value = 2.0f;
            target = new GVector2(value);
            expected = new GVector2(value, value);
            Assert.Equal(expected, target);
        }

        // A test for Vector2 (ReadOnlySpan<scalar>)
        [Fact]
        public void Vector2ConstructorTest5()
        {
            GScalar value = 1;
            Span<GScalar> span = new GScalar[] { value, value };
            GVector2 target = new GVector2(span);
            GVector2 expected = new GVector2(value);

            Assert.Equal(expected, target);
            Assert.Throws<IndexOutOfRangeException>(() => new GVector2(new GScalar[1]));
        }

        // A test for Vector2 (scalar[])
        [Fact]
        public void Vector2ConstructorTest6()
        {
            GScalar value = 1;
            GVector2 target = new GVector2(new GScalar[] { value, value });
            GVector2 expected = new GVector2(value);

            Assert.Equal(expected, target);
            Assert.Throws<IndexOutOfRangeException>(() => new GVector2(new GScalar[1]));
        }

        // A test for Vector2 (scalar[], index)
        [Fact]
        public void Vector2ConstructorTest7()
        {
            GScalar value = 1;
            GVector2 target = new GVector2(new GScalar[] { 0, value, value }, 1);
            GVector2 expected = new GVector2(value);

            Assert.Equal(expected, target);
            Assert.Throws<IndexOutOfRangeException>(() => new GVector2(new GScalar[1]));
        }

        // A test for Add (Vector2, Vector2)
        [Fact]
        public void Vector2AddTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(5.0f, 6.0f);

            GVector2 expected = new GVector2(6.0f, 8.0f);
            GVector2 actual;

            actual = Vector2S.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Vector2, scalar)
        [Fact]
        public void Vector2DivideTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GScalar div = 2.0f;
            GVector2 expected = new GVector2(0.5f, 1.0f);
            GVector2 actual;
            actual = Vector2S.Divide(a, div);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Divide (Vector2, Vector2)
        [Fact]
        public void Vector2DivideTest1()
        {
            GVector2 a = new GVector2(1.0f, 6.0f);
            GVector2 b = new GVector2(5.0f, 2.0f);

            GVector2 expected = new GVector2(1.0f / 5.0f, 6.0f / 2.0f);
            GVector2 actual;

            actual = Vector2S.Divide(a, b);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Vector2EqualsTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(1.0f, 2.0f);

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

        // A test for Multiply (Vector2, scalar)
        [Fact]
        public void Vector2MultiplyTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            const GScalar factor = 2.0f;
            GVector2 expected = new GVector2(2.0f, 4.0f);
            GVector2 actual = Vector2S.Multiply(a, factor);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (scalar, Vector2)
        [Fact]
        public void Vector2MultiplyTest2()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            const GScalar factor = 2.0f;
            GVector2 expected = new GVector2(2.0f, 4.0f);
            GVector2 actual = Vector2S.Multiply(factor, a);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Vector2, Vector2)
        [Fact]
        public void Vector2MultiplyTest3()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(5.0f, 6.0f);

            GVector2 expected = new GVector2(5.0f, 12.0f);
            GVector2 actual;

            actual = Vector2S.Multiply(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Plus (Vector2)
        [Fact]
        public void Vector2PlusTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GVector2 expected = a;
            GVector2 actual;

            actual = Vector2S.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Vector2)
        [Fact]
        public void Vector2NegateTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);

            GVector2 expected = new GVector2(-1.0f, -2.0f);
            GVector2 actual;

            actual = Vector2S.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Vector2, Vector2)
        [Fact]
        public void Vector2InequalityTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(1.0f, 2.0f);

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

        // A test for operator == (Vector2, Vector2)
        [Fact]
        public void Vector2EqualityTest()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(1.0f, 2.0f);

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

        // A test for Subtract (Vector2, Vector2)
        [Fact]
        public void Vector2SubtractTest()
        {
            GVector2 a = new GVector2(1.0f, 6.0f);
            GVector2 b = new GVector2(5.0f, 2.0f);

            GVector2 expected = new GVector2(-4.0f, 4.0f);
            GVector2 actual;

            actual = Vector2S.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for UnitX
        [Fact]
        public void Vector2UnitXTest()
        {
            GVector2 val = new GVector2(1.0f, 0.0f);
            Assert.Equal(val, GVector2.UnitX);
        }

        // A test for UnitY
        [Fact]
        public void Vector2UnitYTest()
        {
            GVector2 val = new GVector2(0.0f, 1.0f);
            Assert.Equal(val, GVector2.UnitY);
        }

        // A test for One
        [Fact]
        public void Vector2OneTest()
        {
            GVector2 val = new GVector2(1.0f, 1.0f);
            Assert.Equal(val, GVector2.One);
        }

        // A test for Zero
        [Fact]
        public void Vector2ZeroTest()
        {
            GVector2 val = new GVector2(0.0f, 0.0f);
            Assert.Equal(val, GVector2.Zero);
        }

        // A test for Equals (Vector2)
        [Fact]
        public void Vector2EqualsTest1()
        {
            GVector2 a = new GVector2(1.0f, 2.0f);
            GVector2 b = new GVector2(1.0f, 2.0f);

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

        // A test for Vector2 comparison involving NaN values
        [Fact]
        public void Vector2EqualsNanTest()
        {
            GVector2 a = new GVector2(GScalar.NaN, 0);
            GVector2 b = new GVector2(0, GScalar.NaN);

            Assert.False(a == GVector2.Zero);
            Assert.False(b == GVector2.Zero);

            Assert.True(a != GVector2.Zero);
            Assert.True(b != GVector2.Zero);

            Assert.False(a.Equals(GVector2.Zero));
            Assert.False(b.Equals(GVector2.Zero));

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
        }

        // A test for Reflect (Vector2, Vector2)
        [Fact]
        public void Vector2ReflectTest()
        {
            GVector2 a = Vector2S.Normalize(new GVector2(1.0f, 1.0f));

            // Reflect on XZ plane.
            GVector2 n = new GVector2(0.0f, 1.0f);
            GVector2 expected = new GVector2(a.X, -a.Y);
            GVector2 actual = Vector2S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Reflect did not return the expected value.");

            // Reflect on XY plane.
            n = new GVector2(0.0f, 0.0f);
            expected = new GVector2(a.X, a.Y);
            actual = Vector2S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Reflect did not return the expected value.");

            // Reflect on YZ plane.
            n = new GVector2(1.0f, 0.0f);
            expected = new GVector2(-a.X, a.Y);
            actual = Vector2S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector2, Vector2)
        // Reflection when normal and source are the same
        [Fact]
        public void Vector2ReflectTest1()
        {
            GVector2 n = new GVector2(0.45f, 1.28f);
            n = Vector2S.Normalize(n);
            GVector2 a = n;

            GVector2 expected = -n;
            GVector2 actual = Vector2S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Reflect did not return the expected value.");
        }

        // A test for Reflect (Vector2, Vector2)
        // Reflection when normal and source are negation
        [Fact]
        public void Vector2ReflectTest2()
        {
            GVector2 n = new GVector2(0.45f, 1.28f);
            n = Vector2S.Normalize(n);
            GVector2 a = -n;

            GVector2 expected = n;
            GVector2 actual = Vector2S.Reflect(a, n);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.Reflect did not return the expected value.");
        }

        [Fact]
        public void Vector2AbsTest()
        {
            GVector2 v1 = new GVector2(-2.5f, 2.0f);
            GVector2 v3 = Vector2S.Abs(new GVector2(0.0f, GScalar.NegativeInfinity));
            GVector2 v = Vector2S.Abs(v1);
            Assert.Equal(2.5f, (GScalar)v.X);
            Assert.Equal(2.0f, (GScalar)v.Y);
            Assert.Equal(0.0f, (GScalar)v3.X);
            Assert.Equal(GScalar.PositiveInfinity, (GScalar)v3.Y);
        }

        [Fact]
        public void Vector2SqrtTest()
        {
            GVector2 v1 = new GVector2(-2.5f, 2.0f);
            GVector2 v2 = new GVector2(5.5f, 4.5f);
            Assert.Equal(2, (int)Vector2S.SquareRoot(v2).X);
            Assert.Equal(2, (int)Vector2S.SquareRoot(v2).Y);
            Assert.Equal(GScalar.NaN, (GScalar)Vector2S.SquareRoot(v1).X);
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Vector2SizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 2, sizeof(GVector2));
            Assert.Equal(scalarSize * 4, sizeof(GVector_2x));
            Assert.Equal(scalarSize * 2 + scalarSize, sizeof(GVectorPlusScalar));
            Assert.Equal((scalarSize * 2 + scalarSize) * 2, sizeof(GVectorPlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct GVector_2x
        {
            private GVector2 _a;
            private GVector2 _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct GVectorPlusScalar
        {
            private GVector2 _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct GVectorPlusScalar_2x
        {
            private GVectorPlusScalar _a;
            private GVectorPlusScalar _b;
        }

        [Fact]
        public void SetFieldsTest()
        {
            GVector2 v3 = new GVector2(4f, 5f);
            v3 = v3 with { X = 1.0f };
            v3 = v3 with { Y = 2.0f };
            Assert.Equal(1.0f, (GScalar)v3.X);
            Assert.Equal(2.0f, (GScalar)v3.Y);
            GVector2 v4 = v3;
            v4 = v4 with { Y = 0.5f };
            Assert.Equal(1.0f, (GScalar)v4.X);
            Assert.Equal(0.5f, (GScalar)v4.Y);
            Assert.Equal(2.0f, (GScalar)v3.Y);
        }

        [Fact]
        public void EmbeddedVectorSetFields()
        {
            EmbeddedVectorObject evo = new EmbeddedVectorObject();
            evo.FieldVector = evo.FieldVector with { X = 5.0f };
            evo.FieldVector = evo.FieldVector with { Y = 5.0f };

            Assert.Equal(5.0f, (GScalar)evo.FieldVector.X);
            Assert.Equal(5.0f, (GScalar)evo.FieldVector.Y);
        }

        private class EmbeddedVectorObject
        {
            public GVector2 FieldVector;
        }
    }
}
