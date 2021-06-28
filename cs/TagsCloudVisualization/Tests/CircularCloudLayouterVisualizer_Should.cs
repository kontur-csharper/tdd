using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Core;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class CircularCloudLayouterVisualizer_Should
    {
        private CircularCloudLayouter cloudLayouter;
        private CircularCloudLayouterVisualizer visualizer;

        [SetUp]
        public void SetUp()
        {
            cloudLayouter = new CircularCloudLayouter(Point.Empty);
            visualizer = new CircularCloudLayouterVisualizer(cloudLayouter);
        }

        [Test]
        public void CircularCloudLayouterVisualizerCtor_NullReference_ThrowArgumentNullException()
        {
            Action callCtor = () => _ = new CircularCloudLayouterVisualizer(null);
            callCtor.Should().Throw<ArgumentNullException>();
        }

        [TestCase(null, TestName = "NullReference")]
        [TestCase("", TestName = "EmptyString")]
        [TestCase(" ", TestName = "OneSpace")]
        [TestCase("      ", TestName = "ManySpaces")]
        public void SaveInFile_IncorrectInputs_ThrowArgumentException(string input)
        {
            Action saveInFile = () => visualizer.SaveInFile(input);
            saveInFile.Should().Throw<ArgumentException>();
        }

        [Test]
        public void SaveInFile_ForbiddenCharsForFilename_ThrowArgumentException()
        {
            Action saveInFile = () => visualizer.SaveInFile(@"\/?<>|""*");
            saveInFile.Should().Throw<ArgumentException>();
        }

        [TestCase("filename", TestName = "PlainName")]
        [TestCase("jpg", TestName = "FilenameSameAsExtension")]
        [TestCase("asbjpg", TestName = "FilenameEndsWithExtensionWithoutDot")]
        [TestCase("asb pg", TestName = "FilenameHasWhiteSpaceInsteadDot")]
        public void SaveInFile_FilenamesWithoutRequiredExtension_ImageNamesHasRequiredExtension(string input)
        {
            var expectedFileName = $"{input}.jpg";
            var expectedFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                                   + @"\TagClouds\"
                                   + expectedFileName;

            visualizer.SaveInFile(input);
            Path.GetFileName(expectedFilePath).Should().Be(expectedFileName);

            DeleteFile(expectedFilePath);
        }

        [Test]
        public void SaveInFile_CreateImageWithDefaultName_ImageExists()
        {
            var expectedFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                                   @"\TagClouds\last_cloud.jpg";

            visualizer.SaveInFile();
            File.Exists(expectedFilePath).Should().BeTrue();

            DeleteFile(expectedFilePath);
        }

        [TestCase("success", TestName = "Name is \"success\"")]
        [TestCase("1231", TestName = "Name is \"1231\"")]
        [TestCase("&.,!%$#@", TestName = "Name is \"&.,!%$#@\"")]
        public void SaveInFile_NonDefaultFilenames_ImagesWithTheseFilenamesExist(string input)
        {
            var expectedFilePath =
                Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\TagClouds\" + input;
            if (!expectedFilePath.EndsWith(".jpg"))
                expectedFilePath += ".jpg";

            visualizer.SaveInFile(input);
            File.Exists(expectedFilePath).Should().BeTrue();

            DeleteFile(expectedFilePath);
        }

        private static void DeleteFile(string expectedFilePath)
        {
            if (File.Exists(expectedFilePath))
                File.Delete(expectedFilePath);
        }
    }
}