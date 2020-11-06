using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualizationTests
{
    public class PlainEnvironmentTests
    {
        private PlainEnvironment sut;
        [SetUp]
        public void SetUp()
        {
            sut = new PlainEnvironment();
        }


        [Test]
        public void ContainsAllAddedElements()
        {
            var items = new List<Rectangle>
            {
                new Rectangle(),
                new Rectangle(),
                Rectangle.Empty,
                new Rectangle(0, 0, 0, 0),
                new Rectangle(1, 1, 10, 2),
            };

            foreach (var rectangle in items)
                sut.Add(rectangle);
            
            CollectionAssert.AreEquivalent(sut, items);
        }

    }
}