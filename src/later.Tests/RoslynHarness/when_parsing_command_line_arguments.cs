using System.Linq;
using NUnit.Framework;
using later.RoslynHarness;

namespace later.Tests
{
    [TestFixture]
    public class when_parsing_command_line_arguments
    {
        [Test]
        public void can_find_files_on_command_line()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("foo.cs", "foo2.cs");
            Assert.AreEqual(2, arguments.Files.Count());
        }

        [Test]
        public void files_are_kept_in_order()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("foo.cs", "foo2.cs");
            Assert.AreEqual("foo.cs", arguments.Files.First());
            Assert.AreEqual("foo2.cs", arguments.Files.Last());
        }

        [Test]
        public void can_read_a_quoted_file()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("\"My", "foo.cs\"");
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("My foo.cs", arguments.Files.First());
        }

        [Test]
        public void can_read_a_quoted_file_with_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("\"Myfoo.cs\"");
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("Myfoo.cs", arguments.Files.First());
        }

        [Test]
        public void can_read_a_null_file()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom((string) null);
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("", arguments.Files.First());
        }

        [Test]
        public void can_read_a_reference()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom(@"/reference:C:\foo.dll");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_multiple_references()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom(@"/reference:C:\foo.dll", @"/reference:C:\foo2.dll");
            Assert.AreEqual(2, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
            Assert.AreEqual(@"C:\foo2.dll", arguments.References.Last());
        }

        [Test]
        public void can_read_quoted_references()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/reference:\"C:\\Program", "Files\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\Program Files\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_quoted_references_with_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/reference:\"C:\\ProgramFiles\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\ProgramFiles\foo.dll", arguments.References.First());
        }


        [Test]
        public void can_read_resource()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/resource:foo.dll");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
        }

        [Test]
        public void can_read_multiple_resources_and_keep_order()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/resource:foo.dll", "/resource:foo2.dll");
            Assert.AreEqual(2, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
            Assert.AreEqual(@"foo2.dll", arguments.Resources.Last());
        }

        [Test]
        public void can_read_quoted_resource()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/resource:\"C:\\Program", "Files\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\Program Files\\foo.cs", arguments.Resources.First());
        }
        [Test]
        public void can_read_quoted_resource_with_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/resource:\"C:\\ProgramFiles\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\ProgramFiles\\foo.cs", arguments.Resources.First());
        }

        [Test]
        public void can_read_single_define()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/define:DEBUG");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
        }

        [Test]
        public void can_read_multiple_defines()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/define:DEBUG", "/define:debug2");
            Assert.AreEqual(2, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
            Assert.AreEqual("debug2", arguments.Defines.Last());
        }

        [Test]
        public void can_read_quoted_define()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/define:\"DEBUG", "ME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG ME", arguments.Defines.First());
        }
        [Test]
        public void can_read_quoted_define_with_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/define:\"DEBUGME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUGME", arguments.Defines.First());
        }

        [Test]
        public void can_read_output()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/output:foo.dll");
            Assert.AreEqual("foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_output_with_quoted_identifier()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/output:\"C:\\Program", "Files\\foo.dll\"");
            Assert.AreEqual("C:\\Program Files\\foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_output_with_quoted_identifier_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/output:\"C:\\ProgramFiles\\foo.dll\"");
            Assert.AreEqual("C:\\ProgramFiles\\foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_file_align()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/align:512");
            Assert.AreEqual(512, arguments.Alignment);
            Assert.AreEqual(0, arguments.Errors.Count());
        }

        [Test]
        public void non_integer_value_for_alignment_produces_an_error()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/align:GREG");
            Assert.AreEqual(0, arguments.Alignment);
            Assert.AreEqual(1, arguments.Errors.Count());
            Assert.AreEqual("Alignment must be a numeric value", arguments.Errors.First().ErrorText);
        }

        [Test]
        public void can_read_pdb_output_file()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/pdb:Foo.pdb");
            Assert.AreEqual("Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_quoted_pdb_output_file()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/pdb:\"C:\\Program", "Files\\Foo.pdb\"");
            Assert.AreEqual(@"C:\Program Files\Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_quoted_pdb_output_file_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/pdb:\"C:\\ProgramFiles\\Foo.pdb\"");
            Assert.AreEqual(@"C:\ProgramFiles\Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_target()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/target:exe");
            Assert.AreEqual("exe", arguments.Target);
        }

        [Test]
        public void can_read_quoted_target()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/target:\"e","xe\"");
            Assert.AreEqual(@"e xe", arguments.Target);
        }

        [Test]
        public void can_read_quoted_target_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/target:\"exe\"");
            Assert.AreEqual(@"exe", arguments.Target);
        }

        [Test]
        public void can_read_win32_icon()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32icon:foo.ico");
            Assert.AreEqual(@"foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_quoted_icon()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32icon:\"C:\\Program", "Files\\foo.ico");
            Assert.AreEqual(@"C:\Program Files\foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_quoted_icon_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32icon:\"C:\\ProgramFiles\\foo.ico");
            Assert.AreEqual(@"C:\ProgramFiles\foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_win32_resource()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32res:foo.rs");
            Assert.AreEqual(@"foo.rs", arguments.Win32Resource);
        }

        [Test]
        public void can_read_quoted_win32_resource()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32res:\"C:\\Program", "Files\\foo.rs");
            Assert.AreEqual(@"C:\Program Files\foo.rs", arguments.Win32Resource);
        }

        [Test]
        public void can_read_quoted_resource_no_spaces()
        {
            var arguments = CommandLineArgumentsBuilder.BuildFrom("/win32res:\"C:\\ProgramFiles\\foo.rs");
            Assert.AreEqual(@"C:\ProgramFiles\foo.rs", arguments.Win32Resource);
        }
    }
}
