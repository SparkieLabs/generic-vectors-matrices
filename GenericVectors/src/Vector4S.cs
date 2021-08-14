// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Numerics
{
    [RequiresPreviewFeatures]
    public partial struct Vector4S
    {
        // "Friendly" Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Vector4<T> Plus<T>(in Vector4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return value;
        }

        /// <summary>Negates a specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Negate<T>(in Vector4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return -value;
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Add<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left + right;
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The difference vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Subtract<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left - right;
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Multiply<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Divide<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left / right;
        }

        /// <summary>Multiplies a vector by a specified scalar.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Multiply<T>(in Vector4<T> left, T right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The vector that results from the division.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Divide<T>(in Vector4<T> left, T right)
            where T : struct, IFloatingPoint<T>
        {
            return left / right;
        }

        /// <summary>Multiplies a scalar value by a specified vector.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Multiply<T>(T left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        // Static Methods

        /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The absolute value vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Abs<T>(in Vector4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                T.Abs(value.X),
                T.Abs(value.Y),
                T.Abs(value.Z),
                T.Abs(value.W)
            );
        }

        /// <summary>Restricts a vector between a minimum and a maximum value.</summary>
        /// <param name="value">The vector to restrict.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The restricted vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Clamp<T>(in Vector4<T> value, in Vector4<T> min, in Vector4<T> max)
            where T : struct, IFloatingPoint<T>
        {
            // We must follow HLSL behavior in the case user specified min value is bigger than max value.
            return Min(Max(value, min), max);
        }

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The distance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Distance<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            T distanceSquared = DistanceSquared<T>(left, right);
            return T.Sqrt(distanceSquared);
        }

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="left">The first point.</param>
        /// <param name="right">The second point.</param>
        /// <returns>The distance squared.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DistanceSquared<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            Vector4<T> difference = left - right;
            return Dot(difference, difference);
        }

        /// <summary>Returns the dot product of two vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The dot product.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Dot<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return (left.X * right.X)
                 + (left.Y * right.Y)
                 + (left.Z * right.Z)
                 + (left.W * right.W);
        }

        /// <summary>Performs a linear interpolation between two vectors based on the given weighting.</summary>
        /// <param name="min">The first vector.</param>
        /// <param name="max">The second vector.</param>
        /// <param name="amount">A value between 0 and 1 that indicates the weight of <paramref name="max" />.</param>
        /// <returns>The interpolated vector.</returns>
        /// <remarks><format type="text/markdown"><![CDATA[
        /// The behavior of this method changed in .NET 5.0. For more information, see [Behavior change for Vector2.Lerp and Vector4.Lerp](/dotnet/core/compatibility/3.1-5.0#behavior-change-for-vector2lerp-and-vector4lerp).
        /// ]]></format></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Lerp<T>(in Vector4<T> min, in Vector4<T> max, T amount)
            where T : struct, IFloatingPoint<T>
        {
            return (min * (T.One - amount)) + (max * amount);
        }

        /// <summary>Returns a vector whose elements are the minimum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The minimized vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Min<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return new(
               (left.X < right.X) ? left.X : right.X,
               (left.Y < right.Y) ? left.Y : right.Y,
               (left.Z < right.Z) ? left.Z : right.Z,
               (left.W < right.W) ? left.W : right.W);
        }

        /// <summary>Returns a vector whose elements are the maximum of each of the pairs of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The maximized vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Max<T>(in Vector4<T> left, in Vector4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                (left.X > right.X) ? left.X : right.X,
                (left.Y > right.Y) ? left.Y : right.Y,
                (left.Z > right.Z) ? left.Z : right.Z,
                (left.W > right.W) ? left.W : right.W);
        }

        /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Normalize<T>(in Vector4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return value / value.Length();
        }

        /// <summary>Returns the reflection of a vector off a surface that has the specified normal.</summary>
        /// <param name="incident">The source vector.</param>
        /// <param name="normal">The normal of the surface being reflected off.</param>
        /// <returns>The reflected vector.</returns>
        public static Vector4<T> Reflect<T>(in Vector4<T> incident, in Vector4<T> normal)
            where T : struct, IFloatingPoint<T>
        {
            T dot = Dot(incident, normal);
            return incident - (T.Create(2.0) * dot * normal);
        }

        /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The square root vector.</returns>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> SquareRoot<T>(in Vector4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                T.Sqrt(value.X),
                T.Sqrt(value.Y),
                T.Sqrt(value.Z),
                T.Sqrt(value.W)
            );
        }

        /// <summary>Transforms a two-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector2<T> position, in Matrix4x4<T> matrix)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) + matrix.M43,
                (position.X * matrix.M14) + (position.Y * matrix.M24) + matrix.M44
            );
        }

        /// <summary>Transforms a three-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector3<T> position, in Matrix4x4<T> matrix)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + matrix.M41,
                (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + matrix.M42,
                (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + matrix.M43,
                (position.X * matrix.M14) + (position.Y * matrix.M24) + (position.Z * matrix.M34) + matrix.M44
            );
        }

        /// <summary>Transforms a four-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector4<T> position, in Matrix4x4<T> matrix)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                (position.X * matrix.M11) + (position.Y * matrix.M21) + (position.Z * matrix.M31) + (position.W * matrix.M41),
                (position.X * matrix.M12) + (position.Y * matrix.M22) + (position.Z * matrix.M32) + (position.W * matrix.M42),
                (position.X * matrix.M13) + (position.Y * matrix.M23) + (position.Z * matrix.M33) + (position.W * matrix.M43),
                (position.X * matrix.M14) + (position.Y * matrix.M24) + (position.Z * matrix.M34) + (position.W * matrix.M44)
            );
        }

        /// <summary>Transforms a two-dimensional vector by the specified Quaternion rotation value.</summary>
        /// <param name="position">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector2<T> position, in Quaternion<T> rotation)
            where T : struct, IFloatingPoint<T>
        {
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

            return new(
                position.X * (T.One - yy2 - zz2) + position.Y * (xy2 - wz2),
                position.X * (xy2 + wz2) + position.Y * (T.One - xx2 - zz2),
                position.X * (xz2 - wy2) + position.Y * (yz2 + wx2),
                T.One
            );
        }

        /// <summary>Transforms a three-dimensional vector by the specified Quaternion rotation value.</summary>
        /// <param name="position">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector3<T> position, in Quaternion<T> rotation)
            where T : struct, IFloatingPoint<T>
        {
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

            return new(
                position.X * (T.One - yy2 - zz2) + position.Y * (xy2 - wz2) + position.Z * (xz2 + wy2),
                position.X * (xy2 + wz2) + position.Y * (T.One - xx2 - zz2) + position.Z * (yz2 - wx2),
                position.X * (xz2 - wy2) + position.Y * (yz2 + wx2) + position.Z * (T.One - xx2 - yy2),
                T.One
            );
        }

        /// <summary>Transforms a four-dimensional vector by the specified Quaternion rotation value.</summary>
        /// <param name="position">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> Transform<T>(in Vector4<T> position, in Quaternion<T> rotation)
            where T : struct, IFloatingPoint<T>
        {
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

            return new(
                position.X * (T.One - yy2 - zz2) + position.Y * (xy2 - wz2) + position.Z * (xz2 + wy2),
                position.X * (xy2 + wz2) + position.Y * (T.One - xx2 - zz2) + position.Z * (yz2 - wx2),
                position.X * (xz2 - wy2) + position.Y * (yz2 + wx2) + position.Z * (T.One - xx2 - yy2),
                position.W);
        }

        /// <summary>Transforms a vector normal by the given 4x4 matrix.</summary>
        /// <param name="normal">The source vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> TransformNormal<T>(in Vector4<T> normal, in Matrix4x4<T> matrix)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                (normal.X * matrix.M11) + (normal.Y * matrix.M21) + (normal.Z * matrix.M31),
                (normal.X * matrix.M12) + (normal.Y * matrix.M22) + (normal.Z * matrix.M32),
                (normal.X * matrix.M13) + (normal.Y * matrix.M23) + (normal.Z * matrix.M33),
                normal.W
            );
        }
    }
}
