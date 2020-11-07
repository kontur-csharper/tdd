using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation.Layouter;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [Explicit]
    [SaveLayouterResults(TestStatus.Failed, TestStatus.Inconclusive, TestStatus.Passed, TestStatus.Warning)]
    public class SourceCodeGenerators : BaseSampleGenerator
    {
        private static readonly Font fontPrototype = SystemFonts.CaptionFont;
        private TextCloudGenerator textCloudGenerator;

        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0), new Size(5, 5));
            WordsLayouterAsset = new WordsLayouterAsset(Layouter, 1);

            textCloudGenerator = new TextCloudGenerator(
                new SourceCodeTextAnalyzer(),
                WordsLayouterAsset,
                true,
                fontPrototype,
                () => new SolidBrush(TestingHelpers.RandomColor));
        }

        [Test]
        public void SourceCodeTermsFrequencies()
        {
            var filesContent = GetSourceFilesContent().ToArray();
            PrintTestingMessage($"Successfully read {filesContent.Length} .cs files");

            foreach (var content in filesContent)
                textCloudGenerator.RegisterText(content);
            textCloudGenerator.GenerateCloud(Enumerable.Range(1, 10).Select(i => i * 2).ToArray());
        }

        private IEnumerable<string> GetSourceFilesContent() =>
            new DirectoryInfo(TestContext.CurrentContext.WorkDirectory)
                    .GoParentUntil(di => di.EnumerateFiles("*.csproj", SearchOption.TopDirectoryOnly).Any())
                    .Parent!
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(ReadFile);

        private string ReadFile(FileInfo fileInfo)
        {
            using var streamReader = new StreamReader(fileInfo.OpenRead());
            return streamReader.ReadToEnd();
        }
    }
}