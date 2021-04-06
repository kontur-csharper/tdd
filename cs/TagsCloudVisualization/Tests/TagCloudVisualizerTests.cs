using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization.Tests
{
    public class TagCloudVisualizerTests
    {
        [Test]
        public void CreateBitmapFromRectangles_ShouldThrowException_WhenInvalidDirectoryPath()
        {
            var visualizer = new TagCloudVisualizer(new []
            {
                new Rectangle(new Point( 0,0), new Size(1,1)) 
            });
            Action action = () => visualizer.CreateBitmapFromRectangles(new Size(100, 100), "L O L");

            action.Should().Throw<ArgumentException>().WithMessage("The specified directory does not exist");
        }
    }
}
