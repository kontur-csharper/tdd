using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;
        private Point center;
        private double expectedDensity = 0.4;
        
        [SetUp]
        public void CreateCloud()
        {
            center = new Point(240, 240);
            cloud = new CircularCloudLayouter(center);
        }

        [TearDown]
        public void SaveWrongResult()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var path = cloud.Save();
            Console.WriteLine(string.IsNullOrEmpty(path)
                ? "Failed to save"
                : $"Tag cloud visualization saved to file {path}");
        }
        
        [Test]
        public void PutNextRectangle_OnCenter_BeforeAnyRectangles()
        {
            var size = new Size(400, 200);
            var expectedRectangle = new Rectangle(center - new Size(200, 100), size);

            cloud.PutNextRectangle(size).Should().BeEquivalentTo(expectedRectangle);
        }

        [Timeout(3000)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(25)]
        [TestCase(60)]
        public void PutNextRectangle_WithoutIntersection_WithSameRectangles(int count)
        {
            var size = new Size(40, 20);
            var actual = new List<Rectangle>();

            for (var i = 0; i < count; i++)
            {
                actual.Add(cloud.PutNextRectangle(size));
            }

            actual.SelectMany(a => actual.Where(other => other != a).Select(a.IntersectsWith))
                .Should().AllBeEquivalentTo(false);
        }

        [Timeout(3000)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(25)]
        [TestCase(60)]
        [TestCase(80)]
        public void PutNextRectangle_WithoutIntersection_WithRandomRectangles(int count)
        {
            var random = new Random();
            var actual = new List<Rectangle>();

            for (var i = 0; i < count; i++)
            {
                var size = new Size(random.Next(10, 100), random.Next(10, 100));
                actual.Add(cloud.PutNextRectangle(size));
            }

            actual.SelectMany(a => actual.Where(other => other != a).Select(a.IntersectsWith))
                .Should().AllBeEquivalentTo(false);
        }

        [Timeout(3000)]
        [TestCase(25)]
        [TestCase(60)]
        [TestCase(100)]
        public void BeCloseToCircleInCenter_WithSameRectangles(int count)
        {
            var size = new Size(40, 20);
            var square = 0;
            var r = 0.0;

            for (var i = 0; i < count; i++)
            {
                var rectangle = cloud.PutNextRectangle(size);
                r = Math.Max(r, GetMaxDistanceToCenter(rectangle));
                square += GetRectangleSquare(rectangle);
            }

            var circleSquare = Math.PI * r * r;
            var density = square / circleSquare;
            density.Should().BeGreaterThan(expectedDensity);
        }
        
        [Timeout(3000)]
        [TestCase(25)]
        [TestCase(60)]
        [TestCase(100)]
        public void BeCloseToCircleInCenter_WithRandomRectangles(int count)
        {
            var random = new Random();
            var square = 0;
            var radius = 0.0;

            for (var i = 0; i < count; i++)
            {
                var size = new Size(random.Next(10, 100), random.Next(10, 100));
                var rectangle = cloud.PutNextRectangle(size);
                radius = Math.Max(radius, GetMaxDistanceToCenter(rectangle));
                square += GetRectangleSquare(rectangle);
            }

            var circleSquare = Math.PI * radius * radius;
            var density = square / circleSquare;
            density.Should().BeGreaterThan(expectedDensity);
        }
        
        private static int GetRectangleSquare(Rectangle rectangle) => rectangle.Size.Width * rectangle.Size.Height;
        
        private double GetDistanceToCenter(int x, int y) => Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));
        private double GetDistanceToCenter(Point point) => GetDistanceToCenter(point.X, point.Y);

        private double GetMaxDistanceToCenter(Rectangle rectangle)
        {
            var distances = new[]
            {
                GetDistanceToCenter(rectangle.Location),
                GetDistanceToCenter(rectangle.Location.X + rectangle.Width, rectangle.Location.Y),
                GetDistanceToCenter(rectangle.Location + rectangle.Size),
                GetDistanceToCenter(rectangle.Location.X, rectangle.Location.Y + rectangle.Width),
            };

            return distances.Max();
        }
    }
}