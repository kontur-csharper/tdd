using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public readonly double CoefOfSpiralEquation;
        public readonly double DeltaOfAnglePhi;
        public readonly Point Center;

        public double AnglePhi { get; private set; }

        public Spiral(Point center, double coefOfSpiralEquation = 0.5, double deltaOfAnglePhi = Math.PI / 90)
        {
            if (coefOfSpiralEquation <= 0)
                throw new ArgumentException("Коэфициент в уравнении спирали должен быть положительным");
            if (deltaOfAnglePhi <= 0)
                throw new ArgumentException("Изменение угла должно быть положительным");
            CoefOfSpiralEquation = coefOfSpiralEquation;
            DeltaOfAnglePhi = deltaOfAnglePhi;
            Center = center;
        }

        public Point GetNextPointOnSpiral()
        {
            var x = (int)Math.Round(CoefOfSpiralEquation * AnglePhi * Math.Cos(AnglePhi)) + Center.X;
            var y = (int)Math.Round(CoefOfSpiralEquation * AnglePhi * Math.Sin(AnglePhi)) + Center.Y;
            AnglePhi += DeltaOfAnglePhi;
            return new Point(x, y);
        }
    }
}