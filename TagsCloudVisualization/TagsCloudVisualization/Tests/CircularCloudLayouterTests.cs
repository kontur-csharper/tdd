﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization.Tests
{
     public class CircularCloudLayouterTests
     {
         private List<Rectangle> _rectangles;
        [SetUp]
        public void SetUp()
        {
            _rectangles = new List<Rectangle>();
        }
        
        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Failure) return;
            if (!Directory.Exists( Environment.CurrentDirectory + @"\TagCloudTests"))
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\TagCloudTests");
            var circularCloudDrawing = new CircularCloudDrawing(new Size(2000, 2000));
            foreach (var rectangle in _rectangles) 
                circularCloudDrawing.DrawRectangle(rectangle);
            var testName = TestContext.CurrentContext.Test.FullName;
            var path = Environment.CurrentDirectory + @"\TagCloudTests\{testName}.png";
            circularCloudDrawing.SaveImage(path);
        }
        [TestCaseSource(nameof(_coordinateCenter))] 
        public void Constructor_DoesNotThrow_WithСorrectСenter(Point center) 
        { 
            Action action = () => new CircularCloudLayouter(center); 
            action.Should().NotThrow(); 
        }
        
        [TestCaseSource(nameof(_coordinateCenter))]
        public void PutNextRectangle_LocateFirstRectangle_OnSpecifiedByXCenter(Point center)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center); 
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(31 , 42));
            rectangle.X.Should().Be(center.X - 31 / 2);
        }
        
        [TestCaseSource(nameof(_coordinateCenter))]
        public void PutNextRectangle_LocateFirstRectangle_OnSpecifiedByYCenter(Point center)
        {
            var circularCloudLayouter = new CircularCloudLayouter(center); 
            var rectangle = circularCloudLayouter.PutNextRectangle(new Size(31 , 42));
            rectangle.Y.Should().Be(center.Y - 42 / 2);
        }
        
        [Test]
        public void PutNextRectangle_TwoRectangleMustNotIntersect()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(1, 1));
            var rectangles = new List<Rectangle>
            {
                circularCloudLayouter.PutNextRectangle(new Size(31, 42)),
                circularCloudLayouter.PutNextRectangle(new Size(10, 23))
            };
            IsNonoverlappingRectangle(rectangles).Should().BeFalse();
        }
        
        [Test]
        public void PutNextRectangle_TenRectangleMustNotIntersect()
        {
            var circularCloudLayouter = new CircularCloudLayouter(new Point(1, 1));
            var rectangles = new List<Rectangle>();

            for (var i = 11; i < 22; i+= 1)
            {
                rectangles.Add(circularCloudLayouter.PutNextRectangle(new Size(i * 3, i)));

            }
            IsNonoverlappingRectangle(rectangles).Should().BeFalse();
        }
        
        
        private static bool IsNonoverlappingRectangle(List<Rectangle> rectangles)
        {
            return rectangles.Any(i => rectangles.Any(j => i != j && i.IntersectsWith(j)));
        }


        private static IEnumerable<TestCaseData> _coordinateCenter = Enumerable 
        .Range(-1, 3) 
        .SelectMany(i => Enumerable 
            .Range(-1, 3) 
            .Select(j => new TestCaseData(new Point(i, j)).SetName("{m}: " + $"X = {i}, Y = {j}"))); 
    }
}