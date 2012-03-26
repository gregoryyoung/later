using System.Collections.Generic;
using later.RoslynHarness;

namespace later.Tests.RoslynHarness
{
    class CompilerArgumentBuilder
    {
        public List<string> Files = new List<string>();
        public List<string> References = new List<string>();
        public List<string> Resources = new List<string>();
        public List<string> Defines = new List<string>();
        public List<string> WarningFilters = new List<string>(); 
        public List<ParsingError> Errors = new List<ParsingError>();
        public string Output;
        public int Alignment;
        public string PdbOutput;
        public string Target = "";
        public string Win32Icon;
        public string Win32Resource;
        public bool Optimize;
        public string Platform;
        public string Main;
        public bool IsChecked; 
        public string DebugLevel = "NONE";
        public int WarningLevel;
        public bool NoStdLib;
        public bool NoConfig;

        public CompilerArguments ToCompilerArguments()
        {
            return new CompilerArguments(Files.ToArray(), References.ToArray(), Resources.ToArray(), Defines.ToArray(), Output,
                                         Alignment, Errors.ToArray(), PdbOutput, Target, Win32Icon, Win32Resource, Optimize,
                                         Platform, Main, IsChecked, DebugLevel, WarningLevel, WarningFilters.ToArray(),
                                         NoStdLib, NoConfig);
        }
    }

}
