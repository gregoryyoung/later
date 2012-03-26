using System.Linq;
using later.RoslynHarness;

namespace later
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = CommandLineArgumentsParser.BuildFrom(args);
            var compilation = Initializer.InitializeCompilationFrom(arguments);
            if (compilation.GetDiagnostics().Any())
            {
                DiagnosticsFormatter.GetDiagnosticsFrom(compilation);
            }
        }
    }
}
