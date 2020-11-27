using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

namespace TagsCloudVisualizationTests
{
    public class BitmapSaverTests
    {
        private Bitmap ImageBitmap { get; set; }

        [SetUp]
        public void SetUp()
        {
            const int height = 10;
            const int width = 10;
            ImageBitmap = new Bitmap(width, height);
        }

        [TestCase(@"<html></html>", TestName = "Directory dont exist")]
        [TestCase(@"C:/Dir/image.jpg", TestName = "Not backslash separator")]
        [TestCase(@"C:\image jpg", TestName = "Doesnt have dot separator")]
        [TestCase(@"C:\image.", TestName = "Doesnt have filename extension")]
        public void SaveBitmapToDirectory_ThrowException_When(string path)
        {
            Action saveImage = () => BitmapSaver.SaveBitmapToDirectory(ImageBitmap, path);

            saveImage.Should().Throw<ArgumentException>();
        }

        [TestCase(@"C:\image.jpg", TestName = "Relative path")]
        [TestCase(@"..\image.jpg", TestName = "Absolute path")]
        public void SaveBitmapToDirectory_DoesntThrowException_When(string path)
        {
            Action saveImage = () => BitmapSaver.SaveBitmapToDirectory(ImageBitmap, path);

            saveImage.Should().NotThrow<ArgumentException>();
        }
    }
}