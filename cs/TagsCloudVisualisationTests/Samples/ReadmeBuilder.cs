using System.IO;
using System.Text;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    public class ReadmeBuilder
    {
        private readonly FileInfo readmeFile;
        private readonly StringBuilder stringBuilder;

        public ReadmeBuilder(FileInfo readmeFile, string header)
        {
            this.readmeFile = readmeFile;
            stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(MarkdownElements.H1(header));
        }

        public void AddResource(FileInfo resourceFile)
        {
            var relativePath = Path.GetRelativePath(readmeFile.Directory!.FullName, resourceFile.FullName)
                .Replace('\\', '/');

            stringBuilder.AppendLine(MarkdownElements.H2(resourceFile.FileNameOnly()));
            stringBuilder.AppendLine(MarkdownElements.Image(relativePath));
            stringBuilder.AppendLine();
        }

        public void Save()
        {
            using var writer = new StreamWriter(readmeFile.Open(FileMode.Create));
            writer.Write(stringBuilder.ToString());
        }

        public override string ToString() => stringBuilder.ToString();
    }
}