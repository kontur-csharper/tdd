using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
    class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _layouter;
        private Point _canvasCenter;

        [SetUp]
        public void SetUp()
        {
            _canvasCenter = new Point(200, 200);
            _layouter = new CircularCloudLayouter(_canvasCenter);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var visualizer = new TagCloudVisualizer(_layouter.GetRectangles());

                var filePath = visualizer.CreateBitmapFromRectangles(_layouter.GetSize());
                Console.WriteLine($@"Tag cloud visualization saved to file <{filePath}>");
            }
        }

        [Test]
        public void PutNextRectangle_ShouldPutCorrect_WhenPutFirstRectangleWithEvenSize()
        {
            var expectedRectangle = new Rectangle(new Point(195, 195), new Size(10, 10));

            _layouter.PutNextRectangle(new Size(10, 10)).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldPutCorrect_WhenPutFirstRectangleWithOddSize()
        {
            var expectedRectangle = new Rectangle(new Point(196, 196), new Size(9, 9));

            _layouter.PutNextRectangle(new Size(9, 9)).Should().BeEquivalentTo(expectedRectangle);
        }

        [Test]
        public void PutNextRectangle_ShouldNotIntersect_WhenPutRectangles()
        {
            var random = new Random();
            
            for (var i = 0; i < 50; i++)
                _layouter.PutNextRectangle(new Size(random.Next(10, 50), random.Next(10, 50)));

            ContainsAnyIntersections().Should().BeFalse();

            bool ContainsAnyIntersections()
            {
                var rectangles = _layouter.GetRectangles();

                for (var i = 0; i < rectangles.Count; i++)
                {
                    if (rectangles[i].IntersectsWith(rectangles.Take(i).Skip(1)))
                        return true;
                }

                return false;
            }
        }

        [Test]
        public void PutNextRectangle_AllRectanglesShouldLieInsideCircle_WhenPutRectangles()
        {
            const int radius = 106;

            for(var i = 0; i < 20; i++)
                _layouter.PutNextRectangle(new Size(40, 40));

            _layouter.GetRectangles().Any(x => GetDistance(x.GetMiddlePoint(), _canvasCenter) >= radius).Should().BeFalse();

            static double GetDistance(Point p1, Point p2)
            {
                return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            }
        }

        [Test]
        public void PutNextRectangle_AllRectanglesShouldMoveToCanvasCenter_WhenPutRectangles()
        {
            for (var i = 0; i < 20; i++)
            {
                _layouter.PutNextRectangle(new Size(40,40));
            }

            var _rectangles = _layouter.GetRectangles();

            _rectangles.Any(x => !CanMove(x, _rectangles, _canvasCenter, 1)).Should().BeTrue();

            static bool CanMove(Rectangle rectangle, IEnumerable<Rectangle> rectangles, Point toPoint, int axisPoint)
            {
                return !rectangle.MoveOneStepTowardsPoint(toPoint, axisPoint).IntersectsWith(rectangles);
            }
        }

        [TestCase(-1, 5)]
        [TestCase(5, -1)]
        [TestCase(5, 0)]
        [TestCase(0, 5)]
        public void PutNextRectangle_ShouldThrowException_WhenSizeIncorrect(int width, int height)
        {
            var rectangleSize = new Size(width, height);
            Action act = () => _layouter.PutNextRectangle(rectangleSize);

            act.Should().Throw<ArgumentException>().WithMessage("Width and height of the rectangle must be positive");
        }

        [Test]
        public void GetSize_ShouldCorrect_WhenPutRectangles()
        {
            var expectedSize = new Size(405, 400);

            for (var i = 0; i < 5; i++)
                _layouter.PutNextRectangle(new Size(10, 10));

            _layouter.GetSize().Should().BeEquivalentTo(expectedSize);
        }
    }
}
