using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Renders;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudTests
    {
        [SetUp]
        public void SetUp()
        {
            center = new Point(0, 0);
            layouter = new CircularCloudLayouter(center);
            rectangles = new List<Rectangle>();
            random = new Random();
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
                Directory.CreateDirectory(PathToDebugImages);
                var name = $"{context.Test.Name}_{DateTime.Now:HH-mm-ss_tt-zz}.png";
                var tagCloud = new RectangleTagCloud();
                tagCloud.AddElements(rectangles);
                var vis = new RectangleVisualizer(tagCloud);
                new FileCloudRender(vis, Path.Combine(PathToDebugImages, name)).Render();
            }
        }

        private const string PathToDebugImages = "testFails/";

        private CircularCloudLayouter layouter;
        private Point center;
        private List<Rectangle> rectangles;
        private Random random;

        [Test]
        public void PutNextRectangle_IsEmpty_ShouldReturnCorrectSize()
        {
            var rectangle = PutRectangle(new Size(4, 11));

            rectangle.Width.Should().Be(4);
            rectangle.Height.Should().Be(11);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_RectanglesShouldNotIntersect()
        {
            var rect1 = PutRectangle(new Size(200, 30));
            var rect2 = PutRectangle(new Size(200, 30));

            rect1.IntersectsWith(rect2).Should().BeFalse();
        }

        [Test]
        public void PutNextRectangle_ManyRectangles_RectanglesShouldNotIntersect()
        {
            var size = new Size(300, 20);
            var count = 1000;

            for (var i = 0; i < count; i++)
                PutRectangle(size);

            ListOfRectangles_EnumerableWithRectangles_RectanglesShouldNotIntersect(rectangles);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_FirstRectangleShouldBeAroundCenter()
        {
            layouter = new CircularCloudLayouter(new Point(-500, 4000));
            var rectangle = PutRectangle(new Size(1, 1));

            rectangle.X.Should().BeInRange(-502, -498);
            rectangle.Y.Should().BeInRange(3998, 4002);
        }

        [Test]
        [Timeout(1000)]
        public void PutNextRectangle_IsEmpty_ShouldAdd1000RectanglesIn1Second()
        {
            var count = 1000;
            var size = new Size(20, 50);

            for (var i = 0; i < count; i++)
                PutRectangle(size);
        }

        [Test]
        public void PutNextRectangle_IsEmpty_ShouldLooksAsCircle()
        {
            var count = 1000;
            var area = 0;
            var farthestPointDistance = 0d;

            for (var i = 0; i < count; i++)
            {
                var size = random.GetSize(1, 100, 10);
                area += size.Width * size.Height;
                var rectangle = PutRectangle(size);
                var distance = rectangle.MaxDistanceFromCorner(center);
                if (distance > farthestPointDistance)
                    farthestPointDistance = distance;
            }

            var circleArea = Math.PI * farthestPointDistance * farthestPointDistance;
            var different = circleArea / area;

            different.Should().BeInRange(1d, 4d);
        }

        [Test]
        public void PutNextRectangle_SizeWithNegativeSide_ShouldBeThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(-2, 3)));
        }

        [Test]
        public void PutNextRectangle_SizeWithZeroArea_ShouldBeThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => layouter.PutNextRectangle(new Size(0, 0)));
        }

        private void ListOfRectangles_EnumerableWithRectangles_RectanglesShouldNotIntersect(List<Rectangle> candidates)
        {
            for (var i = 0; i < candidates.Count; ++i)
            for (var j = 0; j < candidates.Count; ++j)
            {
                if (i == j)
                    continue;
                candidates[i].IntersectsWith(candidates[j]).Should().Be(false);
            }
        }

        private Rectangle PutRectangle(Size size)
        {
            var rectangle = layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
