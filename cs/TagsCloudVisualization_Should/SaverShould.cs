﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization;

namespace TagsCloudVisualization_Should
{
    public class SaverShould
    {
        [Test]
        public void SaveImage_ThrowArgumentException_FileNameContainsInvalidCharacters()
        {
            var fileName = "/\\:*?\"<>|";
            var bitmap = new Bitmap(1, 1);
            var saver = new BmpSaver();

            Action act = () => saver.SaveImage(bitmap, fileName);

            act.ShouldThrow<ArgumentException>().WithMessage("File name contains invalid characters");

        }

        [Test]
        public void SaveImage_ImageExist_CorrectData()
        {
            var bitmap = new Bitmap(100, 100);
            var saver = new BmpSaver();
            var fileName = "ImageExist";
            var path = Directory.GetCurrentDirectory();

            saver.SaveImage(bitmap, fileName);

            File.Exists($"{path}\\{fileName}.bmp").Should().BeTrue();
        }
    }
}
