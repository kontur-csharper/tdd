using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularPositionerTests
    {
        [SetUp]
        public void SetUp()
        {
            positioner = new CircularPositioner(Point.Empty, 0, Math.PI / 6, Math.PI / 360);
        }

        private CircularPositioner positioner;

        [Test]
        public void GetIterations_CustomCenter_ShouldBeReturnPointsAroundCenter()
        {
            var center = new Point(-200, 300);
            positioner = new CircularPositioner(center, 0, Math.PI / 6, Math.PI / 360);

            var iterations = positioner.GetIteration(point => true, 1);
            var distance = iterations.Select(point => point.DistanceBetween(center)).Min();

            distance.Should().BeApproximately(0, 1);
        }

        [Test]
        public void GetIterations_CustomRadiusThreshold_ShouldBeReturnPointsAroundRadiusThreshold()
        {
            var center = Point.Empty;
            var radiusThreshold = 100;
            positioner = new CircularPositioner(center, radiusThreshold, Math.PI / 6, Math.PI / 360);

            var iterations = positioner.GetIteration(point => true, 1);
            var distance = iterations.Select(point => point.DistanceBetween(center)).Min();

            distance.Should().BeApproximately(radiusThreshold, 1);
        }

        [Test]
        public void GetIteration_SearchAngleStepIsPiDividedBySix_ShouldBeReturn12Iterations()
        {
            positioner = new CircularPositioner(Point.Empty, 0, Math.PI / 6, Math.PI / 360);

            var actual = positioner.GetIteration(point => true, 1).ToArray();

            actual.Length.Should().Be(12);
        }

        [Test]
        public void GetIterations_DefaultPositioner_ShouldNotReturnPositionsWhereRectanglesCannotBePlaced()
        {
            var center = Point.Empty;
            var availableRadius = 300;
            positioner = new CircularPositioner(center, 0, Math.PI / 6, Math.PI / 360);
            var iterations = positioner.GetIteration(point => point.DistanceBetween(center) > availableRadius, 1);
            var minDistance = iterations.Select(point => point.DistanceBetween(center)).Min();

            minDistance.Should().BeGreaterThan(availableRadius);
        }
    }
}
