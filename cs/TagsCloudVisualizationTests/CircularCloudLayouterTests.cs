using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;
using TagsCloudVisualization.Graphic;
using TagsCloudVisualization.Infrastructure;

namespace TagsCloudVisualizationTests
{
    
    public class CircularCloudLayouterTests
    {
        private CircularCloudLayouter sut;
        private Point anchor;
        private List<Rectangle> rectangles;
        [SetUp]
        [Timeout(1000)]
        public void Setup()
        {
            anchor = new Point();
            sut = new CircularCloudLayouter(anchor);
            rectangles = new List<Rectangle>();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var drawer = new RainbowDrawer(1, 10);
                var saver = new BitmapSaver(AppDomain.CurrentDomain.BaseDirectory);
                var name = TestContext.CurrentContext.Test.Name + TestContext.CurrentContext.Test.ID;
                var path = saver.GetPath(name);
                var image = drawer.GetImage(rectangles);
                saver.Save(image, path);
                Console.WriteLine($"Tag cloud visualization saved to file '{path}'");
            }
        }


        private IEnumerable<Rectangle> PlaceRectangles(CircularCloudLayouter layouter, IEnumerable<Size> rectangleSizes)
        {
            return rectangleSizes.Select(layouter.PutNextRectangle);
        }
        
        public static IEnumerable<TestCaseData> DataLayouts()
        {
            (string dataName, Size[] sizes)[] testData = {
                ("Long thin tiles", new[]
                {
                    new Size(100, 1),
                    new Size(1, 100),
                }),
                ("Small tiles", new[]
                {
                    new Size(1, 1),
                    new Size(1, 1),
                }),
                ("Quadratic tiles", new []
                {
                    new Size(5, 5),
                    new Size(1,1),
                    new Size(2,2),
                    new Size(3,3),
                    new Size(4,4),
                }),
                ("Growing horizontal tiles", new []
                {
                    new Size(10, 1),
                    new Size(20,1),
                    new Size(30,1),
                    new Size(40,1),
                }),
                ("Growing vertical tiles", new []
                {
                    new Size(1, 10),
                    new Size(1,20),
                    new Size(1,30),
                    new Size(1,40),
                }),
                ("Random", GetRandomSizes(300).ToArray())
                
            };

            return testData.Select(test
                => new TestCaseData(test.sizes) {TestName = test.dataName});
        }

        private static IEnumerable<Size> GetRandomSizes(int count)
        {
            var random = new Random();
            var maxWidth = 50;
            var maxHeight = 20;
            for (int i = 0; i < count; i++)
                yield return new Size(random.Next(maxWidth),random.Next(maxHeight));
        }

        [Test]
        public void Constructor_Exists()
        {
            var center = new Point();
            
            var _ = new CircularCloudLayouter(center);
        }
        
        [Test]
        public void Constructor_DoesNotThrow()
        {
            var center = new Point();
            
            Assert.DoesNotThrow(() => _ = new CircularCloudLayouter(center)); 
        }
        
        [Test]
        public void PutNextRectangle_Exists()
        {
            var center = new Point();
            var rectangleSize = new Size();
            var layouter = new CircularCloudLayouter(center);
            
            Rectangle _ = layouter.PutNextRectangle(rectangleSize);
        }
        
        [Test]
        public void PutNextRectangle_DoesNotThrow_WhenEmptySize()
        {
            var center = Point.Empty;
            var rectangleSize = Size.Empty;
            var layouter = new CircularCloudLayouter(center);

            Assert.DoesNotThrow(() => _ = layouter.PutNextRectangle(rectangleSize));
        }

        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_IsCenteredByPoint(IEnumerable<Size> rectangleSizes)
        {
            rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
            var centers = rectangles
                .Select(rectangle => rectangle.GetCenter())
                .ToList();
            var centersTotal = centers.Aggregate(
                Point.Empty, 
                (total, rectangleCenter) => total.Add(rectangleCenter));
            var averageCenter = new Point(centersTotal.X / centers.Count, centersTotal.Y / centers.Count);
            var deviation = new Point
            (
                Math.Abs(averageCenter.X - anchor.X),
                Math.Abs(averageCenter.Y - anchor.Y)
            );
            Assert.That(deviation.X, Is.LessThan(rectangles.Max(rectangle => rectangle.Width)));
            Assert.That(deviation.Y, Is.LessThan(rectangles.Max(rectangle => rectangle.Height)));
        }

        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_RectanglesDoNotIntersect(IEnumerable<Size> rectangleSizes)
        {
            rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
            foreach (var rectangle1 in rectangles)
            foreach (var rectangle2 in rectangles
                .Where(rectangle2 => rectangle1 != rectangle2))
                Assert.False(rectangle1.IntersectsWith(rectangle2), 
                    $"{rectangle1}X{rectangle2}");
        }
        
        [TestCaseSource(nameof(DataLayouts))]
        public void Cloud_RectanglesAreArrangedCompactly(Size[] rectangleSizes)
        {
            var expected = Math.Max(
                rectangleSizes.Max(rectangle => rectangle.Width),
                rectangleSizes.Max(rectangle => rectangle.Height)
            );
            
            rectangles = PlaceRectangles(sut, rectangleSizes).ToList();
            
            foreach (var neighbourDistance in rectangles
                .Select(rectangle1 => rectangles
                .Where(rectangle2 => rectangle1 != rectangle2)
                .Min(rectangle2 => rectangle1.GetCenter().DistanceFrom(rectangle2.GetCenter()))))
            {
                Assert.That(neighbourDistance, Is.LessThanOrEqualTo(expected));
            }
        }
    }
}