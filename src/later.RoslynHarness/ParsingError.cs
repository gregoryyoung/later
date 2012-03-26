namespace later.RoslynHarness
{
    public class ParsingError
    {
        public readonly string ErrorText;

        public ParsingError(string errorText)
        {
            ErrorText = errorText;
        }
    }
}