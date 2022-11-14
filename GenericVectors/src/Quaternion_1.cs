// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Numerics
{
    /// <summary>Represents a vector that is used to encode three-dimensional physical rotations.</summary>
    /// <remarks>The <see cref="System.Numerics.Quaternion" /> structure is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where:
    /// <c>w = cos(theta/2)</c></remarks>
    [Intrinsic]
    public readonly record struct Quaternion<T> :
        IAdditionOperators<Quaternion<T>, Quaternion<T>, Quaternion<T>>,
        IAdditiveIdentity<Quaternion<T>, Quaternion<T>>,
        IDivisionOperators<Quaternion<T>, Quaternion<T>, Quaternion<T>>,
        IEquatable<Quaternion<T>>,
        IEqualityOperators<Quaternion<T>, Quaternion<T>, bool>,
        IFormattable,
        IMultiplicativeIdentity<Quaternion<T>, Quaternion<T>>,
        IMultiplyOperators<Quaternion<T>, Quaternion<T>, Quaternion<T>>,
        IMultiplyOperators<Quaternion<T>, T, Quaternion<T>>,
        ISubtractionOperators<Quaternion<T>, Quaternion<T>, Quaternion<T>>,
        IUnaryNegationOperators<Quaternion<T>, Quaternion<T>>,
        IUnaryPlusOperators<Quaternion<T>, Quaternion<T>>
        where T : struct, IFloatingPointIeee754<T>
    {
        // Fields

        /// <summary>The X value of the vector component of the quaternion.</summary>
        public T X { get; init; }

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public T Y { get; init; }

        /// <summary>The Z value of the vector component of the quaternion.</summary>
        public T Z { get; init; }

        /// <summary>The rotation component of the quaternion.</summary>
        public T W { get; init; }

        // Constructors

        /// <summary>Constructs a quaternion from the specified components.</summary>
        /// <param name="x">The value to assign to the X component of the quaternion.</param>
        /// <param name="y">The value to assign to the Y component of the quaternion.</param>
        /// <param name="z">The value to assign to the Z component of the quaternion.</param>
        /// <param name="w">The value to assign to the W component of the quaternion.</param>
        public Quaternion(T x, T y, T z, T w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>Creates a quaternion from the specified vector and rotation parts.</summary>
        /// <param name="vector">The vector part of the quaternion.</param>
        /// <param name="rotation">The rotation part of the quaternion.</param>
        public Quaternion(in Vector3<T> vector, T rotation)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            W = rotation;
        }

        // Static Properties

        /// <summary>Gets a quaternion that represents no rotation.</summary>
        /// <value>A quaternion whose values are <c>(0, 0, 0, 1)</c>.</value>
        public static Quaternion<T> Identity { get; } = new Quaternion<T>(T.Zero, T.Zero, T.Zero, T.One);

        // Properties

        /// <summary>Gets a value that indicates whether the current instance is the identity quaternion.</summary>
        /// <value><see langword="true" /> if the current instance is the identity quaternion; otherwise, <see langword="false" />.</value>
        /// <altmember cref="System.Numerics.Quaternion.Identity"/>
        public readonly bool IsIdentity => this == Identity;

        static Quaternion<T> IAdditiveIdentity<Quaternion<T>, Quaternion<T>>.AdditiveIdentity
            => new Quaternion<T>(T.Zero, T.Zero, T.Zero, T.Zero);

        static Quaternion<T> IMultiplicativeIdentity<Quaternion<T>, Quaternion<T>>.MultiplicativeIdentity => Identity;



        // Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Quaternion<T> operator +(Quaternion<T> value) => value;

        /// <summary>Reverses the sign of each component of the quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negated quaternion.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_UnaryNegation" /> method defines the operation of the unary negation operator for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator -(Quaternion<T> value)
        {
            return new(
                -value.X,
                -value.Y,
                -value.Z,
                -value.W);
        }

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_Addition" /> method defines the operation of the addition operator for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator +(Quaternion<T> left, Quaternion<T> right)
        {
            return new(
              left.X + right.X,
              left.Y + right.Y,
              left.Z + right.Z,
              left.W + right.W);
        }

        /// <summary>Subtracts each element in a second quaternion from its corresponding element in a first quaternion.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The quaternion containing the values that result from subtracting each element in <paramref name="right" /> from its corresponding element in <paramref name="left" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_Subtraction" /> method defines the operation of the subtraction operator for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator -(Quaternion<T> left, Quaternion<T> right)
        {
            return new(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W);
        }

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="left">The first quaternion.</param>
        /// <param name="right">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator *(Quaternion<T> left, Quaternion<T> right)
        {
            T q1x = left.X;
            T q1y = left.Y;
            T q1z = left.Z;
            T q1w = left.W;

            T q2x = right.X;
            T q2y = right.Y;
            T q2z = right.Z;
            T q2w = right.W;

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

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_Division" /> method defines the division operation for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator /(Quaternion<T> left, Quaternion<T> right)
        {
            T q1x = left.X;
            T q1y = left.Y;
            T q1z = left.Z;
            T q1w = left.W;

            //-------------------------------------
            // Inverse part.
            T ls = right.X * right.X + right.Y * right.Y +
                       right.Z * right.Z + right.W * right.W;
            T invNorm = T.One / ls;

            T q2x = -right.X * invNorm;
            T q2y = -right.Y * invNorm;
            T q2z = -right.Z * invNorm;
            T q2w = right.W * invNorm;

            //-------------------------------------
            // Multiply part.

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

        /// <summary>Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.</summary>
        /// <param name="left">The source quaternion.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled quaternion.</returns>
        /// <remarks>The <see cref="System.Numerics.Quaternion.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Quaternion" /> objects.</remarks>
        public static Quaternion<T> operator *(Quaternion<T> left, T right)
        {
            return new(
                left.X * right,
                left.Y * right,
                left.Z * right,
                left.W * right);
        }

        // Methods

        /// <summary>Returns a value that indicates whether this instance and another quaternion are equal.</summary>
        /// <param name="other">The other quaternion.</param>
        /// <returns><see langword="true" /> if the two quaternions are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two quaternions are equal if each of their corresponding components is equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion<T> other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        /// <summary>Calculates the length of the quaternion.</summary>
        /// <returns>The computed length of the quaternion.</returns>
        public readonly T Length()
        {
            T lengthSquared = LengthSquared();
            return T.Sqrt(lengthSquared);
        }

        /// <summary>Calculates the squared length of the quaternion.</summary>
        /// <returns>The length squared of the quaternion.</returns>
        public readonly T LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>Returns a string that represents this quaternion.</summary>
        /// <returns>The string representation of this quaternion.</returns>
        /// <remarks>The numeric values in the returned string are formatted by using the conventions of the current culture. For example, for the en-US culture, the returned string might appear as <c>{X:1.1 Y:2.2 Z:3.3 W:4.4}</c>.</remarks>
        public override readonly string ToString() =>
            $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString(string? format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append("{X:");
            sb.Append(X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(" Y:");
            sb.Append(Y.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(" Z:");
            sb.Append(Z.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(" W:");
            sb.Append(W.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
