using System;
using System.Drawing;
using System.IO;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Renders;
using TagsCloudVisualization.TagClouds;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization
{
    internal class Program
    {
        private static Random random = new Random();
        private static string examplesPath = "templates/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Creating examples...");
            if (!Directory.Exists(examplesPath))
                Directory.CreateDirectory(examplesPath);
            CreateCircleExamples();
        }

        private static void CreateCircleExamples()
        {
            var cloud = new CircleTagCloud(Point.Empty);
            var circleLayouter = new CircularCloudLayouter(Point.Empty);

            for (var i = 0; i < 2000; i++)
                cloud.AddElement(circleLayouter.PutNextRectangle(random.GetSize(2, 20, random.NextDouble() * 4 + 2)));

            var solidVisualizer = new SolidVisualizer(cloud, Color.MediumVioletRed);
            var distanceVisualizer = new DistanceColorVisualizer(
                cloud,
                Color.FromArgb(255, 107, 196, 255),
                0,
                Color.FromArgb(0, 11, 47, 84),
                700);

            new FileCloudRender(solidVisualizer, Path.Combine(examplesPath, "solid.png")).Render();
            new FileCloudRender(distanceVisualizer, Path.Combine(examplesPath, "distance.png")).Render();

            cloud = new CircleTagCloud(Point.Empty, 400);
            circleLayouter = new CircularCloudLayouter(Point.Empty, 400);
            for (var i = 0; i < 2000; i++)
                cloud.AddElement(circleLayouter.PutNextRectangle(random.GetSize(2, 20, random.NextDouble() * 4 + 2)));

            solidVisualizer = new SolidVisualizer(cloud, Color.MediumSeaGreen);
            new FileCloudRender(solidVisualizer, Path.Combine(examplesPath, "ring.png")).Render();
        }
    }
}