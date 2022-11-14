// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Numerics
{
    public partial struct PlaneS
    {
        // Static Methods

        /// <summary>Creates a <see cref="System.Numerics.Plane" /> object that contains three specified points.</summary>
        /// <param name="point1">The first point defining the plane.</param>
        /// <param name="point2">The second point defining the plane.</param>
        /// <param name="point3">The third point defining the plane.</param>
        /// <returns>The plane containing the three points.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane<T> CreateFromVertices<T>(in Vector3<T> point1, in Vector3<T> point2, in Vector3<T> point3)
            where T : struct, IFloatingPointIeee754<T>
        {
            T ax = point2.X - point1.X;
            T ay = point2.Y - point1.Y;
            T az = point2.Z - point1.Z;

            T bx = point3.X - point1.X;
            T by = point3.Y - point1.Y;
            T bz = point3.Z - point1.Z;

            // N=Cross(a,b)
            T nx = ay * bz - az * by;
            T ny = az * bx - ax * bz;
            T nz = ax * by - ay * bx;

            // Normalize(N)
            T ls = nx * nx + ny * ny + nz * nz;
            T invNorm = T.One / T.Sqrt(ls);

            Vector3<T> normal = new(
                nx * invNorm,
                ny * invNorm,
                nz * invNorm);

            return new(
                normal,
                -(normal.X * point1.X + normal.Y * point1.Y + normal.Z * point1.Z));
        }

        /// <summary>Calculates the dot product of a plane and a 4-dimensional vector.</summary>
        /// <param name="left">The plane.</param>
        /// <param name="right">The four-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Dot<T>(in Plane<T> left, in Vector4<T> right)
            where T : struct, IFloatingPointIeee754<T>
        {
            return left.Normal.X * right.X +
                   left.Normal.Y * right.Y +
                   left.Normal.Z * right.Z +
                   left.Distance * right.W;
        }

        /// <summary>Returns the dot product of a specified three-dimensional vector and the normal vector of this plane plus the distance (<see cref="System.Numerics.Plane.D" />) value of the plane.</summary>
        /// <param name="left">The plane.</param>
        /// <param name="right">The 3-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DotCoordinate<T>(in Plane<T> left, in Vector3<T> right)
            where T : struct, IFloatingPointIeee754<T>
        {
            return left.Normal.X * right.X +
                   left.Normal.Y * right.Y +
                   left.Normal.Z * right.Z +
                   left.Distance;
        }

        /// <summary>Returns the dot product of a specified three-dimensional vector and the <see cref="System.Numerics.Plane.Normal" /> vector of this plane.</summary>
        /// <param name="left">The plane.</param>
        /// <param name="right">The three-dimensional vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DotNormal<T>(in Plane<T> left, in Vector3<T> right)
            where T : struct, IFloatingPointIeee754<T>
        {
            return left.Normal.X * right.X +
                   left.Normal.Y * right.Y +
                   left.Normal.Z * right.Z;
        }

        /// <summary>Creates a new <see cref="System.Numerics.Plane" /> object whose normal vector is the source plane's normal vector normalized.</summary>
        /// <param name="value">The source plane.</param>
        /// <returns>The normalized plane.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane<T> Normalize<T>(in Plane<T> value)
            where T : struct, IFloatingPointIeee754<T>
        {
            T normalizeEpsilon = T.CreateChecked(1.192092896e-07); // smallest such that 1.0+NormalizeEpsilon != 1.0

            T f = value.Normal.X * value.Normal.X + value.Normal.Y * value.Normal.Y + value.Normal.Z * value.Normal.Z;

            if (T.Abs(f - T.One) < normalizeEpsilon)
            {
                return value; // It already normalized, so we don't need to further process.
            }

            T fInv = T.One / T.Sqrt(f);

            return new(
                value.Normal.X * fInv,
                value.Normal.Y * fInv,
                value.Normal.Z * fInv,
                value.Distance * fInv);
        }

        /// <summary>Transforms a normalized plane by a 4x4 matrix.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="matrix">The transformation matrix to apply to <paramref name="plane" />.</param>
        /// <returns>The transformed plane.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="System.Numerics.Plane.Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane<T> Transform<T>(in Plane<T> plane, in Matrix4x4<T> matrix)
            where T : struct, IFloatingPointIeee754<T>
        {
            Matrix4x4S.Invert(matrix, out Matrix4x4<T> m);

            T x = plane.Normal.X, y = plane.Normal.Y, z = plane.Normal.Z, w = plane.Distance;

            return new(
                x * m.M11 + y * m.M12 + z * m.M13 + w * m.M14,
                x * m.M21 + y * m.M22 + z * m.M23 + w * m.M24,
                x * m.M31 + y * m.M32 + z * m.M33 + w * m.M34,
                x * m.M41 + y * m.M42 + z * m.M43 + w * m.M44);
        }

        /// <summary>Transforms a normalized plane by a Quaternion rotation.</summary>
        /// <param name="plane">The normalized plane to transform.</param>
        /// <param name="rotation">The Quaternion rotation to apply to the plane.</param>
        /// <returns>A new plane that results from applying the Quaternion rotation.</returns>
        /// <remarks><paramref name="plane" /> must already be normalized so that its <see cref="System.Numerics.Plane.Normal" /> vector is of unit length before this method is called.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Plane<T> Transform<T>(in Plane<T> plane, in Quaternion<T> rotation)
            where T : struct, IFloatingPointIeee754<T>
        {
            // Compute rotation matrix.
            T x2 = rotation.X + rotation.X;
            T y2 = rotation.Y + rotation.Y;
            T z2 = rotation.Z + rotation.Z;

            T wx2 = rotation.W * x2;
            T wy2 = rotation.W * y2;
            T wz2 = rotation.W * z2;
            T xx2 = rotation.X * x2;
            T xy2 = rotation.X * y2;
            T xz2 = rotation.X * z2;
            T yy2 = rotation.Y * y2;
            T yz2 = rotation.Y * z2;
            T zz2 = rotation.Z * z2;

            T m11 = T.One - yy2 - zz2;
            T m21 = xy2 - wz2;
            T m31 = xz2 + wy2;

            T m12 = xy2 + wz2;
            T m22 = T.One - xx2 - zz2;
            T m32 = yz2 - wx2;

            T m13 = xz2 - wy2;
            T m23 = yz2 + wx2;
            T m33 = T.One - xx2 - yy2;

            T x = plane.Normal.X, y = plane.Normal.Y, z = plane.Normal.Z;

            return new(
                x * m11 + y * m21 + z * m31,
                x * m12 + y * m22 + z * m32,
                x * m13 + y * m23 + z * m33,
                plane.Distance);
        }
    }
}
