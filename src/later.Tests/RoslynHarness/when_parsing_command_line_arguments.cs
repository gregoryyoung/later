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
            var arguments = CommandLineArgumentsParser.BuildFrom("foo.cs", "foo2.cs");
            Assert.AreEqual(2, arguments.Files.Count());
        }

        [Test]
        public void files_are_kept_in_order()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("foo.cs", "foo2.cs");
            Assert.AreEqual("foo.cs", arguments.Files.First());
            Assert.AreEqual("foo2.cs", arguments.Files.Last());
        }

        [Test]
        public void can_read_a_quoted_file()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("\"My", "foo.cs\"");
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("My foo.cs", arguments.Files.First());
        }

        [Test]
        public void can_read_a_quoted_file_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("\"Myfoo.cs\"");
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("Myfoo.cs", arguments.Files.First());
        }

        [Test]
        public void can_read_a_null_file()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom((string) null);
            Assert.AreEqual(1, arguments.Files.Count());
            Assert.AreEqual("", arguments.Files.First());
        }

        [Test]
        public void can_read_a_short_form_reference()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom(@"/r:C:\foo.dll");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_multiple_short_form_references()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom(@"/r:C:\foo.dll", @"/reference:C:\foo2.dll");
            Assert.AreEqual(2, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
            Assert.AreEqual(@"C:\foo2.dll", arguments.References.Last());
        }

        [Test]
        public void can_read_quoted_short_form_references()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/r:\"C:\\Program", "Files\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\Program Files\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_quoted_short_form_references_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/r:\"C:\\ProgramFiles\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\ProgramFiles\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_a_reference()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom(@"/reference:C:\foo.dll");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_multiple_references()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom(@"/reference:C:\foo.dll", @"/reference:C:\foo2.dll");
            Assert.AreEqual(2, arguments.References.Count());
            Assert.AreEqual(@"C:\foo.dll", arguments.References.First());
            Assert.AreEqual(@"C:\foo2.dll", arguments.References.Last());
        }

        [Test]
        public void can_read_quoted_references()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/reference:\"C:\\Program", "Files\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\Program Files\foo.dll", arguments.References.First());
        }

        [Test]
        public void can_read_quoted_references_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/reference:\"C:\\ProgramFiles\\foo.dll\"");
            Assert.AreEqual(1, arguments.References.Count());
            Assert.AreEqual(@"C:\ProgramFiles\foo.dll", arguments.References.First());
        }


        [Test]
        public void can_read_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/resource:foo.dll");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
        }

        [Test]
        public void can_read_multiple_resources_and_keep_order()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/resource:foo.dll", "/resource:foo2.dll");
            Assert.AreEqual(2, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
            Assert.AreEqual(@"foo2.dll", arguments.Resources.Last());
        }

        [Test]
        public void can_read_quoted_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/resource:\"C:\\Program", "Files\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\Program Files\\foo.cs", arguments.Resources.First());
        }
        [Test]
        public void can_read_quoted_resource_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/resource:\"C:\\ProgramFiles\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\ProgramFiles\\foo.cs", arguments.Resources.First());
        }

        [Test]
        public void can_read_short_form_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/res:foo.dll");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
        }

        [Test]
        public void can_read_multiple_short_form_resources_and_keep_order()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/res:foo.dll", "/resource:foo2.dll");
            Assert.AreEqual(2, arguments.Resources.Count());
            Assert.AreEqual(@"foo.dll", arguments.Resources.First());
            Assert.AreEqual(@"foo2.dll", arguments.Resources.Last());
        }

        [Test]
        public void can_read_quoted_short_form_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/res:\"C:\\Program", "Files\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\Program Files\\foo.cs", arguments.Resources.First());
        }
        [Test]
        public void can_read_quoted_short_form_resource_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/res:\"C:\\ProgramFiles\\foo.cs");
            Assert.AreEqual(1, arguments.Resources.Count());
            Assert.AreEqual("C:\\ProgramFiles\\foo.cs", arguments.Resources.First());
        }


        [Test]
        public void can_read_single_define()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/define:DEBUG");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
        }

        [Test]
        public void can_read_multiple_defines()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/define:DEBUG", "/define:debug2");
            Assert.AreEqual(2, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
            Assert.AreEqual("debug2", arguments.Defines.Last());
        }

        [Test]
        public void can_read_quoted_define()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/define:\"DEBUG", "ME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG ME", arguments.Defines.First());
        }
        [Test]
        public void can_read_quoted_define_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/define:\"DEBUGME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUGME", arguments.Defines.First());
        }


        [Test]
        public void can_read_single_short_form_define()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/d:DEBUG");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
        }

        [Test]
        public void can_read_multiple_short_form_defines()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/d:DEBUG", "/define:debug2");
            Assert.AreEqual(2, arguments.Defines.Count());
            Assert.AreEqual("DEBUG", arguments.Defines.First());
            Assert.AreEqual("debug2", arguments.Defines.Last());
        }

        [Test]
        public void can_read_quoted_short_form_define()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/d:\"DEBUG", "ME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUG ME", arguments.Defines.First());
        }
        [Test]
        public void can_read_quoted_short_form_define_with_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/d:\"DEBUGME\"");
            Assert.AreEqual(1, arguments.Defines.Count());
            Assert.AreEqual("DEBUGME", arguments.Defines.First());
        }

        [Test]
        public void can_read_output()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/out:foo.dll");
            Assert.AreEqual("foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_output_with_quoted_identifier()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/out:\"C:\\Program", "Files\\foo.dll\"");
            Assert.AreEqual("C:\\Program Files\\foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_output_with_quoted_identifier_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/out:\"C:\\ProgramFiles\\foo.dll\"");
            Assert.AreEqual("C:\\ProgramFiles\\foo.dll", arguments.Output);
        }

        [Test]
        public void can_read_file_align()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/filealign:512");
            Assert.AreEqual(512, arguments.Alignment);
            Assert.AreEqual(0, arguments.Errors.Count());
        }

        [Test]
        public void non_integer_value_for_alignment_produces_an_error()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/filealign:GREG");
            Assert.AreEqual(0, arguments.Alignment);
            Assert.AreEqual(1, arguments.Errors.Count());
            Assert.AreEqual("Alignment must be a numeric value", arguments.Errors.First().ErrorText);
        }

        [Test]
        public void can_read_pdb_output_file()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/pdb:Foo.pdb");
            Assert.AreEqual("Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_quoted_pdb_output_file()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/pdb:\"C:\\Program", "Files\\Foo.pdb\"");
            Assert.AreEqual(@"C:\Program Files\Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_quoted_pdb_output_file_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/pdb:\"C:\\ProgramFiles\\Foo.pdb\"");
            Assert.AreEqual(@"C:\ProgramFiles\Foo.pdb", arguments.PdbOutput);
        }

        [Test]
        public void can_read_target()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/target:exe");
            Assert.AreEqual("exe", arguments.Target);
        }

        [Test]
        public void can_read_quoted_target()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/target:\"e","xe\"");
            Assert.AreEqual(@"e xe", arguments.Target);
        }

        [Test]
        public void can_read_quoted_target_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/target:\"exe\"");
            Assert.AreEqual(@"exe", arguments.Target);
        }

        [Test]
        public void can_read_win32_icon()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32icon:foo.ico");
            Assert.AreEqual(@"foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_quoted_icon()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32icon:\"C:\\Program", "Files\\foo.ico");
            Assert.AreEqual(@"C:\Program Files\foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_quoted_icon_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32icon:\"C:\\ProgramFiles\\foo.ico");
            Assert.AreEqual(@"C:\ProgramFiles\foo.ico", arguments.Win32Icon);
        }

        [Test]
        public void can_read_win32_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32res:foo.rs");
            Assert.AreEqual(@"foo.rs", arguments.Win32Resource);
        }

        [Test]
        public void can_read_quoted_win32_resource()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32res:\"C:\\Program", "Files\\foo.rs");
            Assert.AreEqual(@"C:\Program Files\foo.rs", arguments.Win32Resource);
        }

        [Test]
        public void can_read_quoted_resource_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/win32res:\"C:\\ProgramFiles\\foo.rs");
            Assert.AreEqual(@"C:\ProgramFiles\foo.rs", arguments.Win32Resource);
        }

        [Test]
        public void can_read_main_definition()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/main:Foo.Bar.Something");
            Assert.AreEqual(@"Foo.Bar.Something", arguments.Main);
        }

        [Test]
        public void can_read_quoted_main_definition()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/main:\"Foo.Bar", ".Something\"");
            Assert.AreEqual(@"Foo.Bar .Something", arguments.Main);
        }

        [Test]
        public void can_read_quoted_main_definition_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/main:\"Foo.Bar.Something\"");
            Assert.AreEqual(@"Foo.Bar.Something", arguments.Main);
        }

        [Test]
        public void can_read_platform_definition()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/platform:anycpu");
            Assert.AreEqual(@"anycpu", arguments.Platform);
        }

        [Test]
        public void can_read_quoted_platform_definition()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/platform:\"any", "cpu\"");
            Assert.AreEqual(@"any cpu", arguments.Platform);
        }

        [Test]
        public void can_read_quoted_platform_definition_no_spaces()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/platform:\"anycpu\"");
            Assert.AreEqual(@"anycpu", arguments.Platform);
        }

        [Test]
        public void can_read_optimize_enabled()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/optimize+");
            Assert.IsTrue(arguments.Optimize);
        }

        [Test]
        public void can_read_optimize_enabled_no_plus()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/optimize");
            Assert.IsTrue(arguments.Optimize);
        }

        [Test]
        public void if_optimize_off_then_optimize_on_it_will_be_on()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/optimize-", "/optimize+");
            Assert.IsTrue(arguments.Optimize);
        }

        [Test]
        public void if_optimize_on_then_optimize_off_it_will_be_off()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/optimize+", "/optimize-");
            Assert.IsFalse(arguments.Optimize);
        }

        [Test]
        public void can_read_checked_enabled()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/checked+");
            Assert.IsTrue(arguments.Checked);
        }

        [Test]
        public void can_read_checked_enabled_no_plus()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/checked");
            Assert.IsTrue(arguments.Checked);
        }

        [Test]
        public void if_checked_off_then_checked_on_it_will_be_on()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/checked-", "/checked+");
            Assert.IsTrue(arguments.Checked);
        }

        [Test]
        public void if_checked_on_then_checked_off_it_will_be_off()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/checked+", "/checked-");
            Assert.IsFalse(arguments.Checked);
        }

        [Test]
        public void defaults_to_no_debug()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom();
            Assert.AreEqual("NONE", arguments.DebugLevel);
        }

        [Test]
        public void can_read_debug_with_no_parameter()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug");
            Assert.AreEqual("FULL", arguments.DebugLevel);
        }

        [Test]
        public void can_read_debug_with_plus()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug+");
            Assert.AreEqual("FULL", arguments.DebugLevel);
        }

        [Test]
        public void can_read_debug_with_minus()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug-");
            Assert.AreEqual("NONE", arguments.DebugLevel);
        }

        [Test]
        public void when_on_then_off_debug_is_off()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug+", "/debug-");
            Assert.AreEqual("NONE", arguments.DebugLevel);
        }

        [Test]
        public void when_off_then_on_debug_is_on()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug-", "/debug+");
            Assert.AreEqual("FULL", arguments.DebugLevel);
        }

        [Test]
        public void when_pdb_only_then_full_is_full()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug:pdbonly", "/debug+");
            Assert.AreEqual("FULL", arguments.DebugLevel);
        }

        [Test]
        public void when_full_then_pdb_only_is_pdb_only()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug+", "/debug:pdbonly");
            Assert.AreEqual("PDBONLY", arguments.DebugLevel);
        }

        [Test]
        public void can_ready_pdbonly()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/debug:pdbonly");
            Assert.AreEqual("PDBONLY", arguments.DebugLevel);
        }

        [Test]
        public void single_warning_filter_can_be_read()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nowarn:1701");
            Assert.AreEqual(1, arguments.WarningFilters.Count());
            Assert.AreEqual("1701", arguments.WarningFilters.First());
        }

        [Test]
        public void multiple_warning_filters_can_be_read_comma_separated()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nowarn:1701,1702");
            Assert.AreEqual(2, arguments.WarningFilters.Count());
            Assert.AreEqual("1701", arguments.WarningFilters.First());
            Assert.AreEqual("1702", arguments.WarningFilters.Last());
        }

        [Test]
        public void multiple_warning_filters_can_be_read_semicolon_separated()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nowarn:1701;1702");
            Assert.AreEqual(2, arguments.WarningFilters.Count());
            Assert.AreEqual("1701", arguments.WarningFilters.First());
            Assert.AreEqual("1702", arguments.WarningFilters.Last());
        }

        [Test]
        public void multiple_warning_filters_can_be_put_on_command_line()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nowarn:1701", "/nowarn:1702");
            Assert.AreEqual(2, arguments.WarningFilters.Count());
            Assert.AreEqual("1701", arguments.WarningFilters.First());
            Assert.AreEqual("1702", arguments.WarningFilters.Last());
        }

        [Test]
        public void can_handle_nostdlib_on()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nostdlib");
            Assert.IsTrue(arguments.NoStdLib);
        }

        [Test]
        public void can_handle_nostdlib_on_with_plus()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nostdlib+");
            Assert.IsTrue(arguments.NoStdLib);
        }

        [Test]
        public void nostdlib_on_then_off_means_its_off()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/nostdlib+", "/nostdlib-");
            Assert.IsFalse(arguments.NoStdLib);
        }

        [Test]
        public void should_read_rsp_config_by_default()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom();
            Assert.IsFalse(arguments.NoConfig);
        }

        [Test]
        public void can_read_no_config()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/noconfig");
            Assert.IsTrue(arguments.NoConfig);
        }

        [Test]
        public void can_read_warning_level()
        {
            var arguments = CommandLineArgumentsParser.BuildFrom("/warn:2");
            Assert.AreEqual(2, arguments.WarnLevel);
        }
    }
}
