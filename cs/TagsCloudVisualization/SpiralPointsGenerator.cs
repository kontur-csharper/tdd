﻿using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralPointsGenerator : IPointsGenerator
    {
        public List<Point> AllGeneratedPoints { get; private set; }

        public SpiralPointsGenerator(int distanceBetweenPoints)
        {

        }

        public Point GetNextPoint()
        {
            throw new System.NotImplementedException();
        }
    }
}