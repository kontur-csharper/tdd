using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class PointTests
    {
        [Test]
        public void CenterWith_Size100x100AndZeroPoint_ShouldBeHalfSide()
        {
            var point = new Point(0, 0);
            var size = new Size(100, 100);
            var expected = new Point(50, 50);

            var center = point.CenterWith(size);

            center.Should().Be(expected);
        }

        [Test]
        public void DistanceBetween_TwoPoints_ShouldReturnCorrectDistance()
        {
            var distance = new Point(0, 0).DistanceBetween(new Point(100, 120));

            distance.Should().BeApproximately(156.2, 1);
        }
    }
}
