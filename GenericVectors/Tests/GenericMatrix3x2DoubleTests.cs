// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Xunit;
using GMatrix3x2 = System.Numerics.Matrix3x2<double>;
using GMatrix4x4 = System.Numerics.Matrix4x4<double>;
using GScalar = System.Double;
using GVector2 = System.Numerics.Vector2<double>;
using GVector4 = System.Numerics.Vector4<double>;

namespace System.Numerics.Tests
{
    [RequiresPreviewFeatures]
    public class GenericMatrix3x2DoubleTests
    {
        static GMatrix3x2 GenerateIncrementalMatrixNumber(GScalar value = 0)
        {
            return new(
                value + 1,
                value + 2,
                value + 3,
                value + 4,
                value + 5,
                value + 6);
        }

        static GMatrix3x2 GenerateTestMatrix()
        {
            GMatrix3x2 m = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30.0f))
                with
            {
                M31 = 111.0f,
                M32 = 222.0f
            };
            return m;
        }

        // A test for Identity
        [Fact]
        public void Matrix3x2IdentityTest()
        {
            GMatrix3x2 val = new GMatrix3x2
            {
                M11 = 1,
                M22 = 1,
            };

            Assert.True(GenericMathHelper.Equal(val, GMatrix3x2.Identity), "GMatrix3x2.Indentity was not set correctly.");
        }

        // A test for Determinant
        [Fact]
        public void Matrix3x2DeterminantTest()
        {
            GMatrix3x2 target = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30.0f));

            GScalar val = 1.0f;
            GScalar det = target.GetDeterminant();

            Assert.True(GenericMathHelper.Equal(val, det), "GMatrix3x2.Determinant was not set correctly.");
        }

        // A test for Determinant
        // Determinant test |A| = 1 / |A'|
        [Fact]
        public void Matrix3x2DeterminantTest1()
        {
            GMatrix3x2 a = new(
                5,
                2,
                12,
                6.8f,
                6.5f,
                1);
            GMatrix3x2 i;
            Assert.True(Matrix3x2S.Invert(a, out i));

            GScalar detA = a.GetDeterminant();
            GScalar detI = i.GetDeterminant();
            GScalar t = (GScalar)(1.0 / detI);

            // only accurate to 3 precision
            Assert.True(System.Math.Abs(detA - t) < 1e-3, "GMatrix3x2.Determinant was not set correctly.");

            // sanity check against 4x4 version
            Assert.Equal(new GMatrix4x4(a).GetDeterminant(), detA);
            Assert.Equal(new GMatrix4x4(i).GetDeterminant(), detI);
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertTest()
        {
            GMatrix3x2 mtx = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30.0f));

            GMatrix3x2 expected = new(
                0.8660254f,
                -0.5f,
                0.5f,
                0.8660254f,
                0,
                0);

            GMatrix3x2 actual;

            Assert.True(Matrix3x2S.Invert(mtx, out actual));
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.Invert did not return the expected value.");

            GMatrix3x2 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix3x2.Identity), "GMatrix3x2.Invert did not return the expected value.");
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertIdentityTest()
        {
            GMatrix3x2 mtx = GMatrix3x2.Identity;

            GMatrix3x2 actual;
            Assert.True(Matrix3x2S.Invert(mtx, out actual));

            Assert.True(GenericMathHelper.Equal(actual, GMatrix3x2.Identity));
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertTranslationTest()
        {
            GMatrix3x2 mtx = Matrix3x2S.CreateTranslation((GScalar)23, (GScalar)42);

            GMatrix3x2 actual;
            Assert.True(Matrix3x2S.Invert(mtx, out actual));

            GMatrix3x2 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix3x2.Identity));
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertRotationTest()
        {
            GMatrix3x2 mtx = Matrix3x2S.CreateRotation((GScalar)2.0);

            GMatrix3x2 actual;
            Assert.True(Matrix3x2S.Invert(mtx, out actual));

            GMatrix3x2 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix3x2.Identity));
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertScaleTest()
        {
            GMatrix3x2 mtx = Matrix3x2S.CreateScale((GScalar)23.0, (GScalar)(-42.0));

            GMatrix3x2 actual;
            Assert.True(Matrix3x2S.Invert(mtx, out actual));

            GMatrix3x2 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix3x2.Identity));
        }

        // A test for Invert (Matrix3x2)
        [Fact]
        public void Matrix3x2InvertAffineTest()
        {
            GMatrix3x2 mtx = Matrix3x2S.CreateRotation((GScalar)2) *
                            Matrix3x2S.CreateScale((GScalar)23, (GScalar)(-42)) *
                            Matrix3x2S.CreateTranslation((GScalar)17, (GScalar)53);

            GMatrix3x2 actual;
            Assert.True(Matrix3x2S.Invert(mtx, out actual));

            GMatrix3x2 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix3x2.Identity));
        }

        // A test for CreateRotation (scalar)
        [Fact]
        public void Matrix3x2CreateRotationTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(50.0f);

            GMatrix3x2 expected = new GMatrix3x2() with
            {
                M11 = 0.642787635f,
                M12 = 0.766044438f,
                M21 = -0.766044438f,
                M22 = 0.642787635f
            };

            GMatrix3x2 actual;
            actual = Matrix3x2S.CreateRotation(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.CreateRotation did not return the expected value.");
        }

        // A test for CreateRotation (scalar, Vector2)
        [Fact]
        public void Matrix3x2CreateRotationCenterTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(30.0f);
            GVector2 center = new GVector2(23, 42);

            GMatrix3x2 rotateAroundZero = Matrix3x2S.CreateRotation(radians, GVector2.Zero);
            GMatrix3x2 rotateAroundZeroExpected = Matrix3x2S.CreateRotation(radians);
            Assert.True(GenericMathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            GMatrix3x2 rotateAroundCenter = Matrix3x2S.CreateRotation(radians, center);
            GMatrix3x2 rotateAroundCenterExpected = Matrix3x2S.CreateTranslation(-center) * Matrix3x2S.CreateRotation(radians) * Matrix3x2S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateRotation (scalar)
        [Fact]
        public void Matrix3x2CreateRotationRightAngleTest()
        {
            // 90 degree rotations must be exact!
            GMatrix3x2 actual = Matrix3x2S.CreateRotation((GScalar)0);
            Assert.Equal(new GMatrix3x2(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(GenericMathHelper.Pi / 2));
            Assert.Equal(new GMatrix3x2(0, 1, -1, 0, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(GenericMathHelper.Pi));
            Assert.Equal(new GMatrix3x2(-1, 0, 0, -1, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(GenericMathHelper.Pi * 3 / 2));
            Assert.Equal(new GMatrix3x2(0, -1, 1, 0, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(GenericMathHelper.Pi * 2));
            Assert.Equal(new GMatrix3x2(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(GenericMathHelper.Pi * 5 / 2));
            Assert.Equal(new GMatrix3x2(0, 1, -1, 0, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation((GScalar)(-GenericMathHelper.Pi / 2));
            Assert.Equal(new GMatrix3x2(0, -1, 1, 0, 0, 0), actual);

            // But merely close-to-90 rotations should not be excessively clamped.
            GScalar delta = GenericMathHelper.ToRadians(0.01f);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi + delta);
            Assert.False(GenericMathHelper.Equal(new GMatrix3x2(-1, 0, 0, -1, 0, 0), actual));

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi - delta);
            Assert.False(GenericMathHelper.Equal(new GMatrix3x2(-1, 0, 0, -1, 0, 0), actual));
        }

        // A test for CreateRotation (scalar, Vector2)
        [Fact]
        public void Matrix3x2CreateRotationRightAngleCenterTest()
        {
            GVector2 center = new GVector2(3, 7);

            // 90 degree rotations must be exact!
            GMatrix3x2 actual = Matrix3x2S.CreateRotation(0, center);
            Assert.Equal(new GMatrix3x2(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi / 2, center);
            Assert.Equal(new GMatrix3x2(0, 1, -1, 0, 10, 4), actual);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi, center);
            Assert.Equal(new GMatrix3x2(-1, 0, 0, -1, 6, 14), actual);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi * 3 / 2, center);
            Assert.Equal(new GMatrix3x2(0, -1, 1, 0, -4, 10), actual);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi * 2, center);
            Assert.Equal(new GMatrix3x2(1, 0, 0, 1, 0, 0), actual);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi * 5 / 2, center);
            Assert.Equal(new GMatrix3x2(0, 1, -1, 0, 10, 4), actual);

            actual = Matrix3x2S.CreateRotation(-GenericMathHelper.Pi / 2, center);
            Assert.Equal(new GMatrix3x2(0, -1, 1, 0, -4, 10), actual);

            // But merely close-to-90 rotations should not be excessively clamped.
            GScalar delta = GenericMathHelper.ToRadians(0.01f);

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi + delta, center);
            Assert.False(GenericMathHelper.Equal(new GMatrix3x2(-1, 0, 0, -1, 6, 14), actual));

            actual = Matrix3x2S.CreateRotation(GenericMathHelper.Pi - delta, center);
            Assert.False(GenericMathHelper.Equal(new GMatrix3x2(-1, 0, 0, -1, 6, 14), actual));
        }

        // A test for Invert (Matrix3x2)
        // Non invertible matrix - determinant is zero - singular matrix
        [Fact]
        public void Matrix3x2InvertTest1()
        {
            GMatrix3x2 a = new GMatrix3x2
            {
                M11 = 0,
                M12 = 2,
                M21 = 0,
                M22 = 4,
                M31 = 5,
                M32 = 6,
            };

            GScalar detA = a.GetDeterminant();
            Assert.True(GenericMathHelper.Equal(detA, 0.0f), "GMatrix3x2.Invert did not return the expected value.");

            GMatrix3x2 actual;
            Assert.False(Matrix3x2S.Invert(a, out actual));

            // all the elements in Actual is NaN
            Assert.True(
                GScalar.IsNaN(actual.M11) && GScalar.IsNaN(actual.M12) &&
                GScalar.IsNaN(actual.M21) && GScalar.IsNaN(actual.M22) &&
                GScalar.IsNaN(actual.M31) && GScalar.IsNaN(actual.M32)
                , "GMatrix3x2.Invert did not return the expected value.");
        }

        // A test for Lerp (Matrix3x2, Matrix3x2, scalar)
        [Fact]
        public void Matrix3x2LerpTest()
        {
            GMatrix3x2 a = new GMatrix3x2
            {
                M11 = 11,
                M12 = 12,
                M21 = 21,
                M22 = 22,
                M31 = 31,
                M32 = 32,
            };

            GMatrix3x2 b = GenerateIncrementalMatrixNumber();

            GScalar t = 0.5f;

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 + (b.M11 - a.M11) * t,
                M12 = a.M12 + (b.M12 - a.M12) * t,

                M21 = a.M21 + (b.M21 - a.M21) * t,
                M22 = a.M22 + (b.M22 - a.M22) * t,

                M31 = a.M31 + (b.M31 - a.M31) * t,
                M32 = a.M32 + (b.M32 - a.M32) * t
            };

            GMatrix3x2 actual;
            actual = Matrix3x2S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.Lerp did not return the expected value.");
        }

        // A test for operator - (Matrix3x2)
        [Fact]
        public void Matrix3x2UnaryNegationTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = -1,
                M12 = -2,
                M21 = -3,
                M22 = -4,
                M31 = -5,
                M32 = -6,
            };

            GMatrix3x2 actual = -a;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.operator - did not return the expected value.");
        }

        // A test for operator - (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2SubtractionTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);
            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 - b.M11,
                M12 = a.M12 - b.M12,
                M21 = a.M21 - b.M21,
                M22 = a.M22 - b.M22,
                M31 = a.M31 - b.M31,
                M32 = a.M32 - b.M32
            };

            GMatrix3x2 actual = a - b;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.operator - did not return the expected value.");
        }

        // A test for operator * (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2MultiplyTest1()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 * b.M11 + a.M12 * b.M21,
                M12 = a.M11 * b.M12 + a.M12 * b.M22,

                M21 = a.M21 * b.M11 + a.M22 * b.M21,
                M22 = a.M21 * b.M12 + a.M22 * b.M22,

                M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31,
                M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32
            };

            GMatrix3x2 actual = a * b;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.operator * did not return the expected value.");

            // Sanity check by comparison with 4x4 multiply.
            a = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30f)) * Matrix3x2S.CreateTranslation((GScalar)23, (GScalar)42);
            b = Matrix3x2S.CreateScale((GScalar)3, (GScalar)7) * Matrix3x2S.CreateTranslation((GScalar)666, (GScalar)(-1));

            actual = a * b;

            GMatrix4x4 a44 = new GMatrix4x4(a);
            GMatrix4x4 b44 = new GMatrix4x4(b);
            GMatrix4x4 expected44 = a44 * b44;
            GMatrix4x4 actual44 = new GMatrix4x4(actual);

            Assert.True(GenericMathHelper.Equal(expected44, actual44), "GMatrix3x2.operator * did not return the expected value.");
        }

        // A test for operator * (Matrix3x2, Matrix3x2)
        // Multiply with identity matrix
        [Fact]
        public void Matrix3x2MultiplyTest4()
        {
            GMatrix3x2 a = new GMatrix3x2
            {
                M11 = 1,
                M12 = 2,
                M21 = 5,
                M22 = -6,
                M31 = 9,
                M32 = 10,
            };

            GMatrix3x2 b = GMatrix3x2.Identity;

            GMatrix3x2 expected = a;
            GMatrix3x2 actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.operator * did not return the expected value.");
        }

        // A test for operator + (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2AdditionTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 + b.M11,
                M12 = a.M12 + b.M12,
                M21 = a.M21 + b.M21,
                M22 = a.M22 + b.M22,
                M31 = a.M31 + b.M31,
                M32 = a.M32 + b.M32
            };

            GMatrix3x2 actual;

            actual = a + b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix3x2.operator + did not return the expected value.");
        }

        // A test for ToString ()
        [Fact]
        public void Matrix3x2ToStringTest()
        {
            GMatrix3x2 a = new GMatrix3x2
            {
                M11 = 11,
                M12 = -12,
                M21 = 21,
                M22 = 22,
                M31 = 31,
                M32 = 32,
            };

            string expected = "{ {M11:11 M12:-12} " +
                                "{M21:21 M22:22} " +
                                "{M31:31 M32:32} }";
            string actual;

            actual = a.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2AddTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 + b.M11,
                M12 = a.M12 + b.M12,
                M21 = a.M21 + b.M21,
                M22 = a.M22 + b.M22,
                M31 = a.M31 + b.M31,
                M32 = a.M32 + b.M32
            };

            GMatrix3x2 actual;

            actual = Matrix3x2S.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Matrix3x2EqualsTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            object? obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { M11 = 11 };
            obj = b;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare between different types.
            obj = new GVector4();
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 3: compare against null.
            obj = null;
            expected = false;
            actual = a.Equals(obj);
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2MultiplyTest3()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 * b.M11 + a.M12 * b.M21,
                M12 = a.M11 * b.M12 + a.M12 * b.M22,

                M21 = a.M21 * b.M11 + a.M22 * b.M21,
                M22 = a.M21 * b.M12 + a.M22 * b.M22,

                M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31,
                M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32
            };
            GMatrix3x2 actual;
            actual = Matrix3x2S.Multiply(a, b);

            Assert.Equal(expected, actual);

            // Sanity check by comparison with 4x4 multiply.
            a = Matrix3x2S.CreateRotation((GScalar)GenericMathHelper.ToRadians(30f)) * Matrix3x2S.CreateTranslation((GScalar)23, (GScalar)42);
            b = Matrix3x2S.CreateScale((GScalar)3, (GScalar)7) * Matrix3x2S.CreateTranslation((GScalar)666, (GScalar)(-1));

            actual = Matrix3x2S.Multiply(a, b);

            GMatrix4x4 a44 = new GMatrix4x4(a);
            GMatrix4x4 b44 = new GMatrix4x4(b);
            GMatrix4x4 expected44 = Matrix4x4S.Multiply(a44, b44);
            GMatrix4x4 actual44 = new GMatrix4x4(actual);

            Assert.True(GenericMathHelper.Equal(expected44, actual44), "GMatrix3x2.Multiply did not return the expected value.");
        }

        // A test for Multiply (Matrix3x2, scalar)
        [Fact]
        public void Matrix3x2MultiplyTest5()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 expected = new GMatrix3x2(3, 6, 9, 12, 15, 18);
            GMatrix3x2 actual = Matrix3x2S.Multiply(a, 3);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix3x2, scalar)
        [Fact]
        public void Matrix3x2MultiplyTest6()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 expected = new GMatrix3x2(3, 6, 9, 12, 15, 18);
            GMatrix3x2 actual = a * 3;

            Assert.Equal(expected, actual);
        }

        // A test for Plus (Matrix3x2)
        [Fact]
        public void Matrix3x2PlusTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber(0);

            GMatrix3x2 expected = a;
            GMatrix3x2 actual;

            actual = Matrix3x2S.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Matrix3x2)
        [Fact]
        public void Matrix3x2NegateTest()
        {
            GMatrix3x2 m = GenerateIncrementalMatrixNumber();

            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = -1,
                M12 = -2,
                M21 = -3,
                M22 = -4,
                M31 = -5,
                M32 = -6,
            };
            GMatrix3x2 actual;

            actual = Matrix3x2S.Negate(m);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2InequalityTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { M11 = 11 };
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2EqualityTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { M11 = 11 };
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Matrix3x2, Matrix3x2)
        [Fact]
        public void Matrix3x2SubtractTest()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber(-3.0f);
            GMatrix3x2 expected = new GMatrix3x2
            {
                M11 = a.M11 - b.M11,
                M12 = a.M12 - b.M12,
                M21 = a.M21 - b.M21,
                M22 = a.M22 - b.M22,
                M31 = a.M31 - b.M31,
                M32 = a.M32 - b.M32
            };

            GMatrix3x2 actual;
            actual = Matrix3x2S.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector2)
        [Fact]
        public void Matrix3x2CreateScaleTest1()
        {
            GVector2 scales = new GVector2(2.0f, 3.0f);
            GMatrix3x2 expected = new GMatrix3x2(
                2.0f, 0.0f,
                0.0f, 3.0f,
                0.0f, 0.0f);
            GMatrix3x2 actual = Matrix3x2S.CreateScale(scales);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector2, Vector2)
        [Fact]
        public void Matrix3x2CreateScaleCenterTest1()
        {
            GVector2 scale = new GVector2(3, 4);
            GVector2 center = new GVector2(23, 42);

            GMatrix3x2 scaleAroundZero = Matrix3x2S.CreateScale(scale, GVector2.Zero);
            GMatrix3x2 scaleAroundZeroExpected = Matrix3x2S.CreateScale(scale);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix3x2 scaleAroundCenter = Matrix3x2S.CreateScale(scale, center);
            GMatrix3x2 scaleAroundCenterExpected = Matrix3x2S.CreateTranslation(-center) * Matrix3x2S.CreateScale(scale) * Matrix3x2S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (scalar)
        [Fact]
        public void Matrix3x2CreateScaleTest2()
        {
            GScalar scale = 2.0f;
            GMatrix3x2 expected = new GMatrix3x2(
                2.0f, 0.0f,
                0.0f, 2.0f,
                0.0f, 0.0f);
            GMatrix3x2 actual = Matrix3x2S.CreateScale(scale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (scalar, Vector2)
        [Fact]
        public void Matrix3x2CreateScaleCenterTest2()
        {
            GScalar scale = 5;
            GVector2 center = new GVector2(23, 42);

            GMatrix3x2 scaleAroundZero = Matrix3x2S.CreateScale(scale, GVector2.Zero);
            GMatrix3x2 scaleAroundZeroExpected = Matrix3x2S.CreateScale(scale);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix3x2 scaleAroundCenter = Matrix3x2S.CreateScale(scale, center);
            GMatrix3x2 scaleAroundCenterExpected = Matrix3x2S.CreateTranslation(-center) * Matrix3x2S.CreateScale(scale) * Matrix3x2S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateScaleTest3()
        {
            GScalar xScale = 2.0f;
            GScalar yScale = 3.0f;
            GMatrix3x2 expected = new GMatrix3x2(
                2.0f, 0.0f,
                0.0f, 3.0f,
                0.0f, 0.0f);
            GMatrix3x2 actual = Matrix3x2S.CreateScale(xScale, yScale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (scalar, scalar, Vector2)
        [Fact]
        public void Matrix3x2CreateScaleCenterTest3()
        {
            GVector2 scale = new GVector2(3, 4);
            GVector2 center = new GVector2(23, 42);

            GMatrix3x2 scaleAroundZero = Matrix3x2S.CreateScale(scale.X, scale.Y, GVector2.Zero);
            GMatrix3x2 scaleAroundZeroExpected = Matrix3x2S.CreateScale(scale.X, scale.Y);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix3x2 scaleAroundCenter = Matrix3x2S.CreateScale(scale.X, scale.Y, center);
            GMatrix3x2 scaleAroundCenterExpected = Matrix3x2S.CreateTranslation(-center) * Matrix3x2S.CreateScale(scale.X, scale.Y) * Matrix3x2S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateTranslation (Vector2)
        [Fact]
        public void Matrix3x2CreateTranslationTest1()
        {
            GVector2 position = new GVector2(2.0f, 3.0f);
            GMatrix3x2 expected = new GMatrix3x2(
                1.0f, 0.0f,
                0.0f, 1.0f,
                2.0f, 3.0f);

            GMatrix3x2 actual = Matrix3x2S.CreateTranslation(position);
            Assert.Equal(expected, actual);
        }

        // A test for CreateTranslation (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateTranslationTest2()
        {
            GScalar xPosition = 2.0f;
            GScalar yPosition = 3.0f;

            GMatrix3x2 expected = new GMatrix3x2(
                1, 0,
                0, 1,
                2, 3);

            GMatrix3x2 actual = Matrix3x2S.CreateTranslation(xPosition, yPosition);
            Assert.Equal(expected, actual);
        }

        // A test for Translation
        [Fact]
        public void Matrix3x2TranslationTest()
        {
            GMatrix3x2 a = GenerateTestMatrix();
            GMatrix3x2 b = a;

            // Transformed vector that has same semantics of property must be same.
            GVector2 val = new GVector2(a.M31, a.M32);
            Assert.Equal(val, a.Translation);

            // Set value and get value must be same.
            val = new GVector2(1, 2);
            a = a with { M31 = val.X, M32 = val.Y };
            Assert.Equal(val, a.Translation);

            // Make sure it only modifies expected value of matrix.
            Assert.True(
                a.M11 == b.M11 && a.M12 == b.M12 &&
                a.M21 == b.M21 && a.M22 == b.M22 &&
                a.M31 != b.M31 && a.M32 != b.M32,
                "GMatrix3x2.Translation modified unexpected value of matrix.");
        }

        // A test for Equals (Matrix3x2)
        [Fact]
        public void Matrix3x2EqualsTest1()
        {
            GMatrix3x2 a = GenerateIncrementalMatrixNumber();
            GMatrix3x2 b = GenerateIncrementalMatrixNumber();

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { M11 = 11 };
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for CreateSkew (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateSkewIdentityTest()
        {
            GMatrix3x2 expected = GMatrix3x2.Identity;
            GMatrix3x2 actual = Matrix3x2S.CreateSkew((GScalar)0, (GScalar)0);
            Assert.Equal(expected, actual);
        }

        // A test for CreateSkew (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateSkewXTest()
        {
            GMatrix3x2 expected = new GMatrix3x2(1, 0, -0.414213562373095f, 1, 0, 0);
            GMatrix3x2 actual = Matrix3x2S.CreateSkew((GScalar)(-GenericMathHelper.Pi / 8), (GScalar)0);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            expected = new GMatrix3x2(1, 0, 0.414213562373095f, 1, 0, 0);
            actual = Matrix3x2S.CreateSkew((GScalar)(GenericMathHelper.Pi / 8), (GScalar)0);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            GVector2 result = Vector2S.Transform(new GVector2(0, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(0, 0), result));

            result = Vector2S.Transform(new GVector2(0, 1), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(0.414213568f, 1), result));

            result = Vector2S.Transform(new GVector2(0, -1), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(-0.414213568f, -1), result));

            result = Vector2S.Transform(new GVector2(3, 10), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(7.14213568f, 10), result));
        }

        // A test for CreateSkew (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateSkewYTest()
        {
            GMatrix3x2 expected = new GMatrix3x2(1, -0.414213562373095f, 0, 1, 0, 0);
            GMatrix3x2 actual = Matrix3x2S.CreateSkew((GScalar)0, -GenericMathHelper.Pi / 8);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            expected = new GMatrix3x2(1, 0.414213562373095f, 0, 1, 0, 0);
            actual = Matrix3x2S.CreateSkew((GScalar)0, GenericMathHelper.Pi / 8);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            GVector2 result = Vector2S.Transform(new GVector2(0, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(0, 0), result));

            result = Vector2S.Transform(new GVector2(1, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(1, 0.414213568f), result));

            result = Vector2S.Transform(new GVector2(-1, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(-1, -0.414213568f), result));

            result = Vector2S.Transform(new GVector2(10, 3), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(10, 7.14213568f), result));
        }

        // A test for CreateSkew (scalar, scalar)
        [Fact]
        public void Matrix3x2CreateSkewXYTest()
        {
            GMatrix3x2 expected = new GMatrix3x2(1, -0.414213562373095f, 1, 1, 0, 0);
            GMatrix3x2 actual = Matrix3x2S.CreateSkew((GScalar)(GenericMathHelper.Pi / 4), (GScalar)(-GenericMathHelper.Pi / 8));
            Assert.True(GenericMathHelper.Equal(expected, actual));

            GVector2 result = Vector2S.Transform(new GVector2(0, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(0, 0), result));

            result = Vector2S.Transform(new GVector2(1, 0), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(1, -0.414213562373095f), result));

            result = Vector2S.Transform(new GVector2(0, 1), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(1, 1), result));

            result = Vector2S.Transform(new GVector2(1, 1), actual);
            Assert.True(GenericMathHelper.Equal(new GVector2(2, 0.585786437626905f), result));
        }

        // A test for CreateSkew (scalar, scalar, Vector2)
        [Fact]
        public void Matrix3x2CreateSkewCenterTest()
        {
            GScalar skewX = 1, skewY = 2;
            GVector2 center = new GVector2(23, 42);

            GMatrix3x2 skewAroundZero = Matrix3x2S.CreateSkew(skewX, skewY, GVector2.Zero);
            GMatrix3x2 skewAroundZeroExpected = Matrix3x2S.CreateSkew(skewX, skewY);
            Assert.True(GenericMathHelper.Equal(skewAroundZero, skewAroundZeroExpected));

            GMatrix3x2 skewAroundCenter = Matrix3x2S.CreateSkew(skewX, skewY, center);
            GMatrix3x2 skewAroundCenterExpected = Matrix3x2S.CreateTranslation(-center) * Matrix3x2S.CreateSkew(skewX, skewY) * Matrix3x2S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(skewAroundCenter, skewAroundCenterExpected));
        }

        // A test for IsIdentity
        [Fact]
        public void Matrix3x2IsIdentityTest()
        {
            Assert.True(GMatrix3x2.Identity.IsIdentity);
            Assert.True(new GMatrix3x2(1, 0, 0, 1, 0, 0).IsIdentity);
            Assert.False(new GMatrix3x2(0, 0, 0, 1, 0, 0).IsIdentity);
            Assert.False(new GMatrix3x2(1, 1, 0, 1, 0, 0).IsIdentity);
            Assert.False(new GMatrix3x2(1, 0, 1, 1, 0, 0).IsIdentity);
            Assert.False(new GMatrix3x2(1, 0, 0, 0, 0, 0).IsIdentity);
            Assert.False(new GMatrix3x2(1, 0, 0, 1, 1, 0).IsIdentity);
            Assert.False(new GMatrix3x2(1, 0, 0, 1, 0, 1).IsIdentity);
        }

        // A test for GMatrix3x2 comparison involving NaN values
        [Fact]
        public void Matrix3x2EqualsNanTest()
        {
            GMatrix3x2 a = new GMatrix3x2(GScalar.NaN, 0, 0, 0, 0, 0);
            GMatrix3x2 b = new GMatrix3x2(0, GScalar.NaN, 0, 0, 0, 0);
            GMatrix3x2 c = new GMatrix3x2(0, 0, GScalar.NaN, 0, 0, 0);
            GMatrix3x2 d = new GMatrix3x2(0, 0, 0, GScalar.NaN, 0, 0);
            GMatrix3x2 e = new GMatrix3x2(0, 0, 0, 0, GScalar.NaN, 0);
            GMatrix3x2 f = new GMatrix3x2(0, 0, 0, 0, 0, GScalar.NaN);

            Assert.False(a == new GMatrix3x2());
            Assert.False(b == new GMatrix3x2());
            Assert.False(c == new GMatrix3x2());
            Assert.False(d == new GMatrix3x2());
            Assert.False(e == new GMatrix3x2());
            Assert.False(f == new GMatrix3x2());

            Assert.True(a != new GMatrix3x2());
            Assert.True(b != new GMatrix3x2());
            Assert.True(c != new GMatrix3x2());
            Assert.True(d != new GMatrix3x2());
            Assert.True(e != new GMatrix3x2());
            Assert.True(f != new GMatrix3x2());

            Assert.False(a.Equals(new GMatrix3x2()));
            Assert.False(b.Equals(new GMatrix3x2()));
            Assert.False(c.Equals(new GMatrix3x2()));
            Assert.False(d.Equals(new GMatrix3x2()));
            Assert.False(e.Equals(new GMatrix3x2()));
            Assert.False(f.Equals(new GMatrix3x2()));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);
            Assert.False(e.IsIdentity);
            Assert.False(f.IsIdentity);

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
            Assert.False(d.Equals(d));
            Assert.False(e.Equals(e));
            Assert.False(f.Equals(f));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Matrix3x2SizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 6, sizeof(GMatrix3x2));
            Assert.Equal(scalarSize * 12, sizeof(Matrix3x2_2x));
            Assert.Equal(scalarSize * 7, sizeof(Matrix3x2PlusScalar));
            Assert.Equal(scalarSize * 14, sizeof(Matrix3x2PlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2_2x
        {
            private GMatrix3x2 _a;
            private GMatrix3x2 _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2PlusScalar
        {
            private GMatrix3x2 _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix3x2PlusScalar_2x
        {
            private Matrix3x2PlusScalar _a;
            private Matrix3x2PlusScalar _b;
        }
    }
}
