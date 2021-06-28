﻿using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter layout;
        private Point center;
        private Size[] rectangleSizes;
        private int rectangleCount;
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            center = new Point(100, 100);
            layout = new CircularCloudLayouter(center);
            rectangleSizes = GetRandomSizeSet();
            rectangles = new List<Rectangle>();
            rectangleCount = 10;
        }

        private Size[] GetRandomSizeSet(int length = 4, int maxSideSize = 40, int minSideSize = 10)
        {
            var sizes = new Size[length];
            var random = new Random();
            for (int i = 0; i < length; i++)
                sizes[i] = new Size(random.Next(minSideSize, maxSideSize), random.Next(minSideSize, maxSideSize));
            return sizes;
        }

        private void PutRectangles(int rectangleCount = 10, Size[] sizes = null)
        {
            sizes ??= rectangleSizes;
            this.rectangleCount = rectangleCount;
            var sizesLenght = sizes.Length;
            Size size;
            for (int i = 0; i < rectangleCount; i++)
            {
                size = sizes[i % sizesLenght];
                rectangles.Add(layout.PutNextRectangle(size));
            }
        }

        [Test]
        public void PutNextRectangleShouldNotIntersect()
        {
            PutRectangles();

            var isIntesect = rectangles
                .Any(rect => rect.IntersectsWith(rectangles.Where(other => other != rect)));

            isIntesect.Should().BeFalse();
        }

        [Test]
        public void PutNextRectangleShouldHaveMinimalLenWhenRectIsFirst()
        {
            var rect = layout.PutNextRectangle(new Size(10, 10));
            rect.Location.Should().Be(center);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeLikeACircle()
        {
            /// Для тестирования на расположение в виде окружности 
            /// Решил использовать следующую логику
            ///     предполагаю, что прямоугольники можно расположить крестом (поэтому rectCountInRadius = rectangleCount / 4)
            ///     определяю количесво прямоугольников которые содержать радиус
            ///     определяю радиус
            /// Проверка происходи на утверждении, что все прямоугольники должны быть вписаны в кадрат со стороной 2 радиуса
            ///     
            var size = new Size(20, 20);
            PutRectangles(20, new[] { size });
            var rectCountInRadius = rectangleCount / 4 + 1;
            var radius = Math.Max(size.Height, size.Width) * rectCountInRadius;
            var circumscribedCenter = new Point(center.X - radius, center.Y - radius);
            var circumscribedRectangle = new Rectangle(circumscribedCenter, new Size(radius * 2, radius * 2));

            var isIntersectAll = rectangles.All(rect => rect.IntersectsWith(circumscribedRectangle));

            isIntersectAll.Should().BeTrue();
        }

        [Test]
        public void PutNextRectangleReturnDifferentRectangles()
        {
            var allRectangles = new List<Rectangle>();
            var differentRectangles = new HashSet<Rectangle>();
            Rectangle rectangle;
            var size = new Size(10, 10);

            for (int i = 0; i < 300; i++)
            {
                rectangle = layout.PutNextRectangle(size);
                allRectangles.Add(rectangle);
                differentRectangles.Add(rectangle);
            }

            allRectangles.Should().HaveCount(differentRectangles.Count);
            allRectangles.Should().BeEquivalentTo(differentRectangles);
        }

        [Test]
        public void PutNextRectangleAllRectanglesAdded()
        {
            PutRectangles(100);

            layout.Should().HaveCount(rectangles.Count);
            layout.Should().BeEquivalentTo(rectangles);
        }

        [Test]
        public void PutNextRectangleAllRectangleShouldBeDense()
        {
            PutRectangles(30);

            foreach (var rectangle in rectangles)
            {
                var vector = new TargetVector(center, rectangle.Location);
                foreach (var delta in vector.GetPartialDelta().Take(3))
                    rectangle.TryMoveRectangle(delta, rectangles).Should().BeFalse();
            }
        }

        [TearDown]
        public void WriteErrorLog()
        {
            var testContext = TestContext.CurrentContext;
            if (!testContext.Result.Outcome.Status.HasFlag(TestStatus.Failed))
                return;

            var path = CircularCloudVisualization.SaveImageFromLayout(testContext.Test.Name, layout, 2);

            TestContext.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}
