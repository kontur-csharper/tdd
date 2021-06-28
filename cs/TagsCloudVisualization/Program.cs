using System.IO;

namespace TagsCloudVisualization
{
    class Program
    {
        public static void Main()
        {
            var inputFile1 = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "..", "..", "..", "sizes", "sizes1.txt"));
            var outputFile1 = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), "..", "..", "..", "visualizations", "visualization1.png"));
            CloudVisualizer.VisualizeCloudFromFile(inputFile1, outputFile1, new System.Drawing.Size(1500, 1500));
        }
    }
}
