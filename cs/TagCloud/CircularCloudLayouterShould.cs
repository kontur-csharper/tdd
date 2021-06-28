using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagCloud
{
    [TestFixture]
    public class CircularCloudLayouterShould
    {
        private Point center;
        private CircularCloudLayouter layouter;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(350, 400);
            layouter = new CircularCloudLayouter(center, 1);
            rectangles = new List<Rectangle>();
        }
        
        [Test]
        public void PutNextRectangle_ReturnRectangleWithCorrectSize()
        {
            var size = new Size(10, 10);
            var rectangle = layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);
            rectangle.Size.Should().Be(size);
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_ReturnNotIntersectRectangles()
        {
            var size = new Size(2, 1);
            for (var i = 0; i < 10; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangles_LayoutRectanglesInCircle()
        {
            var random = new Random();
            var width = random.Next(300);
            var height = random.Next(200);
            var count = random.Next(100);
            var size = new Size(width, height);
            for (var i = 0; i < count; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            foreach (var rectangle in rectangles)
            {
                GetDistance(rectangle.Location, center).Should()
                    .BeLessThan(Math.Sqrt(width * height * count));
            }
        }
        
        private double GetDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                var vizualizator = new Vizualizator();
                var path = vizualizator.Draw(rectangles);
                Console.WriteLine(TestContext.CurrentContext.Test.Name + " failed");
                Console.WriteLine("Tag cloud visualization saved to file " + path);
                
            }
        }
    }
}