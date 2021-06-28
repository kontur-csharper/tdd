using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using HomeExerciseTDD;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class SpiralTests
    {
        [Test]
        public void GetNextPoint_ReturnCenterPoint_WhenFirstCall()
        {
            var center = new Point(22, 22);
            Spiral spiral = new Spiral(center);

            var nextPoint = spiral.GetNextPoint();

            nextPoint.Should().BeEquivalentTo(center);
        }
        
        [Test]
        public void GetNextPoint_ReturnOtherPoint_WhenNotFirstCall()
        {
            var center = new Point(22, 22);
            Spiral spiral = new Spiral(center);

            var nextPoint = spiral.GetNextPoint();

            nextPoint.Should().NotBeEquivalentTo(spiral.GetNextPoint());
        }
        
        [Test]
        public void GetNextPoint_ReturnPointСorrespondsSpiral_WhenManyCalls()
        {
            var spiralPoints = new List<Point>();
            var center = new Point(22, 22);
            Spiral spiral = new Spiral(center);
            var expectedSpiralPoints = new List<Point>
            {
                new Point(22, 22),
                new Point(21, 22),
                new Point(21, 21),
                new Point(22, 21)
            };

            spiralPoints.Add(spiral.GetNextPoint());
            spiralPoints.Add(spiral.GetNextPoint());
            spiralPoints.Add(spiral.GetNextPoint());
            spiralPoints.Add(spiral.GetNextPoint());

            spiralPoints.Should().BeEquivalentTo(expectedSpiralPoints);
        }
    }
}