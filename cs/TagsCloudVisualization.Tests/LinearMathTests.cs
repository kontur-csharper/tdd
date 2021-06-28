using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class LinearMathTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GetDiagonal_Size10x10_ShouldBeReturnCorrectDiagonal()
        {
            var size = new Size(10, 10);

            var diagonal = LinearMath.GetDiagonal(size);

            diagonal.Should().BeApproximately(14.14, 1);
        }

        [Test]
        public void PolarToCartesian_PolarCoordinatesCenterIsZero_ShouldBeCorrectConvert()
        {
            var center = new Point(0, 0);
            var angle = Math.PI / 4;
            var radius = 100;

            var cartesianPoint = LinearMath.PolarToCartesian(center, radius, angle);

            cartesianPoint.Should().Be(new Point(70, 70));
        }

        [Test]
        public void LinearInterpolate_HalfOfLine_ShouldBeReturnOneHalf()
        {
            var value1 = 10;
            var value2 = 30;
            var required = 20;
            var expected = 0.5d;

            var actual = LinearMath.LinearInterpolate(value1, value2, required);

            actual.Should().BeApproximately(expected, 0.01);
        }
        [Test]
        public void LinearInterpolate_25of100_ShouldBeReturnOneQuarter()
        {
            var value1 = 10;
            var value2 = 110;
            var required = 35;
            var expected = 0.25d;

            var actual = LinearMath.LinearInterpolate(value1, value2, required);

            actual.Should().BeApproximately(expected, 0.01);
        }
    }
}
