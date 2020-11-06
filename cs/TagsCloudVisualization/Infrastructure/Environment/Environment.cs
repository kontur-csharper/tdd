using System.Collections;
using System.Collections.Generic;

namespace TagsCloudVisualization.Infrastructure.Environment
{
    public abstract class Environment<T> : IEnumerable<T>, ICollisionDetector<T>
    {
        protected List<T> Elements;
        public abstract void Add(T element);
        public abstract void Remove(T element);
        public abstract bool IsColliding(T element);
        
        public IEnumerator<T> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}