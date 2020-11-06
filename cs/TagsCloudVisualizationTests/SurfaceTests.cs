﻿using System.Collections;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    [TestFixture]
    public class SurfaceTests
    {
        private Surface surface;

        [SetUp]
        public void SetUp()
        {
            surface = new Surface();
        }

        [Test]
        public void IsRectangleIntersect_IfRectanglesEquals_ReturnTrue()
        {
            var rect = new Rectangle(-1, -1, 2, 2);
            IsRectangleIntersectTest(
                rect,
                new[] {rect},
                true);
        }

        [Test]
        public void IsRectangleIntersect_IfRectanglesIntersect_ReturnTrue()
        {
            IsRectangleIntersectTest(
                new Rectangle(0, 0, 2, 2),
                new[] {new Rectangle(-1, -1, 2, 2)},
                true);
        }

        [Test]
        public void IsRectangleIntersect_RectIntersectsWithMoreThanOneRect_ReturnTrue()
        {
            IsRectangleIntersectTest(
                new Rectangle(-2, -2, 7, 7),
                new[]
                {
                    new Rectangle(3, 3, 2, 2),
                    new Rectangle(-3, -3, 2, 2)
                },
                true);
        }

        [Test]
        public void IsRectangleIntersect_RectanglesInDifferentQuarter_ReturnFalse()
        {
            IsRectangleIntersectTest(
                new Rectangle(-10, -10, 2, 2),
                new[] {new Rectangle(10, 10, 2, 2)},
                false);
        }

        [TestCaseSource(nameof(FindQuartersForRectangle_WhenRectangleAtOneQuarterCases))]
        public Surface.Quarters FindQuartersForRectangle_WhenRectangleAtOneQuarter(Rectangle rect)
        {
            return Surface.FindQuartersForRectangle(rect).First();
        }

        [TestCaseSource(nameof(FindQuartersForRectangle_RectangleAtSeveralQuartersCases))]
        public void FindQuartersForRectangle_RectangleAtSeveralQuarters_ReturnTheseQuarters(
            int x, int y, Surface.Quarters[] expectedQuarters)
        {
            var rect = new Rectangle(x, y, 10, 10);

            Surface.FindQuartersForRectangle(rect).Should().BeEquivalentTo(expectedQuarters);
        }

        private void IsRectangleIntersectTest(
            Rectangle rectToMethod, Rectangle[] rectsToSurface, bool expectedResult)
        {
            rectsToSurface.ToList().ForEach(surface.AddRectangle);

            surface.RectangleIntersectsWithOther(rectToMethod).Should().Be(expectedResult);
        }

        [Test]
        public void GetShiftedToCenterRectangle_IfRectangleAlreadyAtCenter_ReturnSameRectangle()
        {
            var rect = new Rectangle(0, 0, 5, 10);

            surface.GetShiftedToCenterRectangle(rect).Should().Be(rect);
        }

        [Test]
        public void GetShiftedToCenterRectangle_AtSurfaceAnotherRectangle_MoveWhileTheyNotIntersect()
        {
            surface.AddRectangle(new Rectangle(0, 0, 2, 2));
            var rectangleForCentering = new Rectangle(-2, 5, 4, 2);

            surface.GetShiftedToCenterRectangle(rectangleForCentering).Should()
                .Be(new Rectangle(-2, 2, 4, 2));
        }

        private static IEnumerable FindQuartersForRectangle_WhenRectangleAtOneQuarterCases()
        {
            yield return new TestCaseData(new Rectangle(10, -10, 1, 1))
                .Returns(Surface.Quarters.First);

            yield return new TestCaseData(new Rectangle(-10, -10, 1, 1))
                .Returns(Surface.Quarters.Second);

            yield return new TestCaseData(new Rectangle(-10, 10, 1, 1))
                .Returns(Surface.Quarters.Third);

            yield return new TestCaseData(new Rectangle(10, 10, 1, 1))
                .Returns(Surface.Quarters.Fourth);
        }

        private static IEnumerable FindQuartersForRectangle_RectangleAtSeveralQuartersCases()
        {
            yield return new TestCaseData(-5, -10,
                    new[] {Surface.Quarters.First, Surface.Quarters.Second})
                .SetName("Rectangle at first and second quarter");

            yield return new TestCaseData(-5, 10,
                    new[] {Surface.Quarters.Third, Surface.Quarters.Fourth})
                .SetName("Rectangle at third and fourth quarter");

            yield return new TestCaseData(-10, -2,
                    new[] {Surface.Quarters.Second, Surface.Quarters.Third})
                .SetName("Rectangle at second and third quarter");

            yield return new TestCaseData(-5, -5,
                    new[]
                    {
                        Surface.Quarters.First,
                        Surface.Quarters.Second,
                        Surface.Quarters.Third,
                        Surface.Quarters.Fourth
                    })
                .SetName("Rectangle at all four quarters");
        }
    }
}