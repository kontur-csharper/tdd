

namespace TagsCloudVisualization.Infrastructure.Environment
{
    public interface ICollisionDetector<in T>
    {
        public bool IsColliding(T element);
    }
}