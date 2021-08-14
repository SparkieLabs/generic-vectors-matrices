// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

namespace System.Numerics
{
    /// <summary>Represents a plane in three-dimensional space.</summary>
    /// <remarks><format type="text/markdown"><![CDATA[
    /// [!INCLUDE[vectors-are-rows-paragraph](~/includes/system-numerics-vectors-are-rows.md)]
    /// ]]></format></remarks>
    [Intrinsic]
    [RequiresPreviewFeatures]
    public readonly record struct Plane<T> :
        IEquatable<Plane<T>>,
        IEqualityOperators<Plane<T>, Plane<T>>,
        IFormattable
        where T : struct, IFloatingPoint<T>
    {
        // Fields

        /// <summary>The distance of the plane along its normal from the origin.</summary>
        public T Distance { get; init; }

        /// <summary>The normal vector of the plane.</summary>
        public Vector3<T> Normal { get; init; }

        // Constructors

        /// <summary>Creates a <see cref="System.Numerics.Plane" /> object from the X, Y, and Z components of its normal, and its distance from the origin on that normal.</summary>
        /// <param name="x">The X component of the normal.</param>
        /// <param name="y">The Y component of the normal.</param>
        /// <param name="z">The Z component of the normal.</param>
        /// <param name="distance">The distance of the plane along its normal from the origin.</param>
        public Plane(T x, T y, T z, T distance)
        {
            Normal = new(x, y, z);
            Distance = distance;
        }

        /// <summary>Creates a <see cref="System.Numerics.Plane" /> object from a specified normal and the distance along the normal from the origin.</summary>
        /// <param name="normal">The plane's normal vector.</param>
        /// <param name="distance">The plane's distance from the origin along its normal vector.</param>
        public Plane(in Vector3<T> normal, T distance)
        {
            Normal = normal;
            Distance = distance;
        }

        /// <summary>Creates a <see cref="System.Numerics.Plane" /> object from a specified four-dimensional vector.</summary>
        /// <param name="value">A vector whose first three elements describe the normal vector, and whose <see cref="System.Numerics.Vector4.W" /> defines the distance along that normal from the origin.</param>
        public Plane(in Vector4<T> value)
        {
            Normal = new(value.X, value.Y, value.Z);
            Distance = value.W;
        }

        // Operators

        // Methods

        /// <summary>Returns a value that indicates whether this instance and another plane are equal.</summary>
        /// <param name="other">The other plane.</param>
        /// <returns><see langword="true" /> if the two planes are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="System.Numerics.Plane" /> objects are equal if their <see cref="System.Numerics.Plane.Normal" /> and <see cref="System.Numerics.Plane.D" /> fields are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Plane<T> other)
        {
            return Normal == other.Normal && Distance == other.Distance;
        }

        /// <summary>Returns the string representation of this plane object.</summary>
        /// <returns>A string that represents this <see cref="System.Numerics.Plane" /> object.</returns>
        /// <remarks>The string representation of a <see cref="System.Numerics.Plane" /> object use the formatting conventions of the current culture to format the numeric values in the returned string. For example, a <see cref="System.Numerics.Plane" /> object whose string representation is formatted by using the conventions of the en-US culture might appear as <c>{Normal:&lt;1.1, 2.2, 3.3&gt; D:4.4}</c>.</remarks>
        public override readonly string ToString() => $"{{Normal:{Normal} Distance:{Distance}}}";

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
            sb.Append("{Normal:");
            sb.Append(Normal.ToString(format, formatProvider));
            sb.Append(" Distance:");
            sb.Append(Distance.ToString(format, formatProvider));
            sb.Append('}');
            return sb.ToString();
        }
    }
}
