using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualisation.Extensions;
using TagsCloudVisualisation.Visualisation.TextVisualisation;
using TagsCloudVisualisationTests.Infrastructure;

namespace TagsCloudVisualisationTests.Samples
{
    [Explicit]
    public abstract class BaseSampleGenerator : LayouterTestBase
    {
        private const string ReadmeHeader = "Вот что удалось навизуализировать:";
        private const string ReadmeFileName = "README.md";
        private const string ReadmeResourcesFolderName = "readme-resources";
        private Image renderedImage;

        protected WordsLayouterAsset WordsLayouterAsset;
        protected Color BackgroundColor;

        public override void SetUp()
        {
            renderedImage = null;
            BackgroundColor = Color.FromArgb(46, 46, 46);
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                using (new Measurement($"Save to {ReadmeResourcesFolderName}", TestContext.Progress))
                    SaveReadmeResourceImage(RenderResultImage());
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            using (new Measurement("Create README.md", TestContext.Progress))
                SaveReadme();
        }

        protected sealed override Image RenderResultImage()
        {
            if (renderedImage == null)
            {
                WordsLayouterAsset.DrawQueuedWords();
                renderedImage = WordsLayouterAsset.GetImage().FillBackground(BackgroundColor);
            }

            return renderedImage;
        }

        private static void SaveReadme()
        {
            var readmeBaseDirectory = GetReadmeBaseDirectory();
            var readmeFile = readmeBaseDirectory.GetFiles(ReadmeFileName).Single();
            var resourcesDirectory = GetResourcesDirectory(readmeBaseDirectory);

            var readmeBuilder = new ReadmeBuilder(readmeFile, ReadmeHeader);
            foreach (var file in EnumerateResourcesIn(resourcesDirectory))
                readmeBuilder.AddResource(file);
            readmeBuilder.Save();
        }

        private static void SaveReadmeResourceImage(Image image)
        {
            const string extension = ".png";
            var testName = TestContext.CurrentContext.Test.Name;
            var resourcesDirectory = GetResourcesDirectory(GetReadmeBaseDirectory());
            image.Save(Path.Combine(resourcesDirectory.FullName, testName + extension));
        }

        private static IEnumerable<FileInfo> EnumerateResourcesIn(DirectoryInfo resourcesDirectory) =>
            resourcesDirectory.EnumerateFiles("*.png").OrderBy(x => x.Name);

        private static DirectoryInfo GetResourcesDirectory(DirectoryInfo baseDirectory) => baseDirectory
            .CreateSubdirectory(ReadmeResourcesFolderName);

        private static DirectoryInfo GetReadmeBaseDirectory() =>
            new DirectoryInfo(TestContext.CurrentContext.WorkDirectory).GoParentUntil(di =>
                di.EnumerateFiles(ReadmeFileName, SearchOption.TopDirectoryOnly).Any());
    }
}