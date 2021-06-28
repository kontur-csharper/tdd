using System;
using System.Drawing;

namespace TagCloud
{
    public static class SizesCreator
    {
        public static Size[] CreateSizesArray(int minWidth, int maxWidth, int minHeight, int maxHeight, int amount)
        {
            var rnd = new Random();
            var sizes = new Size[amount];
            for (var index = 0; index < amount; index++)
            {
                sizes[index] = new Size(rnd.Next(minWidth, maxWidth), rnd.Next(minHeight, maxHeight));
            }

            return sizes;
        }
    }
}