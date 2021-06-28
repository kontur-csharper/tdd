using TagsCloudVisualization.Visualization.CloudSamples;
using TagsCloudVisualization.Visualization.VisualizationSettings;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        public static void Main(string workDir = "TagClouds", int imageWidth = 700, int imageHeight = 700, int count = 60)
        {
            var settings = new VisualizerSettings();
            settings.SaveSettingsIntoConfig(workDir, imageWidth, imageHeight);
            
            VisualizationSamples.SampleWithIncreasingRectangleSize(count);
            VisualizationSamples.SampleWithRandomRectangleSize(count);
            VisualizationSamples.SampleWithSameRectangleSize(count);
        }
    }
}