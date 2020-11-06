using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using TagsCloudVisualization;
using TagsCloudVisualization.Infrastructure.Layout;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SpiralPlacingTests
    {
        private SpiralPlacing sut;
        private Point anchor;
        [SetUp]
        [Timeout(1000)]
        public void Setup()
        {
            anchor = new Point();
            sut = new SpiralPlacing(anchor, 1);
        }
        
        [TestCase(-100, 100, -100, 100, TestName = "Whole Square is taken")]
        [TestCase(0, 100, -100, 0, TestName = "Square in I quarter is taken")]
        [TestCase(-100, 0, -100, 0, TestName = "Square in II quarter is taken")]
        [TestCase(-100, 0, 0, 100, TestName = "Square in III quarter is taken")]
        [TestCase(0, 100, 0, 100, TestName = "Square in IV quarter is taken")]
        public void GetPlace_ReturnsValidPlace(int left, int right, int bottom, int top)
        {
            var takenPlaces = new HashSet<Point>();
            for (var x = left; x <= right; x++)
            for (var y = bottom; y <= top; y++)
                takenPlaces.Add(new Point(x, y));
            bool IsValid(Point p) => !takenPlaces.Contains(p);

            var obtainedPlace = sut.GetPoint(IsValid);

            var isInBounds = obtainedPlace.X >= left
                             && obtainedPlace.X <= right
                             && obtainedPlace.Y >= bottom
                             && obtainedPlace.Y <= top;
            Assert.False(isInBounds);
        }
    }
}