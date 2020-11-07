﻿using System;
using System.Drawing;

namespace TagCloud
{
    public class Spiral : ISpiral
    {
        private double DistanceFromCenter { get; set; }
        private double Angle { get; set; }
        public double Density { get; }
        public Point Center { get; }
        public int AngleStep { get; }

        public Point CurrentPoint => ShiftPoint(ConvertToCartesianCoordinates());

        public Spiral(Point center, double density = 0.05, int angleStep = 5)
        {
            Center = center;
            Density = density;
            AngleStep = angleStep;
        }

        private Point ShiftPoint(Point point)
        {
            return new Point(point.X + Center.X, point.Y + Center.Y);
        }

        private Point ConvertToCartesianCoordinates()
        {
            var angleInRadians = Angle * Math.PI / 180;
            var x = (int) Math.Round(DistanceFromCenter * Math.Cos(angleInRadians));
            var y = (int) Math.Round(DistanceFromCenter * Math.Sin(angleInRadians));
            return new Point(x, y);
        }

        public void Next()
        {
            Angle += AngleStep;
            DistanceFromCenter = Density * Angle;
        }
    }
}