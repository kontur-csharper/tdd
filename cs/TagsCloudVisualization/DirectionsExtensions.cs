using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class DirectionsExtensions
    {
        public static Directions CounterClockwise(this Directions direction) => direction switch
        {
            Directions.Up => Directions.Left,
            Directions.Left => Directions.Down,
            Directions.Down => Directions.Right,
            Directions.Right => Directions.Up,
            _ => throw new InvalidOperationException("Unknown direcion")
        };

        public static Directions Clockwise(this Directions direction) => direction switch
        {
            Directions.Up => Directions.Right,
            Directions.Right => Directions.Down,
            Directions.Down => Directions.Left,
            Directions.Left => Directions.Up,
            _ => throw new InvalidOperationException("Unknown direcion")
        };

        public static Directions Opposite(this Directions direction) => direction switch
        {
            Directions.Up => Directions.Down,
            Directions.Left => Directions.Right,
            Directions.Down => Directions.Up,
            Directions.Right => Directions.Left,
            _ => throw new InvalidOperationException("Unknown direcion")
        };

        public static bool IsPositive(this Directions direction) =>
            direction == Directions.Right || direction == Directions.Down;

        public static int IsPositiveModifier(this Directions direction) => direction.IsPositive() ? 1 : -1;

        public static int CompareInDirection(this Directions direction, int first, int second) =>
            (first * direction.IsPositiveModifier()).CompareTo(second * direction.IsPositiveModifier());

        public static Size GetOffset(this Directions direction, int offset = 1) =>
            direction switch
            {
                Directions.Up => new Size(0, -offset),
                Directions.Left => new Size(-offset, 0),
                Directions.Down => new Size(0, offset),
                Directions.Right => new Size(offset, 0),
                _ => new Size(0, 0)
            };
    }
}