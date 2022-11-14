// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using Xunit;
using GMatrix4x4 = System.Numerics.Matrix4x4<float>;
using GPlane = System.Numerics.Plane<float>;
using GQuaternion = System.Numerics.Quaternion<float>;
using GScalar = System.Single;
using GVector3 = System.Numerics.Vector3<float>;
using GVector4 = System.Numerics.Vector4<float>;

namespace System.Numerics.Tests
{
    public class GenericPlaneSingleTests
    {
        // A test for Equals (Plane)
        [Fact]
        public void PlaneEqualsTest1()
        {
            GPlane a = new GPlane(1, 2, 3, 4);
            GPlane b = new GPlane(1, 2, 3, 4);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a.Equals(b);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { Normal = new GVector3(10, b.Normal.Y, b.Normal.Z) };
            expected = false;
            actual = a.Equals(b);
            Assert.Equal(expected, actual);
        }

        // A test for Equals (object)
        [Fact]
        public void PlaneEqualsTest()
        {
            GPlane a = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);
            GPlane b = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            object? obj = b;

            bool expected = true;
            bool actual = a.Equals(obj);
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { Normal = new GVector3(10.0f, b.Normal.Y, b.Normal.Z) };

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
#pragma warning disable CA1508 // Avoid dead conditional code
            actual = a.Equals(obj);
#pragma warning restore CA1508 // Avoid dead conditional code
            Assert.Equal(expected, actual);
        }

        // A test for operator != (Plane, Plane)
        [Fact]
        public void PlaneInequalityTest()
        {
            GPlane a = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);
            GPlane b = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = false;
            bool actual = a != b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { Normal = new GVector3(10.0f, b.Normal.Y, b.Normal.Z) };
            expected = true;
            actual = a != b;
            Assert.Equal(expected, actual);
        }

        // A test for operator == (Plane, Plane)
        [Fact]
        public void PlaneEqualityTest()
        {
            GPlane a = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);
            GPlane b = new GPlane(1.0f, 2.0f, 3.0f, 4.0f);

            // case 1: compare between same values
            bool expected = true;
            bool actual = a == b;
            Assert.Equal(expected, actual);

            // case 2: compare between different values
            b = b with { Normal = new GVector3(10.0f, b.Normal.Y, b.Normal.Z) };
            expected = false;
            actual = a == b;
            Assert.Equal(expected, actual);
        }

        // A test for Plane (scalar, scalar, scalar, scalar)
        [Fact]
        public void PlaneConstructorTest1()
        {
            GScalar a = 1.0f, b = 2.0f, c = 3.0f, d = 4.0f;
            GPlane target = new GPlane(a, b, c, d);

            Assert.True(
                target.Normal.X == a && target.Normal.Y == b && target.Normal.Z == c && target.Distance == d,
                "GPlane.cstor did not return the expected value.");
        }

        // A test for Plane.CreateFromVertices
        [Fact]
        public void PlaneCreateFromVerticesTest()
        {
            GVector3 point1 = new GVector3(0, 1, 1);
            GVector3 point2 = new GVector3(0, 0, 1);
            GVector3 point3 = new GVector3(1, 0, 1);

            GPlane target = PlaneS.CreateFromVertices(point1, point2, point3);
            GPlane expected = new GPlane(new GVector3(0, 0, 1), -1);
            Assert.Equal(target, expected);
        }

        // A test for Plane.CreateFromVertices
        [Fact]
        public void PlaneCreateFromVerticesTest2()
        {
            GVector3 point1 = new GVector3(0, 0, 1);
            GVector3 point2 = new GVector3(1, 0, 0);
            GVector3 point3 = new GVector3(1, 1, 0);

            GPlane target = PlaneS.CreateFromVertices(point1, point2, point3);
            GScalar invRoot2 = (GScalar)(1.0 / Math.Sqrt(2));

            GPlane expected = new GPlane(new GVector3(invRoot2, 0, invRoot2), -invRoot2);
            Assert.True(GenericMathHelper.Equal(target, expected), "GPlane.cstor did not return the expected value.");
        }

        // A test for Plane (Vector3, scalar)
        [Fact]
        public void PlaneConstructorTest3()
        {
            GVector3 normal = new GVector3(1, 2, 3);
            GScalar d = 4;

            GPlane target = new GPlane(normal, d);
            Assert.True(
                target.Normal == normal && target.Distance == d,
                "GPlane.cstor did not return the expected value.");
        }

        // A test for GPlane (Vector4)
        [Fact]
        public void PlaneConstructorTest()
        {
            GVector4 value = new GVector4(1, 2, 3, 4);
            GPlane target = new GPlane(value);

            Assert.True(
                target.Normal.X == value.X && target.Normal.Y == value.Y && target.Normal.Z == value.Z && target.Distance == value.W,
                "GPlane.cstor did not return the expected value.");
        }

        [Fact]
        public void PlaneDotTest()
        {
            GPlane target = new GPlane(2, 3, 4, 5);
            GVector4 value = new GVector4(5, 4, 3, 2);

            GScalar expected = 10 + 12 + 12 + 10;
            GScalar actual = PlaneS.Dot(target, value);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.Dot returns unexpected value.");
        }

        [Fact]
        public void PlaneDotCoordinateTest()
        {
            GPlane target = new GPlane(2, 3, 4, 5);
            GVector3 value = new GVector3(5, 4, 3);

            GScalar expected = 10 + 12 + 12 + 5;
            GScalar actual = PlaneS.DotCoordinate(target, value);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.DotCoordinate returns unexpected value.");
        }

        [Fact]
        public void PlaneDotNormalTest()
        {
            GPlane target = new GPlane(2, 3, 4, 5);
            GVector3 value = new GVector3(5, 4, 3);

            GScalar expected = 10 + 12 + 12;
            GScalar actual = PlaneS.DotNormal(target, value);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.DotCoordinate returns unexpected value.");
        }

        [Fact]
        public void PlaneNormalizeTest()
        {
            GPlane target = new GPlane(1, 2, 3, 4);

            GScalar f = target.Normal.LengthSquared();
            GScalar invF = (GScalar)(1.0 / Math.Sqrt(f));
            GPlane expected = new GPlane(target.Normal * invF, target.Distance * invF);

            GPlane actual = PlaneS.Normalize(target);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.Normalize returns unexpected value.");

            // normalize, normalized normal.
            actual = PlaneS.Normalize(actual);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.Normalize returns unexpected value.");
        }

        [Fact]
        // Transform by matrix
        public void PlaneTransformTest1()
        {
            GPlane target = new GPlane(1, 2, 3, 4);
            target = PlaneS.Normalize(target);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            m = m with { M41 = 10, M42 = 20, M43 = 30 };

            GPlane expected = new GPlane();
            GMatrix4x4 inv;
            Matrix4x4S.Invert(m, out inv);
            GMatrix4x4 itm = Matrix4x4S.Transpose(inv);
            GScalar x = target.Normal.X, y = target.Normal.Y, z = target.Normal.Z, w = target.Distance;
            expected = expected with
            {
                Normal = new GVector3(
                x * itm.M11 + y * itm.M21 + z * itm.M31 + w * itm.M41,
                x * itm.M12 + y * itm.M22 + z * itm.M32 + w * itm.M42,
                x * itm.M13 + y * itm.M23 + z * itm.M33 + w * itm.M43)
            };
            expected = expected with { Distance = x * itm.M14 + y * itm.M24 + z * itm.M34 + w * itm.M44 };

            GPlane actual;
            actual = PlaneS.Transform(target, m);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.Transform did not return the expected value.");
        }

        [Fact]
        // Transform by quaternion
        public void PlaneTransformTest2()
        {
            GPlane target = new GPlane(1, 2, 3, 4);
            target = PlaneS.Normalize(target);

            GMatrix4x4 m =
                Matrix4x4S.CreateRotationX(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationY(GenericMathHelper.ToRadians((GScalar)30)) *
                Matrix4x4S.CreateRotationZ(GenericMathHelper.ToRadians((GScalar)30));
            GQuaternion q = QuaternionS.CreateFromRotationMatrix(m);

            GPlane expected = new GPlane();
            GScalar x = target.Normal.X, y = target.Normal.Y, z = target.Normal.Z, w = target.Distance;
            expected = expected with
            {
                Normal = new GVector3(
                x * m.M11 + y * m.M21 + z * m.M31 + w * m.M41,
                x * m.M12 + y * m.M22 + z * m.M32 + w * m.M42,
                x * m.M13 + y * m.M23 + z * m.M33 + w * m.M43)
            };
            expected = expected with { Distance = x * m.M14 + y * m.M24 + z * m.M34 + w * m.M44 };

            GPlane actual;
            actual = PlaneS.Transform(target, q);
            Assert.True(GenericMathHelper.Equal(expected, actual), "GPlane.Transform did not return the expected value.");
        }

        // A test for Plane comparison involving NaN values
        [Fact]
        public void PlaneEqualsNanTest()
        {
            GPlane a = new GPlane(GScalar.NaN, 0, 0, 0);
            GPlane b = new GPlane(0, GScalar.NaN, 0, 0);
            GPlane c = new GPlane(0, 0, GScalar.NaN, 0);
            GPlane d = new GPlane(0, 0, 0, GScalar.NaN);

            Assert.False(a == new GPlane(0, 0, 0, 0));
            Assert.False(b == new GPlane(0, 0, 0, 0));
            Assert.False(c == new GPlane(0, 0, 0, 0));
            Assert.False(d == new GPlane(0, 0, 0, 0));

            Assert.True(a != new GPlane(0, 0, 0, 0));
            Assert.True(b != new GPlane(0, 0, 0, 0));
            Assert.True(c != new GPlane(0, 0, 0, 0));
            Assert.True(d != new GPlane(0, 0, 0, 0));

            Assert.False(a.Equals(new GPlane(0, 0, 0, 0)));
            Assert.False(b.Equals(new GPlane(0, 0, 0, 0)));
            Assert.False(c.Equals(new GPlane(0, 0, 0, 0)));
            Assert.False(d.Equals(new GPlane(0, 0, 0, 0)));

            // Counterintuitive result - IEEE rules for NaN comparison are weird!
            Assert.False(a.Equals(a));
            Assert.False(b.Equals(b));
            Assert.False(c.Equals(c));
            Assert.False(d.Equals(d));
        }

        // A test to make sure these types are blittable directly into GPU buffer memory layouts
        [Fact]
        public unsafe void PlaneSizeofTest()
        {
            int scalarSize = sizeof(GScalar);
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
            Assert.Equal(scalarSize * 4, sizeof(GPlane));
            Assert.Equal(scalarSize * 8, sizeof(Plane_2x));
            Assert.Equal(scalarSize * 5, sizeof(PlanePlusSingle));
            Assert.Equal(scalarSize * 10, sizeof(PlanePlusFloatSingle_2x));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
        }

        [Fact]
        public void PlaneToStringTest()
        {
            GPlane target = new GPlane(1, 2, 3, 4);
            string expected = string.Format(
                CultureInfo.CurrentCulture,
                "{{Normal:{0:G} Distance:{1}}}",
                target.Normal,
                target.Distance);

            Assert.Equal(expected, target.ToString());
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Plane_2x
        {
            private GPlane _a;
            private GPlane _b;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PlanePlusSingle
        {
            private GPlane _v;
            private GScalar _f;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PlanePlusFloatSingle_2x
        {
            private PlanePlusSingle _a;
            private PlanePlusSingle _b;
        }
    }
}
