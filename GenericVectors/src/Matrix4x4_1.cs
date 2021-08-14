// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

namespace System.Numerics
{
    /// <summary>Represents a 4x4 matrix.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    [RequiresPreviewFeatures]
    public readonly record struct Matrix4x4<T> :
        IAdditionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        IAdditiveIdentity<Matrix4x4<T>, Matrix4x4<T>>,
        IEquatable<Matrix4x4<T>>,
        IEqualityOperators<Matrix4x4<T>, Matrix4x4<T>>,
        IFormattable,
        IMultiplicativeIdentity<Matrix4x4<T>, Matrix4x4<T>>,
        IMultiplyOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        IMultiplyOperators<Matrix4x4<T>, T, Matrix4x4<T>>,
        ISubtractionOperators<Matrix4x4<T>, Matrix4x4<T>, Matrix4x4<T>>,
        IUnaryNegationOperators<Matrix4x4<T>, Matrix4x4<T>>,
        IUnaryPlusOperators<Matrix4x4<T>, Matrix4x4<T>>
        where T : struct, IFloatingPoint<T>
    {
        // Fields
        internal static readonly T BillboardEpsilon = T.Create(1e-4);
        internal static readonly T BillboardMinAngle = T.Create(1.0 - (0.1 * (Math.PI / 180.0))); // 0.1 degrees
        internal static readonly T DecomposeEpsilon = T.Create(0.0001);

        private static readonly Matrix4x4<T> _identity = new Matrix4x4<T>
        (
            T.One, T.Zero, T.Zero, T.Zero,
            T.Zero, T.One, T.Zero, T.Zero,
            T.Zero, T.Zero, T.One, T.Zero,
            T.Zero, T.Zero, T.Zero, T.One
        );

        /// <summary>The first element of the first row.</summary>
        public T M11 { get; init; }

        /// <summary>The second element of the first row.</summary>
        public T M12 { get; init; }

        /// <summary>The third element of the first row.</summary>
        public T M13 { get; init; }

        /// <summary>The fourth element of the first row.</summary>
        public T M14 { get; init; }

        /// <summary>The first element of the second row.</summary>
        public T M21 { get; init; }

        /// <summary>The second element of the second row.</summary>
        public T M22 { get; init; }

        /// <summary>The third element of the second row.</summary>
        public T M23 { get; init; }

        /// <summary>The fourth element of the second row.</summary>
        public T M24 { get; init; }

        /// <summary>The first element of the third row.</summary>
        public T M31 { get; init; }

        /// <summary>The second element of the third row.</summary>
        public T M32 { get; init; }

        /// <summary>The third element of the third row.</summary>
        public T M33 { get; init; }

        /// <summary>The fourth element of the third row.</summary>
        public T M34 { get; init; }

        /// <summary>The first element of the fourth row.</summary>
        public T M41 { get; init; }

        /// <summary>The second element of the fourth row.</summary>
        public T M42 { get; init; }

        /// <summary>The third element of the fourth row.</summary>
        public T M43 { get; init; }

        /// <summary>The fourth element of the fourth row.</summary>
        public T M44 { get; init; }

        // Constructors

        /// <summary>Creates a <see cref="System.Numerics.Matrix4x4" /> object from a specified <see cref="System.Numerics.Matrix3x2" /> object.</summary>
        /// <param name="value">A 3x2 matrix.</param>
        /// <remarks>This constructor creates a 4x4 matrix whose <see cref="System.Numerics.Matrix4x4.M13" />, <see cref="System.Numerics.Matrix4x4.M14" />, <see cref="System.Numerics.Matrix4x4.M23" />, <see cref="System.Numerics.Matrix4x4.M24" />, <see cref="System.Numerics.Matrix4x4.M31" />, <see cref="System.Numerics.Matrix4x4.M32" />, <see cref="System.Numerics.Matrix4x4.M34" />, and <see cref="System.Numerics.Matrix4x4.M43" /> components are zero, and whose <see cref="System.Numerics.Matrix4x4.M33" /> and <see cref="System.Numerics.Matrix4x4.M44" /> components are one.</remarks>
        public Matrix4x4(in Matrix3x2<T> value)
        {
            M11 = value.M11;
            M12 = value.M12;
            M13 = T.Zero;
            M14 = T.Zero;

            M21 = value.M21;
            M22 = value.M22;
            M23 = T.Zero;
            M24 = T.Zero;

            M31 = T.Zero;
            M32 = T.Zero;
            M33 = T.One;
            M34 = T.Zero;

            M41 = value.M31;
            M42 = value.M32;
            M43 = T.Zero;
            M44 = T.One;
        }

        /// <summary>Creates a 4x4 matrix from the specified components.</summary>
        /// <param name="m11">The value to assign to the first element in the first row.</param>
        /// <param name="m12">The value to assign to the second element in the first row.</param>
        /// <param name="m13">The value to assign to the third element in the first row.</param>
        /// <param name="m14">The value to assign to the fourth element in the first row.</param>
        /// <param name="m21">The value to assign to the first element in the second row.</param>
        /// <param name="m22">The value to assign to the second element in the second row.</param>
        /// <param name="m23">The value to assign to the third element in the second row.</param>
        /// <param name="m24">The value to assign to the third element in the second row.</param>
        /// <param name="m31">The value to assign to the first element in the third row.</param>
        /// <param name="m32">The value to assign to the second element in the third row.</param>
        /// <param name="m33">The value to assign to the third element in the third row.</param>
        /// <param name="m34">The value to assign to the fourth element in the third row.</param>
        /// <param name="m41">The value to assign to the first element in the fourth row.</param>
        /// <param name="m42">The value to assign to the second element in the fourth row.</param>
        /// <param name="m43">The value to assign to the third element in the fourth row.</param>
        /// <param name="m44">The value to assign to the fourth element in the fourth row.</param>
        public Matrix4x4(T m11, T m12, T m13, T m14, T m21, T m22, T m23, T m24, T m31, T m32, T m33, T m34, T m41, T m42, T m43, T m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;

            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;

            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;

            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        // Static Properties

        /// <summary>Gets the multiplicative identity matrix.</summary>
        /// <value>Gets the multiplicative identity matrix.</value>
        public static Matrix4x4<T> Identity
        {
            get => _identity;
        }

        static Matrix4x4<T> IAdditiveIdentity<Matrix4x4<T>, Matrix4x4<T>>.AdditiveIdentity
            => new(T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero);

        static Matrix4x4<T> IMultiplicativeIdentity<Matrix4x4<T>, Matrix4x4<T>>.MultiplicativeIdentity => Identity;

        // Properties

        /// <summary>Indicates whether the current matrix is the identity matrix.</summary>
        /// <value><see langword="true" /> if the current matrix is the identity matrix; otherwise, <see langword="false" />.</value>
        public readonly bool IsIdentity
        {
            get
            {
                return M11 == T.One && M22 == T.One && M33 == T.One && M44 == T.One && // Check diagonal element first for early out.
                       M12 == T.Zero && M13 == T.Zero && M14 == T.Zero &&
                       M21 == T.Zero && M23 == T.Zero && M24 == T.Zero &&
                       M31 == T.Zero && M32 == T.Zero && M34 == T.Zero &&
                       M41 == T.Zero && M42 == T.Zero && M43 == T.Zero;
            }
        }

        /// <summary>Gets or sets the translation component of this matrix.</summary>
        /// <value>The translation component of the current instance.</value>
        public Vector3<T> Translation => new Vector3<T>(M41, M42, M43);

        //TODO: Add documentation if decide to keep
        public Matrix4x4<T> WithTranslation(in Vector3<T> translation)
        {
            return this with
            {
                M41 = translation.X,
                M42 = translation.Y,
                M43 = translation.Z
            };
        }

        // Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Matrix4x4<T> operator +(Matrix4x4<T> value) => value;

        /// <summary>Negates the specified matrix by multiplying all its values by -1.</summary>
        /// <param name="value">The matrix to negate.</param>
        /// <returns>The negated matrix.</returns>
        public static Matrix4x4<T> operator -(Matrix4x4<T> value)
        {
            return new(
                -value.M11,
                -value.M12,
                -value.M13,
                -value.M14,
                -value.M21,
                -value.M22,
                -value.M23,
                -value.M24,
                -value.M31,
                -value.M32,
                -value.M33,
                -value.M34,
                -value.M41,
                -value.M42,
                -value.M43,
                -value.M44);
        }

        /// <summary>Adds each element in one matrix with its corresponding element in a second matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix that contains the summed values.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix4x4.op_Addition" /> method defines the operation of the addition operator for <see cref="System.Numerics.Matrix4x4" /> objects.</remarks>
        public static Matrix4x4<T> operator +(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            return new(
                left.M11 + right.M11,
                left.M12 + right.M12,
                left.M13 + right.M13,
                left.M14 + right.M14,
                left.M21 + right.M21,
                left.M22 + right.M22,
                left.M23 + right.M23,
                left.M24 + right.M24,
                left.M31 + right.M31,
                left.M32 + right.M32,
                left.M33 + right.M33,
                left.M34 + right.M34,
                left.M41 + right.M41,
                left.M42 + right.M42,
                left.M43 + right.M43,
                left.M44 + right.M44);
        }

        /// <summary>Subtracts each element in a second matrix from its corresponding element in a first matrix.</summary>
        /// <param name="value1">The first matrix.</param>
        /// <param name="value2">The second matrix.</param>
        /// <returns>The matrix containing the values that result from subtracting each element in <paramref name="value2" /> from its corresponding element in <paramref name="value1" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix4x4.op_Subtraction" /> method defines the operation of the subtraction operator for <see cref="System.Numerics.Matrix4x4" /> objects.</remarks>
        public static Matrix4x4<T> operator -(Matrix4x4<T> left, Matrix4x4<T> right)
        {
            return new(
                left.M11 - right.M11,
                left.M12 - right.M12,
                left.M13 - right.M13,
                left.M14 - right.M14,
                left.M21 - right.M21,
                left.M22 - right.M22,
                left.M23 - right.M23,
                left.M24 - right.M24,
                left.M31 - right.M31,
                left.M32 - right.M32,
                left.M33 - right.M33,
                left.M34 - right.M34,
                left.M41 - right.M41,
                left.M42 - right.M42,
                left.M43 - right.M43,
                left.M44 - right.M44);
        }

        /// <summary>Multiplies two matrices together to compute the product.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The product matrix.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix4x4.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Matrix4x4" /> objects.</remarks>
        public static Matrix4x4<T> operator *(Matrix4x4<T> left, Matrix4x4<T> right)
        {

            return new(
                // First row
                left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41,
                left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42,
                left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43,
                left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44,

                // Second row
                left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41,
                left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42,
                left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43,
                left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44,

                // Third row
                left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41,
                left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42,
                left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43,
                left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44,

                // Fourth row
                left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41,
                left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42,
                left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43,
                left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44);
        }

        /// <summary>Multiplies a matrix by a scalar to compute the product.</summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The scaling value to use.</param>
        /// <returns>The scaled matrix.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix4x4.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Matrix4x4" /> objects.</remarks>
        public static Matrix4x4<T> operator *(Matrix4x4<T> left, T right)
        {
            return new(
                left.M11 * right,
                left.M12 * right,
                left.M13 * right,
                left.M14 * right,
                left.M21 * right,
                left.M22 * right,
                left.M23 * right,
                left.M24 * right,
                left.M31 * right,
                left.M32 * right,
                left.M33 * right,
                left.M34 * right,
                left.M41 * right,
                left.M42 * right,
                left.M43 * right,
                left.M44 * right);
        }

        // Methods

        /// <summary>Returns a value that indicates whether this instance and another 4x4 matrix are equal.</summary>
        /// <param name="other">The other matrix.</param>
        /// <returns><see langword="true" /> if the two matrices are equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Matrix4x4<T> other)
        {
            return (M11 == other.M11 && M22 == other.M22 && M33 == other.M33 && M44 == other.M44 && // Check diagonal element first for early out.
            M12 == other.M12 && M13 == other.M13 && M14 == other.M14 && M21 == other.M21 &&
            M23 == other.M23 && M24 == other.M24 && M31 == other.M31 && M32 == other.M32 &&
            M34 == other.M34 && M41 == other.M41 && M42 == other.M42 && M43 == other.M43);
        }

        /// <summary>Calculates the determinant of the current 4x4 matrix.</summary>
        /// <returns>The determinant.</returns>
        public readonly T GetDeterminant()
        {
            // | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
            // | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
            // | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
            // | m n o p |
            //
            //   | f g h |
            // a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //   | n o p |
            //
            //   | e g h |
            // b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //   | m o p |
            //
            //   | e f h |
            // c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //   | m n p |
            //
            //   | e f g |
            // d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //   | m n o |
            //
            // Cost of operation
            // 17 adds and 28 muls.
            //
            // add: 6 + 8 + 3 = 17
            // mul: 12 + 16 = 28

            T a = M11, b = M12, c = M13, d = M14;
            T e = M21, f = M22, g = M23, h = M24;
            T i = M31, j = M32, k = M33, l = M34;
            T m = M41, n = M42, o = M43, p = M44;

            T kp_lo = k * p - l * o;
            T jp_ln = j * p - l * n;
            T jo_kn = j * o - k * n;
            T ip_lm = i * p - l * m;
            T io_km = i * o - k * m;
            T in_jm = i * n - j * m;

            return a * (f * kp_lo - g * jp_ln + h * jo_kn) -
                   b * (e * kp_lo - g * ip_lm + h * io_km) +
                   c * (e * jp_ln - f * ip_lm + h * in_jm) -
                   d * (e * jo_kn - f * io_km + g * in_jm);
        }

        /// <summary>Returns a string that represents this matrix.</summary>
        /// <returns>The string representation of this matrix.</returns>
        /// <remarks>The numeric values in the returned string are formatted by using the conventions of the current culture. For example, for the en-US culture, the returned string might appear as <c>{ {M11:1.1 M12:1.2 M13:1.3 M14:1.4} {M21:2.1 M22:2.2 M23:2.3 M24:2.4} {M31:3.1 M32:3.2 M33:3.3 M34:3.4} {M41:4.1 M42:4.2 M43:4.3 M44:4.4} }</c>.</remarks>
        public override readonly string ToString() =>
            $"{{ {{M11:{M11} M12:{M12} M13:{M13} M14:{M14}}} {{M21:{M21} M22:{M22} M23:{M23} M24:{M24}}} {{M31:{M31} M32:{M32} M33:{M33} M34:{M34}}} {{M41:{M41} M42:{M42} M43:{M43} M44:{M44}}} }}";

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
            sb.Append("{ ");

            sb.Append('{');
            sb.Append(M11.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M12.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M13.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M14.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(' ');

            sb.Append('{');
            sb.Append(M21.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M22.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M23.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M24.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(' ');

            sb.Append('{');
            sb.Append(M31.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M32.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M33.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M34.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(' ');

            sb.Append('{');
            sb.Append(M41.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M42.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M43.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M44.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(" }");
            return sb.ToString();
        }
    }
}
