using System;

namespace TagsCloudVisualization
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTimeNow();
    }
}