using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    public class LayoutDrawer
    {
        private Point objectsOffset;
        private Size pictureSize;
        private readonly Random random;
        private List<Rectangle> rectangles;
        private readonly List<Circle> circles;
        private readonly StringBuilder svg;

        public LayoutDrawer()
        {
            random = new Random();
            svg = new StringBuilder();
            circles = new List<Circle>();
        }

        public void AddRectangles(List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
        }

        public void AddCircle(Circle circle)
        {
            circles.Add(circle);
        }

        public void Draw()
        {
            SetParameters();
            svg.Append($"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" " +
                       $"width=\"{pictureSize.Width}\" height=\"{pictureSize.Height}\">\n");
            foreach (var rectangle in rectangles)
                svg.Append(RectangleToSvgString(rectangle));
            foreach (var circle in circles)
                svg.Append(CircleToSvgString(circle));
            svg.Append("</svg>");
        }

        private string RectangleToSvgString(Rectangle rectangle)
        {
            return $"<rect x=\"{rectangle.X + objectsOffset.X}\" " +
                   $"y=\"{rectangle.Y + objectsOffset.Y}\" " +
                   $"width=\"{rectangle.Width}\" height=\"{rectangle.Height}\" " +
                   $"style=\"fill:rgb({random.Next(0, 255)}, {random.Next(0, 255)}, {random.Next(0, 255)});\" />\n";
        }

        private string CircleToSvgString(Circle circle)
        {
            return $"<circle cx=\"{circle.Center.X + objectsOffset.X}\" cy=\"{circle.Center.Y + objectsOffset.Y}\" " +
                   $"r=\"{circle.Radius}\" stroke=\"black\" stroke-width=\"2\" fill-opacity=\"0\"/>\n";
        }

        private void SetParameters()
        {
            var left = GetExtremeLeftCoordinate();
            var bottom = GetExtremeBottomCoordinate();
            if (left < 0)
                objectsOffset.X = -left;
            if (bottom < 0)
                objectsOffset.Y = -bottom;
            pictureSize.Width = GetExtremeRightCoordinate() + objectsOffset.X;
            pictureSize.Height = GetExtremeTopCoordinate() + objectsOffset.Y;
        }

        private int GetExtremeLeftCoordinate()
        {
            return Math.Min(rectangles.Select(rectangle => rectangle.X).Min(),
                circles.Select(circle => circle.Center.X - circle.Radius).Min());
        }

        private int GetExtremeRightCoordinate()
        {
            return Math.Max(rectangles.Select(rectangle => rectangle.X + rectangle.Width).Max(),
                circles.Select(circle => circle.Center.X + circle.Radius).Max());
        }

        private int GetExtremeTopCoordinate()
        {
            return Math.Max(rectangles.Select(rectangle => rectangle.Y + rectangle.Height).Max(),
                circles.Select(circle => circle.Center.X + circle.Radius).Max());
        }

        private int GetExtremeBottomCoordinate()
        {
            return Math.Min(rectangles.Select(rectangle => rectangle.Y).Min(),
                circles.Select(circle => circle.Center.Y - circle.Radius).Min());
        }

        public void Save(string filename, string path = null)
        {
            path ??= Directory.GetCurrentDirectory();
            var filePath = Path.Combine(path, $"{filename}.svg");
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var info = new UTF8Encoding(true).GetBytes(svg.ToString());
                fs.Write(info, 0, info.Length);
            }

            Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
    }
}