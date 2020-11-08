using System.Drawing;
using TagsCloudVisualization;
using TagsCloudVisualization_Tests;

namespace TagCloudExample
{
    class Program
    {
        // Пример раскладки
        static void Main()
        {
            var sizes = SizesGenerator.GenerateSizes(500,
                new Size(100, 100), new Size(300, 300));
            DrawLayout(sizes, "random");
            var stringLikeSizes = SizesGenerator.GenerateSizes(300,
                new Size(300, 100), new Size(1000, 300));
            DrawLayout(stringLikeSizes, "string_like");
        }
        
        private static void DrawLayout(Size[] sizes, string fileName)
        {
            var layouter = new CircularCloudLayouter(new Point(5000, 5000));
            
            var picture = new Picture(new Size(10000, 10000));
            foreach (var size in sizes)
            {
                picture.FillRectangle(layouter.PutNextRectangle(size));
            }
            picture.Save(outputFileName:fileName);
        }
    }
}