using System.Collections.Generic;

namespace later.RoslynHarness
{
    public class CompilerArguments
    {
        private readonly string[] _files; 
        private readonly string[] _references;
        private readonly string[] _resources;
        private readonly string[] _defines;
        private readonly string[] _warningFilters;
        private readonly ParsingError[] _errors;
        private readonly string _output;
        private readonly int _alignment;
        private readonly string _pdbOutput;
        private readonly string _target;
        private readonly string _win32Icon;
        private readonly string _win32Resource;
        private readonly bool _optimize;
        private readonly string _platform;
        private readonly string _main;
        private readonly bool _checked;
        private readonly string _debugLevel;
        private readonly int _warnLevel;
        private readonly bool _nostdlib;
        private readonly bool _noConfig;

        public CompilerArguments(string[] files, string[] references, string[] resources, string[] defines, string output, int alignment,
                                ParsingError[] errors, string pdbOutput, string target, string win32Icon, string win32Resource, 
                                bool optimize, string platform, string main, bool ischecked, string debugLevel, int warnLevel, 
                                string[] warningFilters, bool nostdlib, bool noConfig)
        {
            _files = files;
            _references = references;
            _resources = resources;
            _defines = defines;
            _output = output;
            _alignment = alignment;
            _errors = errors;
            _pdbOutput = pdbOutput;
            _target = target;
            _win32Icon = win32Icon;
            _win32Resource = win32Resource;
            _optimize = optimize;
            _platform = platform;
            _main = main;
            _checked = ischecked;
            _debugLevel = debugLevel;
            _warnLevel = warnLevel;
            _warningFilters = warningFilters;
            _nostdlib = nostdlib;
            _noConfig = noConfig;
        }

        public int WarnLevel
        {
            get { return _warnLevel; }
        }

        public string DebugLevel
        {
            get { return _debugLevel; }
        }

        public string Main
        {
            get { return _main; }
        }

        public string Platform
        {
            get { return _platform; }
        }

        public bool Optimize
        {
            get { return _optimize; }
        }

        public IEnumerable<string> Files
        {
            get { return _files; }
        }

        public IEnumerable<string> References
        {
            get { return _references; }
        }

        public IEnumerable<string> Resources
        {
            get { return _resources; }
        }

        public IEnumerable<string> Defines
        {
            get { return _defines; }
        }
        
        public IEnumerable<ParsingError> Errors
        {
            get { return _errors; }
        } 

        public string Output
        {
            get { return _output; }
        }

        public int Alignment
        {
            get { return _alignment; }
        }

        public string PdbOutput
        {
            get { return _pdbOutput; }
        }

        public string Target { get { return _target; } }

        public string Win32Icon
        {
            get { return _win32Icon; }
        }

        public string Win32Resource
        {
            get { return _win32Resource; }
        }

        public bool Checked
        {
            get { return _checked; }
        }

        public IEnumerable<string> WarningFilters
        {
            get { return _warningFilters; }
        }

        public bool NoStdLib
        {
            get { return _nostdlib; }
        }

        public bool NoConfig
        {
            get { return _noConfig; }
        }
    }
}