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
    public class TextFileGenerators : BaseSampleGenerator
    {
        private static readonly char[] separators = {',', '.', ' ', '!', '?', '\n', '\r', '\t', '&', '#', '-'};
        private static readonly Font fontPrototype = SystemFonts.CaptionFont;

        private TextCloudGenerator textCloudGenerator;

        public override void SetUp()
        {
            base.SetUp();
            Layouter = new CircularCloudLayouter(new Point(0, 0), new Size(5, 5));
            WordsLayouterAsset = new WordsLayouterAsset(Layouter, 1);

            textCloudGenerator = new TextCloudGenerator(
                new SimpleTextAnalyzer(6, separators),
                WordsLayouterAsset,
                true,
                fontPrototype,
                () => new SolidBrush(TestingHelpers.RandomColor));
        }

        [TestCaseSource(nameof(filesTestCases))]
        public void GenerateFromFile(string fileName)
        {
            var text = GetFileContent(fileName);
            textCloudGenerator.RegisterText(text);
            textCloudGenerator.GenerateCloud(2000, Enumerable.Range(1, 10).Select(i => i * 2).ToArray());
        }

        private static TestCaseData[] filesTestCases = new DirectoryInfo(TestingHelpers.BaseInputDirectory)
            .EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly)
            .Select(fi => new TestCaseData(fi.Name)
                {TestName = "Частотный-анализ-" + fi.FileNameOnly().Replace(' ', '-')})
            .ToArray();

        private static string GetFileContent(string fileName) =>
            File.ReadAllText(TestingHelpers.GetTestFile(fileName).FullName);
    }
}