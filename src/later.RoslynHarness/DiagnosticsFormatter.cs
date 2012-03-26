using System;
using System.Linq;
using Roslyn.Compilers.CSharp;

namespace later.RoslynHarness
{
    public class DiagnosticsFormatter
    {
        public static void GetDiagnosticsFrom(Compilation compilation)
        {
            if (compilation.GetDiagnostics().Any())
            {
                var errors = compilation.GetDiagnostics().ToList();
                foreach (var diag in errors)
                    Console.WriteLine(diag);
            }
        }
    }
}
