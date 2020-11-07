using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualisation.Visualisation.TextVisualisation
{
    public class SimpleTextAnalyzer : ITextAnalyzer
    {
        private readonly int minWordLength;
        private readonly char[] separators;
        private readonly Dictionary<string, uint> wordsWithCount = new Dictionary<string, uint>();

        public SimpleTextAnalyzer(int minWordLength, char[] separators)
        {
            this.minWordLength = minWordLength;
            this.separators = separators;
        }

        public void RegisterText(string text)
        {
            var words = text.Split(separators)
                .Where(x => !string.IsNullOrEmpty(x))
                .Where(w => w.Length >= minWordLength)
                .Select(w => w.ToLower());

            foreach (var word in words)
                wordsWithCount[word] = wordsWithCount.GetValueOrDefault(word, 0U) + 1;
        }

        public IEnumerable<string> GetSortedWords() => wordsWithCount.OrderByDescending(x => x.Value)
            .Select(x => x.Key);
    }
}