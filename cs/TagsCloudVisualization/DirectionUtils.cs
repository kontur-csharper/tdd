namespace TagsCloudVisualization
{
    public static class DirectionUtils
    {
        private static DirectionToMove[] directions = new[]
        {
            DirectionToMove.Up,
            DirectionToMove.Down,
            DirectionToMove.Left,
            DirectionToMove.Right
        };

        public static DirectionToMove[] GetAllDirections() => directions;
    }

    public enum DirectionToMove
    {
        Up,
        Down,
        Left,
        Right
    }
}
