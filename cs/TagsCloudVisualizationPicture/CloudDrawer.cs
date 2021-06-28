using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualizationPicture
{
    public class CloudDrawer
    {
        public static void DrawAndSaveCloud(Rectangle[] rectangles, string fileName)
        {
            var bitmap = new Bitmap(500, 500);
            var gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, 500, 500);
            gr.FillRectangles(Brushes.Blue, rectangles);
            bitmap.Save(fileName);
        }
    }
}