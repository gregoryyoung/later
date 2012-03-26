using System.Collections.Generic;

namespace later.RoslynHarness
{
    class CompilerArgumentsInternal
    {
        private readonly List<string> _files = new List<string>();
        private readonly List<string> _references = new List<string>();
        private readonly List<string> _resources = new List<string>();
        private readonly List<string> _defines = new List<string>();
        private readonly List<ParsingError> _errors = new List<ParsingError>();
        private readonly List<string> _warningFilter = new List<string>();
        public string Output;
        public int Alignment;
        public string PdbOutput;
        public string Target;
        public string Win32Icon;
        public string Win32Resource;
        public bool Optimize;
        public string Main;
        public string Platform;
        public bool IsChecked;
        public string DebugLevel = "NONE";
        public int WarningLevel;
        public bool NoStdLib;
        public bool NoConfig;

        internal void AddFile(string filename)
        {
            _files.Add(filename);
        }

        internal void AddError(ParsingError error)
        {
            _errors.Add(error);
        }

        internal void AddReference(string reference)
        {
            _references.Add(reference);
        }

        internal void AddWarningFilter(string warningFilter)
        {
            _warningFilter.Add(warningFilter);
        }

        internal void AddWarningFilter(IEnumerable<string> warningFilter)
        {
            _warningFilter.AddRange(warningFilter);
        }

        internal void AddResource(string resource)
        {
            _resources.Add(resource);
        }
        internal void AddDefine(string define)
        {
            _defines.Add(define);
        }
        public CompilerArguments ToCompilerArguments()
        {
            return new CompilerArguments(_files.ToArray(), _references.ToArray(), _resources.ToArray(), _defines.ToArray(), Output,
                                        Alignment, _errors.ToArray(), PdbOutput, Target, Win32Icon, Win32Resource,
                                        Optimize, Platform, Main, IsChecked, DebugLevel, WarningLevel, _warningFilter.ToArray(),
                                        NoStdLib, NoConfig);
        }
    }
}