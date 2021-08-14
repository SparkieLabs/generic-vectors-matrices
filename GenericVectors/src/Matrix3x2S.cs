// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Numerics
{
    [RequiresPreviewFeatures]
    public partial struct Matrix3x2S
    {
        // "Friendly" Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Matrix3x2<T> Plus<T>(in Matrix3x2<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return value;
        }

        /// <summary>Negates the specified matrix by multiplying all its values by -1.</summary>
        /// <param name="value">The matrix to negate.</param>
        /// <returns>The negated matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2<T> Negate<T>(in Matrix3x2<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return -value;
        }

        /// <summary>Adds each element in one matrix with its corresponding element in a second matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2<T> Add<T>(in Matrix3x2<T> left, in Matrix3x2<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left + right;
        }

        /// <summary>Subtracts each element in a second matrix from its corresponding element in a first matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix containing the values that result from subtracting each element in <paramref name="right" /> from its corresponding element in <paramref name="left" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2<T> Subtract<T>(in Matrix3x2<T> left, in Matrix3x2<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left - right;
        }

        /// <summary>Multiplies two matrices together to compute the product.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The product matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2<T> Multiply<T>(in Matrix3x2<T> left, in Matrix3x2<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        /// <summary>
        /// Scales all elements in a matrix by the given scalar factor.
        /// </summary>
        /// <param name="left">The source matrix.</param>
        /// <param name="right">The scaling value to use.</param>
        /// <returns>The resulting matrix.</returns>
        public static Matrix3x2<T> Multiply<T>(in Matrix3x2<T> left, T right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        // Static Methods

        /// <summary>Creates a rotation matrix using the given rotation in radians.</summary>
        /// <param name="radians">The amount of rotation, in radians.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix3x2<T> CreateRotation<T>(T radians)
            where T : struct, IFloatingPoint<T>
        {
            T epsilon = T.Create(0.001 * Math.PI / 180.0);     // 0.1% of a degree

            radians = T.IEEERemainder(radians, T.Tau);

            T c, s;
            if (radians > -epsilon && radians < epsilon)
            {
                // Exact case for zero rotation.
                c = T.One;
                s = T.Zero;
            }
            else if (radians > T.Pi / T.Create(2.0) - epsilon && radians < T.Pi / T.Create(2.0) + epsilon)
            {
                // Exact case for 90 degree rotation.
                c = T.Zero;
                s = T.One;
            }
            else if (radians < -T.Pi + epsilon || radians > T.Pi - epsilon)
            {
                // Exact case for 180 degree rotation.
                c = -T.One;
                s = T.Zero;
            }
            else if (radians > -T.Pi / T.Create(2.0) - epsilon && radians < -T.Pi / T.Create(2.0) + epsilon)
            {
                // Exact case for 270 degree rotation.
                c = T.Zero;
                s = -T.One;
            }
            else
            {
                // Arbitrary rotation.
                c = T.Cos(radians);
                s = T.Sin(radians);
            }

            // [  c  s ]
            // [ -s  c ]
            // [  0  0 ]
            return new(
                c, s,
                -s, c,
                T.Zero, T.Zero);
        }

        /// <summary>Creates a rotation matrix using the specified rotation in radians and a center point.</summary>
        /// <param name="radians">The amount of rotation, in radians.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix3x2<T> CreateRotation<T>(T radians, in Vector2<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T epsilon = T.Create(0.001 * Math.PI / 180.0);     // 0.1% of a degree

            radians = T.IEEERemainder(radians, T.Tau);

            T c, s;
            if (radians > -epsilon && radians < epsilon)
            {
                // Exact case for zero rotation.
                c = T.One;
                s = T.Zero;
            }
            else if (radians > T.Pi / T.Create(2.0) - epsilon && radians < T.Pi / T.Create(2.0) + epsilon)
            {
                // Exact case for 90 degree rotation.
                c = T.Zero;
                s = T.One;
            }
            else if (radians < -T.Pi + epsilon || radians > T.Pi - epsilon)
            {
                // Exact case for 180 degree rotation.
                c = -T.One;
                s = T.Zero;
            }
            else if (radians > -T.Pi / T.Create(2.0) - epsilon && radians < -T.Pi / T.Create(2.0) + epsilon)
            {
                // Exact case for 270 degree rotation.
                c = T.Zero;
                s = -T.One;
            }
            else
            {
                // Arbitrary rotation.
                c = T.Cos(radians);
                s = T.Sin(radians);
            }

            T tx = centerPoint.X * (T.One - c) + centerPoint.Y * s;
            T ty = centerPoint.Y * (T.One - c) - centerPoint.X * s;

            // [  c  s ]
            // [ -s  c ]
            // [  tx  ty ]
            return new(
                c, s,
                -s, c,
                tx, ty);
        }

        /// <summary>Creates a scaling matrix that scales uniformly with the given scale.</summary>
        /// <param name="scale">The uniform scale to use.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(T scale)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                scale, T.Zero,
                T.Zero, scale,
                T.Zero, T.Zero);
        }

        /// <summary>Creates a scaling matrix that scales uniformly with the specified scale with an offset from the specified center.</summary>
        /// <param name="scale">The uniform scale to use.</param>
        /// <param name="centerPoint">The center offset.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(T scale, in Vector2<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scale);
            T ty = centerPoint.Y * (T.One - scale);

            return new(
                scale, T.Zero,
                T.Zero, scale,
                tx, ty);
        }

        /// <summary>Creates a scaling matrix from the specified X and Y components.</summary>
        /// <param name="scaleX">The value to scale by on the X axis.</param>
        /// <param name="scaleY">The value to scale by on the Y axis.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(T scaleX, T scaleY)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                scaleX, T.Zero,
                T.Zero, scaleY,
                T.Zero, T.Zero);
        }

        /// <summary>Creates a scaling matrix that is offset by a given center point.</summary>
        /// <param name="scaleX">The value to scale by on the X axis.</param>
        /// <param name="scaleY">The value to scale by on the Y axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(T scaleX, T scaleY, in Vector2<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scaleX);
            T ty = centerPoint.Y * (T.One - scaleY);

            return new(
                scaleX, T.Zero,
                T.Zero, scaleY,
                tx, ty);
        }

        /// <summary>Creates a scaling matrix from the specified vector scale.</summary>
        /// <param name="scale">The scale to use.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(in Vector2<T> scale)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                scale.X, T.Zero,
                T.Zero, scale.Y,
                T.Zero, T.Zero);
        }

        /// <summary>Creates a scaling matrix from the specified vector scale with an offset from the specified center point.</summary>
        /// <param name="scale">The scale to use.</param>
        /// <param name="centerPoint">The center offset.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3x2<T> CreateScale<T>(in Vector2<T> scale, in Vector2<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scale.X);
            T ty = centerPoint.Y * (T.One - scale.Y);

            return new(
               scale.X, T.Zero,
               T.Zero, scale.Y,
               tx, ty);
        }

        /// <summary>Creates a skew matrix from the specified angles in radians.</summary>
        /// <param name="radiansX">The X angle, in radians.</param>
        /// <param name="radiansY">The Y angle, in radians.</param>
        /// <returns>The skew matrix.</returns>
        public static Matrix3x2<T> CreateSkew<T>(T radiansX, T radiansY)
            where T : struct, IFloatingPoint<T>
        {
            T xTan = T.Tan(radiansX);
            T yTan = T.Tan(radiansY);

            return new(
               T.One, yTan,
               xTan, T.One,
               T.Zero, T.Zero);
        }

        /// <summary>Creates a skew matrix from the specified angles in radians and a center point.</summary>
        /// <param name="radiansX">The X angle, in radians.</param>
        /// <param name="radiansY">The Y angle, in radians.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The skew matrix.</returns>
        public static Matrix3x2<T> CreateSkew<T>(T radiansX, T radiansY, in Vector2<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T xTan = T.Tan(radiansX);
            T yTan = T.Tan(radiansY);

            T tx = -centerPoint.Y * xTan;
            T ty = -centerPoint.X * yTan;

            return new(
               T.One, yTan,
               xTan, T.One,
               tx, ty);
        }

        /// <summary>Creates a translation matrix from the specified X and Y components.</summary>
        /// <param name="xPosition">The X position.</param>
        /// <param name="yPosition">The Y position.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix3x2<T> CreateTranslation<T>(T positionX, T positionY)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                T.One, T.Zero,
                T.Zero, T.One,
                positionX, positionY);
        }

        /// <summary>Creates a translation matrix from the specified 2-dimensional vector.</summary>
        /// <param name="position">The translation position.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix3x2<T> CreateTranslation<T>(in Vector2<T> position)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                T.One, T.Zero,
                T.Zero, T.One,
                position.X, position.Y);
        }

        /// <summary>Tries to invert the specified matrix. The return value indicates whether the operation succeeded.</summary>
        /// <param name="matrix">The matrix to invert.</param>
        /// <param name="result">When this method returns, contains the inverted matrix if the operation succeeded.</param>
        /// <returns><see langword="true" /> if <paramref name="matrix" /> was converted successfully; otherwise,  <see langword="false" />.</returns>
        public static bool Invert<T>(in Matrix3x2<T> matrix, out Matrix3x2<T> result)
            where T : struct, IFloatingPoint<T>
        {
            T det = (matrix.M11 * matrix.M22) - (matrix.M21 * matrix.M12);

            if (T.Abs(det) < T.Epsilon)
            {
                result = new(T.NaN, T.NaN, T.NaN, T.NaN, T.NaN, T.NaN);
                return false;
            }

            T invDet = T.One / det;

            result = new(
                matrix.M22 * invDet,
                -matrix.M12 * invDet,
                -matrix.M21 * invDet,
                matrix.M11 * invDet,
                (matrix.M21 * matrix.M32 - matrix.M31 * matrix.M22) * invDet,
                (matrix.M31 * matrix.M12 - matrix.M11 * matrix.M32) * invDet);

            return true;
        }

        /// <summary>Performs a linear interpolation from one matrix to a second matrix based on a value that specifies the weighting of the second matrix.</summary>
        /// <param name="min">The first matrix.</param>
        /// <param name="max">The second matrix.</param>
        /// <param name="amount">The relative weighting of <paramref name="matrix2" />.</param>
        /// <returns>The interpolated matrix.</returns>
        public static Matrix3x2<T> Lerp<T>(in Matrix3x2<T> min, in Matrix3x2<T> max, T amount)
            where T : struct, IFloatingPoint<T>
        {
            return new(
                 // First row
                 min.M11 + (max.M11 - min.M11) * amount,
                 min.M12 + (max.M12 - min.M12) * amount,

                // Second row
                min.M21 + (max.M21 - min.M21) * amount,
                min.M22 + (max.M22 - min.M22) * amount,

                 // Third row
                 min.M31 + (max.M31 - min.M31) * amount,
                 min.M32 + (max.M32 - min.M32) * amount);
        }
    }
}
