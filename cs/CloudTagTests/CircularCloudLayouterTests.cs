using System;
using System.Drawing;
using System.IO;
using System.Linq;
using CloudTag;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace CloudTagTests
{
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layouter;
        private readonly string failTestsImagesPath = Path.Combine(Environment.CurrentDirectory, "Failures Tests Layouts");

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            if (!Directory.Exists(failTestsImagesPath))
                Directory.CreateDirectory(failTestsImagesPath);

            foreach (var filePath in Directory.GetFiles(failTestsImagesPath))
                File.Delete(filePath);
        }
        
        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure)
                return;

            var name = TestContext.CurrentContext.Test.MethodName;
            if (TestContext.CurrentContext.Test.MethodName != TestContext.CurrentContext.Test.Name)
                name += $" {TestContext.CurrentContext.Test.Name}";

            var fullPath = Path.Combine(failTestsImagesPath, $"{name}.png");
            var savingResult = RectanglePainter.TryDrawAndSaveToFile(layouter.GetRectangles(), Color.Red, fullPath);
            
            TestContext.Out.WriteLine(savingResult ? $"Tag cloud visualization saved to file {fullPath}" : "Error while saving file");
        }

        [Test]
        public void Constructor_DoesNotThrow()
        {
            Action action = () => new CircularCloudLayouter(new Point(0, 0));
            action.Should().NotThrow();
        }

        [Test]
        public void PutNextRectangle_ReturnsFirstRectangle_WithCenterInLayouterCenter()
        {
            var size = new Size(10, 10);
            var expectedRectangle = new Rectangle {Size = size}.SetCenter(new Point(0, 0));
            
            var actualRectangle = layouter.PutNextRectangle(size);

            actualRectangle.Should().Be(expectedRectangle);
        }

        [TestCase(2)]
        [TestCase(10)]
        [TestCase(80)]
        [TestCase(200)]
        public void PutNextRectangle_ShouldNotIntersect_RectSameSize(int rectCount)
        {
            var size = new Size(10, 10);
            var rectangles = Enumerable.Range(0, rectCount).Select(i => layouter.PutNextRectangle(size)).ToArray();

            for (var i = 0; i < rectCount; i++)
            for (var j = i + 1; j < rectCount; j++)
                rectangles[i].IntersectsWith(rectangles[j]).Should()
                    .BeFalse($"{rectangles[i]} should not intersect {rectangles[j]}");
        }

        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(-10, -10)]
        public void PutNextRectangle_ShouldThrow_SizeWithNegativeSides(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(0, 10)]
        [TestCase(10, 0)]
        [TestCase(0, 0)]
        public void PutNextRectangle_ReturnsEmptyRect_OneOfSidesIs0(int width, int height)
        {
            Action action = () => layouter.PutNextRectangle(new Size(width, height));
            action.Should().Throw<ArgumentException>();
        }

        [Timeout(1000)]
        [Test]
        public void PutNextRectangle_PerformanceTest_2500RectSameSize()
        {
            var size = new Size(10, 10);
            for (var i = 0; i < 2500; i++)
                layouter.PutNextRectangle(size);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        public void PutNextRectangle_LayoutShouldBeTight_RectsDifferentSize(int rectCount)
        {
            var rectangles = Enumerable.Range(0, rectCount).Select(i =>
                layouter.PutNextRectangle(new Size((i * 15 + 25) % 90 + 30, (i * 10 + 20) % 60 + 20))).ToArray();
            
            var maxDistanceToCenter = rectangles.Max(rectangle =>
                Math.Sqrt(Math.Pow(rectangle.Location.X, 2) +
                          Math.Pow(rectangle.Location.Y, 2)));

            var commonArea = rectangles.Sum(rectangle => rectangle.Height * rectangle.Width);
            var borderCircleArea = maxDistanceToCenter * maxDistanceToCenter * Math.PI;

            borderCircleArea.Should().BeLessThan(3 * commonArea);
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        public void PutNextRectangle_LayoutShouldBeRoundShape_RectsDifferentSize(int rectCount)
        {
            var rectangles = Enumerable.Range(0, rectCount).Select(i =>
                layouter.PutNextRectangle(new Size((i * 15 + 25) % 90 + 30, (i * 5 + 20) % 60 + 20))).ToArray();

            var commonArea = rectangles.Sum(rectangle => rectangle.Height * rectangle.Width);
            var suggestedCircleRadius = Math.Sqrt(commonArea / Math.PI);

            foreach (var rectangle in rectangles)
            {
                var distanceToCenter = Math.Sqrt(Math.Pow(rectangle.Location.X, 2) +
                                                 Math.Pow(rectangle.Location.Y, 2));

                distanceToCenter.Should().BeLessThan(3 * suggestedCircleRadius / 2);
            }
        }
    }
}