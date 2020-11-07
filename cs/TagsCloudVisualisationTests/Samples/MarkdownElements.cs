namespace TagsCloudVisualisationTests.Samples
{
    public static class MarkdownElements
    {
        public static string Image(string imagePath) => $@"![]({imagePath})";
        public static string H1(string text) => Header(1, text);
        public static string H2(string text) => Header(2, text);

        private static string Header(int level, string text) => $"{new string('#', level)} {text}";
    }
}