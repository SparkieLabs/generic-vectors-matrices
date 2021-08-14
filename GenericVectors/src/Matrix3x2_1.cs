// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

namespace System.Numerics
{
    /// <summary>Represents a 3x2 matrix.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    [RequiresPreviewFeatures]
    public readonly record struct Matrix3x2<T> :
        IAdditionOperators<Matrix3x2<T>, Matrix3x2<T>, Matrix3x2<T>>,
        IAdditiveIdentity<Matrix3x2<T>, Matrix3x2<T>>,
        IEquatable<Matrix3x2<T>>,
        IEqualityOperators<Matrix3x2<T>, Matrix3x2<T>>,
        IFormattable,
        IMultiplicativeIdentity<Matrix3x2<T>, Matrix3x2<T>>,
        IMultiplyOperators<Matrix3x2<T>, Matrix3x2<T>, Matrix3x2<T>>,
        IMultiplyOperators<Matrix3x2<T>, T, Matrix3x2<T>>,
        ISubtractionOperators<Matrix3x2<T>, Matrix3x2<T>, Matrix3x2<T>>,
        IUnaryNegationOperators<Matrix3x2<T>, Matrix3x2<T>>,
        IUnaryPlusOperators<Matrix3x2<T>, Matrix3x2<T>>
        where T : struct, IFloatingPoint<T>
    {
        // Fields

        /// <summary>The first element of the first row.</summary>
        public T M11 { get; init; }

        /// <summary>The second element of the first row.</summary>
        public T M12 { get; init; }

        /// <summary>The first element of the second row.</summary>
        public T M21 { get; init; }

        /// <summary>The second element of the second row.</summary>
        public T M22 { get; init; }

        /// <summary>The first element of the third row.</summary>
        public T M31 { get; init; }

        /// <summary>The second element of the third row.</summary>
        public T M32 { get; init; }

        // Constructors

        /// <summary>Creates a 3x2 matrix from the specified components.</summary>
        /// <param name="m11">The value to assign to the first element in the first row.</param>
        /// <param name="m12">The value to assign to the second element in the first row.</param>
        /// <param name="m21">The value to assign to the first element in the second row.</param>
        /// <param name="m22">The value to assign to the second element in the second row.</param>
        /// <param name="m31">The value to assign to the first element in the third row.</param>
        /// <param name="m32">The value to assign to the second element in the third row.</param>
        public Matrix3x2(T m11, T m12, T m21, T m22, T m31, T m32)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
            M31 = m31;
            M32 = m32;
        }

        // Static Properties

        /// <summary>Gets the multiplicative identity matrix.</summary>
        /// <value>The multiplicative identify matrix.</value>
        public static Matrix3x2<T> Identity { get; } = new(
            T.One, T.Zero,
            T.Zero, T.One,
            T.Zero, T.Zero);

        static Matrix3x2<T> IAdditiveIdentity<Matrix3x2<T>, Matrix3x2<T>>.AdditiveIdentity
            => new(T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero);

        static Matrix3x2<T> IMultiplicativeIdentity<Matrix3x2<T>, Matrix3x2<T>>.MultiplicativeIdentity => Identity;

        // Properties


        /// <summary>Gets a value that indicates whether the current matrix is the identity matrix.</summary>
        /// <value><see langword="true" /> if the current matrix is the identity matrix; otherwise, <see langword="false" />.</value>
        public bool IsIdentity
        {
            get
            {
                return M11 == T.One && M22 == T.One && // Check diagonal element first for early out.
                       M12 == T.Zero && M21 == T.Zero &&
                       M31 == T.Zero && M32 == T.Zero;
            }
        }

        /// <summary>Gets or sets the translation component of this matrix.</summary>
        /// <value>The translation component of the current instance.</value>
        public Vector2<T> Translation => new(M31, M32);

        // Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Matrix3x2<T> operator +(Matrix3x2<T> value)
        {
            return new(
               value.M11,
               value.M12,
               value.M21,
               value.M22,
               value.M31,
               value.M32);
        }

        /// <summary>Negates the specified matrix by multiplying all its values by -1.</summary>
        /// <param name="value">The matrix to negate.</param>
        /// <returns>The negated matrix.</returns>
        /// <altmember cref="System.Numerics.Matrix3x2.Negate(System.Numerics.Matrix3x2)"/>
        public static Matrix3x2<T> operator -(Matrix3x2<T> value)
        {
            return new(
               -value.M11,
               -value.M12,
               -value.M21,
               -value.M22,
               -value.M31,
               -value.M32);
        }

        /// <summary>Adds each element in one matrix with its corresponding element in a second matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix that contains the summed values.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix3x2.op_Addition" /> method defines the operation of the addition operator for <see cref="System.Numerics.Matrix3x2" /> objects.</remarks>
        public static Matrix3x2<T> operator +(Matrix3x2<T> left, Matrix3x2<T> right)
        {
            return new(
                left.M11 + right.M11,
                left.M12 + right.M12,
                left.M21 + right.M21,
                left.M22 + right.M22,
                left.M31 + right.M31,
                left.M32 + right.M32);
        }

        /// <summary>Subtracts each element in a second matrix from its corresponding element in a first matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix containing the values that result from subtracting each element in <paramref name="value2" /> from its corresponding element in <paramref name="value1" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix3x2.Subtract" /> method defines the operation of the subtraction operator for <see cref="System.Numerics.Matrix3x2" /> objects.</remarks>
        public static Matrix3x2<T> operator -(Matrix3x2<T> left, Matrix3x2<T> right)
        {
            return new(
               left.M11 - right.M11,
               left.M12 - right.M12,
               left.M21 - right.M21,
               left.M22 - right.M22,
               left.M31 - right.M31,
               left.M32 - right.M32);
        }

        /// <summary>Multiplies two matrices together to compute the product.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The product matrix.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix3x2.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Matrix3x2" /> objects.</remarks>
        public static Matrix3x2<T> operator *(Matrix3x2<T> left, Matrix3x2<T> right)
        {
            return new(
                // First row
                left.M11 * right.M11 + left.M12 * right.M21,
                left.M11 * right.M12 + left.M12 * right.M22,

                // Second row
                left.M21 * right.M11 + left.M22 * right.M21,
                left.M21 * right.M12 + left.M22 * right.M22,

                // Third row
                left.M31 * right.M11 + left.M32 * right.M21 + right.M31,
                left.M31 * right.M12 + left.M32 * right.M22 + right.M32);
        }

        /// <summary>Multiplies a matrix by a float to compute the product.</summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The scaling value to use.</param>
        /// <returns>The scaled matrix.</returns>
        /// <remarks>The <see cref="System.Numerics.Matrix3x2.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="System.Numerics.Matrix3x2" /> objects.</remarks>
        public static Matrix3x2<T> operator *(Matrix3x2<T> left, T right)
        {
            return new(
                left.M11 * right,
                left.M12 * right,
                left.M21 * right,
                left.M22 * right,
                left.M31 * right,
                left.M32 * right);
        }

        // Methods

        /// <summary>Returns a value that indicates whether this instance and another 3x2 matrix are equal.</summary>
        /// <param name="other">The other matrix.</param>
        /// <returns><see langword="true" /> if the two matrices are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two matrices are equal if all their corresponding elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Matrix3x2<T> other)
        {
            // Check diagonal element first for early out.
            return (M11 == other.M11
                 && M22 == other.M22
                 && M12 == other.M12
                 && M21 == other.M21
                 && M31 == other.M31
                 && M32 == other.M32);
        }

        /// <summary>Calculates the determinant for this matrix.</summary>
        /// <returns>The determinant.</returns>
        /// <remarks>The determinant is calculated by expanding the matrix with a third column whose values are (0,0,1).</remarks>
        public T GetDeterminant()
        {
            // There isn't actually any such thing as a determinant for a non-square matrix,
            // but this 3x2 type is really just an optimization of a 3x3 where we happen to
            // know the rightmost column is always (0, 0, 1). So we expand to 3x3 format:
            //
            //  [ M11, M12, 0 ]
            //  [ M21, M22, 0 ]
            //  [ M31, M32, 1 ]
            //
            // Sum the diagonal products:
            //  (M11 * M22 * 1) + (M12 * 0 * M31) + (0 * M21 * M32)
            //
            // Subtract the opposite diagonal products:
            //  (M31 * M22 * 0) + (M32 * 0 * M11) + (1 * M21 * M12)
            //
            // Collapse out the constants and oh look, this is just a 2x2 determinant!

            return (M11 * M22) - (M21 * M12);
        }

        /// <summary>Returns a string that represents this matrix.</summary>
        /// <returns>The string representation of this matrix.</returns>
        /// <remarks>The numeric values in the returned string are formatted by using the conventions of the current culture. For example, for the en-US culture, the returned string might appear as <c>{ {M11:1.1 M12:1.2} {M21:2.1 M22:2.2} {M31:3.1 M32:3.2} }</c>.</remarks>
        public override string ToString()
            => $"{{ {{M11:{M11} M12:{M12}}} {{M21:{M21} M22:{M22}}} {{M31:{M31} M32:{M32}}} }}";

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
            sb.Append('}');

            sb.Append(' ');

            sb.Append('{');
            sb.Append(M21.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M22.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(' ');

            sb.Append('{');
            sb.Append(M31.ToString(format, formatProvider));
            sb.Append(' ');
            sb.Append(M32.ToString(format, formatProvider));
            sb.Append('}');

            sb.Append(" }");
            return sb.ToString();
        }
    }
}
