using System;
using System.Drawing;

namespace CircularCloud
{
    public class ArchimedeanSpiral
    {
        public Point Center { get; }
        protected const double Distance = 0.5;
        protected double Angle;
        protected Point? Previous;

        public ArchimedeanSpiral(Point center) => Center = center;

        public Point GetNextPoint()
        {
            Point result;
            do
            {
                var radius = Distance * Angle;
                var x = radius * Math.Cos(Angle);
                var y = radius * Math.Sin(Angle);
                Angle += 0.0005;
                result = new Point((int) Math.Round(x) + Center.X, (int) Math.Round(y) + Center.Y);
            } while (result == Previous);

            Previous = result;
            return result;
        }
    }
}