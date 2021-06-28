using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Core;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class ArchimedeanSprial_Should
    {
        private const double theta = 0.5;
        
        [TestCase(0, 0, TestName = "EmptyPoint")]
        [TestCase(1, 2, TestName = "PointWithPositiveCoords")]
        [TestCase(-1, -2, TestName = "PointWithNegativeCoords")]
        [TestCase(1, -2, TestName = "PointWithMixedCoords")]
        public void ArchimedeanSpiralCtor_OnAllPoints_DoesNotThrow(int x, int y)
        {
            Action callCtor = () => _ = new ArchimedeanSpiral(new Point(x, y), theta);
            callCtor.Should().NotThrow();
        }

        [Test]
        public void GetNextPoint_OnEqualSpiralInstance_ShouldBeEqual()
        {
            var firstSpiral = new ArchimedeanSpiral(Point.Empty, theta);
            var secondSpiral = new ArchimedeanSpiral(Point.Empty, theta);

            var firstPoints = Enumerable.Range(0, 50).Select(i => firstSpiral.GetNextPoint());
            var secondPoints = Enumerable.Range(0, 50).Select(i => secondSpiral.GetNextPoint());
            firstPoints.Should().BeEquivalentTo(secondPoints);
        }

        [Test]
        public void GetNextPoint_OnDifferentSpiralInstance_ShouldBeDifferent()
        {
            var firstSpiral = new ArchimedeanSpiral(Point.Empty, theta);
            var secondSpiral = new ArchimedeanSpiral(new Point(1000, 1000), theta);

            var firstPoints = Enumerable.Range(0, 50).Select(i => firstSpiral.GetNextPoint());
            var secondPoints = Enumerable.Range(0, 50).Select(i => secondSpiral.GetNextPoint());
            firstPoints.Should().NotBeEquivalentTo(secondPoints);
        }
    }
}