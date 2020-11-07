using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TagsCloudVisualisation.Visualisation.TextVisualisation;

namespace TagsCloudVisualisationTests.Samples
{
    public class SourceCodeTextAnalyzer : ITextAnalyzer
    {
        private readonly Dictionary<string, uint> wordsWithCount = new Dictionary<string, uint>();
        private static readonly Regex wordsRegex = new Regex(@"\W(?<word>\w+)\W", RegexOptions.Compiled);

        public void RegisterText(string text)
        {
            var lines = text.Split('\n', '\r')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(l => l.Trim(' ', '\t'))
                .Where(l => !l.StartsWith(@"\\"))
                .Select(l => l.ToLower())
                .ToArray();

            foreach (var word in lines.SelectMany(line => wordsRegex.Matches(line).Select(x => x.Groups["word"].Value)))
                AddWord(word);
        }

        public IEnumerable<string> GetSortedWords() => wordsWithCount.OrderByDescending(x => x.Value)
            .Select(x => x.Key);

        private void AddWord(string word) => wordsWithCount[word] = wordsWithCount.GetValueOrDefault(word, 0U) + 1;
    }
}