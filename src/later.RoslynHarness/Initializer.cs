using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;

namespace later.RoslynHarness
{
    public class Initializer
    {
        public static Compilation InitializeCompilationFrom(CompilerArguments args)
        {
            var compilation = Compilation.Create(outputName: args.Output,
                                                 syntaxTrees: BuildSyntaxTrees(args),
                                                 references: BuildReferences(args),
                                                 options: BuildCompilationOptions(args));
            return compilation;
        }

        private static CompilationOptions BuildCompilationOptions(CompilerArguments args)
        {
            return new CompilationOptions(
                                                assemblyKind:GetAssemblyKindFrom(args.Target),
                                                optimize:args.Optimize,
                                                checkOverflow:args.Checked,
                                                mainTypeName: args.Main
                                         );
        }

        private static IEnumerable<MetadataReference> BuildReferences(CompilerArguments args)
        {
            
            return args.References.Select(refer => new AssemblyFileReference(refer));
        }

        private static IEnumerable<SyntaxTree> BuildSyntaxTrees(CompilerArguments args)
        {
            var trees = new List<SyntaxTree>();
            foreach (var file in args.Files)
            {
                trees.Add(SyntaxTree.ParseCompilationUnit(file));
            }
            return trees;
        }

        private static AssemblyKind GetAssemblyKindFrom(string target)
        {
            var c = target ?? "";
            switch (c.ToLower())
            {
                case "exe":
                    return AssemblyKind.ConsoleApplication;
                case "winexe":
                    return AssemblyKind.WindowsApplication;
                default:
                    return AssemblyKind.DynamicallyLinkedLibrary;
            }
        }
    }
}
