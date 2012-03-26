using NUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using later.RoslynHarness;
using System.Linq;
namespace later.Tests.RoslynHarness
{
    [TestFixture]
    public class when_initializing_roslyn
    {
        private Compilation _compiler;

        [SetUp]
        public void Setup()
        {
            var builder = new CompilerArgumentBuilder();
            builder.Files.Add(@"foo.cs");
            builder.Files.Add(@"foo2.cs");
            builder.References.Add(@"C:\foo1.dll");
            builder.References.Add(@"C:\foo2.dll");
            builder.Defines.Add("DEBUG");
            builder.Output = "foo.dll";
            builder.Platform = "anycpu";
            builder.Target = "exe";
            
            builder.Main = "Foo.Bar.Main";
            builder.Optimize = true;
            builder.IsChecked = true;
            _compiler = Initializer.InitializeCompilationFrom(builder.ToCompilerArguments());
        }

        [Test]
        public void syntax_trees_get_created()
        {
            Assert.AreEqual(2, _compiler.SyntaxTrees.Count);
        }

        [Test]
        public void the_first_file_has_a_syntax_tree()
        {
            Assert.IsTrue(_compiler.SyntaxTrees.Count(x => x.ToString() == "foo.cs") > 0);
        }

        [Test]
        public void the_platform_is_set()
        {
            Assert.Inconclusive("don't know where to set/read this from");
            //Assert.AreEqual();
        }

        [Test]
        public void the_second_file_has_a_syntax_tree()
        {
            Assert.IsTrue(_compiler.SyntaxTrees.Count(x => x.ToString()=="foo2.cs") > 0);
        }

        [Test]
        public void main_output_gets_set()
        {
            Assert.AreEqual("Foo.Bar.Main", _compiler.Options.MainTypeName);
        }

        [Test]
        public void optimize_is_set()
        {
            Assert.IsTrue(_compiler.Options.Optimize);
        }

        [Test]
        public void checked_is_set()
        {
            Assert.IsTrue(_compiler.Options.CheckOverflow);
        }

        [Test]
        public void output_is_set()
        {
            Assert.AreEqual("foo",_compiler.Assembly.Name);
        }

        [Test]
        public void references_are_added()
        {
            Assert.AreEqual(2, _compiler.References.Count);
        }

        [Test]
        public void the_first_reference_is_correct()
        {
            Assert.AreEqual(@"C:\foo1.dll", ((AssemblyFileReference)_compiler.References[0]).Path);
        }

        [Test]
        public void the_second_reference_is_correct()
        {
            Assert.AreEqual(@"C:\foo2.dll", ((AssemblyFileReference)_compiler.References[1]).Path);
        }
        [Test]
        public void kind_is_set()
        {
            Assert.AreEqual(AssemblyKind.ConsoleApplication, _compiler.Options.AssemblyKind);
        }

    }
}
