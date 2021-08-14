// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System.Numerics
{
    [RequiresPreviewFeatures]
    public partial struct Matrix4x4S
    {
        // "Friendly" Operators

        /// <summary>Computes the unary plus of a value.</summary>
        /// <param name="value">The value for which to compute its unary plus.</param>
        /// <returns>The unary plus of <paramref name="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Plus<T>(in Matrix4x4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return value;
        }

        /// <summary>Negates the specified matrix by multiplying all its values by -1.</summary>
        /// <param name="value">The matrix to negate.</param>
        /// <returns>The negated matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Negate<T>(in Matrix4x4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return -value;
        }

        /// <summary>Adds each element in one matrix with its corresponding element in a second matrix.</summary>
        /// <param name="value1">The first matrix.</param>
        /// <param name="value2">The second matrix.</param>
        /// <returns>The matrix that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Add<T>(in Matrix4x4<T> left, in Matrix4x4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left + right;
        }

        /// <summary>Subtracts each element in a second matrix from its corresponding element in a first matrix.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The matrix containing the values that result from subtracting each element in <paramref name="right" /> from its corresponding element in <paramref name="left" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Subtract<T>(in Matrix4x4<T> left, in Matrix4x4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left - right;
        }

        /// <summary>Multiplies two matrices together to compute the product.</summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The product matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Multiply<T>(in Matrix4x4<T> left, in Matrix4x4<T> right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        /// <summary>Multiplies a matrix by a float to compute the product.</summary>
        /// <param name="left">The matrix to scale.</param>
        /// <param name="right">The scaling value to use.</param>
        /// <returns>The scaled matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4<T> Multiply<T>(in Matrix4x4<T> left, T right)
            where T : struct, IFloatingPoint<T>
        {
            return left * right;
        }

        // Static Methods

        /// <summary>Creates a spherical billboard that rotates around a specified object position.</summary>
        /// <param name="objectPosition">The position of the object that the billboard will rotate around.</param>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraUpVector">The up vector of the camera.</param>
        /// <param name="cameraForwardVector">The forward vector of the camera.</param>
        /// <returns>The created billboard.</returns>
        public static Matrix4x4<T> CreateBillboard<T>(in Vector3<T> objectPosition, in Vector3<T> cameraPosition, in Vector3<T> cameraUpVector, in Vector3<T> cameraForwardVector)
            where T : struct, IFloatingPoint<T>
        {
            Vector3<T> zaxis = objectPosition - cameraPosition;
            T norm = zaxis.LengthSquared();

            if (norm < Matrix4x4<T>.BillboardEpsilon)
            {
                zaxis = -cameraForwardVector;
            }
            else
            {
                zaxis = Vector3S.Multiply(zaxis, T.One / T.Sqrt(norm));
            }

            Vector3<T> xaxis = Vector3S.Normalize(Vector3S.Cross(cameraUpVector, zaxis));
            Vector3<T> yaxis = Vector3S.Cross(zaxis, xaxis);

            return new(
                xaxis.X,
                xaxis.Y,
                xaxis.Z,
               T.Zero,

                yaxis.X,
                yaxis.Y,
                yaxis.Z,
               T.Zero,

                zaxis.X,
                zaxis.Y,
                zaxis.Z,
               T.Zero,

                objectPosition.X,
                objectPosition.Y,
                objectPosition.Z,
                T.One);
        }

        /// <summary>Creates a cylindrical billboard that rotates around a specified axis.</summary>
        /// <param name="objectPosition">The position of the object that the billboard will rotate around.</param>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="rotateAxis">The axis to rotate the billboard around.</param>
        /// <param name="cameraForwardVector">The forward vector of the camera.</param>
        /// <param name="objectForwardVector">The forward vector of the object.</param>
        /// <returns>The billboard matrix.</returns>
        public static Matrix4x4<T> CreateConstrainedBillboard<T>(in Vector3<T> objectPosition, in Vector3<T> cameraPosition, in Vector3<T> rotateAxis, in Vector3<T> cameraForwardVector, in Vector3<T> objectForwardVector)
            where T : struct, IFloatingPoint<T>
        {
            // Treat the case when object and camera positions are too close.
            Vector3<T> faceDir = objectPosition - cameraPosition;
            T norm = faceDir.LengthSquared();

            if (norm < Matrix4x4<T>.BillboardEpsilon)
            {
                faceDir = -cameraForwardVector;
            }
            else
            {
                faceDir = Vector3S.Multiply(faceDir, T.One / T.Sqrt(norm));
            }

            Vector3<T> yaxis = rotateAxis;
            Vector3<T> xaxis;
            Vector3<T> zaxis;

            // Treat the case when angle between faceDir and rotateAxis is too close to 0.
            T dot = Vector3S.Dot(rotateAxis, faceDir);

            if (T.Abs(dot) > Matrix4x4<T>.BillboardMinAngle)
            {
                zaxis = objectForwardVector;

                // Make sure passed values are useful for compute.
                dot = Vector3S.Dot(rotateAxis, zaxis);

                if (T.Abs(dot) > Matrix4x4<T>.BillboardMinAngle)
                {
                    zaxis = (T.Abs(rotateAxis.Z) > Matrix4x4<T>.BillboardMinAngle) ? new Vector3<T>(T.One, T.Zero, T.Zero) : new Vector3<T>(T.Zero, T.Zero, -T.One);
                }

                xaxis = Vector3S.Normalize(Vector3S.Cross(rotateAxis, zaxis));
                zaxis = Vector3S.Normalize(Vector3S.Cross(xaxis, rotateAxis));
            }
            else
            {
                xaxis = Vector3S.Normalize(Vector3S.Cross(rotateAxis, faceDir));
                zaxis = Vector3S.Normalize(Vector3S.Cross(xaxis, yaxis));
            }

            return new(
                xaxis.X,
                xaxis.Y,
                xaxis.Z,
                T.Zero,

                yaxis.X,
                yaxis.Y,
                yaxis.Z,
                T.Zero,

                zaxis.X,
                zaxis.Y,
                zaxis.Z,
                T.Zero,

                objectPosition.X,
                objectPosition.Y,
                objectPosition.Z,
                T.One);
        }

        /// <summary>Creates a matrix that rotates around an arbitrary vector.</summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The angle to rotate around <paramref name="axis" />, in radians.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateFromAxisAngle<T>(in Vector3<T> axis, T angle)
            where T : struct, IFloatingPoint<T>
        {
            // a: angle
            // x, y, z: unit vector for axis.
            //
            // Rotation matrix M can compute by using below equation.
            //
            //        T               T
            //  M = uu + (cos a)( I-uu ) + (sin a)S
            //
            // Where:
            //
            //  u = ( x, y, z )
            //
            //      [  0 -z  y ]
            //  S = [  z  0 -x ]
            //      [ -y  x  0 ]
            //
            //      [ 1 0 0 ]
            //  I = [ 0 1 0 ]
            //      [ 0 0 1 ]
            //
            //
            //     [  xx+cosa*(1-xx)   yx-cosa*yx-sina*z zx-cosa*xz+sina*y ]
            // M = [ xy-cosa*yx+sina*z    yy+cosa(1-yy)  yz-cosa*yz-sina*x ]
            //     [ zx-cosa*zx-sina*y zy-cosa*zy+sina*x   zz+cosa*(1-zz)  ]
            //
            T x = axis.X, y = axis.Y, z = axis.Z;
            T sa = T.Sin(angle), ca = T.Cos(angle);
            T xx = x * x, yy = y * y, zz = z * z;
            T xy = x * y, xz = x * z, yz = y * z;

            return new(
                xx + ca * (T.One - xx),
                xy - ca * xy + sa * z,
                xz - ca * xz - sa * y,
                T.Zero,

                xy - ca * xy - sa * z,
                yy + ca * (T.One - yy),
                yz - ca * yz + sa * x,
                T.Zero,

                xz - ca * xz + sa * y,
                yz - ca * yz - sa * x,
                zz + ca * (T.One - zz),
                T.Zero,

                T.Zero,
                T.Zero,
                T.Zero,
                T.One);
        }

        /// <summary>Creates a rotation matrix from the specified Quaternion rotation value.</summary>
        /// <param name="quaternion">The source Quaternion.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateFromQuaternion<T>(in Quaternion<T> quaternion)
            where T : struct, IFloatingPoint<T>
        {
            T xx = quaternion.X * quaternion.X;
            T yy = quaternion.Y * quaternion.Y;
            T zz = quaternion.Z * quaternion.Z;

            T xy = quaternion.X * quaternion.Y;
            T wz = quaternion.Z * quaternion.W;
            T xz = quaternion.Z * quaternion.X;
            T wy = quaternion.Y * quaternion.W;
            T yz = quaternion.Y * quaternion.Z;
            T wx = quaternion.X * quaternion.W;

            return new(
                T.One - T.Create(2.0) * (yy + zz),
                T.Create(2.0) * (xy + wz),
                T.Create(2.0) * (xz - wy),
                T.Zero,

                T.Create(2.0) * (xy - wz),
                T.One - T.Create(2.0) * (zz + xx),
                T.Create(2.0) * (yz + wx),
                T.Zero,

                T.Create(2.0) * (xz + wy),
                T.Create(2.0) * (yz - wx),
                T.One - T.Create(2.0) * (yy + xx),
                T.Zero,

                T.Zero,
                T.Zero,
                T.Zero,
                T.One);
        }

        /// <summary>Creates a rotation matrix from the specified yaw, pitch, and roll.</summary>
        /// <param name="yaw">The angle of rotation, in radians, around the Y axis.</param>
        /// <param name="pitch">The angle of rotation, in radians, around the X axis.</param>
        /// <param name="roll">The angle of rotation, in radians, around the Z axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateFromYawPitchRoll<T>(T yaw, T pitch, T roll)
            where T : struct, IFloatingPoint<T>
        {
            Quaternion<T> q = QuaternionS.CreateFromYawPitchRoll(yaw, pitch, roll);
            return CreateFromQuaternion(q);
        }

        /// <summary>Creates a view matrix.</summary>
        /// <param name="cameraPosition">The position of the camera.</param>
        /// <param name="cameraTarget">The target towards which the camera is pointing.</param>
        /// <param name="cameraUpVector">The direction that is "up" from the camera's point of view.</param>
        /// <returns>The view matrix.</returns>
        public static Matrix4x4<T> CreateLookAt<T>(in Vector3<T> cameraPosition, in Vector3<T> cameraTarget, in Vector3<T> cameraUpVector)
            where T : struct, IFloatingPoint<T>
        {
            Vector3<T> zaxis = Vector3S.Normalize(cameraPosition - cameraTarget);
            Vector3<T> xaxis = Vector3S.Normalize(Vector3S.Cross(cameraUpVector, zaxis));
            Vector3<T> yaxis = Vector3S.Cross(zaxis, xaxis);

            return new(
                xaxis.X,
                yaxis.X,
                zaxis.X,
                T.Zero,

                xaxis.Y,
                yaxis.Y,
                zaxis.Y,
                T.Zero,

                xaxis.Z,
                yaxis.Z,
                zaxis.Z,
                T.Zero,

                -Vector3S.Dot(xaxis, cameraPosition),
                -Vector3S.Dot(yaxis, cameraPosition),
                -Vector3S.Dot(zaxis, cameraPosition),
                T.One);
        }

        /// <summary>Creates an orthographic perspective matrix from the given view volume dimensions.</summary>
        /// <param name="width">The width of the view volume.</param>
        /// <param name="height">The height of the view volume.</param>
        /// <param name="zNearPlane">The minimum Z-value of the view volume.</param>
        /// <param name="zFarPlane">The maximum Z-value of the view volume.</param>
        /// <returns>The orthographic projection matrix.</returns>
        public static Matrix4x4<T> CreateOrthographic<T>(T width, T height, T zNearPlane, T zFarPlane)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M11 = T.Create(2.0) / width,
                M22 = T.Create(2.0) / height,
                M33 = T.One / (zNearPlane - zFarPlane),
                M43 = zNearPlane / (zNearPlane - zFarPlane)
            };
        }

        /// <summary>Creates a customized orthographic projection matrix.</summary>
        /// <param name="left">The minimum X-value of the view volume.</param>
        /// <param name="right">The maximum X-value of the view volume.</param>
        /// <param name="bottom">The minimum Y-value of the view volume.</param>
        /// <param name="top">The maximum Y-value of the view volume.</param>
        /// <param name="zNearPlane">The minimum Z-value of the view volume.</param>
        /// <param name="zFarPlane">The maximum Z-value of the view volume.</param>
        /// <returns>The orthographic projection matrix.</returns>
        public static Matrix4x4<T> CreateOrthographicOffCenter<T>(T left, T right, T bottom, T top, T zNearPlane, T zFarPlane)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M11 = T.Create(2.0) / (right - left),
                M22 = T.Create(2.0) / (top - bottom),
                M33 = T.One / (zNearPlane - zFarPlane),
                M41 = (left + right) / (left - right),
                M42 = (top + bottom) / (bottom - top),
                M43 = zNearPlane / (zNearPlane - zFarPlane)
            };
        }

        /// <summary>Creates a perspective projection matrix from the given view volume dimensions.</summary>
        /// <param name="width">The width of the view volume at the near view plane.</param>
        /// <param name="height">The height of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="nearPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="farPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="nearPlaneDistance" /> is greater than or equal to <paramref name="farPlaneDistance" />.</exception>
        public static Matrix4x4<T> CreatePerspective<T>(T width, T height, T nearPlaneDistance, T farPlaneDistance)
            where T : struct, IFloatingPoint<T>
        {
            if (nearPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            if (farPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            T negFarRange = T.IsInfinity(farPlaneDistance) ? -T.One : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return new(
                T.Create(2.0) * nearPlaneDistance / width,
                T.Zero,
                T.Zero,
                T.Zero,

                T.Zero,
                T.Create(2.0) * nearPlaneDistance / height,
                T.Zero,
                T.Zero,

                T.Zero,
                T.Zero,
                negFarRange,
                -T.One,

                T.Zero,
                T.Zero,
                nearPlaneDistance * negFarRange,
                T.Zero);
        }

        /// <summary>Creates a perspective projection matrix based on a field of view, aspect ratio, and near and far view plane distances.</summary>
        /// <param name="fieldOfView">The field of view in the y direction, in radians.</param>
        /// <param name="aspectRatio">The aspect ratio, defined as view space width divided by height.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="fieldOfView" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="fieldOfView" /> is greater than or equal to <see cref="System.Math.PI" />.
        /// <paramref name="nearPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="farPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="nearPlaneDistance" /> is greater than or equal to <paramref name="farPlaneDistance" />.</exception>
        public static Matrix4x4<T> CreatePerspectiveFieldOfView<T>(T fieldOfView, T aspectRatio, T nearPlaneDistance, T farPlaneDistance)
            where T : struct, IFloatingPoint<T>
        {
            if (fieldOfView <= T.Zero || fieldOfView >= T.Pi)
                throw new ArgumentOutOfRangeException(nameof(fieldOfView));

            if (nearPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            if (farPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            T yScale = T.One / T.Tan(fieldOfView * T.Create(0.5));
            T xScale = yScale / aspectRatio;

            T negFarRange = T.IsInfinity(farPlaneDistance) ? -T.One : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return new(
                xScale,
                T.Zero,
                T.Zero,
                T.Zero,

                T.Zero,
                yScale,
                T.Zero,
                T.Zero,

                T.Zero,
                T.Zero,
                negFarRange,
                -T.One,

                T.Zero,
                T.Zero,
                nearPlaneDistance * negFarRange,
                T.Zero);
        }

        /// <summary>Creates a customized perspective projection matrix.</summary>
        /// <param name="left">The minimum x-value of the view volume at the near view plane.</param>
        /// <param name="right">The maximum x-value of the view volume at the near view plane.</param>
        /// <param name="bottom">The minimum y-value of the view volume at the near view plane.</param>
        /// <param name="top">The maximum y-value of the view volume at the near view plane.</param>
        /// <param name="nearPlaneDistance">The distance to the near view plane.</param>
        /// <param name="farPlaneDistance">The distance to the far view plane.</param>
        /// <returns>The perspective projection matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="nearPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="farPlaneDistance" /> is less than or equal to zero.
        /// -or-
        /// <paramref name="nearPlaneDistance" /> is greater than or equal to <paramref name="farPlaneDistance" />.</exception>
        public static Matrix4x4<T> CreatePerspectiveOffCenter<T>(T left, T right, T bottom, T top, T nearPlaneDistance, T farPlaneDistance)
            where T : struct, IFloatingPoint<T>
        {
            if (nearPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            if (farPlaneDistance <= T.Zero)
                throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));

            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            T negFarRange = T.IsInfinity(farPlaneDistance) ? -T.One : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            return new(
                T.Create(2.0) * nearPlaneDistance / (right - left),
                T.Zero,
                T.Zero,
                T.Zero,

                T.Zero,
                T.Create(2.0) * nearPlaneDistance / (top - bottom),
                T.Zero,
                T.Zero,

                (left + right) / (right - left),
                (top + bottom) / (top - bottom),
                negFarRange,
                -T.One,

                T.Zero,
                T.Zero,
                nearPlaneDistance * negFarRange,
                T.Zero);
        }

        /// <summary>Creates a matrix that reflects the coordinate system about a specified plane.</summary>
        /// <param name="value">The plane about which to create a reflection.</param>
        /// <returns>A new matrix expressing the reflection.</returns>
        public static Matrix4x4<T> CreateReflection<T>(in Plane<T> value)
            where T : struct, IFloatingPoint<T>
        {
            var normalized = PlaneS.Normalize(value);

            T a = normalized.Normal.X;
            T b = normalized.Normal.Y;
            T c = normalized.Normal.Z;

            T fa = T.Create(-2.0) * a;
            T fb = T.Create(-2.0) * b;
            T fc = T.Create(-2.0) * c;

            return new(
                fa * a + T.One,
                fb * a,
                fc * a,
                T.Zero,

                fa * b,
                fb * b + T.One,
                fc * b,
                T.Zero,

                fa * c,
                fb * c,
                fc * c + T.One,
                T.Zero,

                fa * normalized.Distance,
                fb * normalized.Distance,
                fc * normalized.Distance,
                T.One);
        }

        /// <summary>Creates a matrix for rotating points around the X axis.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the X axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationX<T>(T radians)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  0  0  1 ]

            return Matrix4x4<T>.Identity with
            {
                M22 = c,
                M23 = s,
                M32 = -s,
                M33 = c
            };
        }


        /// <summary>Creates a matrix for rotating points around the X axis from a center point.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the X axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationX<T>(T radians, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            T y = centerPoint.Y * (T.One - c) + centerPoint.Z * s;
            T z = centerPoint.Z * (T.One - c) - centerPoint.Y * s;

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  y  z  1 ]

            return Matrix4x4<T>.Identity with
            {
                M22 = c,
                M23 = s,
                M32 = -s,
                M33 = c,
                M42 = y,
                M43 = z
            };
        }

        /// <summary>Creates a matrix for rotating points around the Y axis.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationY<T>(T radians)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  0  0  0  1 ]

            return Matrix4x4<T>.Identity with
            {
                M11 = c,
                M13 = -s,
                M31 = s,
                M33 = c
            };
        }

        /// <summary>The amount, in radians, by which to rotate around the Y axis from a center point.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Y-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationY<T>(T radians, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            T x = centerPoint.X * (T.One - c) - centerPoint.Z * s;
            T z = centerPoint.Z * (T.One - c) + centerPoint.X * s;

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  x  0  z  1 ]

            return Matrix4x4<T>.Identity with
            {
                M11 = c,
                M13 = -s,
                M31 = s,
                M33 = c,
                M41 = x,
                M43 = z
            };
        }

        /// <summary>Creates a matrix for rotating points around the Z axis.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationZ<T>(T radians)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  0  0  0  1 ]

            return Matrix4x4<T>.Identity with
            {
                M11 = c,
                M12 = s,
                M21 = -s,
                M22 = c
            };
        }

        /// <summary>Creates a matrix for rotating points around the Z axis from a center point.</summary>
        /// <param name="radians">The amount, in radians, by which to rotate around the Z-axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4x4<T> CreateRotationZ<T>(T radians, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T c = T.Cos(radians);
            T s = T.Sin(radians);

            T x = centerPoint.X * (T.One - c) + centerPoint.Y * s;
            T y = centerPoint.Y * (T.One - c) - centerPoint.X * s;

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  x  y  0  1 ]

            return Matrix4x4<T>.Identity with
            {
                M11 = c,
                M12 = s,
                M21 = -s,
                M22 = c,
                M41 = x,
                M42 = y
            };
        }

        /// <summary>Creates a uniform scaling matrix that scale equally on each axis.</summary>
        /// <param name="scale">The uniform scaling factor.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(T scale)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M11 = scale,
                M22 = scale,
                M33 = scale
            };
        }

        /// <summary>Creates a uniform scaling matrix that scales equally on each axis with a center point.</summary>
        /// <param name="scale">The uniform scaling factor.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(T scale, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scale);
            T ty = centerPoint.Y * (T.One - scale);
            T tz = centerPoint.Z * (T.One - scale);

            return Matrix4x4<T>.Identity with
            {
                M11 = scale,
                M22 = scale,
                M33 = scale,

                M41 = tx,
                M42 = ty,
                M43 = tz
            };
        }

        /// <summary>Creates a scaling matrix from the specified X, Y, and Z components.</summary>
        /// <param name="scaleX">The value to scale by on the X axis.</param>
        /// <param name="scaleY">The value to scale by on the Y axis.</param>
        /// <param name="scaleZ">The value to scale by on the Z axis.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(T scaleX, T scaleY, T scaleZ)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M11 = scaleX,
                M22 = scaleY,
                M33 = scaleZ
            };
        }

        /// <summary>Creates a scaling matrix that is offset by a given center point.</summary>
        /// <param name="scaleX">The value to scale by on the X axis.</param>
        /// <param name="scaleY">The value to scale by on the Y axis.</param>
        /// <param name="scaleZ">The value to scale by on the Z axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(T scaleX, T scaleY, T scaleZ, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scaleX);
            T ty = centerPoint.Y * (T.One - scaleY);
            T tz = centerPoint.Z * (T.One - scaleZ);

            return Matrix4x4<T>.Identity with
            {
                M11 = scaleX,
                M22 = scaleY,
                M33 = scaleZ,
                M41 = tx,
                M42 = ty,
                M43 = tz
            };
        }

        /// <summary>Creates a scaling matrix from the specified vector scale.</summary>
        /// <param name="scale">The scale to use.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(in Vector3<T> scale)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M11 = scale.X,
                M22 = scale.Y,
                M33 = scale.Z
            };
        }

        /// <summary>Creates a scaling matrix with a center point.</summary>
        /// <param name="scale">The vector that contains the amount to scale on each axis.</param>
        /// <param name="centerPoint">The center point.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix4x4<T> CreateScale<T>(in Vector3<T> scale, in Vector3<T> centerPoint)
            where T : struct, IFloatingPoint<T>
        {
            T tx = centerPoint.X * (T.One - scale.X);
            T ty = centerPoint.Y * (T.One - scale.Y);
            T tz = centerPoint.Z * (T.One - scale.Z);

            return Matrix4x4<T>.Identity with
            {
                M11 = scale.X,
                M22 = scale.Y,
                M33 = scale.Z,
                M41 = tx,
                M42 = ty,
                M43 = tz
            };
        }

        /// <summary>Creates a matrix that flattens geometry into a specified plane as if casting a shadow from a specified light source.</summary>
        /// <param name="lightDirection">The direction from which the light that will cast the shadow is coming.</param>
        /// <param name="plane">The plane onto which the new matrix should flatten geometry so as to cast a shadow.</param>
        /// <returns>A new matrix that can be used to flatten geometry onto the specified plane from the specified direction.</returns>
        public static Matrix4x4<T> CreateShadow<T>(in Vector3<T> lightDirection, in Plane<T> plane)
            where T : struct, IFloatingPoint<T>
        {
            Plane<T> p = PlaneS.Normalize(plane);

            T dot = p.Normal.X * lightDirection.X + p.Normal.Y * lightDirection.Y + p.Normal.Z * lightDirection.Z;
            T a = -p.Normal.X;
            T b = -p.Normal.Y;
            T c = -p.Normal.Z;
            T d = -p.Distance;

            return Matrix4x4<T>.Identity with
            {
                M11 = a * lightDirection.X + dot,
                M21 = b * lightDirection.X,
                M31 = c * lightDirection.X,
                M41 = d * lightDirection.X,

                M12 = a * lightDirection.Y,
                M22 = b * lightDirection.Y + dot,
                M32 = c * lightDirection.Y,
                M42 = d * lightDirection.Y,

                M13 = a * lightDirection.Z,
                M23 = b * lightDirection.Z,
                M33 = c * lightDirection.Z + dot,
                M43 = d * lightDirection.Z,

                M44 = dot
            };
        }

        /// <summary>Creates a translation matrix from the specified X, Y, and Z components.</summary>
        /// <param name="positionX">The amount to translate on the X axis.</param>
        /// <param name="positionY">The amount to translate on the Y axis.</param>
        /// <param name="positionZ">The amount to translate on the Z axis.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix4x4<T> CreateTranslation<T>(T positionX, T positionY, T positionZ)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M41 = positionX,
                M42 = positionY,
                M43 = positionZ
            };
        }

        /// <summary>Creates a translation matrix from the specified 3-dimensional vector.</summary>
        /// <param name="position">The amount to translate in each axis.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix4x4<T> CreateTranslation<T>(in Vector3<T> position)
            where T : struct, IFloatingPoint<T>
        {
            return Matrix4x4<T>.Identity with
            {
                M41 = position.X,
                M42 = position.Y,
                M43 = position.Z
            };
        }

        /// <summary>Creates a world matrix with the specified parameters.</summary>
        /// <param name="position">The position of the object.</param>
        /// <param name="forward">The forward direction of the object.</param>
        /// <param name="up">The upward direction of the object. Its value is usually <c>[0, 1, 0]</c>.</param>
        /// <returns>The world matrix.</returns>
        /// <remarks><paramref name="position" /> is used in translation operations.</remarks>
        public static Matrix4x4<T> CreateWorld<T>(in Vector3<T> position, in Vector3<T> forward, in Vector3<T> up)
            where T : struct, IFloatingPoint<T>
        {
            Vector3<T> zaxis = Vector3S.Normalize(-forward);
            Vector3<T> xaxis = Vector3S.Normalize(Vector3S.Cross(up, zaxis));
            Vector3<T> yaxis = Vector3S.Cross(zaxis, xaxis);

            return Matrix4x4<T>.Identity with
            {
                M11 = xaxis.X,
                M12 = xaxis.Y,
                M13 = xaxis.Z,

                M21 = yaxis.X,
                M22 = yaxis.Y,
                M23 = yaxis.Z,

                M31 = zaxis.X,
                M32 = zaxis.Y,
                M33 = zaxis.Z,

                M41 = position.X,
                M42 = position.Y,
                M43 = position.Z
            };
        }


        /// <summary>Attempts to extract the scale, translation, and rotation components from the given scale, rotation, or translation matrix. The return value indicates whether the operation succeeded.</summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="scale">When this method returns, contains the scaling component of the transformation matrix if the operation succeeded.</param>
        /// <param name="rotation">When this method returns, contains the rotation component of the transformation matrix if the operation succeeded.</param>
        /// <param name="translation">When the method returns, contains the translation component of the transformation matrix if the operation succeeded.</param>
        /// <returns><see langword="true" /> if <paramref name="matrix" /> was decomposed successfully; otherwise,  <see langword="false" />.</returns>
        public static bool Decompose<T>(in Matrix4x4<T> matrix, out Vector3<T> scale, out Quaternion<T> rotation, out Vector3<T> translation)
            where T : struct, IFloatingPoint<T>
        {
            bool result = true;

            Vector3<T>[] matTemp = { Vector3<T>.UnitX, Vector3<T>.UnitY, Vector3<T>.UnitZ };
            Vector3<T>[] canonicalBasis = {
                new Vector3<T>(T.One,T.Zero,T.Zero),
                new Vector3<T>(T.Zero, T.One,T.Zero),
                new Vector3<T>(T.Zero,T.Zero, T.One)
            };

            translation = new Vector3<T>(
                matrix.M41,
                matrix.M42,
                matrix.M43);

            int[] pVectorBasis = { 0, 1, 2 };

            matTemp[pVectorBasis[0]] = new Vector3<T>(matrix.M11, matrix.M12, matrix.M13);
            matTemp[pVectorBasis[1]] = new Vector3<T>(matrix.M21, matrix.M22, matrix.M23);
            matTemp[pVectorBasis[2]] = new Vector3<T>(matrix.M31, matrix.M32, matrix.M33);

            T[] scaleBase = new T[] {
                        matTemp[pVectorBasis[0]].Length(),
                        matTemp[pVectorBasis[1]].Length(),
                        matTemp[pVectorBasis[2]].Length()};

            int a, b, c;
            #region Ranking
            T x = scaleBase[0], y = scaleBase[1], z = scaleBase[2];

            if (x < y)
            {
                if (y < z)
                {
                    a = 2;
                    b = 1;
                    c = 0;
                }
                else
                {
                    a = 1;

                    if (x < z)
                    {
                        b = 2;
                        c = 0;
                    }
                    else
                    {
                        b = 0;
                        c = 2;
                    }
                }
            }
            else
            {
                if (x < z)
                {
                    a = 2;
                    b = 0;
                    c = 1;
                }
                else
                {
                    a = 0;

                    if (y < z)
                    {
                        b = 2;
                        c = 1;
                    }
                    else
                    {
                        b = 1;
                        c = 2;
                    }
                }
            }
            #endregion

            if (scaleBase[a] < Matrix4x4<T>.DecomposeEpsilon)
            {
                matTemp[pVectorBasis[a]] = canonicalBasis[a];
            }

            matTemp[pVectorBasis[a]] = Vector3S.Normalize(matTemp[pVectorBasis[a]]);

            if (scaleBase[b] < Matrix4x4<T>.DecomposeEpsilon)
            {
                int cc;
                T fAbsX, fAbsY, fAbsZ;

                fAbsX = T.Abs(matTemp[pVectorBasis[a]].X);
                fAbsY = T.Abs(matTemp[pVectorBasis[a]].Y);
                fAbsZ = T.Abs(matTemp[pVectorBasis[a]].Z);

                #region Ranking
                if (fAbsX < fAbsY)
                {
                    if (fAbsY < fAbsZ)
                    {
                        cc = 0;
                    }
                    else
                    {
                        if (fAbsX < fAbsZ)
                        {
                            cc = 0;
                        }
                        else
                        {
                            cc = 2;
                        }
                    }
                }
                else
                {
                    if (fAbsX < fAbsZ)
                    {
                        cc = 1;
                    }
                    else
                    {
                        if (fAbsY < fAbsZ)
                        {
                            cc = 1;
                        }
                        else
                        {
                            cc = 2;
                        }
                    }
                }
                #endregion

                matTemp[pVectorBasis[b]] = Vector3S.Cross(matTemp[pVectorBasis[a]], canonicalBasis[cc]);
            }

            matTemp[pVectorBasis[b]] = Vector3S.Normalize(matTemp[pVectorBasis[b]]);

            if (scaleBase[c] < Matrix4x4<T>.DecomposeEpsilon)
            {
                matTemp[pVectorBasis[c]] = Vector3S.Cross(matTemp[pVectorBasis[a]], matTemp[pVectorBasis[b]]);
            }

            matTemp[pVectorBasis[c]] = Vector3S.Normalize(matTemp[pVectorBasis[c]]);

            T det = ToMatrix4x4(matTemp).GetDeterminant();

            // use Kramer's rule to check for handedness of coordinate system
            if (det < T.Zero)
            {
                // switch coordinate system by negating the scale and inverting the basis vector on the x-axis
                scaleBase[a] = -scaleBase[a];
                matTemp[pVectorBasis[a]] = -matTemp[pVectorBasis[a]];

                det = -det;
            }

            det -= T.One;
            det *= det;

            if ((Matrix4x4<T>.DecomposeEpsilon < det))
            {
                // Non-SRT matrix encountered
                rotation = Quaternion<T>.Identity;
                result = false;
            }
            else
            {
                // generate the quaternion from the matrix
                rotation = QuaternionS.CreateFromRotationMatrix(ToMatrix4x4(matTemp));
            }
            scale = new Vector3<T>(scaleBase[0], scaleBase[1], scaleBase[2]);

            return result;


            static Matrix4x4<T> ToMatrix4x4(in Vector3<T>[] matrixT)
            {
                return Matrix4x4<T>.Identity with
                {
                    M11 = matrixT[0].X,
                    M12 = matrixT[0].Y,
                    M13 = matrixT[0].Z,

                    M21 = matrixT[1].X,
                    M22 = matrixT[1].Y,
                    M23 = matrixT[1].Z,

                    M31 = matrixT[2].X,
                    M32 = matrixT[2].Y,
                    M33 = matrixT[2].Z,
                };
            }
        }

        /// <summary>Tries to invert the specified matrix. The return value indicates whether the operation succeeded.</summary>
        /// <param name="matrix">The matrix to invert.</param>
        /// <param name="result">When this method returns, contains the inverted matrix if the operation succeeded.</param>
        /// <returns><see langword="true" /> if <paramref name="matrix" /> was converted successfully; otherwise,  <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invert<T>(in Matrix4x4<T> matrix, out Matrix4x4<T> result)
            where T : struct, IFloatingPoint<T>
        {
            // This implementation is based on the DirectX Math Library XMMatrixInverse method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl

            //                                       -1
            // If you have matrix M, inverse Matrix M   can compute
            //
            //     -1       1
            //    M   = --------- A
            //            det(M)
            //
            // A is adjugate (adjoint) of M, where,
            //
            //      T
            // A = C
            //
            // C is Cofactor matrix of M, where,
            //           i + j
            // C   = (-1)      * det(M  )
            //  ij                    ij
            //
            //     [ a b c d ]
            // M = [ e f g h ]
            //     [ i j k l ]
            //     [ m n o p ]
            //
            // First Row
            //           2 | f g h |
            // C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
            //  11         | n o p |
            //
            //           3 | e g h |
            // C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
            //  12         | m o p |
            //
            //           4 | e f h |
            // C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
            //  13         | m n p |
            //
            //           5 | e f g |
            // C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
            //  14         | m n o |
            //
            // Second Row
            //           3 | b c d |
            // C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
            //  21         | n o p |
            //
            //           4 | a c d |
            // C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
            //  22         | m o p |
            //
            //           5 | a b d |
            // C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
            //  23         | m n p |
            //
            //           6 | a b c |
            // C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
            //  24         | m n o |
            //
            // Third Row
            //           4 | b c d |
            // C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
            //  31         | n o p |
            //
            //           5 | a c d |
            // C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
            //  32         | m o p |
            //
            //           6 | a b d |
            // C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
            //  33         | m n p |
            //
            //           7 | a b c |
            // C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
            //  34         | m n o |
            //
            // Fourth Row
            //           5 | b c d |
            // C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
            //  41         | j k l |
            //
            //           6 | a c d |
            // C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
            //  42         | i k l |
            //
            //           7 | a b d |
            // C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
            //  43         | i j l |
            //
            //           8 | a b c |
            // C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
            //  44         | i j k |
            //
            // Cost of operation
            // 53 adds, 104 muls, and 1 div.
            T a = matrix.M11, b = matrix.M12, c = matrix.M13, d = matrix.M14;
            T e = matrix.M21, f = matrix.M22, g = matrix.M23, h = matrix.M24;
            T i = matrix.M31, j = matrix.M32, k = matrix.M33, l = matrix.M34;
            T m = matrix.M41, n = matrix.M42, o = matrix.M43, p = matrix.M44;

            T kp_lo = k * p - l * o;
            T jp_ln = j * p - l * n;
            T jo_kn = j * o - k * n;
            T ip_lm = i * p - l * m;
            T io_km = i * o - k * m;
            T in_jm = i * n - j * m;

            T a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
            T a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            T a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
            T a14 = -(e * jo_kn - f * io_km + g * in_jm);

            T det = a * a11 + b * a12 + c * a13 + d * a14;

            if (T.Abs(det) < T.Epsilon)
            {
                result = new Matrix4x4<T>(T.NaN, T.NaN, T.NaN, T.NaN,
                                          T.NaN, T.NaN, T.NaN, T.NaN,
                                          T.NaN, T.NaN, T.NaN, T.NaN,
                                          T.NaN, T.NaN, T.NaN, T.NaN);
                return false;
            }

            T invDet = T.One / det;

            T gp_ho = g * p - h * o;
            T fp_hn = f * p - h * n;
            T fo_gn = f * o - g * n;
            T ep_hm = e * p - h * m;
            T eo_gm = e * o - g * m;
            T en_fm = e * n - f * m;

            T gl_hk = g * l - h * k;
            T fl_hj = f * l - h * j;
            T fk_gj = f * k - g * j;
            T el_hi = e * l - h * i;
            T ek_gi = e * k - g * i;
            T ej_fi = e * j - f * i;

            result = new()
            {
                M11 = a11 * invDet,
                M21 = a12 * invDet,
                M31 = a13 * invDet,
                M41 = a14 * invDet,

                M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet,
                M22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet,
                M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet,
                M42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet,


                M13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet,
                M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet,
                M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet,
                M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet,

                M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet,
                M24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet,
                M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet,
                M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet
            };

            return true;
        }

        /// <summary>Performs a linear interpolation from one matrix to a second matrix based on a value that specifies the weighting of the second matrix.</summary>
        /// <param name="min">The first matrix.</param>
        /// <param name="max">The second matrix.</param>
        /// <param name="amount">The relative weighting of <paramref name="max" />.</param>
        /// <returns>The interpolated matrix.</returns>
        public static Matrix4x4<T> Lerp<T>(in Matrix4x4<T> min, in Matrix4x4<T> max, T amount)
            where T : struct, IFloatingPoint<T>
        {
            return new()
            {
                // First row
                M11 = min.M11 + (max.M11 - min.M11) * amount,
                M12 = min.M12 + (max.M12 - min.M12) * amount,
                M13 = min.M13 + (max.M13 - min.M13) * amount,
                M14 = min.M14 + (max.M14 - min.M14) * amount,

                // Second row
                M21 = min.M21 + (max.M21 - min.M21) * amount,
                M22 = min.M22 + (max.M22 - min.M22) * amount,
                M23 = min.M23 + (max.M23 - min.M23) * amount,
                M24 = min.M24 + (max.M24 - min.M24) * amount,

                // Third row
                M31 = min.M31 + (max.M31 - min.M31) * amount,
                M32 = min.M32 + (max.M32 - min.M32) * amount,
                M33 = min.M33 + (max.M33 - min.M33) * amount,
                M34 = min.M34 + (max.M34 - min.M34) * amount,

                // Fourth row
                M41 = min.M41 + (max.M41 - min.M41) * amount,
                M42 = min.M42 + (max.M42 - min.M42) * amount,
                M43 = min.M43 + (max.M43 - min.M43) * amount,
                M44 = min.M44 + (max.M44 - min.M44) * amount
            };
        }

        /// <summary>Transforms the specified matrix by applying the specified Quaternion rotation.</summary>
        /// <param name="value">The matrix to transform.</param>
        /// <param name="rotation">The rotation t apply.</param>
        /// <returns>The transformed matrix.</returns>
        public static Matrix4x4<T> Transform<T>(in Matrix4x4<T> value, in Quaternion<T> rotation)
            where T : struct, IFloatingPoint<T>
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

            T q11 = T.One - yy2 - zz2;
            T q21 = xy2 - wz2;
            T q31 = xz2 + wy2;

            T q12 = xy2 + wz2;
            T q22 = T.One - xx2 - zz2;
            T q32 = yz2 - wx2;

            T q13 = xz2 - wy2;
            T q23 = yz2 + wx2;
            T q33 = T.One - xx2 - yy2;

            return new()
            {
                // First row
                M11 = value.M11 * q11 + value.M12 * q21 + value.M13 * q31,
                M12 = value.M11 * q12 + value.M12 * q22 + value.M13 * q32,
                M13 = value.M11 * q13 + value.M12 * q23 + value.M13 * q33,
                M14 = value.M14,

                // Second row
                M21 = value.M21 * q11 + value.M22 * q21 + value.M23 * q31,
                M22 = value.M21 * q12 + value.M22 * q22 + value.M23 * q32,
                M23 = value.M21 * q13 + value.M22 * q23 + value.M23 * q33,
                M24 = value.M24,

                // Third row
                M31 = value.M31 * q11 + value.M32 * q21 + value.M33 * q31,
                M32 = value.M31 * q12 + value.M32 * q22 + value.M33 * q32,
                M33 = value.M31 * q13 + value.M32 * q23 + value.M33 * q33,
                M34 = value.M34,

                // Fourth row
                M41 = value.M41 * q11 + value.M42 * q21 + value.M43 * q31,
                M42 = value.M41 * q12 + value.M42 * q22 + value.M43 * q32,
                M43 = value.M41 * q13 + value.M42 * q23 + value.M43 * q33,
                M44 = value.M44,
            };
        }

        /// <summary>Transposes the rows and columns of a matrix.</summary>
        /// <param name="value">The matrix to transpose.</param>
        /// <returns>The transposed matrix.</returns>
        public static Matrix4x4<T> Transpose<T>(in Matrix4x4<T> value)
            where T : struct, IFloatingPoint<T>
        {
            return new()
            {
                M11 = value.M11,
                M12 = value.M21,
                M13 = value.M31,
                M14 = value.M41,
                M21 = value.M12,
                M22 = value.M22,
                M23 = value.M32,
                M24 = value.M42,
                M31 = value.M13,
                M32 = value.M23,
                M33 = value.M33,
                M34 = value.M43,
                M41 = value.M14,
                M42 = value.M24,
                M43 = value.M34,
                M44 = value.M44,
            };
        }

    }
}
