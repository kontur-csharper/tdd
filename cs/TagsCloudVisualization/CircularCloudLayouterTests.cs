using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _circularCloudLayouter;
        private Point _center;

        [SetUp]
        public void Initialize()
        {
            _center = new Point(200, 200);
            _circularCloudLayouter = new CircularCloudLayouter(_center);
        }

        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(-10, -10)]
        public void CircularCloudLayouter_ThrowsException(int x, int y)
        {
            Action create = () => new CircularCloudLayouter(new Point(x, y));
            create.Should().Throw<ArgumentException>();
        }

        [TestCase(-10, 10)]
        [TestCase(10, -10)]
        [TestCase(-10, -10)]
        [TestCase(10, 0)]
        [TestCase(0, 10)]
        [TestCase(0, 0)]
        public void PutNextRectangle_ThrowsException(int width, int height)
        {
            Action create = () => _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            create.Should().Throw<ArgumentException>();
        }

        [TestCase(4, 4)]
        [TestCase(7, 5)]
        public void PutNextRectangle_PlacesFirstRectangleInCenter(int width, int height)
        {
            _circularCloudLayouter.PutNextRectangle(new Size(width, height));
            _circularCloudLayouter.Rectangles[0].Location.Should()
                .Be(new Point(_center.X - width / 2, _center.Y - height / 2));
        }


        [Test]
        public void CircularCloudLayouter_DoesNotContainAnyRectangles_AfterCreating()
        {
            _circularCloudLayouter.Rectangles.Should().BeEmpty();
        }

        [Test]
        public void CircularCloudLayouter_ContainsManyRectangles_AfterAdding()
        {
            for (var i = 0; i < 10; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            _circularCloudLayouter.Rectangles.Should().HaveCount(10);
        }

        [Test, Timeout(1000)]
        public void PutNextRectangle_HasSufficientPerformance()
        {
            for (var i = 0; i < 1000; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            _circularCloudLayouter.Rectangles.Should().HaveCount(1000);
        }

        [Test]
        public void CircularCloudLayouter_ContainsCorrectRectangle_AfterAdding()
        {
            _circularCloudLayouter.PutNextRectangle(new Size(10, 10));
            _circularCloudLayouter.Rectangles[0].Size.Should().Be(new Size(10, 10));
        }

        [Test]
        public void Rectangles_DoNotIntersectEachOther()
        {
            for (var i = 0; i < 10; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(5, 2));

            var rectangles = _circularCloudLayouter.Rectangles;
            for (var j = 0; j < 9; j++)
                for (var i = j + 1; i < 10; i++)
                {
                    rectangles[j].IntersectsWith(rectangles[i]).Should().BeFalse();
                }
        }

        [TestCase(3, 3)]
        [TestCase(5, 3)]
        public void Cloud_IsCloseToCircle(int width, int height)
        {
            for (var i = 0; i < 20; i++)
                _circularCloudLayouter.PutNextRectangle(new Size(width, height));

            var rectangles = _circularCloudLayouter.Rectangles;
            var leftRadius = rectangles.Select(r => Math.Abs(r.Left)).Max();
            var rightRadius = rectangles.Select(r => Math.Abs(r.Right)).Max();
            var topRadius = rectangles.Select(r => Math.Abs(r.Left)).Max();
            var bottomRadius = rectangles.Select(r => Math.Abs(r.Right)).Max();

            leftRadius.Should().BeCloseTo(rightRadius, (uint)rightRadius / 10);
            topRadius.Should().BeCloseTo(bottomRadius, (uint)bottomRadius / 10);
        }

        [TearDown]
        public void SaveImage()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                DrawImage(_circularCloudLayouter,
                    $"{Environment.CurrentDirectory}\\failedTests\\{testName}.png");
                TestContext.Error.WriteLine($"Tag cloud visualization saved to file \"failedTests\\{testName}\"");
            }
        }

        private static void DrawImage(CircularCloudLayouter circularCloudLayouter, string path)
        {
            var imageSize = circularCloudLayouter.Center.X * 2;
            Bitmap bitmap = new Bitmap(imageSize, imageSize);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in circularCloudLayouter.Rectangles)
            {
                graphics.DrawRectangle(new Pen(Color.Red), rectangle);
            }

            bitmap.Save(path, ImageFormat.Png);
        }
    }
}
