// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Numerics
{
    [RequiresPreviewFeatures]
    public partial struct QuaternionS
    {
        // "Friendly" Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Quaternion<T> Plus<T>(in Quaternion<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return value;
        }

        /// <summary>Reverses the sign of each component of the quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negated quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Negate<T>(in Quaternion<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return -value;
        }

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="left" /> and <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Add<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left + right;
        }

        /// <summary>Subtracts each element in a second quaternion from its corresponding element in a first quaternion.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The quaternion containing the values that result from subtracting each element in <paramref name="right" /> from its corresponding element in <paramref name="left" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Subtract<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left - right;
        }

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Multiply<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Divide<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left / right;
        }

        /// <summary>Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.</summary>
        /// <param name="left">The source quaternion.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion<T> Multiply<T>(in Quaternion<T> left, T right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        // Static Methods

        /// <summary>Concatenates two quaternions.</summary>
        /// <param name="left">The first quaternion rotation in the series.</param>
        /// <param name="right">The second quaternion rotation in the series.</param>
        /// <returns>A new quaternion representing the concatenation of the <paramref name="left" /> rotation followed by the <paramref name="right" /> rotation.</returns>
        public static Quaternion<T> Concatenate<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
            // So that's why value2 goes q1 and value1 goes q2.
            T q1x = right.X;
            T q1y = right.Y;
            T q1z = right.Z;
            T q1w = right.W;

            T q2x = left.X;
            T q2y = left.Y;
            T q2z = left.Z;
            T q2w = left.W;

            // cross(av, bv)
            T cx = q1y * q2z - q1z * q2y;
            T cy = q1z * q2x - q1x * q2z;
            T cz = q1x * q2y - q1y * q2x;

            T dot = q1x * q2x + q1y * q2y + q1z * q2z;

            return new(
                q1x * q2w + q2x * q1w + cx,
                q1y * q2w + q2y * q1w + cy,
                q1z * q2w + q2z * q1w + cz,
                q1w * q2w - dot);
        }

        /// <summary>Returns the conjugate of a specified quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns>A new quaternion that is the conjugate of <see langword="value" />.</returns>
        public static Quaternion<T> Conjugate<T>(in Quaternion<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                -value.X,
                -value.Y,
                -value.Z,
                value.W);
        }

        /// <summary>Creates a quaternion from a unit vector and an angle to rotate around the vector.</summary>
        /// <param name="axis">The unit vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The newly created quaternion.</returns>
        /// <remarks><paramref name="axis" /> vector must be normalized before calling this method or the resulting <see cref="System.Numerics.Quaternion" /> will be incorrect.</remarks>
        public static Quaternion<T> CreateFromAxisAngle<T>(in Vector3<T> axis, T angle)
            where T : struct, IFloatingPoint<T>
        {
            T halfAngle = angle * T.Create(0.5);
            T s = T.Sin(halfAngle);
            T c = T.Cos(halfAngle);

            return new(
                axis.X * s,
                axis.Y * s,
                axis.Z * s,
                c);
        }

        /// <summary>Creates a quaternion from the specified rotation matrix.</summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The newly created quaternion.</returns>
        public static Quaternion<T> CreateFromRotationMatrix<T>(in Matrix4x4<T> matrix)
            where T : struct, IFloatingPoint<T>
        {
            T trace = matrix.M11 + matrix.M22 + matrix.M33;

            if (trace > T.Zero)
            {
                T s = T.Sqrt(trace + T.One);
                T invS = T.Create(0.5) / s;
                return new(
                    (matrix.M23 - matrix.M32) * invS,
                    (matrix.M31 - matrix.M13) * invS,
                    (matrix.M12 - matrix.M21) * invS,
                    s * T.Create(0.5));
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    T s = T.Sqrt(T.One + matrix.M11 - matrix.M22 - matrix.M33);
                    T invS = T.Create(0.5) / s;
                    return new(
                        T.Create(0.5) * s,
                        (matrix.M12 + matrix.M21) * invS,
                        (matrix.M13 + matrix.M31) * invS,
                        (matrix.M23 - matrix.M32) * invS);
                }
                if (matrix.M22 > matrix.M33)
                {
                    T s = T.Sqrt(T.One + matrix.M22 - matrix.M11 - matrix.M33);
                    T invS = T.Create(0.5) / s;
                    return new(
                        (matrix.M21 + matrix.M12) * invS,
                        T.Create(0.5) * s,
                        (matrix.M32 + matrix.M23) * invS,
                        (matrix.M31 - matrix.M13) * invS);
                }
                else
                {
                    T s = T.Sqrt(T.One + matrix.M33 - matrix.M11 - matrix.M22);
                    T invS = T.Create(0.5) / s;
                    return new(
                        (matrix.M31 + matrix.M13) * invS,
                        (matrix.M32 + matrix.M23) * invS,
                        T.Create(0.5) * s,
                        (matrix.M12 - matrix.M21) * invS);
                }
            }
        }

        /// <summary>Creates a new quaternion from the given yaw, pitch, and roll.</summary>
        /// <param name="yaw">The yaw angle, in radians, around the Y axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z axis.</param>
        /// <returns>The resulting quaternion.</returns>
        public static Quaternion<T> CreateFromYawPitchRoll<T>(T yaw, T pitch, T roll)
            where T : struct, IFloatingPoint<T>
        {
            //  Roll first, about axis the object is facing, then
            //  pitch upward, then yaw to face into the new heading
            T sr, cr, sp, cp, sy, cy;

            T halfRoll = roll * T.Create(0.5);
            sr = T.Sin(halfRoll);
            cr = T.Cos(halfRoll);

            T halfPitch = pitch * T.Create(0.5);
            sp = T.Sin(halfPitch);
            cp = T.Cos(halfPitch);

            T halfYaw = yaw * T.Create(0.5);
            sy = T.Sin(halfYaw);
            cy = T.Cos(halfYaw);

            return new(
                cy * sp * cr + sy * cp * sr,
                sy * cp * cr - cy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                cy * cp * cr + sy * sp * sr);
        }

        /// <summary>Calculates the dot product of two quaternions.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The dot product.</returns>
        public static T Dot<T>(in Quaternion<T> left, in Quaternion<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left.X * right.X +
                   left.Y * right.Y +
                   left.Z * right.Z +
                   left.W * right.W;
        }

        /// <summary>Returns the inverse of a quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns>The inverted quaternion.</returns>
        public static Quaternion<T> Inverse<T>(in Quaternion<T> value)
            where T : struct, IFloatingPoint<T>
        {
            //  -1   (       a              -v       )
            // q   = ( -------------   ------------- )
            //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

            T ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;
            T invNorm = T.One / ls;

            return new(
                -value.X * invNorm,
                -value.Y * invNorm,
                -value.Z * invNorm,
                value.W * invNorm);
        }

        /// <summary>Performs a linear interpolation between two quaternions based on a value that specifies the weighting of the second quaternion.</summary>
        /// <param name="min">The first quaternion.</param>
        /// <param name="max">The second quaternion.</param>
        /// <param name="amount">The relative weight of <paramref name="max" /> in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static Quaternion<T> Lerp<T>(in Quaternion<T> min, in Quaternion<T> max, T amount)
            where T : struct, IFloatingPoint<T>
        {
            T t = amount;
            T t1 = T.One - t;

            T dot = min.X * max.X + min.Y * max.Y +
                        min.Z * max.Z + min.W * max.W;

            Quaternion<T> r;
            if (dot >= T.Zero)
            {
                r = new(
                    t1 * min.X + t * max.X,
                    t1 * min.Y + t * max.Y,
                    t1 * min.Z + t * max.Z,
                    t1 * min.W + t * max.W);
            }
            else
            {
                r = new(
                t1 * min.X - t * max.X,
                t1 * min.Y - t * max.Y,
                t1 * min.Z - t * max.Z,
                t1 * min.W - t * max.W);
            }

            // Normalize it.
            T ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
            T invNorm = T.One / T.Sqrt(ls);
            return new(
                r.X * invNorm,
                r.Y * invNorm,
                r.Z * invNorm,
                r.W * invNorm);
        }

        /// <summary>Interpolates between two quaternions, using spherical linear interpolation.</summary>
        /// <param name="min">The first quaternion.</param>
        /// <param name="max">The second quaternion.</param>
        /// <param name="amount">The relative weight of the second quaternion in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static Quaternion<T> Slerp<T>(in Quaternion<T> min, in Quaternion<T> max, T amount)
            where T : struct, IFloatingPoint<T>
        {
            T slerpEpsilon = T.Create(1e-6);

            T t = amount;

            T cosOmega = min.X * max.X + min.Y * max.Y +
                             min.Z * max.Z + min.W * max.W;

            bool flip = false;

            if (cosOmega < T.Zero)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            T s1, s2;

            if (cosOmega > (T.One - slerpEpsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = T.One - t;
                s2 = (flip) ? -t : t;
            }
            else
            {
                T omega = T.Acos(cosOmega);
                T invSinOmega = T.One / T.Sin(omega);

                s1 = T.Sin((T.One - t) * omega) * invSinOmega;
                s2 = (flip)
                    ? -T.Sin(t * omega) * invSinOmega
                    : T.Sin(t * omega) * invSinOmega;
            }

            return new(
                s1 * min.X + s2 * max.X,
                s1 * min.Y + s2 * max.Y,
                s1 * min.Z + s2 * max.Z,
                s1 * min.W + s2 * max.W);
        }

        /// <summary>Divides each component of a specified <see cref="System.Numerics.Quaternion" /> by its length.</summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <returns>The normalized quaternion.</returns>
        public static Quaternion<T> Normalize<T>(in Quaternion<T> value)
            where T : struct, IFloatingPoint<T>
        {
            T ls = value.X * value.X + value.Y * value.Y + value.Z * value.Z + value.W * value.W;

            T invNorm = T.One / T.Sqrt(ls);

            return new(
                value.X * invNorm,
                value.Y * invNorm,
                value.Z * invNorm,
                value.W * invNorm);
        }
    }
}
