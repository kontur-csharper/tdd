using System;
using System.Drawing;
using System.IO;
using System.Linq;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Visualization.VisualizationSettings;

namespace TagsCloudVisualization.Visualization
{
    public class CircularCloudLayouterVisualizer
    {
        private CircularCloudLayouter CloudLayouter { get; }
        private VisualizerSettings Settings { get; }

        public CircularCloudLayouterVisualizer(CircularCloudLayouter cloudLayouter)
        {
            CloudLayouter = cloudLayouter ?? throw new ArgumentNullException(nameof(cloudLayouter));
            Settings = VisualizerSettings.ReadSettingsFromConfig();
        }

        public void SaveInFile(string filename = "last_cloud.jpg")
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename shouldn`t be null, empty or white space!");
            if (filename.Any(c => Path.GetInvalidPathChars().Contains(c)))
                throw new ArgumentException(@"Filename shouldn't contains \ / : * ? "" < > |");

            if (!filename.EndsWith(".jpg"))
                filename += ".jpg";

            using var bmp = new Bitmap(Settings.ImageWidth, Settings.ImageHeight);
            CloudLayouter.SaveLayoutIntoBitmap(bmp);

            var path = Path.Combine(Settings.RootDirectory, Settings.WorkDirectory, filename);
            bmp.Save(path);

            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }
    }
}