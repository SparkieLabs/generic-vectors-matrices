// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using GMatrix4x4 = System.Numerics.Matrix4x4<float>;
using GQuaternion = System.Numerics.Quaternion<float>;
using GScalar = System.Single;
using GVector3 = System.Numerics.Vector3<float>;
using GVector4 = System.Numerics.Vector4<float>;

namespace System.Numerics.Tests
{
    public class GenericQuaternionSingleTests
    {
        // A test for Dot (Quaternion, Quaternion)
        [Fact]
        public void QuaternionDotTest()
        {
            GQuaternion a = new GQuaternion(1, 2, 3, 4);
            GQuaternion b = new GQuaternion(5, 6, 7, 8);

            GScalar expected = 70.0f;
            GScalar actual;

            actual = QuaternionS.Dot(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Dot did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Length ()
        [Fact]
        public void QuaternionLengthTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);

            GScalar w = 4.0f;

            GQuaternion target = new GQuaternion(v, w);

            GScalar expected = 5.477226f;
            GScalar actual;

            actual = target.Length();

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Length did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for LengthSquared ()
        [Fact]
        public void QuaternionLengthSquaredTest()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar w = 4.0f;

            GQuaternion target = new GQuaternion(v, w);

            GScalar expected = 30.0f;
            GScalar actual;

            actual = target.LengthSquared();

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.LengthSquared did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (Quaternion, Quaternion, scalar)
        [Fact]
        public void QuaternionLerpTest()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 0.5f;

            GQuaternion expected = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(20.0f));
            GQuaternion actual;

            actual = QuaternionS.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Lerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionS.Lerp(a, a, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (Quaternion, Quaternion, scalar)
        // Lerp test when t = 0
        [Fact]
        public void QuaternionLerpTest1()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 0.0f;

            GQuaternion expected = new GQuaternion(a.X, a.Y, a.Z, a.W);
            GQuaternion actual = QuaternionS.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (Quaternion, Quaternion, scalar)
        // Lerp test when t = 1
        [Fact]
        public void QuaternionLerpTest2()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 1.0f;

            GQuaternion expected = new GQuaternion(b.X, b.Y, b.Z, b.W);
            GQuaternion actual = QuaternionS.Lerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Lerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Lerp (Quaternion, Quaternion, scalar)
        // Lerp test when the two quaternions are more than 90 degree (dot product <0)
        [Fact]
        public void QuaternionLerpTest3()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.Negate(a);

            GScalar t = 1.0f;

            GQuaternion actual = QuaternionS.Lerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == a, $"GQuaternion.Lerp did not return the expected value: expected {a} actual {actual}");
        }

        // A test for Conjugate (Quaternion)
        [Fact]
        public void QuaternionConjugateTest1()
        {
            GQuaternion a = new GQuaternion(1, 2, 3, 4);

            GQuaternion expected = new GQuaternion(-1, -2, -3, 4);
            GQuaternion actual;

            actual = QuaternionS.Conjugate(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Conjugate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (Quaternion)
        [Fact]
        public void QuaternionNormalizeTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            GQuaternion expected = new GQuaternion(0.182574168f, 0.365148336f, 0.5477225f, 0.7302967f);
            GQuaternion actual;

            actual = QuaternionS.Normalize(a);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Normalize did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Normalize (Quaternion)
        // Normalize zero length quaternion
        [Fact]
        public void QuaternionNormalizeTest1()
        {
            GQuaternion a = new GQuaternion(0.0f, 0.0f, -0.0f, 0.0f);

            GQuaternion actual = QuaternionS.Normalize(a);
            Assert.True(GScalar.IsNaN(actual.X) && GScalar.IsNaN(actual.Y) && GScalar.IsNaN(actual.Z) && GScalar.IsNaN(actual.W)
                , $"GQuaternion.Normalize did not return the expected value: expected {new GQuaternion(GScalar.NaN, GScalar.NaN, GScalar.NaN, GScalar.NaN)} actual {actual}");
        }

        // A test for Concatenate(Quaternion, Quaternion)
        [Fact]
        public void QuaternionConcatenateTest1()
        {
            GQuaternion b = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion a = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(24.0f, 48.0f, 48.0f, -6.0f);
            GQuaternion actual;

            actual = QuaternionS.Concatenate(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Concatenate did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - (Quaternion, Quaternion)
        [Fact]
        public void QuaternionSubtractionTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 6.0f, 7.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 2.0f, 3.0f, 8.0f);

            GQuaternion expected = new GQuaternion(-4.0f, 4.0f, 4.0f, -4.0f);
            GQuaternion actual;

            actual = a - b;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (Quaternion, scalar)
        [Fact]
        public void QuaternionMultiplyTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GScalar factor = 0.5f;

            GQuaternion expected = new GQuaternion(0.5f, 1.0f, 1.5f, 2.0f);
            GQuaternion actual;

            actual = a * factor;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator * (Quaternion, Quaternion)
        [Fact]
        public void QuaternionMultiplyTest1()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(24.0f, 48.0f, 48.0f, -6.0f);
            GQuaternion actual;

            actual = a * b;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator * did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator / (Quaternion, Quaternion)
        [Fact]
        public void QuaternionDivisionTest1()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(-0.045977015f, -0.09195402f, -7.450581E-9f, 0.402298868f);
            GQuaternion actual;

            actual = a / b;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator / did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator + (Quaternion, Quaternion)
        [Fact]
        public void QuaternionAdditionTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(6.0f, 8.0f, 10.0f, 12.0f);
            GQuaternion actual;

            actual = a + b;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator + did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Quaternion (scalar, scalar, scalar, scalar)
        [Fact]
        public void QuaternionConstructorTest()
        {
            GScalar x = 1.0f;
            GScalar y = 2.0f;
            GScalar z = 3.0f;
            GScalar w = 4.0f;

            GQuaternion target = new GQuaternion(x, y, z, w);

            Assert.True(GenericMathHelper.Equal(target.X, x) && GenericMathHelper.Equal(target.Y, y) && GenericMathHelper.Equal(target.Z, z) && GenericMathHelper.Equal(target.W, w),
                "GQuaternion.constructor (x,y,z,w) did not return the expected value.");
        }

        // A test for Quaternion (Vector3, scalar)
        [Fact]
        public void QuaternionConstructorTest1()
        {
            GVector3 v = new GVector3(1.0f, 2.0f, 3.0f);
            GScalar w = 4.0f;

            GQuaternion target = new GQuaternion(v, w);
            Assert.True(GenericMathHelper.Equal(target.X, v.X) && GenericMathHelper.Equal(target.Y, v.Y) && GenericMathHelper.Equal(target.Z, v.Z) && GenericMathHelper.Equal(target.W, w),
                "GQuaternion.constructor (Vector3,w) did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3, scalar)
        [Fact]
        public void QuaternionCreateFromAxisAngleTest()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GScalar angle = GenericMathHelper.ToRadians(30.0f);

            GQuaternion expected = new GQuaternion(0.0691723f, 0.1383446f, 0.207516879f, 0.9659258f);
            GQuaternion actual;

            actual = QuaternionS.CreateFromAxisAngle(axis, angle);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.CreateFromAxisAngle did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for CreateFromAxisAngle (Vector3, scalar)
        // CreateFromAxisAngle of zero vector
        [Fact]
        public void QuaternionCreateFromAxisAngleTest1()
        {
            GVector3 axis = new GVector3();
            GScalar angle = GenericMathHelper.ToRadians(-30.0f);

            GScalar cos = (GScalar)Math.Cos(angle / 2.0f);
            GQuaternion actual = QuaternionS.CreateFromAxisAngle(axis, angle);

            Assert.True(actual.X == 0.0f && actual.Y == 0.0f && actual.Z == 0.0f && GenericMathHelper.Equal(cos, actual.W)
                , "GQuaternion.CreateFromAxisAngle did not return the expected value.");
        }

        // A test for CreateFromAxisAngle (Vector3, scalar)
        // CreateFromAxisAngle of angle = 30 && 750
        [Fact]
        public void QuaternionCreateFromAxisAngleTest2()
        {
            GVector3 axis = new GVector3(1, 0, 0);
            GScalar angle1 = GenericMathHelper.ToRadians(30.0f);
            GScalar angle2 = GenericMathHelper.ToRadians(750.0f);

            GQuaternion actual1 = QuaternionS.CreateFromAxisAngle(axis, angle1);
            GQuaternion actual2 = QuaternionS.CreateFromAxisAngle(axis, angle2);
            Assert.True(GenericMathHelper.Equal(actual1, actual2), $"GQuaternion.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        // A test for CreateFromAxisAngle (Vector3, scalar)
        // CreateFromAxisAngle of angle = 30 && 390
        [Fact]
        public void QuaternionCreateFromAxisAngleTest3()
        {
            GVector3 axis = new GVector3(1, 0, 0);
            GScalar angle1 = GenericMathHelper.ToRadians(30.0f);
            GScalar angle2 = GenericMathHelper.ToRadians(390.0f);

            GQuaternion actual1 = QuaternionS.CreateFromAxisAngle(axis, angle1);
            GQuaternion actual2 = QuaternionS.CreateFromAxisAngle(axis, angle2);
            actual1 = actual1 with { X = -actual1.X, W = -actual1.W };

            Assert.True(GenericMathHelper.Equal(actual1, actual2), $"GQuaternion.CreateFromAxisAngle did not return the expected value: actual1 {actual1} actual2 {actual2}");
        }

        [Fact]
        public void QuaternionCreateFromYawPitchRollTest1()
        {
            GScalar yawAngle = GenericMathHelper.ToRadians(30.0f);
            GScalar pitchAngle = GenericMathHelper.ToRadians(40.0f);
            GScalar rollAngle = GenericMathHelper.ToRadians(50.0f);

            GQuaternion yaw = QuaternionS.CreateFromAxisAngle(GVector3.UnitY, yawAngle);
            GQuaternion pitch = QuaternionS.CreateFromAxisAngle(GVector3.UnitX, pitchAngle);
            GQuaternion roll = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, rollAngle);

            GQuaternion expected = yaw * pitch * roll;
            GQuaternion actual = QuaternionS.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.QuaternionCreateFromYawPitchRollTest1 did not return the expected value: expected {expected} actual {actual}");
        }

        // Covers more numeric rigions
        [Fact]
        public void QuaternionCreateFromYawPitchRollTest2()
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

                        GQuaternion yaw = QuaternionS.CreateFromAxisAngle(GVector3.UnitY, yawRad);
                        GQuaternion pitch = QuaternionS.CreateFromAxisAngle(GVector3.UnitX, pitchRad);
                        GQuaternion roll = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, rollRad);

                        GQuaternion expected = yaw * pitch * roll;
                        GQuaternion actual = QuaternionS.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                        Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.QuaternionCreateFromYawPitchRollTest2 Yaw:{yawAngle} Pitch:{pitchAngle} Roll:{rollAngle} did not return the expected value: expected {expected} actual {actual}");
                    }
                }
            }
        }

        // A test for Slerp (Quaternion, Quaternion, scalar)
        [Fact]
        public void QuaternionSlerpTest()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 0.5f;

            GQuaternion expected = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(20.0f));
            GQuaternion actual;

            actual = QuaternionS.Slerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");

            // Case a and b are same.
            expected = a;
            actual = QuaternionS.Slerp(a, a, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (Quaternion, Quaternion, scalar)
        // Slerp test where t = 0
        [Fact]
        public void QuaternionSlerpTest1()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 0.0f;

            GQuaternion expected = new GQuaternion(a.X, a.Y, a.Z, a.W);
            GQuaternion actual = QuaternionS.Slerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (Quaternion, Quaternion, scalar)
        // Slerp test where t = 1
        [Fact]
        public void QuaternionSlerpTest2()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 1.0f;

            GQuaternion expected = new GQuaternion(b.X, b.Y, b.Z, b.W);
            GQuaternion actual = QuaternionS.Slerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (Quaternion, Quaternion, scalar)
        // Slerp test where dot product is < 0
        [Fact]
        public void QuaternionSlerpTest3()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = -a;

            GScalar t = 1.0f;

            GQuaternion expected = a;
            GQuaternion actual = QuaternionS.Slerp(a, b, t);
            // Note that in quaternion world, Q == -Q. In the case of quaternions dot product is zero,
            // one of the quaternion will be flipped to compute the shortest distance. When t = 1, we
            // expect the result to be the same as quaternion b but flipped.
            Assert.True(actual == expected, $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Slerp (Quaternion, Quaternion, scalar)
        // Slerp test where the quaternion is flipped
        [Fact]
        public void QuaternionSlerpTest4()
        {
            GVector3 axis = Vector3S.Normalize(new GVector3(1.0f, 2.0f, 3.0f));
            GQuaternion a = QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(10.0f));
            GQuaternion b = -QuaternionS.CreateFromAxisAngle(axis, GenericMathHelper.ToRadians(30.0f));

            GScalar t = 0.0f;

            GQuaternion expected = new GQuaternion(a.X, a.Y, a.Z, a.W);
            GQuaternion actual = QuaternionS.Slerp(a, b, t);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Slerp did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for operator - Quaternion
        [Fact]
        public void QuaternionUnaryNegationTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            GQuaternion expected = new GQuaternion(-1.0f, -2.0f, -3.0f, -4.0f);
            GQuaternion actual;

            actual = -a;

            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.operator - did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Inverse Quaternion
        [Fact]
        public void QuaternionInverseTest()
        {
            GQuaternion a = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(-0.0287356321f, -0.03448276f, -0.0402298868f, 0.04597701f);
            GQuaternion actual;

            actual = QuaternionS.Inverse(a);
            GenericAssertExtensions.Equal(expected, actual);
        }

        // A test for Inverse Quaternion
        // Invert zero length quaternion
        [Fact]
        public void QuaternionInverseTest1()
        {
            GQuaternion a = new GQuaternion();
            GQuaternion actual = QuaternionS.Inverse(a);

            Assert.True(GScalar.IsNaN(actual.X) && GScalar.IsNaN(actual.Y) && GScalar.IsNaN(actual.Z) && GScalar.IsNaN(actual.W)
                , $"GQuaternion.Inverse - did not return the expected value: expected {new GQuaternion(GScalar.NaN, GScalar.NaN, GScalar.NaN, GScalar.NaN)} actual {actual}");
        }

        // A test for ToString ()
        [Fact]
        public void QuaternionToStringTest()
        {
            GQuaternion target = new GQuaternion(-1, (GScalar)2.2, (GScalar)3.3, -(GScalar)4.4);

            string expected = string.Format(CultureInfo.CurrentCulture
                , "{{X:{0} Y:{1} Z:{2} W:{3}}}"
                , -1, (GScalar)2.2, (GScalar)3.3, -(GScalar)4.4);

            string actual = target.ToString();
            Assert.Equal(expected, actual);
        }

        // A test for Add (Quaternion, Quaternion)
        [Fact]
        public void QuaternionAddTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(6.0f, 8.0f, 10.0f, 12.0f);
            GQuaternion actual;

            actual = QuaternionS.Add(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for Divide (Quaternion, Quaternion)
        [Fact]
        public void QuaternionDivideTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(-0.045977015f, -0.09195402f, -7.450581E-9f, 0.402298868f);
            GQuaternion actual;

            actual = QuaternionS.Divide(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Divide did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Equals (object)
        [Fact]
        public void QuaternionEqualsTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            object? obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10 };
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

        // A test for Multiply (Quaternion, scalar)
        [Fact]
        public void QuaternionMultiplyTest2()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GScalar factor = 0.5f;

            GQuaternion expected = new GQuaternion(0.5f, 1.0f, 1.5f, 2.0f);
            GQuaternion actual;

            actual = QuaternionS.Multiply(a, factor);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Multiply (Quaternion, Quaternion)
        [Fact]
        public void QuaternionMultiplyTest3()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 6.0f, 7.0f, 8.0f);

            GQuaternion expected = new GQuaternion(24.0f, 48.0f, 48.0f, -6.0f);
            GQuaternion actual;

            actual = QuaternionS.Multiply(a, b);
            Assert.True(GenericMathHelper.Equal(expected, actual), $"GQuaternion.Multiply did not return the expected value: expected {expected} actual {actual}");
        }

        // A test for Plus (Quaternion)
        [Fact]
        public void QuaternionPlusTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            GQuaternion expected = a;
            GQuaternion actual;

            actual = QuaternionS.Plus(a);
            Assert.Equal(expected, actual);
        }

        // A test for Negate Quaternion
        [Fact]
        public void QuaternionNegateTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            GQuaternion expected = new GQuaternion(-1.0f, -2.0f, -3.0f, -4.0f);
            GQuaternion actual;

            actual = QuaternionS.Negate(a);
            Assert.Equal(expected, actual);
        }

        // A test for Subtract (Quaternion, Quaternion)
        [Fact]
        public void QuaternionSubtractTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 6.0f, 7.0f, 4.0f);
            GQuaternion b = new GQuaternion(5.0f, 2.0f, 3.0f, 8.0f);

            GQuaternion expected = new GQuaternion(-4.0f, 4.0f, 4.0f, -4.0f);
            GQuaternion actual;

            actual = QuaternionS.Subtract(a, b);
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Quaternion, Quaternion)
        [Fact]
        public void QuaternionInequalityTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10 };
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Quaternion, Quaternion)
        [Fact]
        public void QuaternionEqualityTest()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10 };
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Convert Identity matrix test
        [Fact]
        public void QuaternionFromRotationMatrixTest1()
        {
            GMatrix4x4 matrix = GMatrix4x4.Identity;

            GQuaternion expected = new GQuaternion(0.0f, 0.0f, 0.0f, 1.0f);
            GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
            Assert.True(GenericMathHelper.Equal(expected, actual),
                $"GQuaternion.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
            Assert.True(GenericMathHelper.Equal(matrix, m2),
                $"GQuaternion.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Convert X axis rotation matrix
        [Fact]
        public void QuaternionFromRotationMatrixTest2()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GMatrix4x4 matrix = Matrix4x4S.CreateRotationX(angle);

                GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);
                GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
                Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                    $"GQuaternion.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
                Assert.True(GenericMathHelper.Equal(matrix, m2),
                    $"GQuaternion.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Convert Y axis rotation matrix
        [Fact]
        public void QuaternionFromRotationMatrixTest3()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GMatrix4x4 matrix = Matrix4x4S.CreateRotationY(angle);

                GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle);
                GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
                Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                    $"GQuaternion.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
                Assert.True(GenericMathHelper.Equal(matrix, m2),
                    $"GQuaternion.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Convert Z axis rotation matrix
        [Fact]
        public void QuaternionFromRotationMatrixTest4()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GMatrix4x4 matrix = Matrix4x4S.CreateRotationZ(angle);

                GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle);
                GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
                Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                    $"GQuaternion.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
                Assert.True(GenericMathHelper.Equal(matrix, m2),
                    $"GQuaternion.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Convert XYZ axis rotation matrix
        [Fact]
        public void QuaternionFromRotationMatrixTest5()
        {
            for (GScalar angle = 0.0f; angle < 720.0f; angle += 10.0f)
            {
                GMatrix4x4 matrix = Matrix4x4S.CreateRotationX(angle) * Matrix4x4S.CreateRotationY(angle) * Matrix4x4S.CreateRotationZ(angle);

                GQuaternion expected =
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle) *
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle) *
                    QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);

                GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
                Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                    $"GQuaternion.CreateFromRotationMatrix angle:{angle} did not return the expected value: expected {expected} actual {actual}");

                // make sure convert back to matrix is same as we passed matrix.
                GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
                Assert.True(GenericMathHelper.Equal(matrix, m2),
                    $"GQuaternion.CreateFromQuaternion angle:{angle} did not return the expected value: matrix {matrix} m2 {m2}");
            }
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // X axis is most large axis case
        [Fact]
        public void QuaternionFromRotationMatrixWithScaledMatrixTest1()
        {
            GScalar angle = GenericMathHelper.ToRadians(180.0f);
            GMatrix4x4 matrix = Matrix4x4S.CreateRotationY(angle) * Matrix4x4S.CreateRotationZ(angle);

            GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle) * QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle);
            GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
            Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                $"GQuaternion.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
            Assert.True(GenericMathHelper.Equal(matrix, m2),
                $"GQuaternion.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Y axis is most large axis case
        [Fact]
        public void QuaternionFromRotationMatrixWithScaledMatrixTest2()
        {
            GScalar angle = GenericMathHelper.ToRadians(180.0f);
            GMatrix4x4 matrix = Matrix4x4S.CreateRotationX(angle) * Matrix4x4S.CreateRotationZ(angle);

            GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitZ, angle) * QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);
            GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
            Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                $"GQuaternion.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
            Assert.True(GenericMathHelper.Equal(matrix, m2),
                $"GQuaternion.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for CreateFromRotationMatrix (Matrix4x4)
        // Z axis is most large axis case
        [Fact]
        public void QuaternionFromRotationMatrixWithScaledMatrixTest3()
        {
            GScalar angle = GenericMathHelper.ToRadians(180.0f);
            GMatrix4x4 matrix = Matrix4x4S.CreateRotationX(angle) * Matrix4x4S.CreateRotationY(angle);

            GQuaternion expected = QuaternionS.CreateFromAxisAngle(GVector3.UnitY, angle) * QuaternionS.CreateFromAxisAngle(GVector3.UnitX, angle);
            GQuaternion actual = QuaternionS.CreateFromRotationMatrix(matrix);
            Assert.True(GenericMathHelper.EqualRotation(expected, actual),
                $"GQuaternion.CreateFromRotationMatrix did not return the expected value: expected {expected} actual {actual}");

            // make sure convert back to matrix is same as we passed matrix.
            GMatrix4x4 m2 = Matrix4x4S.CreateFromQuaternion(actual);
            Assert.True(GenericMathHelper.Equal(matrix, m2),
                $"GQuaternion.CreateFromQuaternion did not return the expected value: matrix {matrix} m2 {m2}");
        }

        // A test for Equals Quaternion
        [Fact]
        public void QuaternionEqualsTest1()
        {
            GQuaternion a = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);
            GQuaternion b = new GQuaternion(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { X = 10 };
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Identity
        [Fact]
        public void QuaternionIdentityTest()
        {
            GQuaternion val = new GQuaternion(0, 0, 0, 1);
            Assert.Equal(val, GQuaternion.Identity);
        }

        // A test for IsIdentity
        [Fact]
        public void QuaternionIsIdentityTest()
        {
            Assert.True(GQuaternion.Identity.IsIdentity);
            Assert.True(new GQuaternion(0, 0, 0, 1).IsIdentity);
            Assert.False(new GQuaternion(1, 0, 0, 1).IsIdentity);
            Assert.False(new GQuaternion(0, 1, 0, 1).IsIdentity);
            Assert.False(new GQuaternion(0, 0, 1, 1).IsIdentity);
            Assert.False(new GQuaternion(0, 0, 0, 0).IsIdentity);
        }

        // A test for GQuaternion comparison involving NaN values
        [Fact]
        public void QuaternionEqualsNanTest()
        {
            GQuaternion a = new GQuaternion(GScalar.NaN, 0, 0, 0);
            GQuaternion b = new GQuaternion(0, GScalar.NaN, 0, 0);
            GQuaternion c = new GQuaternion(0, 0, GScalar.NaN, 0);
            GQuaternion d = new GQuaternion(0, 0, 0, GScalar.NaN);

            Assert.False(a == new GQuaternion(0, 0, 0, 0));
            Assert.False(b == new GQuaternion(0, 0, 0, 0));
            Assert.False(c == new GQuaternion(0, 0, 0, 0));
            Assert.False(d == new GQuaternion(0, 0, 0, 0));

            Assert.True(a != new GQuaternion(0, 0, 0, 0));
            Assert.True(b != new GQuaternion(0, 0, 0, 0));
            Assert.True(c != new GQuaternion(0, 0, 0, 0));
            Assert.True(d != new GQuaternion(0, 0, 0, 0));

            Assert.False(a.Equals(new GQuaternion(0, 0, 0, 0)));
            Assert.False(b.Equals(new GQuaternion(0, 0, 0, 0)));
            Assert.False(c.Equals(new GQuaternion(0, 0, 0, 0)));
            Assert.False(d.Equals(new GQuaternion(0, 0, 0, 0)));

            Assert.False(a.IsIdentity);
            Assert.False(b.IsIdentity);
            Assert.False(c.IsIdentity);
            Assert.False(d.IsIdentity);

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
            Assert.False(d.Equals(d));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void QuaternionSizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 4, sizeof(GQuaternion));
            Assert.Equal(scalarSize * 8, sizeof(Quaternion_2x));
            Assert.Equal(scalarSize * 5, sizeof(QuaternionPlusScalar));
            Assert.Equal(scalarSize * 10, sizeof(QuaternionPlusScalar_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Quaternion_2x
        {
            private GQuaternion _a;
            private GQuaternion _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionPlusScalar
        {
            private GQuaternion _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct QuaternionPlusScalar_2x
        {
            private QuaternionPlusScalar _a;
            private QuaternionPlusScalar _b;
        }
    }
}

