// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Numerics.Tests
{
    public class GenericVector2Tests
    {
        // A test for operator + (Vector2, Vector2)
        [Fact]
        public void Vector2AdditionTestHalf() => Vector2AdditionTest<Half>();
        [Fact]
        public void Vector2AdditionTestFloat() => Vector2AdditionTest<float>();
        [Fact]
        public void Vector2AdditionTestDouble() => Vector2AdditionTest<double>();

        private static void Vector2AdditionTest<T>()
            where T : struct, IFloatingPointIeee754<T>
        {
            Vector2<T> a = new Vector2<T>(T.One, T.CreateChecked(2.0));
            Vector2<T> b = new Vector2<T>(T.CreateChecked(3.0), T.CreateChecked(4.0));

            Vector2<T> expected = new Vector2<T>(T.CreateChecked(4.0), T.CreateChecked(6.0));
            Vector2<T> actual;

            actual = a + b;

            Assert.True(GenericMathHelper.Equal(expected, actual), "Vector2.operator + did not return the expected value.");
        }

        [Fact]
        public void Vector2AdditiveIdentityHalfTest() => Vector2AdditiveIdentityTest<Half>();
        [Fact]
        public void Vector2AdditiveIdentitySingleTest() => Vector2AdditiveIdentityTest<float>();
        [Fact]
        public void Vector2AdditiveIdentityDoubleTest() => Vector2AdditiveIdentityTest<double>();

        static void Vector2AdditiveIdentityTest<T>()
            where T : struct, IFloatingPointIeee754<T>
        {
            Vector2<T> v1 = new Vector2<T>(T.CreateChecked(42), T.CreateChecked(142));
            Vector2<T> actual = Add(v1);
            Vector2<T> expected = v1;
            Assert.Equal(actual, expected);

            static T Add<T>(T value)
                where T : IAdditiveIdentity<T, T>, IAdditionOperators<T, T, T>
            {
                return T.AdditiveIdentity + value;
            }
        }

        [Fact]
        public void Vector2MultiplicativeIdentityHalfTest() => Vector2MultiplicativeIdentityTest<Half>();
        [Fact]
        public void Vector2MultiplicativeIdentitySingleTest() => Vector2MultiplicativeIdentityTest<float>();
        [Fact]
        public void Vector2MultiplicativeIdentityDoubleTest() => Vector2MultiplicativeIdentityTest<double>();

        static void Vector2MultiplicativeIdentityTest<T>()
            where T : struct, IFloatingPointIeee754<T>
        {
            Vector2<T> v1 = new Vector2<T>(T.CreateChecked(42), T.CreateChecked(142));
            Vector2<T> actual = Multiply(v1);
            Vector2<T> expected = v1;
            Assert.Equal(actual, expected);

            static T Multiply<T>(T value)
                where T : IMultiplicativeIdentity<T, T>, IMultiplyOperators<T, T, T>
            {
                return T.MultiplicativeIdentity * value;
            }
        }
    }
}
