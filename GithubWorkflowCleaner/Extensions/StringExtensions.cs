namespace GithubWorkflowCleaner.Extensions
{
    public static class StringExtensions
    {
        public static List<string> SplitByNewLine(this string str)
        {
            var result = str
                .Split(Environment.NewLine)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            return result;
        }
    }
}
