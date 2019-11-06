﻿using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class DistanceUtils_Should
    {
        [Test]
        public void GetDistanceFromPointToPoint_ShouldReturnDistance_WhenPointsOnOneLine()
        {
            var firstPoint = new Point(1, 2);
            var secondPoint = new Point(3, 2);

            var distance = DistanceUtils.GetDistanceFromPointToPoint(firstPoint, secondPoint);

            distance.Should().BeApproximately(2, 1e-5);
        }

        [Test]
        public void GetDistanceFromPointToPoint_ShouldReturnIrrationalDistance_WhenPointsOnDiagonal()
        {
            var firstPoint = new Point(1, 1);
            var secondPoint = new Point(2, 2);

            var distance = DistanceUtils.GetDistanceFromPointToPoint(firstPoint, secondPoint);

            distance.Should().BeApproximately(Math.Sqrt(2), 1e-5);
        }

        [TestCase(3, 4, 5)]
        [TestCase(5, 12, 13)]
        [TestCase(8, 15, 17)]
        public void GetDistanceFromPointToPoint_ShouldReturnDistance_WhenPointsOnDiagonal(
            int x, int y, double expectedResult)
        {
            var firstPoint = new Point(0, 0);
            var secondPoint = new Point(x, y);

            var distance = DistanceUtils.GetDistanceFromPointToPoint(firstPoint, secondPoint);

            distance.Should().BeApproximately(expectedResult, 1e-5);
        }

        [Test]
        public void GetDistanceFromSegmentToPoint_ShouldReturnDistanceFromTheCenter_WhenRightUnderThePoint()
        {
            var point = new Point(0, 0);
            var segment = new Segment(new Point(-1, 1), new Point(1, 1));

            var distance = DistanceUtils.GetDistanceFromSegmentToPoint(point, segment);

            distance.Should().BeApproximately(1, 1e-5);
        }

        [Test]
        public void GetDistanceFromSegmentToPoint_ShouldReturnDistanceToTheEnd_WhenPlacedOnDiagonal()
        {
            var point = new Point(0, 0);
            var segment = new Segment(new Point(1, 2), new Point(3, 2));

            var distance = DistanceUtils.GetDistanceFromSegmentToPoint(point, segment);

            distance.Should().BeApproximately(Math.Sqrt(5), 1e-5);
        }

        [Test]
        public void GetDistanceFromRectangleToPoint_ShouldReturnTheDistanceFromTheCenter_WhenRightUnderThePoint()
        {
            var point = new Point(0, 0);
            var rectangle = new Rectangle(new Point(-1, 1), new Size(2, 1));

            var distance = DistanceUtils.GetDistanceFromRectangleToPoint(point, rectangle);

            distance.Should().BeApproximately(1, 1e-5);
        }

        [Test]
        public void GetDistanceFromRectangleToPoint_ShouldReturnTheDistanceFromTheCorner_WhenOnTheDiagonal()
        {
            var point = new Point(0, 0);
            var rectangle = new Rectangle(new Point(3, 3), new Size(5, 3));

            var distance = DistanceUtils.GetDistanceFromRectangleToPoint(point, rectangle);

            distance.Should().BeApproximately(Math.Sqrt(18), 1e-5);
        }

        [Test]
        public void GetRectangleSides_ShouldReturnCorrectSegments_OnRectangle()
        {
            var rectangle = new Rectangle(new Point(0, 0), new Size(4, 3));
            var sides = new[]
            {
                new Segment(new Point(0, 0), new Point(4, 0)),
                new Segment(new Point(0, 0), new Point(0, 3)),
                new Segment(new Point(4, 0), new Point(4, 3)),
                new Segment(new Point(0, 3), new Point(4, 3)),
            };

            var result = GeometryUtils.GetRectangleSides(rectangle);

            result.Should().BeEquivalentTo(sides);
        }

        [Test]
        public void GetClosestToThePointRectangle_ShouldReturnClosestRectangle_WhenThreeRectangles()
        {
            var point = new Point(0, 0);
            var firstRectangle = new Rectangle(new Point(2, 1), new Size(3, 2));
            var secondRectangle = new Rectangle(new Point(-5, 0), new Size(4, 2));
            var thirdRectangle = new Rectangle(new Point(-1, 3), new Size(2, 3));

            var result = DistanceUtils.GetClosestToThePointRectangle(
                point, new[] { firstRectangle, secondRectangle, thirdRectangle });

            result.Should().Be(secondRectangle);
        }
    }
}
