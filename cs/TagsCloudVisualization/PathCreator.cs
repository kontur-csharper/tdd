using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization
{
    public static class PathCreator
    {
        public static string GetPathToFileWithName(int nestingInRootDirectory, params string[] fileSequens)
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (var i = 0; i < nestingInRootDirectory; i++)
                directoryInfo = directoryInfo.Parent;
            var pathSeq = new List<string>(fileSequens);
            pathSeq.Insert(0, directoryInfo.FullName);
            return Path.Combine(pathSeq.ToArray());
        }
    }
}
