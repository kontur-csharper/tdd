using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Tests
    {
        private IRectangleLayouter layouter;
        private Size minSize;
        private Size maxSize;
        private const double MinOccupiedAreaRatio = 0.65;
        private static readonly Point layoutCenter = new Point(1000, 1000);

        [SetUp]
        public void SetUp()
        {
            // Закомментируйте одну из строк ниже
            layouter = new CircularCloudLayouter(layoutCenter);
            // layouter = new BadLayouter(new Point(1000, 1000));
            minSize = new Size(5, 5);
            maxSize = new Size(20, 20);
        }

        [TearDown]
        public void DrawLayoutAfterTestFailed()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var picture = new Picture(new Size(2 * layoutCenter.X, 2 * layoutCenter.Y));
            var expectedMaximumRadius = GetExpectedMaximumLayoutRadius(layouter);

            Color color;
            foreach (var rectangle in layouter.Rectangles)
            {
                color = GetRectangleColor(rectangle, layouter, expectedMaximumRadius);
                picture.FillRectangle(rectangle, color);
            }
            
            picture.DrawCircle(layouter.Center,
                (float)expectedMaximumRadius, Color.Cyan);
            picture.Save(outputFileName:$"Layout failed test {TestContext.CurrentContext.Test.Name}");
        }

        [Test]
        public void PutNextRectangle_ShouldReturnRectangleWithSameSize_WhenSomeSizesAdded()
        {
            foreach (var size in SizesGenerator.GenerateSizes(5, minSize, maxSize, seed:128))
                layouter.PutNextRectangle(size).Size
                    .Should().BeEquivalentTo(size);
        }
        
        [Test]
        public void Layout_ShouldContainsAllRectangles_WhenSomeSizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(5, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);

            layouter.Rectangles.Select(rect => rect.Size)
                .Should().BeEquivalentTo(sizes);
        }
        
        [Test]
        public void LayoutRectangles_ShouldNotIntersectEachOther_WhenSomeRectanglesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(25, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);

            foreach (var rectangle in layouter.Rectangles)
            {
                IsRectangleIntersectOther(layouter, rectangle)
                    .Should().BeFalse("rectangles must not intersect");
            }
        }
        
        [Test]
        public void LayoutOccupiedArea_ShouldBeGreaterOrEqualThanMinimal_WhenManySizesAdded()
        {
            var sizes = SizesGenerator.GenerateSizes(600, minSize, maxSize, seed:128);
            FillLayoutWithSomeRectangles(layouter, sizes);
            
            var occupiedArea = layouter.Rectangles
                .Sum(rectangle => rectangle.Width * rectangle.Height);

            var maxLayoutRadius = 0.0;

            foreach (var rectangle in layouter.Rectangles)
            {
                var maxDistanceToRectangle = GetMaxDistanceToRectangle(layouter.Center, rectangle);
                if (maxLayoutRadius < maxDistanceToRectangle)
                    maxLayoutRadius = maxDistanceToRectangle;
            }

            var totalArea = GetCircleAreaFromRadius(maxLayoutRadius);
            var actualOccupiedAreaRatio = occupiedArea / totalArea;
            actualOccupiedAreaRatio.Should().BeGreaterOrEqualTo(MinOccupiedAreaRatio);
        }

        private static double GetMaxDistanceToRectangle(Point center, Rectangle rectangle)
        {
            var cornerX = GetNumberWithBiggerDistanceFromGiven(center.X, 
                rectangle.X, rectangle.X + rectangle.Width);
            var cornerY = GetNumberWithBiggerDistanceFromGiven(center.Y, 
                rectangle.Y, rectangle.Y + rectangle.Height);
            return GetDistance(center, new Point(cornerX, cornerY));
        }
        
        private static int GetNumberWithBiggerDistanceFromGiven(int givenNumber, 
            int firstNumber, int secondNumber)
        {
            return Math.Abs(givenNumber - firstNumber) >= Math.Abs(givenNumber - secondNumber)
                ? firstNumber
                : secondNumber;
        } 
        
        private static double GetCircleRadiusFromArea(double circleArea) 
            => Math.Sqrt(circleArea / Math.PI);
        
        private static double GetCircleAreaFromRadius(double radius) 
            => Math.PI * Math.Pow(radius, 2);
        
        private static double GetDistance(Point first, Point second)
            => Math.Sqrt(Math.Pow(first.X - second.X, 2)
                         + Math.Pow(first.Y - second.Y, 2));
        
        private static void FillLayoutWithSomeRectangles(IRectangleLayouter layouter,
            IEnumerable<Size> rectangleSizes)
        {
            foreach (var size in rectangleSizes)
                layouter.PutNextRectangle(size);
        }

        private static bool IsRectangleIntersectOther(IRectangleLayouter layouter, Rectangle rectangle)
        {
            foreach (var otherRectangle in layouter.Rectangles)
            {
                if (rectangle != otherRectangle 
                    && rectangle.IntersectsWith(otherRectangle))
                    return true;
            }

            return false;
        }
        
        private static bool IsDistanceToRectangleOverLimit(Rectangle rectangle,
            Point point, double limit)
        {
            var distance = GetMaxDistanceToRectangle(point, rectangle);

            return distance > limit;
        }
        
        private static double GetExpectedMaximumLayoutRadius(IRectangleLayouter layouter)
        {
            var occupiedArea = layouter.Rectangles
                .Sum(rectangle => rectangle.Width * rectangle.Height);
            return GetCircleRadiusFromArea(occupiedArea / MinOccupiedAreaRatio);
        }

        private static Color GetRectangleColor(Rectangle rectangle, IRectangleLayouter layouter, double layoutRadius)
        {
            if (IsRectangleIntersectOther(layouter, rectangle))
                return Color.Red;
            if (IsDistanceToRectangleOverLimit(rectangle, layouter.Center, layoutRadius))
                return Color.Yellow;
            return Color.LawnGreen;
        }
    }
}
