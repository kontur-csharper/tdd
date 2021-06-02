using System;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        public static readonly Size[][] FourRectanglesCases =
        {
            new[]
            {
                new Size(1, 1),
                new Size(1, 1),
                new Size(1, 1),
                new Size(1, 1),
            },
            new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            },
        };

        public static readonly Size[][] ALotRectanglesCases =
        {
            new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
                new Size(7, 5),
                new Size(3, 10),
                new Size(15, 1),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
                new Size(14, 23),
                new Size(15, 22),
                new Size(16, 21),
                new Size(17, 20),
                new Size(18, 19),
                new Size(31, 10),
                new Size(11, 26),
                new Size(12, 25),
                new Size(13, 24),
            }
        };
        
        [Test]
        public void ThrowException_WhenPutNotPositiveSize()
        {
            var layouter = new CircularCloudLayouter(new Point(7, 8));

            Action act = () => layouter.PutNextRectangle(new Size(-1, 0));
            act.Should().Throw<ArgumentException>();
        }

        public DirectoryInfo ImageOutputDirectory => new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
            .Parent?.Parent?
            .CreateSubdirectory("Images") ?? new DirectoryInfo(".");
        

        [TestCaseSource(nameof(FourRectanglesCases))]
        public void PlaceFirstFourRectanglesAroundCenter(Size[] rectangleSizes)
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);
            
            var rectangle = layouter.PutNextRectangle(rectangleSizes[0]);
            rectangle.Location.Should().Be(center);

            rectangle = layouter.PutNextRectangle(rectangleSizes[1]);
            rectangle.X.Should().Be(center.X);
            rectangle.Bottom.Should().Be(center.Y);

            rectangle = layouter.PutNextRectangle(rectangleSizes[2]);
            rectangle.Right.Should().Be(center.X);
            rectangle.Bottom.Should().Be(center.Y);
            
            rectangle = layouter.PutNextRectangle(rectangleSizes[3]);
            rectangle.Right.Should().Be(center.X);
            rectangle.Y.Should().Be(center.Y);
        }

        [TestCaseSource(nameof(FourRectanglesCases))]
        public void NotHaveIntersections_WithFourRectangles(Size[] rectangleSizes)
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);
            
            var result = rectangleSizes.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result
                .Where(r => result.Where(rr => rr != r).Any(r.IntersectsWith)).Should().BeEmpty();
        }
        
        [TestCaseSource(nameof(ALotRectanglesCases))]
        public void NotHaveIntersections_WithALotRectangles(Size[] rectangleSizes)
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);
            var rectangles = rectangleSizes.Select(r => layouter.PutNextRectangle(r)).ToArray();
            
            var intersectionIndices = Enumerable.Range(0, rectangles.Length)
                .Where(i => rectangles.Any(r => r != rectangles[i] && r.IntersectsWith(rectangles[i])))
                .ToArray();
            
            if (intersectionIndices.Length != 0)
            {
                var outputFile = $"{ImageOutputDirectory.FullName}\\intersections_fail.bmp";
                var rectangleSizesScaled = rectangleSizes.Select(s => new Size(s.Width * 5, s.Height * 5)).ToArray();
                ImageGenerator.GenerateImageWithRectanglesFromLayouter(rectangleSizesScaled, outputFile,
                    (i) => intersectionIndices.Contains(i)
                        ? Color.Red
                        : Color.Green);
                intersectionIndices.Should()
                    .BeEmpty($"rectangles shouldn't intersect with each other. See \"{outputFile}\" for info");
            }
        }
        
        [TestCaseSource(nameof(FourRectanglesCases))]
        public void NotChangeSize_WithFourRectangles(Size[] rectangleSizes)
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);
            
            var result = rectangleSizes.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangleSizes, options => options.WithStrictOrdering());
        }
        
        [TestCaseSource(nameof(ALotRectanglesCases))]
        public void NotChangeSize_WithALotRectangles(Size[] rectangleSizes)
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);
            
            var result = rectangleSizes.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangleSizes, options => options.WithStrictOrdering());
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            var arguments = context.Test.Arguments;
            if (context.Result.Outcome == ResultState.Failure)
            {
                var rectangleSizes = arguments.Length > 0
                    ? arguments[0] as Size[]
                    : null;
                if (rectangleSizes != null)
                {
                    rectangleSizes = rectangleSizes.Select(s => new Size(s.Width * 5, s.Height * 5)).ToArray();
                    var outputFile = $"{ImageOutputDirectory.FullName}\\test_output.bmp";
                    ImageGenerator.GenerateImageWithRectanglesFromLayouter(rectangleSizes, outputFile);
                    TestContext.Out.WriteLine($"Tag cloud visualization saved to file \"{outputFile}\"");
                }
            }
        }

    }
}