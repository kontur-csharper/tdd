using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Models;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    class ArchimedeanSpiralTests
    {
        private ArchimedeanSpiral spiral;
        private Point start;

        [SetUp]
        public void SetUp()
        {
            start = new Point(0, 0);
            spiral = new ArchimedeanSpiral(start);
        }

        [Test]
        public void GetNextPoint_ShouldFirstlyReturnStartPoint()
        {
            new PointFComparer()
                .Equals(spiral.GetNextPoint(), start).Should().BeTrue();
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(50)]
        public void GetNextPoint_ShouldReturnDifferentPoints(int pointsCount)
        {
            var nonRepeatingPoints = new HashSet<PointF>(new PointFComparer());
            for (int i = 0; i < pointsCount; i++)
                nonRepeatingPoints.Add(spiral.GetNextPoint());

            nonRepeatingPoints.Count.Should().Be(pointsCount);
        }
    }
}
