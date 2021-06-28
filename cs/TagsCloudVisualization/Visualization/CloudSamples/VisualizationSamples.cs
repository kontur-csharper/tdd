using System;
using System.Drawing;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Visualization.VisualizationSettings;

namespace TagsCloudVisualization.Visualization.CloudSamples
{
    public static class VisualizationSamples
    {
        private static readonly VisualizerSettings Setting = VisualizerSettings.ReadSettingsFromConfig();
        private static readonly Point Center = new Point(Setting.ImageWidth / 2, Setting.ImageHeight / 2);

        public static void SampleWithSameRectangleSize(int count)
        {
            var cloudLayouter = new CircularCloudLayouter(Center);

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(50, 50));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("same_rectangle_cloud_sample");
        }

        public static void SampleWithIncreasingRectangleSize(int count)
        {
            var cloudLayouter = new CircularCloudLayouter(Center);

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(i, i));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("increasing_rectangle_cloud_sample");
        }

        public static void SampleWithRandomRectangleSize(int count)
        {
            var cloudLayouter = new CircularCloudLayouter(Center);
            var rnd = new Random();

            for (var i = 0; i < count; i++)
                cloudLayouter.PutNextRectangle(new Size(rnd.Next(100), rnd.Next(100)));

            var visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
            visualizer.SaveInFile("random_rectangle_cloud_sample");
        }
    }
}