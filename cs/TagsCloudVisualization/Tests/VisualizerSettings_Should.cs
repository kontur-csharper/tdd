using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Visualization.VisualizationSettings;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    public class VisualizerSettings_Should
    {
        private VisualizerSettings settings;

        [SetUp]
        public void SetUp() => settings = new VisualizerSettings();

        [Test]
        public void ReadSettingsFromConfig_CallMethod_ConfigFileExists()
        {
            var configPath = Path.Combine(Environment.CurrentDirectory, "Visualization", "VisualizationSettings",
                "config.json");
            File.Exists(configPath).Should().BeTrue();
        }

        [Test]
        public void ReadSettings_AfterSaveNewSettings_CorrectReadingFromNewConfig()
        {
            settings.SaveSettingsIntoConfig("Temp", 800, 850);
            var changedSettings = VisualizerSettings.ReadSettingsFromConfig();
            changedSettings.WorkDirectory.Should().Be("Temp");
            changedSettings.ImageWidth.Should().Be(800);
            changedSettings.ImageHeight.Should().Be(850);
        }

        [Test]
        public void ReadSettings_AfterSaveNewSettings_BeSaveSettings()
        {
            settings.SaveSettingsIntoConfig("Temp", 800, 850);
            var changedSettings = VisualizerSettings.ReadSettingsFromConfig();
            changedSettings.Should().BeEquivalentTo(settings);
        }

        [Test]
        public void SaveSettings_WorkDirIsNull_ThrowArgumentException()
        {
            Action callSave = () => settings.SaveSettingsIntoConfig(null, 100, 100);
            callSave.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void SaveSettings_NonexistentDirectory_MakeDirectory()
        {
            var tempSettings = new VisualizerSettings {RootDirectory = Path.GetTempPath()};

            const string workDir = "newDirectory";
            var pathToDir = Path.Combine(Path.Combine(tempSettings.RootDirectory, workDir));
            DeleteDirectoryIfExists(pathToDir);

            tempSettings.SaveSettingsIntoConfig(workDir, 100, 100);
            Directory.Exists(pathToDir).Should().BeTrue();
            
            DeleteDirectoryIfExists(pathToDir);

            static void DeleteDirectoryIfExists(string path)
            {
                if (Directory.Exists(path))
                    Directory.Delete(path);
            }
        }

        [TearDown]
        public static void ReturnToDefaultSettings()
        {
            var defaultSettings = new VisualizerSettings
            {
                RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            };
            defaultSettings.SaveSettingsIntoConfig("TagClouds", 700, 700);
        }
    }
}