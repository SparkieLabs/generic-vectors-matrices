// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Numerics
{
    /// <summary>Represents a vector with four single-precision floating-point values.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// The <xref:System.Numerics.Vector4> structure provides support for hardware acceleration.
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    public readonly record struct Vector4<T> :
        IAdditionOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        IAdditiveIdentity<Vector4<T>, Vector4<T>>,
        IDivisionOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        IDivisionOperators<Vector4<T>, T, Vector4<T>>,
        IEquatable<Vector4<T>>,
        IEqualityOperators<Vector4<T>, Vector4<T>, bool>,
        IFormattable,
        IMultiplicativeIdentity<Vector4<T>, Vector4<T>>,
        IMultiplyOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        IMultiplyOperators<Vector4<T>, T, Vector4<T>>,
        ISubtractionOperators<Vector4<T>, Vector4<T>, Vector4<T>>,
        IUnaryNegationOperators<Vector4<T>, Vector4<T>>,
        IUnaryPlusOperators<Vector4<T>, Vector4<T>>
    where T : struct, IFloatingPointIeee754<T>
    {
        // Fields

        /// <summary>The X component of the vector.</summary>
        public T X { get; init; }

        /// <summary>The Y component of the vector.</summary>
        public T Y { get; init; }

        /// <summary>The Z component of the vector.</summary>
        public T Z { get; init; }

        /// <summary>The W component of the vector.</summary>
        public T W { get; init; }

        // Constructors

        /// <summary>Creates a new <see cref="System.Numerics.Vector4" /> object whose four elements have the same value.</summary>
        /// <param name="value">The value to assign to all four elements.</param>
        [Intrinsic]
        public Vector4(T value)
        {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>Creates a vector whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="System.Numerics.Vector4.X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="System.Numerics.Vector4.Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="System.Numerics.Vector4.Z" /> field.</param>
        /// <param name="w">The value to assign to the <see cref="System.Numerics.Vector4.W" /> field.</param>
        [Intrinsic]
        public Vector4(T x, T y, T z, T w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>Creates a   new <see cref="System.Numerics.Vector4" /> object from the specified <see cref="System.Numerics.Vector2" /> object and a Z and a W component.</summary>
        /// <param name="value">The vector to use for the X and Y components.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        [Intrinsic]
        public Vector4(in Vector2<T> vector, T z, T w)
        {
            X = vector.X;
            Y = vector.Y;
            Z = z;
            W = w;
        }

        /// <summary>Constructs a new <see cref="System.Numerics.Vector4" /> object from the specified <see cref="System.Numerics.Vector3" /> object and a W component.</summary>
        /// <param name="value">The vector to use for the X, Y, and Z components.</param>
        /// <param name="w">The W component.</param>
        [Intrinsic]
        public Vector4(in Vector3<T> vector, T w)
        {
            X = vector.X;
            Y = vector.Y;
            Z = vector.Z;
            W = w;
        }

        // Doesn't exist for Vector4
        public Vector4(T[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            X = value[0];
            Y = value[1];
            Z = value[2];
            W = value[3];
        }

        public Vector4(T[] value, int offset)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            X = value[0 + offset];
            Y = value[1 + offset];
            Z = value[2 + offset];
            W = value[3 + offset];
        }

        /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Single}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        public Vector4(ReadOnlySpan<T> value)
        {
            if (value.Length < 4)
            {
                VectorS.ThrowInsufficientNumberOfElementsException(4);
            }

            this = Unsafe.ReadUnaligned<Vector4<T>>(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(value)));
        }

        // Static Properties

        /// <summary>Gets a vector whose 4 elements are equal to one.</summary>
        /// <value>Returns <see cref="System.Numerics.Vector4" />.</value>
        /// <remarks>A vector whose four elements are equal to one (that is, it returns the vector <c>(1,1,1,1)</c>.</remarks>
        public static Vector4<T> One { [Intrinsic] get; } = new(T.One);

        /// <summary>Gets the vector (1,0,0,0).</summary>
        /// <value>The vector <c>(1,0,0,0)</c>.</value>
        public static Vector4<T> UnitX { get; } = new(T.One, T.Zero, T.Zero, T.Zero);

        /// <summary>Gets the vector (0,1,0,0).</summary>
        /// <value>The vector <c>(0,1,0,0)</c>.</value>
        public static Vector4<T> UnitY { get; } = new(T.Zero, T.One, T.Zero, T.Zero);

        /// <summary>Gets the vector (0,0,1,0).</summary>
        /// <value>The vector <c>(0,0,1,0)</c>.</value>
        public static Vector4<T> UnitZ { get; } = new(T.Zero, T.Zero, T.One, T.Zero);

        /// <summary>Gets the vector (0,0,0,1).</summary>
        /// <value>The vector <c>(0,0,0,1)</c>.</value>
        public static Vector4<T> UnitW { get; } = new(T.Zero, T.Zero, T.Zero, T.One);

        /// <summary>Gets a vector whose 4 elements are equal to zero.</summary>
        /// <value>A vector whose four elements are equal to zero (that is, it returns the vector <c>(0,0,0,0)</c>.</value>
        public static Vector4<T> Zero { [Intrinsic] get; } = new(T.Zero, T.Zero, T.Zero, T.Zero);

        static Vector4<T> IAdditiveIdentity<Vector4<T>, Vector4<T>>.AdditiveIdentity => Zero;

        static Vector4<T> IMultiplicativeIdentity<Vector4<T>, Vector4<T>>.MultiplicativeIdentity => One;

        // Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        public static Vector4<T> operator +(Vector4<T> value) => value;

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_UnaryNegation" /> method defines the unary negation operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator -(Vector4<T> value) => Zero - value;

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Addition" /> method defines the addition operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator +(Vector4<T> left, Vector4<T> right)
        {
            return new(
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z,
                left.W + right.W
            );
        }

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Subtraction" /> method defines the subtraction operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator -(Vector4<T> left, Vector4<T> right)
        {
            return new(
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z,
                left.W - right.W
            );
        }

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Multiply" /> method defines the multiplication operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator *(Vector4<T> left, Vector4<T> right)
        {
            return new(
                left.X * right.X,
                left.Y * right.Y,
                left.Z * right.Z,
                left.W * right.W
            );
        }

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Division" /> method defines the division operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator /(Vector4<T> left, Vector4<T> right)
        {
            return new(
                left.X / right.X,
                left.Y / right.Y,
                left.Z / right.Z,
                left.W / right.W
            );
        }

        /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Multiply" /> method defines the multiplication operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator *(Vector4<T> left, T right)
        {
            return new(left.X * right, left.Y * right, left.Z * right, left.W * right);
        }

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="value1">The vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Division" /> method defines the division operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator /(Vector4<T> left, T right)
        {
            return new(left.X / right, left.Y / right, left.Z / right, left.W / right);
        }

        /// <summary>Multiplies the scalar value by the specified vector.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="System.Numerics.Vector4.op_Multiply" /> method defines the multiplication operation for <see cref="System.Numerics.Vector4" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4<T> operator *(T left, Vector4<T> right)
        {
            return new(left * right.X, left * right.Y, left * right.Z, left * right.W);
        }

        // Methods

        /// <summary>Copies the elements of the vector to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least four elements. The method copies the vector's elements starting at index 0.</remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="System.RankException"><paramref name="array" /> is multidimensional.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the vector.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four vector elements. In other words, elements <paramref name="index" /> through <paramref name="index" /> + 3 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="System.ArgumentNullException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// -or-
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="System.RankException"><paramref name="array" /> is multidimensional.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(T[] array, int index)
        {
            if (array is null)
            {
                // Match the JIT's exception type here. For perf, a NullReference is thrown instead of an ArgumentNull.
                throw new NullReferenceException(SR.Arg_NullArgumentNullRef);
            }

            if ((index < 0) || (index >= array.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(index), SR.Format(SR.Arg_ArgumentOutOfRangeException, index));
            }

            if ((array.Length - index) < 4)
            {
                throw new ArgumentException(SR.Format(SR.Arg_ElementsInSourceIsGreaterThanDestination, index));
            }

            array[index] = X;
            array[index + 1] = Y;
            array[index + 2] = Z;
            array[index + 3] = W;
        }

        /// <summary>Copies the vector to the given <see cref="Span{T}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="System.ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
        public readonly void CopyTo(Span<T> destination)
        {
            if (destination.Length < 4)
            {
                ThrowHelper.ThrowArgumentException_DestinationTooShort();
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), this);
        }

        /// <summary>Attempts to copy the vector to the given <see cref="Span{Single}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
        public readonly bool TryCopyTo(Span<T> destination)
        {
            if (destination.Length < 4)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), this);

            return true;
        }

        /// <summary>Returns a value that indicates whether this instance and another vector are equal.</summary>
        /// <param name="other">The other vector.</param>
        /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two vectors are equal if their <see cref="System.Numerics.Vector4.X" />, <see cref="System.Numerics.Vector4.Y" />, <see cref="System.Numerics.Vector4.Z" />, and <see cref="System.Numerics.Vector4.W" /> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector4<T> other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        /// <summary>Returns the length of this vector object.</summary>
        /// <returns>The vector's length.</returns>
        /// <altmember cref="System.Numerics.Vector4.LengthSquared"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T Length()
        {
            T lengthSquared = LengthSquared();
            return T.Sqrt(lengthSquared);
        }

        /// <summary>Returns the length of the vector squared.</summary>
        /// <returns>The vector's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="System.Numerics.Vector4.Length" /> method.</remarks>
        /// <altmember cref="System.Numerics.Vector4.Length"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T LengthSquared()
        {
            return Vector4S.Dot(this, this);
        }

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

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
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString(string? format, IFormatProvider? formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            sb.Append('<');
            sb.Append(X.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Y.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(Z.ToString(format, formatProvider));
            sb.Append(separator);
            sb.Append(' ');
            sb.Append(W.ToString(format, formatProvider));
            sb.Append('>');
            return sb.ToString();
        }
    }
}
