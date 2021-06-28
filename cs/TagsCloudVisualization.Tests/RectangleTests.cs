using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class RectangleTests
    {
        [Test]
        public void Center_Rectangle100x100onZeroPoint_ShouldBeHalfSide()
        {
            var rectangle = new Rectangle(0, 0, 100, 100);
            var expected = new Point(50, 50);

            var center = rectangle.Center();

            center.Should().Be(expected);
        }

        [Test]
        public void MaxDistanceFromCorner_Rectangle20x20on3and1FromZero_ShouldBeLargestDistance()
        {
            var anchor = new Point(0, 0);
            var rectangle = new Rectangle(3, 1, 20, 20);
            var expected = anchor.DistanceBetween(new Point(23, 21));

            var distance = rectangle.MaxDistanceFromCorner(anchor);

            distance.Should().BeApproximately(expected, 0.01);
        }

        [Test]
        public void GetCorners_Rectangle_ShouldBeReturnFourPointOnRectangle()
        {
            var rectangle = new Rectangle(3, 1, 20, 50);
            var expected = new Point[]
            {
                new Point(23, 1),
                new Point(3, 1),
                new Point(23, 51),
                new Point(3, 51)
            };

            var points = rectangle.GetCorners().ToArray();

            points.Should().BeEquivalentTo(expected);
        }
    }
}
