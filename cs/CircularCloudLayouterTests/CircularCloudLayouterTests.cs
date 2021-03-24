using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

namespace CircularCloudLayouterTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter circularCloudLayouter;
        private Painter painter;
        private int pictureWidth = 700;
        private int pictureHeight = 500;
        private readonly int rectanglesCount = 70;

        [SetUp]
        public void SetUp()
        {
            circularCloudLayouter = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2));
            painter = new Painter(pictureWidth, pictureHeight);
            for (int i = 0; i < rectanglesCount; i++)
            {
                var rectangle = circularCloudLayouter.PutNextRectangle(MakeRandomSize());
                painter.PaintRectangle(rectangle);
            }
        }

        private Size MakeRandomSize()
        {
            var rand = new Random(Environment.TickCount);
            return new Size(rand.Next(10, 50), rand.Next(10, 30));
        }

        [Test]
        public void PutNextRectangle_PlaceFirstRectangleOnCenter()
        {
            circularCloudLayouter.Rectangles[0].X.Should().Be(pictureWidth / 2);
            circularCloudLayouter.Rectangles[0].Y.Should().Be(pictureHeight / 2);
        }

        [Test]
        public void PutNextRectangle_PlaceFirstRectangleOnCenter_WhenOnlyOneRectangle()
        {
            var rectangle = new CircularCloudLayouter(new Point(pictureWidth / 2, pictureHeight / 2))
                .PutNextRectangle(MakeRandomSize());
            rectangle.X.Should().Be(pictureWidth / 2);
            rectangle.Y.Should().Be(pictureHeight / 2);
        }

        [Test]
        public void PutNextRectangle_ThrowsException_RectangleSizeLessOrEqualZero()
        {
            Assert.Throws<ArgumentException>(() => circularCloudLayouter.PutNextRectangle(new Size(0, 0)));
        }

        [Test]
        public void PutNextRectangle_NotCross_PreviousRectangle()
        {
            for (var i = 1; i < rectanglesCount; i++)
            {
                circularCloudLayouter.Rectangles[i - 1].IntersectsWith(circularCloudLayouter.Rectangles[i]).Should().BeFalse();
            }
        }

        [Test]
        public void PutNextRectangle_NotCross_AllPlacedRectangles()
        {
            for (int i = 0; i < rectanglesCount; i++)
            {
                for (int j = i + 1; j < rectanglesCount; j++)
                {
                    circularCloudLayouter.Rectangles[i].IntersectsWith(circularCloudLayouter.Rectangles[j]).Should().BeFalse();
                }
            }
        }

        [Test]
        public void PutNextRectangle_PlaceRectanglesTightly()
        {
            var squareOfRectangles = 0.0;
            var radius = 0.0;
            for (int i = 0; i < rectanglesCount; i++)
            {
                radius = Math.Max(GetRadiusOfFramingCircle(circularCloudLayouter.Rectangles[0].Location, circularCloudLayouter.Rectangles[i]), radius);
                squareOfRectangles += circularCloudLayouter.Rectangles[i].Height * circularCloudLayouter.Rectangles[i].Width;
            }

            painter.PaintCircle(circularCloudLayouter.Rectangles[0].Location, radius);
            var squareOfFramingCircle = Math.PI * radius * radius;
            (squareOfRectangles / squareOfFramingCircle).Should().BeInRange(0.6, 1);
        }

        [Test]
        public void PutNextRectangle_PlaceRectangleOnSpiral()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0));
            var coordinates = new[]
            {
                new Point(0, 0), new Point(0, 1),  new Point(-1, 1), new Point(-1,0),
                new Point(-2,0), new Point(-2,-1), new Point(-1,-2), new Point(-1,-3),
                new Point(0,-3), new Point(1,-3), new Point(2,-3), new Point(3, -2),
            };
            painter = new Painter(pictureWidth, pictureHeight);
            foreach (var pointOnSpiral in coordinates)
            {
                var rectangle = circularCloudLayouter.PutNextRectangle(new Size(1, 1));
                rectangle.Location.GetDistanceTo(pointOnSpiral).Should().BeLessOrEqualTo(1.05 * rectangle.GetDiagonal());
                painter.PaintRectangle(rectangle);
            }
        }

        private double GetRadiusOfFramingCircle(Point center, Rectangle rectangle)
        {
            var distanceBetweenCenterAndLeftTopAngle = center.GetDistanceTo(rectangle.Location);
            var distanceBetweenCenterAndLeftBottomAngle = center.GetDistanceTo(new Point(rectangle.Left,
                rectangle.Bottom));
            var distanceBetweenCenterAndRightBottomAngle = center.GetDistanceTo(new Point(rectangle.Right,
                rectangle.Bottom));
            var distanceBetweenCenterAndRightTopAngle =
                center.GetDistanceTo(new Point(rectangle.Right,
                    rectangle.Top));
            return Math.Max(
                Math.Max(distanceBetweenCenterAndLeftTopAngle, distanceBetweenCenterAndLeftBottomAngle),
                Math.Max(distanceBetweenCenterAndRightBottomAngle, distanceBetweenCenterAndRightTopAngle));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                painter.SavePicture($"{TestContext.CurrentContext.Test.Name}.jpeg");
                Console.WriteLine($"Tag cloud visualization saved to file {Path.GetFullPath($"{TestContext.CurrentContext.Test.Name}.jpeg")}");
            }
        }
    }
}