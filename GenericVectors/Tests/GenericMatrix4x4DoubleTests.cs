// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using GMatrix3x2 = System.Numerics.Matrix3x2<double>;
using GMatrix4x4 = System.Numerics.Matrix4x4<double>;
using GPlane = System.Numerics.Plane<double>;
using GQuaternion = System.Numerics.Quaternion<double>;
using GScalar = System.Double;
using GVector3 = System.Numerics.Vector3<double>;
using GVector4 = System.Numerics.Vector4<double>;

namespace System.Numerics.Tests
{
    public class GenericMatrix4x4DoubleTests
    {
        static GMatrix4x4 GenerateIncrementalMatrixNumber(GScalar value)
        {
            return new(
                value + 1,
                value + 2,
                value + 3,
                value + 4,
                value + 5,
                value + 6,
                value + 7,
                value + 8,
                value + 9,
                value + 10,
                value + 11,
                value + 12,
                value + 13,
                value + 14,
                value + 15,
                value + 16);
        }

        static GMatrix4x4 GenerateTestMatrix()
        {
            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m.WithTranslation(new GVector3(111, 222, 333));
            return m;
        }

        // A test for Identity
        [Fact]
        public void Matrix4x4IdentityTest()
        {
            GMatrix4x4 val = new GMatrix4x4();
            val = val with
            {
                M11 = 1,
                M22 = 1,
                M33 = 1,
                M44 = 1
            };

            Assert.True(GenericMathHelper.Equal(val, GMatrix4x4.Identity), "Matrix4x4.Indentity was not set correctly.");
        }

        // A test for Determinant
        [Fact]
        public void Matrix4x4DeterminantTest()
        {
            GMatrix4x4 target =
                    Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                    Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                    Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));

            GScalar val = 1.0f;
            GScalar det = target.GetDeterminant();

            Assert.True(GenericMathHelper.Equal(val, det), "Matrix4x4.Determinant was not set correctly.");
        }

        // A test for Determinant
        // Determinant test |A| = 1 / |A'|
        [Fact]
        public void Matrix4x4DeterminantTest1()
        {
            GMatrix4x4 a = new(
                5, 2, 8.25f, 1,
                12, 6.8f, 2.14f, 9.6f,
                6.5f, 1, 3.14f, 2.22f,
                0, 0.86f, 4, 1.0f);

            GMatrix4x4 i;
            Assert.True(Matrix4x4S.Invert(a, out i));

            GScalar detA = a.GetDeterminant();
            GScalar detI = i.GetDeterminant();
            GScalar t = 1.0f / detI;

            // only accurate to 3 precision
            Assert.True(System.Math.Abs(detA - t) < 1e-3, "Matrix4x4.Determinant was not set correctly.");
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertTest()
        {
            GMatrix4x4 mtx =
                    Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                    Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                    Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.74999994f,
                M12 = -0.216506317f,
                M13 = 0.62499994f,
                M14 = 0,

                M21 = 0.433012635f,
                M22 = 0.87499994f,
                M23 = -0.216506317f,
                M24 = 0,

                M31 = -0.49999997f,
                M32 = 0.433012635f,
                M33 = 0.74999994f,
                M34 = 0,

                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 0.99999994f,
            };

            GMatrix4x4 actual;

            Assert.True(Matrix4x4S.Invert(mtx, out actual));
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.Invert did not return the expected value.");

            // Make sure M*M is identity matrix
            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity), "GMatrix4x4.Invert did not return the expected value.");
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertIdentityTest()
        {
            GMatrix4x4 mtx = GMatrix4x4.Identity;

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            Assert.True(GenericMathHelper.Equal(actual, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertTranslationTest()
        {
            GMatrix4x4 mtx = Matrix4x4S.CreateTranslation((GScalar)23, (GScalar)42, (GScalar)666);

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertRotationTest()
        {
            GMatrix4x4 mtx = Matrix4x4S.CreateFromYawPitchRoll((GScalar)3, (GScalar)4, (GScalar)5);

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertScaleTest()
        {
            GMatrix4x4 mtx = Matrix4x4S.CreateScale((GScalar)23, (GScalar)42, (GScalar)(-666));

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertProjectionTest()
        {
            GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)1, (GScalar)1.333, (GScalar)0.1, (GScalar)666);

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertAffineTest()
        {
            GMatrix4x4 mtx = Matrix4x4S.CreateFromYawPitchRoll((GScalar)3, (GScalar)4, (GScalar)5) *
                            Matrix4x4S.CreateScale((GScalar)23, (GScalar)42, (GScalar)(-666)) *
                            Matrix4x4S.CreateTranslation((GScalar)17, (GScalar)53, (GScalar)89);

            GMatrix4x4 actual;
            Assert.True(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.True(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        // A test for Invert (Matrix4x4)
        [Fact]
        public void Matrix4x4InvertRank3()
        {
            // A 4x4 Matrix having a rank of 3
            GMatrix4x4 mtx = new GMatrix4x4(1.0f, 2.0f, 3.0f, 0.0f,
                                          5.0f, 1.0f, 6.0f, 0.0f,
                                          8.0f, 9.0f, 1.0f, 0.0f,
                                          4.0f, 7.0f, 3.0f, 0.0f);

            GMatrix4x4 actual;
            Assert.False(Matrix4x4S.Invert(mtx, out actual));

            GMatrix4x4 i = mtx * actual;
            Assert.False(GenericMathHelper.Equal(i, GMatrix4x4.Identity));
        }

        static void DecomposeTest(GScalar yaw, GScalar pitch, GScalar roll, GVector3 expectedTranslation, GVector3 expectedScales)
        {
            GQuaternion expectedRotation = QuaternionS.CreateFromYawPitchRoll(GenericMathHelper.ToRadians(yaw),
                                                                            GenericMathHelper.ToRadians(pitch),
                                                                            GenericMathHelper.ToRadians(roll));

            GMatrix4x4 m = Matrix4x4S.CreateScale(expectedScales) *
                          Matrix4x4S.CreateFromQuaternion(expectedRotation) *
                          Matrix4x4S.CreateTranslation(expectedTranslation);

            GVector3 scales;
            GQuaternion rotation;
            GVector3 translation;

            bool actualResult = Matrix4x4S.Decompose(m, out scales, out rotation, out translation);
            Assert.True(actualResult, "GMatrix4x4.Decompose did not return expected value.");

            bool scaleIsZeroOrNegative = expectedScales.X <= 0 ||
                                         expectedScales.Y <= 0 ||
                                         expectedScales.Z <= 0;

            if (scaleIsZeroOrNegative)
            {
                Assert.True(GenericMathHelper.Equal(Math.Abs(expectedScales.X), Math.Abs(scales.X)), "GMatrix4x4.Decompose did not return expected value.");
                Assert.True(GenericMathHelper.Equal(Math.Abs(expectedScales.Y), Math.Abs(scales.Y)), "GMatrix4x4.Decompose did not return expected value.");
                Assert.True(GenericMathHelper.Equal(Math.Abs(expectedScales.Z), Math.Abs(scales.Z)), "GMatrix4x4.Decompose did not return expected value.");
            }
            else
            {
                Assert.True(GenericMathHelper.Equal(expectedScales, scales), string.Format("GMatrix4x4.Decompose did not return expected value Expected:{0} actual:{1}.", expectedScales, scales));
                Assert.True(GenericMathHelper.EqualRotation(expectedRotation, rotation), string.Format("GMatrix4x4.Decompose did not return expected value. Expected:{0} actual:{1}.", expectedRotation, rotation));
            }

            Assert.True(GenericMathHelper.Equal(expectedTranslation, translation), string.Format("GMatrix4x4.Decompose did not return expected value. Expected:{0} actual:{1}.", expectedTranslation, translation));
        }

        // Various rotation decompose test.
        [Fact]
        public void Matrix4x4DecomposeTest01()
        {
            DecomposeTest(10.0f, 20.0f, 30.0f, new GVector3(10, 20, 30), new GVector3(2, 3, 4));

            const GScalar step = 35.0f;

            for (GScalar yawAngle = -720.0f; yawAngle <= 720.0f; yawAngle += step)
            {
                for (GScalar pitchAngle = -720.0f; pitchAngle <= 720.0f; pitchAngle += step)
                {
                    for (GScalar rollAngle = -720.0f; rollAngle <= 720.0f; rollAngle += step)
                    {
                        DecomposeTest(yawAngle, pitchAngle, rollAngle, new GVector3(10, 20, 30), new GVector3(2, 3, 4));
                    }
                }
            }
        }

        // Various scaled matrix decompose test.
        [Fact]
        public void Matrix4x4DecomposeTest02()
        {
            DecomposeTest(10, 20, 30, new GVector3(10, 20, 30), new GVector3(2, 3, 4));

            // Various scales.
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(1, 2, 3));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(1, 3, 2));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(2, 1, 3));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(2, 3, 1));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(3, 1, 2));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(3, 2, 1));

            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3(-2, 1, 1));

            // Small scales.
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)1e-4, (GScalar)2e-4, (GScalar)3e-4));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)1e-4, (GScalar)3e-4, (GScalar)2e-4));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)2e-4, (GScalar)1e-4, (GScalar)3e-4));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)2e-4, (GScalar)3e-4, (GScalar)1e-4));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)3e-4, (GScalar)1e-4, (GScalar)2e-4));
            DecomposeTest(0, 0, 0, GVector3.Zero, new GVector3((GScalar)3e-4, (GScalar)2e-4, (GScalar)1e-4));

            // Zero scales.
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(0, 0, 0));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, 0, 0));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(0, 1, 0));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(0, 0, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(0, 1, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, 0, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, 1, 0));

            // Negative scales.
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(-1, -1, -1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, -1, -1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(-1, 1, -1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(-1, -1, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(-1, 1, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, -1, 1));
            DecomposeTest(0, 0, 0, new GVector3(10, 20, 30), new GVector3(1, 1, -1));
        }

        static void DecomposeScaleTest(GScalar sx, GScalar sy, GScalar sz)
        {
            GMatrix4x4 m = Matrix4x4S.CreateScale(sx, sy, sz);

            GVector3 expectedScales = new GVector3(sx, sy, sz);
            GVector3 scales;
            GQuaternion rotation;
            GVector3 translation;

            bool actualResult = Matrix4x4S.Decompose(m, out scales, out rotation, out translation);
            Assert.True(actualResult, "GMatrix4x4.Decompose did not return expected value.");
            Assert.True(GenericMathHelper.Equal(expectedScales, scales), "GMatrix4x4.Decompose did not return expected value.");
            Assert.True(GenericMathHelper.EqualRotation(GQuaternion.Identity, rotation), "GMatrix4x4.Decompose did not return expected value.");
            Assert.True(GenericMathHelper.Equal(GVector3.Zero, translation), "GMatrix4x4.Decompose did not return expected value.");
        }

        // Tiny scale decompose test.
        [Fact]
        public void Matrix4x4DecomposeTest03()
        {
            DecomposeScaleTest(1, 2e-4f, 3e-4f);
            DecomposeScaleTest(1, 3e-4f, 2e-4f);
            DecomposeScaleTest(2e-4f, 1, 3e-4f);
            DecomposeScaleTest(2e-4f, 3e-4f, 1);
            DecomposeScaleTest(3e-4f, 1, 2e-4f);
            DecomposeScaleTest(3e-4f, 2e-4f, 1);
        }

        [Fact]
        public void Matrix4x4DecomposeTest04()
        {
            GVector3 scales;
            GQuaternion rotation;
            GVector3 translation;

            Assert.False(Matrix4x4S.Decompose(GenerateIncrementalMatrixNumber(0), out scales, out rotation, out translation), "decompose should have failed.");
            Assert.False(Matrix4x4S.Decompose(new GMatrix4x4(Matrix3x2S.CreateSkew((GScalar)1, (GScalar)2)), out scales, out rotation, out translation), "decompose should have failed.");
        }

        // Transform by quaternion test
        [Fact]
        public void Matrix4x4TransformTest()
        {
            GMatrix4x4 target = GenerateIncrementalMatrixNumber(0);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));

            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GMatrix4x4 expected = target * m;
            GMatrix4x4 actual;
            actual = Matrix4x4S.Transform(target, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.Transform did not return the expected value.");
        }

        // A test for CreateRotationX (scalar)
        [Fact]
        public void Matrix4x4CreateRotationXTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(30.0f);

            GMatrix4x4 expected = new GMatrix4x4();
            expected = expected with
            {
                M11 = 1.0f,
                M22 = 0.8660254f,
                M23 = 0.5f,
                M32 = -0.5f,
                M33 = 0.8660254f,
                M44 = 1.0f
            };

            GMatrix4x4 actual;

            actual = Matrix4x4S.CreateRotationX(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateRotationX did not return the expected value.");
        }

        // A test for CreateRotationX (scalar)
        // CreateRotationX of zero degree
        [Fact]
        public void Matrix4x4CreateRotationXTest1()
        {
            GScalar radians = 0;

            GMatrix4x4 expected = GMatrix4x4.Identity;
            GMatrix4x4 actual = Matrix4x4S.CreateRotationX(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateRotationX did not return the expected value.");
        }

        // A test for CreateRotationX (scalar, Vector3)
        [Fact]
        public void Matrix4x4CreateRotationXCenterTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(30.0f);
            GVector3 center = new GVector3(23, 42, 66);

            GMatrix4x4 rotateAroundZero = Matrix4x4S.CreateRotationX(radians, GVector3.Zero);
            GMatrix4x4 rotateAroundZeroExpected = Matrix4x4S.CreateRotationX(radians);
            Assert.True(GenericMathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            GMatrix4x4 rotateAroundCenter = Matrix4x4S.CreateRotationX(radians, center);
            GMatrix4x4 rotateAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateRotationX(radians) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateRotationY (scalar)
        [Fact]
        public void Matrix4x4CreateRotationYTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(60.0f);

            GMatrix4x4 expected = new GMatrix4x4();
            expected = expected with
            {
                M11 = 0.49999997f,
                M13 = -0.866025448f,
                M22 = 1.0f,
                M31 = 0.866025448f,
                M33 = 0.49999997f,
                M44 = 1.0f
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreateRotationY(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateRotationY did not return the expected value.");
        }

        // A test for RotationY (scalar)
        // CreateRotationY test for negative angle
        [Fact]
        public void Matrix4x4CreateRotationYTest1()
        {
            GScalar radians = GenericMathHelper.ToRadians(-300.0f);

            GMatrix4x4 expected = new GMatrix4x4();
            expected = expected with
            {
                M11 = 0.49999997f,
                M13 = -0.866025448f,
                M22 = 1.0f,
                M31 = 0.866025448f,
                M33 = 0.49999997f,
                M44 = 1.0f
            };

            GMatrix4x4 actual = Matrix4x4S.CreateRotationY(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateRotationY did not return the expected value.");
        }

        // A test for CreateRotationY (scalar, Vector3)
        [Fact]
        public void Matrix4x4CreateRotationYCenterTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(30.0f);
            GVector3 center = new GVector3(23, 42, 66);

            GMatrix4x4 rotateAroundZero = Matrix4x4S.CreateRotationY(radians, GVector3.Zero);
            GMatrix4x4 rotateAroundZeroExpected = Matrix4x4S.CreateRotationY(radians);
            Assert.True(GenericMathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            GMatrix4x4 rotateAroundCenter = Matrix4x4S.CreateRotationY(radians, center);
            GMatrix4x4 rotateAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateRotationY(radians) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CreateFromAxisAngle(Vector3,scalar)
        [Fact]
        public void Matrix4x4CreateFromAxisAngleTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(-30.0f);

            GMatrix4x4 expected = Matrix4x4S.CreateRotationX(radians);
            GMatrix4x4 actual = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitX, radians);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            expected = Matrix4x4S.CreateRotationY(radians);
            actual = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitY, radians);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            expected = Matrix4x4S.CreateRotationZ(radians);
            actual = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitZ, radians);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            expected = Matrix4x4S.CreateFromQuaternion(QuaternionS.CreateFromAxisAngle(Vector3S.Normalize(GVector3.One), radians));
            actual = Matrix4x4S.CreateFromAxisAngle(Vector3S.Normalize(GVector3.One), radians);
            Assert.True(GenericMathHelper.Equal(expected, actual));

            const int rotCount = 16;
            for (int i = 0; i < rotCount; ++i)
            {
                GScalar latitude = (2.0f * GenericMathHelper.Pi) * (i / (GScalar)rotCount);
                for (int j = 0; j < rotCount; ++j)
                {
                    GScalar longitude = -GenericMathHelper.PiOver2 + GenericMathHelper.Pi * (j / (GScalar)rotCount);

                    GMatrix4x4 m = Matrix4x4S.CreateRotationZ(longitude) * Matrix4x4S.CreateRotationY(latitude);
                    GVector3 axis = new GVector3(m.M11, m.M12, m.M13);
                    for (int k = 0; k < rotCount; ++k)
                    {
                        GScalar rot = (2.0f * GenericMathHelper.Pi) * (k / (GScalar)rotCount);
                        expected = Matrix4x4S.CreateFromQuaternion(QuaternionS.CreateFromAxisAngle(axis, rot));
                        actual = Matrix4x4S.CreateFromAxisAngle(axis, rot);
                        Assert.True(GenericMathHelper.Equal(expected, actual));
                    }
                }
            }
        }

        [Fact]
        public void Matrix4x4CreateFromYawPitchRollTest1()
        {
            GScalar yawAngle = GenericMathHelper.ToRadians(30.0f);
            GScalar pitchAngle = GenericMathHelper.ToRadians(40.0f);
            GScalar rollAngle = GenericMathHelper.ToRadians(50.0f);

            GMatrix4x4 yaw = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitY, yawAngle);
            GMatrix4x4 pitch = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitX, pitchAngle);
            GMatrix4x4 roll = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitZ, rollAngle);

            GMatrix4x4 expected = roll * pitch * yaw;
            GMatrix4x4 actual = Matrix4x4S.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            Assert.True(GenericMathHelper.Equal(expected, actual));
        }

        // Covers more numeric regions
        [Fact]
        public void Matrix4x4CreateFromYawPitchRollTest2()
        {
            const GScalar step = 35.0f;

            for (GScalar yawAngle = -720.0f; yawAngle <= 720.0f; yawAngle += step)
            {
                for (GScalar pitchAngle = -720.0f; pitchAngle <= 720.0f; pitchAngle += step)
                {
                    for (GScalar rollAngle = -720.0f; rollAngle <= 720.0f; rollAngle += step)
                    {
                        GScalar yawRad = GenericMathHelper.ToRadians(yawAngle);
                        GScalar pitchRad = GenericMathHelper.ToRadians(pitchAngle);
                        GScalar rollRad = GenericMathHelper.ToRadians(rollAngle);
                        GMatrix4x4 yaw = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitY, yawRad);
                        GMatrix4x4 pitch = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitX, pitchRad);
                        GMatrix4x4 roll = Matrix4x4S.CreateFromAxisAngle(GVector3.UnitZ, rollRad);

                        GMatrix4x4 expected = roll * pitch * yaw;
                        GMatrix4x4 actual = Matrix4x4S.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                        Assert.True(GenericMathHelper.Equal(expected, actual), string.Format("Yaw:{0} Pitch:{1} Roll:{2}", yawAngle, pitchAngle, rollAngle));
                    }
                }
            }
        }

        // Simple shadow test.
        [Fact]
        public void Matrix4x4CreateShadowTest01()
        {
            GVector3 lightDir = GVector3.UnitY;
            GPlane GPlane = new GPlane(GVector3.UnitY, 0);

            GMatrix4x4 expected = Matrix4x4S.CreateScale((GScalar)1, (GScalar)0, (GScalar)1);

            GMatrix4x4 actual = Matrix4x4S.CreateShadow(lightDir, GPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateShadow did not returned expected value.");
        }

        // Various Plane projections.
        [Fact]
        public void Matrix4x4CreateShadowTest02()
        {
            // Complex cases.
            GPlane[] planes = {
                new GPlane( 0, 1, 0, 0 ),
                new GPlane( 1, 2, 3, 4 ),
                new GPlane( 5, 6, 7, 8 ),
                new GPlane(-1,-2,-3,-4 ),
                new GPlane(-5,-6,-7,-8 ),
            };

            GVector3[] points = {
                new GVector3( 1, 2, 3),
                new GVector3( 5, 6, 7),
                new GVector3( 8, 9, 10),
                new GVector3(-1,-2,-3),
                new GVector3(-5,-6,-7),
                new GVector3(-8,-9,-10),
            };

            foreach (GPlane p in planes)
            {
                GPlane GPlane = PlaneS.Normalize(p);

                // Try various direction of light directions.
                var testDirections = new GVector3[]
                {
                    new GVector3( -1.0f, 1.0f, 1.0f ),
                    new GVector3(  0.0f, 1.0f, 1.0f ),
                    new GVector3(  1.0f, 1.0f, 1.0f ),
                    new GVector3( -1.0f, 0.0f, 1.0f ),
                    new GVector3(  0.0f, 0.0f, 1.0f ),
                    new GVector3(  1.0f, 0.0f, 1.0f ),
                    new GVector3( -1.0f,-1.0f, 1.0f ),
                    new GVector3(  0.0f,-1.0f, 1.0f ),
                    new GVector3(  1.0f,-1.0f, 1.0f ),

                    new GVector3( -1.0f, 1.0f, 0.0f ),
                    new GVector3(  0.0f, 1.0f, 0.0f ),
                    new GVector3(  1.0f, 1.0f, 0.0f ),
                    new GVector3( -1.0f, 0.0f, 0.0f ),
                    new GVector3(  0.0f, 0.0f, 0.0f ),
                    new GVector3(  1.0f, 0.0f, 0.0f ),
                    new GVector3( -1.0f,-1.0f, 0.0f ),
                    new GVector3(  0.0f,-1.0f, 0.0f ),
                    new GVector3(  1.0f,-1.0f, 0.0f ),

                    new GVector3( -1.0f, 1.0f,-1.0f ),
                    new GVector3(  0.0f, 1.0f,-1.0f ),
                    new GVector3(  1.0f, 1.0f,-1.0f ),
                    new GVector3( -1.0f, 0.0f,-1.0f ),
                    new GVector3(  0.0f, 0.0f,-1.0f ),
                    new GVector3(  1.0f, 0.0f,-1.0f ),
                    new GVector3( -1.0f,-1.0f,-1.0f ),
                    new GVector3(  0.0f,-1.0f,-1.0f ),
                    new GVector3(  1.0f,-1.0f,-1.0f ),
                };

                foreach (GVector3 lightDirInfo in testDirections)
                {
                    if (lightDirInfo.Length() < 0.1f)
                        continue;
                    GVector3 lightDir = Vector3S.Normalize(lightDirInfo);

                    if (PlaneS.DotNormal(GPlane, lightDir) < 0.1f)
                        continue;

                    GMatrix4x4 m = Matrix4x4S.CreateShadow(lightDir, GPlane);
                    GVector3 pp = -GPlane.Distance * GPlane.Normal; // origin of the GPlane.

                    //
                    foreach (GVector3 point in points)
                    {
                        GVector4 v4 = Vector4S.Transform(point, m);

                        GVector3 sp = new GVector3(v4.X, v4.Y, v4.Z) / v4.W;

                        // Make sure transformed position is on the GPlane.
                        GVector3 v = sp - pp;
                        GScalar d = Vector3S.Dot(v, GPlane.Normal);
                        Assert.True(GenericMathHelper.Equal(d, 0), "Matrix4x4S.CreateShadow did not provide expected value.");

                        // make sure direction between transformed position and original position are same as light direction.
                        if (Vector3S.Dot(point - pp, GPlane.Normal) > 0.0001f)
                        {
                            GVector3 dir = Vector3S.Normalize(point - sp);
                            Assert.True(GenericMathHelper.Equal(dir, lightDir), "Matrix4x4S.CreateShadow did not provide expected value.");
                        }
                    }
                }
            }
        }

        static void CreateReflectionTest(GPlane GPlane, GMatrix4x4 expected)
        {
            GMatrix4x4 actual = Matrix4x4S.CreateReflection(GPlane);
            Assert.True(GenericMathHelper.Equal(actual, expected), "Matrix4x4S.CreateReflection did not return expected value.");
        }

        [Fact]
        public void Matrix4x4CreateReflectionTest01()
        {
            // XY Plane.
            CreateReflectionTest(new GPlane(GVector3.UnitZ, 0), Matrix4x4S.CreateScale((GScalar)1, (GScalar)1, (GScalar)(-1)));
            // XZ Plane.
            CreateReflectionTest(new GPlane(GVector3.UnitY, 0), Matrix4x4S.CreateScale((GScalar)1, (GScalar)(-1), (GScalar)1));
            // YZ Plane.
            CreateReflectionTest(new GPlane(GVector3.UnitX, 0), Matrix4x4S.CreateScale((GScalar)(-1), (GScalar)1, (GScalar)1));

            // Complex cases.
            GPlane[] planes = {
                new GPlane( 0, 1, 0, 0 ),
                new GPlane( 1, 2, 3, 4 ),
                new GPlane( 5, 6, 7, 8 ),
                new GPlane(-1,-2,-3,-4 ),
                new GPlane(-5,-6,-7,-8 ),
            };

            GVector3[] points = {
                new GVector3( 1, 2, 3),
                new GVector3( 5, 6, 7),
                new GVector3(-1,-2,-3),
                new GVector3(-5,-6,-7),
            };

            foreach (GPlane p in planes)
            {
                GPlane GPlane = PlaneS.Normalize(p);
                GMatrix4x4 m = Matrix4x4S.CreateReflection(GPlane);
                GVector3 pp = -GPlane.Distance * GPlane.Normal; // Position on the GPlane.

                //
                foreach (GVector3 point in points)
                {
                    GVector3 rp = Vector3S.Transform(point, m);

                    // Manually compute reflection point and compare results.
                    GVector3 v = point - pp;
                    GScalar d = Vector3S.Dot(v, GPlane.Normal);
                    GVector3 vp = point - 2.0f * d * GPlane.Normal;
                    Assert.True(GenericMathHelper.Equal(rp, vp), "GMatrix4x4.Reflection did not provide expected value.");
                }
            }
        }

        // A test for CreateRotationZ (scalar)
        [Fact]
        public void Matrix4x4CreateRotationZTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(50.0f);

            GMatrix4x4 expected = new GMatrix4x4();
            expected = expected with
            {
                M11 = 0.642787635f,
                M12 = 0.766044438f,
                M21 = -0.766044438f,
                M22 = 0.642787635f,
                M33 = 1,
                M44 = 1,
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreateRotationZ(radians);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateRotationZ did not return the expected value.");
        }

        // A test for CreateRotationZ (scalar, Vector3)
        [Fact]
        public void Matrix4x4CreateRotationZCenterTest()
        {
            GScalar radians = GenericMathHelper.ToRadians(30.0f);
            GVector3 center = new GVector3(23, 42, 66);

            GMatrix4x4 rotateAroundZero = Matrix4x4S.CreateRotationZ(radians, GVector3.Zero);
            GMatrix4x4 rotateAroundZeroExpected = Matrix4x4S.CreateRotationZ(radians);
            Assert.True(GenericMathHelper.Equal(rotateAroundZero, rotateAroundZeroExpected));

            GMatrix4x4 rotateAroundCenter = Matrix4x4S.CreateRotationZ(radians, center);
            GMatrix4x4 rotateAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateRotationZ(radians) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(rotateAroundCenter, rotateAroundCenterExpected));
        }

        // A test for CrateLookAt (Vector3, Vector3, Vector3)
        [Fact]
        public void Matrix4x4CreateLookAtTest()
        {
            GVector3 cameraPosition = new GVector3(10.0f, 20.0f, 30.0f);
            GVector3 cameraTarget = new GVector3(3.0f, 2.0f, -4.0f);
            GVector3 cameraUpVector = new GVector3(0.0f, 1.0f, 0.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.979457f,
                M12 = -0.0928267762f,
                M13 = 0.179017f,

                M21 = 0,
                M22 = 0.8877481f,
                M23 = 0.460329473f,

                M31 = -0.201652914f,
                M32 = -0.450872928f,
                M33 = 0.8695112f,

                M41 = -3.74498272f,
                M42 = -3.30050683f,
                M43 = -37.0820961f,
                M44 = 1,
            };

            GMatrix4x4 actual = Matrix4x4S.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateLookAt did not return the expected value.");
        }

        // A test for CreateWorld (Vector3, Vector3, Vector3)
        [Fact]
        public void Matrix4x4CreateWorldTest()
        {
            GVector3 objectPosition = new GVector3(10.0f, 20.0f, 30.0f);
            GVector3 objectForwardDirection = new GVector3(3.0f, 2.0f, -4.0f);
            GVector3 objectUpVector = new GVector3(0.0f, 1.0f, 0.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.799999952f,
                M12 = 0,
                M13 = 0.599999964f,
                M14 = 0,

                M21 = -0.2228344f,
                M22 = 0.928476632f,
                M23 = 0.297112525f,
                M24 = 0,

                M31 = -0.557086f,
                M32 = -0.371390671f,
                M33 = 0.742781341f,
                M34 = 0,

                M41 = 10,
                M42 = 20,
                M43 = 30,
                M44 = 1,
            };

            GMatrix4x4 actual = Matrix4x4S.CreateWorld(objectPosition, objectForwardDirection, objectUpVector);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateWorld did not return the expected value.");

            Assert.Equal(objectPosition, actual.Translation);
            Assert.True(Vector3S.Dot(Vector3S.Normalize(objectUpVector), new GVector3(actual.M21, actual.M22, actual.M23)) > 0);
            Assert.True(Vector3S.Dot(Vector3S.Normalize(objectForwardDirection), new GVector3(-actual.M31, -actual.M32, -actual.M33)) > 0.999f);
        }

        // A test for CreateOrtho (scalar, scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreateOrthoTest()
        {
            GScalar width = 100.0f;
            GScalar height = 200.0f;
            GScalar zNearPlane = 1.5f;
            GScalar zFarPlane = 1000.0f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.02f,
                M22 = 0.01f,
                M33 = -0.00100150227f,
                M43 = -0.00150225335f,
                M44 = 1,
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreateOrthographic(width, height, zNearPlane, zFarPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateOrtho did not return the expected value.");
        }

        // A test for CreateOrthoOffCenter (scalar, scalar, scalar, scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreateOrthoOffCenterTest()
        {
            GScalar left = 10.0f;
            GScalar right = 90.0f;
            GScalar bottom = 20.0f;
            GScalar top = 180.0f;
            GScalar zNearPlane = 1.5f;
            GScalar zFarPlane = 1000.0f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.025f,
                M22 = 0.0125f,
                M33 = -0.00100150227f,
                M41 = -1.25f,
                M42 = -1.25f,
                M43 = -0.00150225335f,
                M44 = 1,
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateOrthoOffCenter did not return the expected value.");
        }

        // A test for CreatePerspective (scalar, scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreatePerspectiveTest()
        {
            GScalar width = 100.0f;
            GScalar height = 200.0f;
            GScalar zNearPlane = 1.5f;
            GScalar zFarPlane = 1000.0f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.03f,
                M22 = 0.015f,
                M33 = -1.00150228f,
                M34 = -1,
                M43 = -1.50225341f,
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreatePerspective(width, height, zNearPlane, zFarPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreatePerspective did not return the expected value.");
        }

        // A test for CreatePerspective (scalar, scalar, scalar, scalar)
        // CreatePerspective test where znear = zfar
        [Fact]
        public void Matrix4x4CreatePerspectiveTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GScalar width = 100.0f;
                GScalar height = 200.0f;
                GScalar zNearPlane = 0.0f;
                GScalar zFarPlane = 0.0f;

                GMatrix4x4 actual = Matrix4x4S.CreatePerspective(width, height, zNearPlane, zFarPlane);
            });
        }

        // A test for CreatePerspective (scalar, scalar, scalar, scalar)
        // CreatePerspective test where near GPlane is negative value
        [Fact]
        public void Matrix4x4CreatePerspectiveTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 actual = Matrix4x4S.CreatePerspective((GScalar)10, (GScalar)10, (GScalar)(-10), (GScalar)10);
            });
        }

        // A test for CreatePerspective (scalar, scalar, scalar, scalar)
        // CreatePerspective test where far GPlane is negative value
        [Fact]
        public void Matrix4x4CreatePerspectiveTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 actual = Matrix4x4S.CreatePerspective((GScalar)10, (GScalar)10, (GScalar)10, (GScalar)(-10));
            });
        }

        // A test for CreatePerspective (scalar, scalar, scalar, scalar)
        // CreatePerspective test where near GPlane is beyond far GPlane
        [Fact]
        public void Matrix4x4CreatePerspectiveTest4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 actual = Matrix4x4S.CreatePerspective((GScalar)10, (GScalar)10, (GScalar)10, (GScalar)1);
            });
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest()
        {
            GScalar fieldOfView = GenericMathHelper.ToRadians(30.0f);
            GScalar aspectRatio = 1280.0f / 720.0f;
            GScalar zNearPlane = 1.5f;
            GScalar zFarPlane = 1000.0f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 2.09927845f,
                M22 = 3.73205066f,
                M33 = -1.00150228f,
                M34 = -1,
                M43 = -1.50225341f,
            };
            GMatrix4x4 actual;

            actual = Matrix4x4S.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNearPlane, zFarPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreatePerspectiveFieldOfView did not return the expected value.");
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        // CreatePerspectiveFieldOfView test where filedOfView is negative value.
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)(-1), (GScalar)1, (GScalar)1, (GScalar)10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        // CreatePerspectiveFieldOfView test where filedOfView is more than pi.
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)(GenericMathHelper.Pi + 0.01), (GScalar)1, (GScalar)1, (GScalar)10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        // CreatePerspectiveFieldOfView test where nearPlaneDistance is negative value.
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)GenericMathHelper.PiOver4, (GScalar)1, (GScalar)(-1), (GScalar)10);
            });
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        // CreatePerspectiveFieldOfView test where farPlaneDistance is negative value.
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)GenericMathHelper.PiOver4, (GScalar)1, (GScalar)1, (GScalar)(-10));
            });
        }

        // A test for CreatePerspectiveFieldOfView (scalar, scalar, scalar, scalar)
        // CreatePerspectiveFieldOfView test where nearPlaneDistance is larger than farPlaneDistance.
        [Fact]
        public void Matrix4x4CreatePerspectiveFieldOfViewTest5()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GMatrix4x4 mtx = Matrix4x4S.CreatePerspectiveFieldOfView((GScalar)GenericMathHelper.PiOver4, (GScalar)1, (GScalar)10, (GScalar)1);
            });
        }

        // A test for CreatePerspectiveOffCenter (scalar, scalar, scalar, scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreatePerspectiveOffCenterTest()
        {
            GScalar left = 10.0f;
            GScalar right = 90.0f;
            GScalar bottom = 20.0f;
            GScalar top = 180.0f;
            GScalar zNearPlane = 1.5f;
            GScalar zFarPlane = 1000.0f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.0375f,
                M22 = 0.01875f,
                M31 = 1.25f,
                M32 = 1.25f,
                M33 = -1.00150228f,
                M34 = -1,
                M43 = -1.50225341f,
            };

            GMatrix4x4 actual;
            actual = Matrix4x4S.CreatePerspectiveOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreatePerspectiveOffCenter did not return the expected value.");
        }

        // A test for CreatePerspectiveOffCenter (scalar, scalar, scalar, scalar, scalar, scalar)
        // CreatePerspectiveOffCenter test where nearPlaneDistance is negative.
        [Fact]
        public void Matrix4x4CreatePerspectiveOffCenterTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GScalar left = 10.0f, right = 90.0f, bottom = 20.0f, top = 180.0f;
                GMatrix4x4 actual = Matrix4x4S.CreatePerspectiveOffCenter(left, right, bottom, top, -1, 10);
            });
        }

        // A test for CreatePerspectiveOffCenter (scalar, scalar, scalar, scalar, scalar, scalar)
        // CreatePerspectiveOffCenter test where farPlaneDistance is negative.
        [Fact]
        public void Matrix4x4CreatePerspectiveOffCenterTest2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GScalar left = 10.0f, right = 90.0f, bottom = 20.0f, top = 180.0f;
                GMatrix4x4 actual = Matrix4x4S.CreatePerspectiveOffCenter(left, right, bottom, top, 1, -10);
            });
        }

        // A test for CreatePerspectiveOffCenter (scalar, scalar, scalar, scalar, scalar, scalar)
        // CreatePerspectiveOffCenter test where test where nearPlaneDistance is larger than farPlaneDistance.
        [Fact]
        public void Matrix4x4CreatePerspectiveOffCenterTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GScalar left = 10.0f, right = 90.0f, bottom = 20.0f, top = 180.0f;
                GMatrix4x4 actual = Matrix4x4S.CreatePerspectiveOffCenter(left, right, bottom, top, 10, 1);
            });
        }

        // A test for Invert (Matrix4x4)
        // Non invertible matrix - determinant is zero - singular matrix
        [Fact]
        public void Matrix4x4InvertTest1()
        {
            GMatrix4x4 a = new GMatrix4x4
            {
                M11 = 1,
                M12 = 2,
                M13 = 3,
                M14 = 4,
                M21 = 5,
                M22 = 6,
                M23 = 7,
                M24 = 8,
                M31 = 9,
                M32 = 10,
                M33 = 11,
                M34 = 12,
                M41 = 13,
                M42 = 14,
                M43 = 15,
                M44 = 16,
            };

            GScalar detA = a.GetDeterminant();
            Assert.True(GenericMathHelper.Equal(detA, 0.0f), "GMatrix4x4.Invert did not return the expected value.");

            GMatrix4x4 actual;
            Assert.False(Matrix4x4S.Invert(a, out actual));

            // all the elements in Actual is NaN
            Assert.True(
                GScalar.IsNaN(actual.M11) && GScalar.IsNaN(actual.M12) && GScalar.IsNaN(actual.M13) && GScalar.IsNaN(actual.M14) &&
                GScalar.IsNaN(actual.M21) && GScalar.IsNaN(actual.M22) && GScalar.IsNaN(actual.M23) && GScalar.IsNaN(actual.M24) &&
                GScalar.IsNaN(actual.M31) && GScalar.IsNaN(actual.M32) && GScalar.IsNaN(actual.M33) && GScalar.IsNaN(actual.M34) &&
                GScalar.IsNaN(actual.M41) && GScalar.IsNaN(actual.M42) && GScalar.IsNaN(actual.M43) && GScalar.IsNaN(actual.M44)
                , "GMatrix4x4.Invert did not return the expected value.");
        }

        // A test for Lerp (Matrix4x4, Matrix4x4, scalar)
        [Fact]
        public void Matrix4x4LerpTest()
        {
            GMatrix4x4 a = new GMatrix4x4
            {
                M11 = 11,
                M12 = 12,
                M13 = 13,
                M14 = 14,
                M21 = 21,
                M22 = 22,
                M23 = 23,
                M24 = 24,
                M31 = 31,
                M32 = 32,
                M33 = 33,
                M34 = 34,
                M41 = 41,
                M42 = 42,
                M43 = 43,
                M44 = 44,
            };

            GMatrix4x4 b = GenerateIncrementalMatrixNumber(0);

            GScalar t = 0.5f;

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 + (b.M11 - a.M11) * t,
                M12 = a.M12 + (b.M12 - a.M12) * t,
                M13 = a.M13 + (b.M13 - a.M13) * t,
                M14 = a.M14 + (b.M14 - a.M14) * t,

                M21 = a.M21 + (b.M21 - a.M21) * t,
                M22 = a.M22 + (b.M22 - a.M22) * t,
                M23 = a.M23 + (b.M23 - a.M23) * t,
                M24 = a.M24 + (b.M24 - a.M24) * t,

                M31 = a.M31 + (b.M31 - a.M31) * t,
                M32 = a.M32 + (b.M32 - a.M32) * t,
                M33 = a.M33 + (b.M33 - a.M33) * t,
                M34 = a.M34 + (b.M34 - a.M34) * t,

                M41 = a.M41 + (b.M41 - a.M41) * t,
                M42 = a.M42 + (b.M42 - a.M42) * t,
                M43 = a.M43 + (b.M43 - a.M43) * t,
                M44 = a.M44 + (b.M44 - a.M44) * t,
            };
            GMatrix4x4 actual;
            actual = Matrix4x4S.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.Lerp did not return the expected value.");
        }

        // A test for operator - (Matrix4x4)
        [Fact]
        public void Matrix4x4UnaryNegationTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = -1,
                M12 = -2,
                M13 = -3,
                M14 = -4,
                M21 = -5,
                M22 = -6,
                M23 = -7,
                M24 = -8,
                M31 = -9,
                M32 = -10,
                M33 = -11,
                M34 = -12,
                M41 = -13,
                M42 = -14,
                M43 = -15,
                M44 = -16,
            };

            GMatrix4x4 actual = -a;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.operator - did not return the expected value.");
        }

        // A test for operator - (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4SubtractionTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 - b.M11,
                M12 = a.M12 - b.M12,
                M13 = a.M13 - b.M13,
                M14 = a.M14 - b.M14,
                M21 = a.M21 - b.M21,
                M22 = a.M22 - b.M22,
                M23 = a.M23 - b.M23,
                M24 = a.M24 - b.M24,
                M31 = a.M31 - b.M31,
                M32 = a.M32 - b.M32,
                M33 = a.M33 - b.M33,
                M34 = a.M34 - b.M34,
                M41 = a.M41 - b.M41,
                M42 = a.M42 - b.M42,
                M43 = a.M43 - b.M43,
                M44 = a.M44 - b.M44,
            };

            GMatrix4x4 actual = a - b;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.operator - did not return the expected value.");
        }

        // A test for operator * (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4MultiplyTest1()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44,
            };

            GMatrix4x4 actual = a * b;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.operator * did not return the expected value.");
        }

        // A test for operator * (Matrix4x4, Matrix4x4)
        // Multiply with identity matrix
        [Fact]
        public void Matrix4x4MultiplyTest4()
        {
            GMatrix4x4 a = new GMatrix4x4
            {
                M11 = 1,
                M12 = 2,
                M13 = 3,
                M14 = 4,
                M21 = 5,
                M22 = -6,
                M23 = 7,
                M24 = -8,
                M31 = 9,
                M32 = 10,
                M33 = 11,
                M34 = 12,
                M41 = 13,
                M42 = -14,
                M43 = 15,
                M44 = -16,
            };

            GMatrix4x4 b = GMatrix4x4.Identity;

            GMatrix4x4 expected = a;
            GMatrix4x4 actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.operator * did not return the expected value.");
        }

        // A test for operator + (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4AdditionTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 + b.M11,
                M12 = a.M12 + b.M12,
                M13 = a.M13 + b.M13,
                M14 = a.M14 + b.M14,
                M21 = a.M21 + b.M21,
                M22 = a.M22 + b.M22,
                M23 = a.M23 + b.M23,
                M24 = a.M24 + b.M24,
                M31 = a.M31 + b.M31,
                M32 = a.M32 + b.M32,
                M33 = a.M33 + b.M33,
                M34 = a.M34 + b.M34,
                M41 = a.M41 + b.M41,
                M42 = a.M42 + b.M42,
                M43 = a.M43 + b.M43,
                M44 = a.M44 + b.M44,
            };

            GMatrix4x4 actual = a + b;
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.operator + did not return the expected value.");
        }

        // A test for Transpose (Matrix4x4)
        [Fact]
        public void Matrix4x4TransposeTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11,
                M12 = a.M21,
                M13 = a.M31,
                M14 = a.M41,
                M21 = a.M12,
                M22 = a.M22,
                M23 = a.M32,
                M24 = a.M42,
                M31 = a.M13,
                M32 = a.M23,
                M33 = a.M33,
                M34 = a.M43,
                M41 = a.M14,
                M42 = a.M24,
                M43 = a.M34,
                M44 = a.M44,
            };

            GMatrix4x4 actual = Matrix4x4S.Transpose(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.Transpose did not return the expected value.");
        }

        // A test for Transpose (Matrix4x4)
        // Transpose Identity matrix
        [Fact]
        public void Matrix4x4TransposeTest1()
        {
            GMatrix4x4 a = GMatrix4x4.Identity;
            GMatrix4x4 expected = GMatrix4x4.Identity;

            GMatrix4x4 actual = Matrix4x4S.Transpose(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GMatrix4x4.Transpose did not return the expected value.");
        }

        // A test for GMatrix4x4 (Quaternion)
        [Fact]
        public void Matrix4x4FromQuaternionTest1()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion q = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = 0.875595033f,
                M12 = 0.420031041f,
                M13 = -0.2385524f,
                M14 = 0,

                M21 = -0.38175258f,
                M22 = 0.904303849f,
                M23 = 0.1910483f,
                M24 = 0,

                M31 = 0.295970082f,
                M32 = -0.07621294f,
                M33 = 0.952151954f,
                M34 = 0,

                M41 = 0,
                M42 = 0,
                M43 = 0,
                M44 = 1,
            };

            GMatrix4x4 target = Matrix4x4S.CreateFromQuaternion(q);
            Assert.True(GenericMathHelper.Equal(expected, target), "GMatrix4x4.GMatrix4x4(Quaternion) did not return the expected value.");
        }

        // A test for FromQuaternion (Matrix4x4)
        // Convert X axis rotation matrix
        [Fact]
        public void Matrix4x4FromQuaternionTest2()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GQuaternion quat = QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);

                GMatrix4x4 expected = Matrix4x4S.CreateRotationX(angle);
                GMatrix4x4 actual = Matrix4x4S.CreateFromQuaternion(quat);
                Assert.True(GenericMathHelper.Equal(expected, actual),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                GQuaternion q2 = QuaternionS.CreateFromRotationMatrix(actual);
                Assert.True(GenericMathHelper.EqualRotation(quat, q2),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternion (Matrix4x4)
        // Convert Y axis rotation matrix
        [Fact]
        public void Matrix4x4FromQuaternionTest3()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GQuaternion quat = QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle);

                GMatrix4x4 expected = Matrix4x4S.CreateRotationY(angle);
                GMatrix4x4 actual = Matrix4x4S.CreateFromQuaternion(quat);
                Assert.True(GenericMathHelper.Equal(expected, actual),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                GQuaternion q2 = QuaternionS.CreateFromRotationMatrix(actual);
                Assert.True(GenericMathHelper.EqualRotation(quat, q2),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternion (Matrix4x4)
        // Convert Z axis rotation matrix
        [Fact]
        public void Matrix4x4FromQuaternionTest4()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GQuaternion quat = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle);

                GMatrix4x4 expected = Matrix4x4S.CreateRotationZ(angle);
                GMatrix4x4 actual = Matrix4x4S.CreateFromQuaternion(quat);
                Assert.True(GenericMathHelper.Equal(expected, actual),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                GQuaternion q2 = QuaternionS.CreateFromRotationMatrix(actual);
                Assert.True(GenericMathHelper.EqualRotation(quat, q2),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for FromQuaternion (Matrix4x4)
        // Convert XYZ axis rotation matrix
        [Fact]
        public void Matrix4x4FromQuaternionTest5()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GQuaternion quat =
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle) *
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle) *
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);

                GMatrix4x4 expected =
                    Matrix4x4S.CreateRotationX(angle) *
                    Matrix4x4S.CreateRotationY(angle) *
                    Matrix4x4S.CreateRotationZ(angle);
                GMatrix4x4 actual = Matrix4x4S.CreateFromQuaternion(quat);
                Assert.True(GenericMathHelper.Equal(expected, actual),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));

                // make sure convert back to quaternion is same as we passed quaternion.
                GQuaternion q2 = QuaternionS.CreateFromRotationMatrix(actual);
                Assert.True(GenericMathHelper.EqualRotation(quat, q2),
                    string.Format("Quaternion.FromQuaternion did not return the expected value. angle:{0}",
                    angle.ToString()));
            }
        }

        // A test for ToString ()
        [Fact]
        public void Matrix4x4ToStringTest()
        {
            GMatrix4x4 a = new GMatrix4x4
            {
                M11 = 11,
                M12 = -12,
                M13 = (GScalar)(-13.3),
                M14 = (GScalar)14.4,
                M21 = 21,
                M22 = 22,
                M23 = 23,
                M24 = 24,
                M31 = 31,
                M32 = 32,
                M33 = 33,
                M34 = 34,
                M41 = 41,
                M42 = 42,
                M43 = 43,
                M44 = 44,
            };

            string expected = string.Format(CultureInfo.CurrentCulture,
                "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
                    11, -12, (GScalar)(-13.3), (GScalar)14.4,
                    21, 22, 23, 24,
                    31, 32, 33, 34,
                    41, 42, 43, 44);

            string actual = a.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4AddTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 + b.M11,
                M12 = a.M12 + b.M12,
                M13 = a.M13 + b.M13,
                M14 = a.M14 + b.M14,
                M21 = a.M21 + b.M21,
                M22 = a.M22 + b.M22,
                M23 = a.M23 + b.M23,
                M24 = a.M24 + b.M24,
                M31 = a.M31 + b.M31,
                M32 = a.M32 + b.M32,
                M33 = a.M33 + b.M33,
                M34 = a.M34 + b.M34,
                M41 = a.M41 + b.M41,
                M42 = a.M42 + b.M42,
                M43 = a.M43 + b.M43,
                M44 = a.M44 + b.M44,
            };

            GMatrix4x4 actual = Matrix4x4S.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void Matrix4x4EqualsTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(0);

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
#pragma warning disable CA1508 // Avoid dead conditional code
            actual = a.Equals(obj);
#pragma warning restore CA1508 // Avoid dead conditional code
            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4MultiplyTest3()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44,
            };
            GMatrix4x4 actual;
            actual = Matrix4x4S.Multiply(a, b);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4, scalar)
        [Fact]
        public void Matrix4x4MultiplyTest5()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 expected = new GMatrix4x4(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
            GMatrix4x4 actual = Matrix4x4S.Multiply(a, 3);

            Assert.Equal(expected, actual);
        }

        // A test for Multiply (Matrix4x4, scalar)
        [Fact]
        public void Matrix4x4MultiplyTest6()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 expected = new GMatrix4x4(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
            GMatrix4x4 actual = a * 3;

            Assert.Equal(expected, actual);
        }

        // A test for Plus (Matrix4x4)
        [Fact]
        public void Matrix4x4PlusTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);

            GMatrix4x4 expected = a;
            GMatrix4x4 actual;

            actual = Matrix4x4S.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate (Matrix4x4)
        [Fact]
        public void Matrix4x4NegateTest()
        {
            GMatrix4x4 m = GenerateIncrementalMatrixNumber(0);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = -1,
                M12 = -2,
                M13 = -3,
                M14 = -4,
                M21 = -5,
                M22 = -6,
                M23 = -7,
                M24 = -8,
                M31 = -9,
                M32 = -10,
                M33 = -11,
                M34 = -12,
                M41 = -13,
                M42 = -14,
                M43 = -15,
                M44 = -16,
            };
            GMatrix4x4 actual;

            actual = Matrix4x4S.Negate(m);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4InequalityTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(0);

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

        // A test for operator == (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4EqualityTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(0);

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

        // A test for Subtract (Matrix4x4, Matrix4x4)
        [Fact]
        public void Matrix4x4SubtractTest()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(-8.0f);

            GMatrix4x4 expected = new GMatrix4x4
            {
                M11 = a.M11 - b.M11,
                M12 = a.M12 - b.M12,
                M13 = a.M13 - b.M13,
                M14 = a.M14 - b.M14,
                M21 = a.M21 - b.M21,
                M22 = a.M22 - b.M22,
                M23 = a.M23 - b.M23,
                M24 = a.M24 - b.M24,
                M31 = a.M31 - b.M31,
                M32 = a.M32 - b.M32,
                M33 = a.M33 - b.M33,
                M34 = a.M34 - b.M34,
                M41 = a.M41 - b.M41,
                M42 = a.M42 - b.M42,
                M43 = a.M43 - b.M43,
                M44 = a.M44 - b.M44,
            };

            GMatrix4x4 actual = Matrix4x4S.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        private static void CreateBillboardFact(GVector3 placeDirection, GVector3 cameraUpVector, GMatrix4x4 expectedRotation)
        {
            GVector3 cameraPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 objectPosition = cameraPosition + placeDirection * 10.0f;
            GMatrix4x4 expected = expectedRotation * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4.CreateBillboard did not return the expected value.");
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Forward side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest01()
        {
            // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
            CreateBillboardFact(new GVector3(0, 0, -1), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Backward side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest02()
        {
            // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
            CreateBillboardFact(new GVector3(0, 0, 1), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Right side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest03()
        {
            // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
            CreateBillboardFact(new GVector3(1, 0, 0), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Left side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest04()
        {
            // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
            CreateBillboardFact(new GVector3(-1, 0, 0), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)(-90))));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Up side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest05()
        {
            // Place object at Up side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(new GVector3(0, 1, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Down side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest06()
        {
            // Place object at Down side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(new GVector3(0, -1, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Right side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest07()
        {
            // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(new GVector3(1, 0, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Left side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest08()
        {
            // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateBillboardFact(new GVector3(-1, 0, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)(-90.0))));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Up side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest09()
        {
            // Place object at Up side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(new GVector3(0, 1, 0), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)(-90))));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Down side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest10()
        {
            // Place object at Down side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(new GVector3(0, -1, 0), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Forward side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest11()
        {
            // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(new GVector3(0, 0, -1), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Backward side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateBillboardTest12()
        {
            // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateBillboardFact(new GVector3(0, 0, 1), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ((GScalar)GenericMathHelper.ToRadians(90.0f)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Object and camera positions are too close and doesn't pass cameraForwardVector.
        [Fact]
        public void Matrix4x4CreateBillboardTooCloseTest1()
        {
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 cameraPosition = objectPosition;
            GVector3 cameraUpVector = new GVector3(0, 1, 0);

            // Doesn't pass camera face direction. CreateBillboard uses new Vector3(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, new GVector3(0, 0, 1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateBillboard did not return the expected value.");
        }

        // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Object and camera positions are too close and passed cameraForwardVector.
        [Fact]
        public void Matrix4x4CreateBillboardTooCloseTest2()
        {
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 cameraPosition = objectPosition;
            GVector3 cameraUpVector = new GVector3(0, 1, 0);

            // Passes Vector3.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, new GVector3(1, 0, 0));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateBillboard did not return the expected value.");
        }

        private static void CreateConstrainedBillboardFact(GVector3 placeDirection, GVector3 rotateAxis, GMatrix4x4 expectedRotation)
        {
            GVector3 cameraPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 objectPosition = cameraPosition + placeDirection * 10.0f;
            GMatrix4x4 expected = expectedRotation * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");

            // When you move camera along rotateAxis, result must be same.
            cameraPosition += rotateAxis * 10.0f;
            actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");

            cameraPosition -= rotateAxis * 30.0f;
            actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Forward side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest01()
        {
            // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 0, -1), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Backward side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest02()
        {
            // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 0, 1), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Right side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest03()
        {
            // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(new GVector3(1, 0, 0), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Left side of camera on XZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest04()
        {
            // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
            CreateConstrainedBillboardFact(new GVector3(-1, 0, 0), new GVector3(0, 1, 0), Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)(-90))));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Up side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest05()
        {
            // Place object at Up side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 1, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Down side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest06()
        {
            // Place object at Down side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(new GVector3(0, -1, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Right side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest07()
        {
            // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(new GVector3(1, 0, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Left side of camera on XY-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest08()
        {
            // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
            CreateConstrainedBillboardFact(new GVector3(-1, 0, 0), new GVector3(0, 0, 1),
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)(-90))));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Up side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest09()
        {
            // Place object at Up side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 1, 0), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)(-90))));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Down side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest10()
        {
            // Place object at Down side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(new GVector3(0, -1, 0), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)90)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Forward side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest11()
        {
            // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 0, -1), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)180)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Place object at Backward side of camera on YZ-GPlane
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTest12()
        {
            // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
            CreateConstrainedBillboardFact(new GVector3(0, 0, 1), new GVector3(-1, 0, 0),
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)90)) * Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)0)));
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Object and camera positions are too close and doesn't pass cameraForwardVector.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTooCloseTest1()
        {
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 cameraPosition = objectPosition;
            GVector3 cameraUpVector = new GVector3(0, 1, 0);

            // Doesn't pass camera face direction. CreateConstrainedBillboard uses new Vector3(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, new GVector3(0, 0, 1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Object and camera positions are too close and passed cameraForwardVector.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardTooCloseTest2()
        {
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 cameraPosition = objectPosition;
            GVector3 cameraUpVector = new GVector3(0, 1, 0);

            // Passes Vector3.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, new GVector3(1, 0, 0), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Angle between rotateAxis and camera to object vector is too small. And use doesn't passed objectForwardVector parameter.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardAlongAxisTest1()
        {
            // Place camera at up side of object.
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 rotateAxis = new GVector3(0, 1, 0);
            GVector3 cameraPosition = objectPosition + rotateAxis * 10.0f;

            // In this case, CreateConstrainedBillboard picks new Vector3(0, 0, -1) as object forward vector.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Angle between rotateAxis and camera to object vector is too small. And user doesn't passed objectForwardVector parameter.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardAlongAxisTest2()
        {
            // Place camera at up side of object.
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 rotateAxis = new GVector3(0, 0, -1);
            GVector3 cameraPosition = objectPosition + rotateAxis * 10.0f;

            // In this case, CreateConstrainedBillboard picks new Vector3(1, 0, 0) as object forward vector.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed correct objectForwardVector parameter.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardAlongAxisTest3()
        {
            // Place camera at up side of object.
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 rotateAxis = new GVector3(0, 1, 0);
            GVector3 cameraPosition = objectPosition + rotateAxis * 10.0f;

            // User passes correct objectForwardVector.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardAlongAxisTest4()
        {
            // Place camera at up side of object.
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 rotateAxis = new GVector3(0, 1, 0);
            GVector3 cameraPosition = objectPosition + rotateAxis * 10.0f;

            // User passes correct objectForwardVector.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)180)) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 1, 0));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
        // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
        [Fact]
        public void Matrix4x4CreateConstrainedBillboardAlongAxisTest5()
        {
            // Place camera at up side of object.
            GVector3 objectPosition = new GVector3(3.0f, 4.0f, 5.0f);
            GVector3 rotateAxis = new GVector3(0, 0, -1);
            GVector3 cameraPosition = objectPosition + rotateAxis * 10.0f;

            // In this case, CreateConstrainedBillboard picks Vector3.Right as object forward vector.
            GMatrix4x4 expected = Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)(-90))) * Matrix4x4S.CreateTranslation(objectPosition);
            GMatrix4x4 actual = Matrix4x4S.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new GVector3(0, 0, -1), new GVector3(0, 0, -1));
            Assert.True(GenericMathHelper.Equal(expected, actual), "Matrix4x4S.CreateConstrainedBillboard did not return the expected value.");
        }

        // A test for CreateScale (Vector3)
        [Fact]
        public void Matrix4x4CreateScaleTest1()
        {
            GVector3 scales = new GVector3(2.0f, 3.0f, 4.0f);
            GMatrix4x4 expected = new GMatrix4x4(
                2.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 3.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 4.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            GMatrix4x4 actual = Matrix4x4S.CreateScale(scales);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (Vector3, Vector3)
        [Fact]
        public void Matrix4x4CreateScaleCenterTest1()
        {
            GVector3 scale = new GVector3(3, 4, 5);
            GVector3 center = new GVector3(23, 42, 666);

            GMatrix4x4 scaleAroundZero = Matrix4x4S.CreateScale(scale, GVector3.Zero);
            GMatrix4x4 scaleAroundZeroExpected = Matrix4x4S.CreateScale(scale);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix4x4 scaleAroundCenter = Matrix4x4S.CreateScale(scale, center);
            GMatrix4x4 scaleAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateScale(scale) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (scalar)
        [Fact]
        public void Matrix4x4CreateScaleTest2()
        {
            GScalar scale = 2.0f;
            GMatrix4x4 expected = new GMatrix4x4(
                2.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 2.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 2.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            GMatrix4x4 actual = Matrix4x4S.CreateScale(scale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (scalar, Vector3)
        [Fact]
        public void Matrix4x4CreateScaleCenterTest2()
        {
            GScalar scale = 5;
            GVector3 center = new GVector3(23, 42, 666);

            GMatrix4x4 scaleAroundZero = Matrix4x4S.CreateScale(scale, GVector3.Zero);
            GMatrix4x4 scaleAroundZeroExpected = Matrix4x4S.CreateScale(scale);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix4x4 scaleAroundCenter = Matrix4x4S.CreateScale(scale, center);
            GMatrix4x4 scaleAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateScale(scale) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateScale (scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreateScaleTest3()
        {
            GScalar xScale = 2.0f;
            GScalar yScale = 3.0f;
            GScalar zScale = 4.0f;
            GMatrix4x4 expected = new GMatrix4x4(
                2.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 3.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 4.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            GMatrix4x4 actual = Matrix4x4S.CreateScale(xScale, yScale, zScale);
            Assert.Equal(expected, actual);
        }

        // A test for CreateScale (scalar, scalar, scalar, Vector3)
        [Fact]
        public void Matrix4x4CreateScaleCenterTest3()
        {
            GVector3 scale = new GVector3(3, 4, 5);
            GVector3 center = new GVector3(23, 42, 666);

            GMatrix4x4 scaleAroundZero = Matrix4x4S.CreateScale(scale.X, scale.Y, scale.Z, GVector3.Zero);
            GMatrix4x4 scaleAroundZeroExpected = Matrix4x4S.CreateScale(scale.X, scale.Y, scale.Z);
            Assert.True(GenericMathHelper.Equal(scaleAroundZero, scaleAroundZeroExpected));

            GMatrix4x4 scaleAroundCenter = Matrix4x4S.CreateScale(scale.X, scale.Y, scale.Z, center);
            GMatrix4x4 scaleAroundCenterExpected = Matrix4x4S.CreateTranslation(-center) * Matrix4x4S.CreateScale(scale.X, scale.Y, scale.Z) * Matrix4x4S.CreateTranslation(center);
            Assert.True(GenericMathHelper.Equal(scaleAroundCenter, scaleAroundCenterExpected));
        }

        // A test for CreateTranslation (Vector3)
        [Fact]
        public void Matrix4x4CreateTranslationTest1()
        {
            GVector3 position = new GVector3(2.0f, 3.0f, 4.0f);
            GMatrix4x4 expected = new GMatrix4x4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                2.0f, 3.0f, 4.0f, 1.0f);

            GMatrix4x4 actual = Matrix4x4S.CreateTranslation(position);
            Assert.Equal(expected, actual);
        }

        // A test for CreateTranslation (scalar, scalar, scalar)
        [Fact]
        public void Matrix4x4CreateTranslationTest2()
        {
            GScalar xPosition = 2.0f;
            GScalar yPosition = 3.0f;
            GScalar zPosition = 4.0f;

            GMatrix4x4 expected = new GMatrix4x4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                2.0f, 3.0f, 4.0f, 1.0f);

            GMatrix4x4 actual = Matrix4x4S.CreateTranslation(xPosition, yPosition, zPosition);
            Assert.Equal(expected, actual);
        }

        // A test for Translation
        [Fact]
        public void Matrix4x4TranslationTest()
        {
            GMatrix4x4 a = GenerateTestMatrix();
            GMatrix4x4 b = a;

            // Transformed vector that has same semantics of property must be same.
            GVector3 val = new GVector3(a.M41, a.M42, a.M43);
            Assert.Equal(val, a.Translation);

            // Set value and get value must be same.
            val = new GVector3(1.0f, 2.0f, 3.0f);
            a = a.WithTranslation(val);
            Assert.Equal(val, a.Translation);

            // Make sure it only modifies expected value of matrix.
            Assert.True(
                a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 && a.M14 == b.M14 &&
                a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 && a.M24 == b.M24 &&
                a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33 && a.M34 == b.M34 &&
                a.M41 != b.M41 && a.M42 != b.M42 && a.M43 != b.M43 && a.M44 == b.M44);
        }

        // A test for Equals (Matrix4x4)
        [Fact]
        public void Matrix4x4EqualsTest1()
        {
            GMatrix4x4 a = GenerateIncrementalMatrixNumber(0);
            GMatrix4x4 b = GenerateIncrementalMatrixNumber(0);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { M11 = 11.0f };
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for IsIdentity
        [Fact]
        public void Matrix4x4IsIdentityTest()
        {
            Assert.True(GMatrix4x4.Identity.IsIdentity);
            Assert.True(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1).IsIdentity);
            Assert.False(new GMatrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0).IsIdentity);
        }

        // A test for Matrix4x4 (Matrix3x2)
        [Fact]
        public void Matrix4x4From3x2Test()
        {
            GMatrix3x2 source = new GMatrix3x2(1, 2, 3, 4, 5, 6);
            GMatrix4x4 result = new GMatrix4x4(source);

            Assert.Equal(source.M11, result.M11);
            Assert.Equal(source.M12, result.M12);
            Assert.Equal(0f, result.M13);
            Assert.Equal(0f, result.M14);

            Assert.Equal(source.M21, result.M21);
            Assert.Equal(source.M22, result.M22);
            Assert.Equal(0f, result.M23);
            Assert.Equal(0f, result.M24);

            Assert.Equal(0f, result.M31);
            Assert.Equal(0f, result.M32);
            Assert.Equal(1f, result.M33);
            Assert.Equal(0f, result.M34);

            Assert.Equal(source.M31, result.M41);
            Assert.Equal(source.M32, result.M42);
            Assert.Equal(0f, result.M43);
            Assert.Equal(1f, result.M44);
        }

        // A test for Matrix4x4 comparison involving NaN values
        [Fact]
        public void Matrix4x4EqualsNanTest()
        {
            GMatrix4x4 a = new GMatrix4x4(GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 b = new GMatrix4x4(0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 c = new GMatrix4x4(0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 d = new GMatrix4x4(0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 e = new GMatrix4x4(0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 f = new GMatrix4x4(0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 g = new GMatrix4x4(0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 h = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 i = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 j = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0, 0);
            GMatrix4x4 k = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0, 0);
            GMatrix4x4 l = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0, 0);
            GMatrix4x4 m = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0, 0);
            GMatrix4x4 n = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0, 0);
            GMatrix4x4 o = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN, 0);
            GMatrix4x4 p = new GMatrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GScalar.NaN);

            Assert.False(a == new GMatrix4x4());
            Assert.False(b == new GMatrix4x4());
            Assert.False(c == new GMatrix4x4());
            Assert.False(d == new GMatrix4x4());
            Assert.False(e == new GMatrix4x4());
            Assert.False(f == new GMatrix4x4());
            Assert.False(g == new GMatrix4x4());
            Assert.False(h == new GMatrix4x4());
            Assert.False(i == new GMatrix4x4());
            Assert.False(j == new GMatrix4x4());
            Assert.False(k == new GMatrix4x4());
            Assert.False(l == new GMatrix4x4());
            Assert.False(m == new GMatrix4x4());
            Assert.False(n == new GMatrix4x4());
            Assert.False(o == new GMatrix4x4());
            Assert.False(p == new GMatrix4x4());

            Assert.True(a != new GMatrix4x4());
            Assert.True(b != new GMatrix4x4());
            Assert.True(c != new GMatrix4x4());
            Assert.True(d != new GMatrix4x4());
            Assert.True(e != new GMatrix4x4());
            Assert.True(f != new GMatrix4x4());
            Assert.True(g != new GMatrix4x4());
            Assert.True(h != new GMatrix4x4());
            Assert.True(i != new GMatrix4x4());
            Assert.True(j != new GMatrix4x4());
            Assert.True(k != new GMatrix4x4());
            Assert.True(l != new GMatrix4x4());
            Assert.True(m != new GMatrix4x4());
            Assert.True(n != new GMatrix4x4());
            Assert.True(o != new GMatrix4x4());
            Assert.True(p != new GMatrix4x4());

            Assert.False(a.Equals(new GMatrix4x4()));
            Assert.False(b.Equals(new GMatrix4x4()));
            Assert.False(c.Equals(new GMatrix4x4()));
            Assert.False(d.Equals(new GMatrix4x4()));
            Assert.False(e.Equals(new GMatrix4x4()));
            Assert.False(f.Equals(new GMatrix4x4()));
            Assert.False(g.Equals(new GMatrix4x4()));
            Assert.False(h.Equals(new GMatrix4x4()));
            Assert.False(i.Equals(new GMatrix4x4()));
            Assert.False(j.Equals(new GMatrix4x4()));
            Assert.False(k.Equals(new GMatrix4x4()));
            Assert.False(l.Equals(new GMatrix4x4()));
            Assert.False(m.Equals(new GMatrix4x4()));
            Assert.False(n.Equals(new GMatrix4x4()));
            Assert.False(o.Equals(new GMatrix4x4()));
            Assert.False(p.Equals(new GMatrix4x4()));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);
            Assert.False(e.IsIdentity);
            Assert.False(f.IsIdentity);
            Assert.False(g.IsIdentity);
            Assert.False(h.IsIdentity);
            Assert.False(i.IsIdentity);
            Assert.False(j.IsIdentity);
            Assert.False(k.IsIdentity);
            Assert.False(l.IsIdentity);
            Assert.False(m.IsIdentity);
            Assert.False(n.IsIdentity);
            Assert.False(o.IsIdentity);
            Assert.False(p.IsIdentity);

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
            Assert.False(d.Equals(d));
            Assert.False(e.Equals(e));
            Assert.False(f.Equals(f));
            Assert.False(g.Equals(g));
            Assert.False(h.Equals(h));
            Assert.False(i.Equals(i));
            Assert.False(j.Equals(j));
            Assert.False(k.Equals(k));
            Assert.False(l.Equals(l));
            Assert.False(m.Equals(m));
            Assert.False(n.Equals(n));
            Assert.False(o.Equals(o));
            Assert.False(p.Equals(p));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void Matrix4x4SizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 16, sizeof(GMatrix4x4));
            Assert.Equal(scalarSize * 32, sizeof(Matrix4x4_2x));
            Assert.Equal(scalarSize * 17, sizeof(Matrix4x4PlusScalar));
            Assert.Equal(scalarSize * 34, sizeof(Matrix4x4PlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4_2x
        {
            private GMatrix4x4 _a;
            private GMatrix4x4 _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4PlusScalar
        {
            private GMatrix4x4 _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Matrix4x4PlusScalar_2x
        {
            private Matrix4x4PlusScalar _a;
            private Matrix4x4PlusScalar _b;
        }

        [Fact]
        public void PerspectiveFarPlaneAtInfinityTest()
        {
            var nearPlaneDistance = 0.125f;
            var m = Matrix4x4S.CreatePerspective(1.0f, 1.0f, nearPlaneDistance, GScalar.PositiveInfinity);
            Assert.Equal(-1, m.M33);
            Assert.Equal(-nearPlaneDistance, m.M43);
        }

        [Fact]
        public void PerspectiveFieldOfViewFarPlaneAtInfinityTest()
        {
            var nearPlaneDistance = 0.125f;
            var m = Matrix4x4S.CreatePerspectiveFieldOfView(GenericMathHelper.ToRadians(60.0f), 1.5f, nearPlaneDistance, GScalar.PositiveInfinity);
            Assert.Equal(-1, m.M33);
            Assert.Equal(-nearPlaneDistance, m.M43);
        }

        [Fact]
        public void PerspectiveOffCenterFarPlaneAtInfinityTest()
        {
            var nearPlaneDistance = 0.125f;
            var m = Matrix4x4S.CreatePerspectiveOffCenter(0.0f, 0.0f, 1.0f, 1.0f, nearPlaneDistance, GScalar.PositiveInfinity);
            Assert.Equal(-1, m.M33);
            Assert.Equal(-nearPlaneDistance, m.M43);
        }
    }
}
