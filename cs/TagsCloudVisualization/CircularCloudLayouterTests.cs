using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouterTests
    {
        private ICloudLayouter layouter;
        private List<Rectangle> rectangles;
        private Point center = new Point(500, 500);

        [SetUp]
        public void SetLayouter()
        {
            layouter = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void PrintTestResult()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure || 
                TestContext.CurrentContext.Result.Outcome == ResultState.Error)
            {
                var outputFile = Path.GetFullPath(Path.Combine(
                    Directory.GetCurrentDirectory(), "..", "..", "..", "failures", 
                    TestContext.CurrentContext.Test.Name + ".png"));
                var imageSize = new Size(1000, 1000);
                CloudVisualizer.VisualizeCloud(rectangles, outputFile, imageSize);
            }
        }

        [Test]
        public void PutNextRectangle_OnCenter_IfRectangleIsFirst()
        {
            SetRandomRectangles(1);
            var rectangle = rectangles[0];
            rectangle.Location.Should().BeEquivalentTo(new Point(
                center.X - rectangle.Width / 2, 
                center.Y - rectangle.Height / 2));
        }

        [Test]
        public void PutNextRectangle_NoIntersects_AfterPutting()
        {
            foreach (var rectangle in rectangles.ToArray())
            {
                rectangles.Remove(rectangle);
                rectangle.IntersectsWith(rectangles).Should().BeFalse();
                rectangles.Add(rectangle);
            }
        }

        [Test]
        public void PutNextRectangle_AsCloseAsPossible()
        {
            SetRandomRectangles(30);
            for (var i = 1; i < rectangles.Count; i++)
            {
                foreach (var direction in DirectionUtils.GetAllDirections())
                {
                    CheckDensity(rectangles[i], direction);
                }
            }
        }

        [Test]
        public void PutNextRectangle_ShouldFormCircularCloud()
        {
            SetRandomRectangles(20);
            var cloudHorizontalDiameter = GetCloudHorizontalDiameter();
            var cloudVerticalDiameter = GetCloudVerticalDiameter();
            var square = rectangles.Select(rectangle => rectangle.GetArea()).Sum();
            var diameter = Math.Sqrt(square / Math.PI) * 2;
            (Math.Abs(cloudHorizontalDiameter - diameter) <= GetMaxRectangleWidthInCloud()).Should().BeTrue();
            (Math.Abs(cloudVerticalDiameter - diameter) <= GetMaxRectangleHeightInCloud()).Should().BeTrue();
        }

        [Test]
        public void TestOnlyForDemonstrationThirdTask()
        {
            SetRandomRectangles(15);
            true.Should().BeFalse();
        }

        private int GetCloudHorizontalDiameter()
        {
            var top = rectangles.Min(rectangle => rectangle.Top);
            var bottom = rectangles.Max(rectangle => rectangle.Bottom);
            return bottom - top;
        }

        private int GetCloudVerticalDiameter()
        {
            var left = rectangles.Min(rectangle => rectangle.Left);
            var right = rectangles.Max(rectangle => rectangle.Right);
            return right - left;
        }

        private int GetMaxRectangleHeightInCloud()
        {
            return rectangles.Max(rectangle => rectangle.Height);
        }

        private int GetMaxRectangleWidthInCloud()
        {
            return rectangles.Max(rectangle => rectangle.Width);
        }

        private void CheckDensity(Rectangle rectangle, DirectionToMove direction)
        {
            var shift = 1;
            var tempRectangle = rectangle.GetMovedCopy(direction, shift);
            if (tempRectangle.GetDistanceToPoint(center) < rectangle.GetDistanceToPoint(center))
            {
                tempRectangle.IntersectsWith(rectangles.Except(new[] {rectangle})).Should().BeTrue();
            }
        }

        private void SetRandomRectangles(int rectanglesCount)
        {
            var rnd = new Random();
            for (var i = 0; i < rectanglesCount; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(rnd.Next(100, 200), rnd.Next(100, 200))));
            }
        }
    }
}
