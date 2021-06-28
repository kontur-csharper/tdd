using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private readonly Point center = new Point(300, 300);

        [SetUp]
        public void SetUp() => layouter = new CircularCloudLayouter(center);

        [TestCase(0, 0, TestName = "EmptyPoint")]
        [TestCase(1, 2, TestName = "PointWithPositiveCoords")]
        [TestCase(-1, -2, TestName = "PointWithNegativeCoords")]
        [TestCase(1, -2, TestName = "PointWithMixedCoords")]
        public void CircularCloudLayouterCtor_OnAllPoints_DoesNotThrow(int x, int y)
        {
            Action callCtor = () => _ = new CircularCloudLayouter(new Point(x, y));
            callCtor.Should().NotThrow();
        }

        [TestCase(0, 0, TestName = "EmptySize")]
        [TestCase(5, 5, TestName = "SameDimensions")]
        [TestCase(5, 10, TestName = "DifferentDimensions")]
        public void PutNextRectangle_OnSizes_ReturnsRectanglesWithSameSizes(int width, int height)
        {
            var size = new Size(5, 5);
            layouter.PutNextRectangle(size).Height.Should().Be(5);
            layouter.PutNextRectangle(size).Width.Should().Be(5);
        }

        [TestCase(1, TestName = "OneRectangle")]
        [TestCase(30, TestName = "ThirtyRectangles")]
        [TestCase(50, TestName = "FiftyRectangles")]
        public void PutNextRectangle_OnCountCalls_ReturnsCountRectangles(int count)
        {
            var rectanglesCount = Enumerable.Range(0, count)
                .Select(size => layouter.PutNextRectangle(new Size(size, size)))
                .Count();
            rectanglesCount.Should().Be(count);
        }
        
        
        [TestCase(1, TestName = "OneRectangle")]
        [TestCase(2, TestName = "TwoRectangle")]
        [TestCase(10, TestName = "TenRectangle")]
        [TestCase(100, TestName = "HundredRectangle")]
        public void PutNextRectangle_ManyRectangles_ShouldNotIntersect(int count)
        {
            var rectangles = Enumerable.Range(0, count)
                .Select(i => layouter.PutNextRectangle(new Size(i, i)))
                .ToArray();
            IsAnyTwoRectanglesAreIntersect(rectangles).Should().BeFalse();

            static bool IsAnyTwoRectanglesAreIntersect(Rectangle[] rectangleArray)
            {
                for (var i = 0; i < rectangleArray.Length; i++)
                    for (var j = i + 1; j < rectangleArray.Length; j++)
                        if (rectangleArray[i].IntersectsWith(rectangleArray[j]))
                            return true;
                return false;
            }
        }

        [Test]
        public void SaveLayoutIntoBitmap_WithNullReference_ThrowNewException()
        {
            Action callSave = () => layouter.SaveLayoutIntoBitmap(null);
            callSave.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void SaveLayoutIntoBitmap_WhenEmptyRectangles_DoesNotThrow()
        {
            Action callSave = () => new CircularCloudLayouter(center).SaveLayoutIntoBitmap(new Bitmap(10, 10));
            callSave.Should().NotThrow<ArgumentNullException>();
        }
        

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.ToString() == "Passed") 
                return;
            var visualizer = new CircularCloudLayouterVisualizer(layouter);
            visualizer.SaveInFile($"FailedTest_{TestContext.CurrentContext.Test.Name}_State");
        }
    }
}