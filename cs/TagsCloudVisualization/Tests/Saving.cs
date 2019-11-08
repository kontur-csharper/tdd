﻿using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace TagsCloudVisualization.Tests
{
    public static class Saving
    {
        public static void SaveImageToDefaultDirectory(string name, Bitmap image)
        {
            var path = GetImagesPath(name);
            image.Save(path);
        }

        public static string GetImagesPath(string name)
        {
            var directoryName =
                new StringBuilder(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory))))
                    .Append(@"\Images\");

            return Path.HasExtension(name)
                ? directoryName.Append(name).ToString()
                : directoryName.Append(name).Append(".png").ToString();
        }
    }
}