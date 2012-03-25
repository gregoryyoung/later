using System;
using System.Collections.Generic;

namespace later.RoslynHarness
{
    public class CommandLineArgumentsBuilder
    {
        private const string REFERENCEARG = "/reference:";
        private const string RESOURCEARG = "/resource:";
        private const string DEFINEARG = "/define:";
        private const string OUTPUTARG = "/output:";
        private const string ALIGNARG = "/align:";
        private const string PDBARG = "/pdb:";
        private const string TARGETARG = "/target:";
        private const string WIN32ICONARG = "/win32icon:";
        private const string WIN32RESOURCEARG = "/win32res:";
        private const string NOARG = null;

        public static CompilerArguments BuildFrom(params string[] args)
        {
            return BuildFrom((IEnumerable<string>) args);
        }
        public static CompilerArguments BuildFrom(IEnumerable<string> args)
        {
            var ret = new CompilerArgumentsInternal();
            var pieces = new Queue<string>(args);
            while(pieces.Count > 0)
            {
                var current = pieces.Dequeue() ?? "";
                var lower = current.ToLower();
                if(lower.StartsWith(REFERENCEARG))
                {
                    ret.AddReference(GetPossiblyQuotedString(current, pieces, REFERENCEARG));
                }
                else if (lower.StartsWith(RESOURCEARG))
                {
                    ret.AddResource(GetPossiblyQuotedString(current, pieces, RESOURCEARG));
                }
                else if (lower.StartsWith(DEFINEARG))
                {
                    ret.AddDefine(GetPossiblyQuotedString(current, pieces, DEFINEARG));
                }
                else if (lower.StartsWith(OUTPUTARG))
                {
                    ret.Output = GetPossiblyQuotedString(current, pieces, OUTPUTARG);
                }
                else if (lower.StartsWith(PDBARG))
                {
                    ret.PdbOutput = GetPossiblyQuotedString(current, pieces, PDBARG);
                }
                else if (lower.StartsWith(ALIGNARG))
                {
                    int align = 0;
                    if(!int.TryParse(GetPossiblyQuotedString(current, pieces, ALIGNARG), out align))
                    {
                        ret.AddError(new ParsingError("Alignment must be a numeric value"));
                    }
                    ret.Alignment = align;
                }
                else if (lower.StartsWith(TARGETARG))
                {
                    ret.Target = GetPossiblyQuotedString(current, pieces, TARGETARG);
                }
                else if (lower.StartsWith(WIN32ICONARG))
                {
                    ret.Win32Icon = GetPossiblyQuotedString(current, pieces, WIN32ICONARG);
                }
                else if (lower.StartsWith(WIN32RESOURCEARG))
                {
                    ret.Win32Resource = GetPossiblyQuotedString(current, pieces, WIN32RESOURCEARG);
                }
                else
                {
                    ret.AddFile(GetPossiblyQuotedString(current, pieces, NOARG));
                }
            }
            return ret.ToCompilerArguments();
        }

        private static string GetPossiblyQuotedString(string first, Queue<string> args, string named)
        {
            if (first == null) return "";
            if(named != null)
                first = first.Replace(named, "");
            if(!first.Contains("\""))
            {
                return first;
            }
            var firstidx = first.IndexOf('\"');
            var lastidx = first.LastIndexOf('\"');
            if (firstidx != lastidx && lastidx != -1) return first.Replace("\"", "");
            var finished = false;
            var ret = first.Replace("\"", "");
            while(!finished && args.Count > 0)
            {
                var current = args.Dequeue() ?? "";
                finished = current.Contains("\"");
                ret += " " + current.Replace("\"", "");
            }
            return ret;
        }
    }

    public class ParsingError
    {
        public readonly string ErrorText;

        public ParsingError(string errorText)
        {
            ErrorText = errorText;
        }
    }

    class CompilerArgumentsInternal
    {
        private readonly List<string> _files = new List<string>();
        private readonly List<String> _references = new List<string>();
        private readonly List<String> _resources = new List<string>();
        private readonly List<String> _defines = new List<string>();
        private readonly List<ParsingError> _errors = new List<ParsingError>(); 
        public string Output;
        public int Alignment;
        public string PdbOutput;
        public string Target;
        public string Win32Icon;
        public string Win32Resource;

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
            return new CompilerArguments(_files.ToArray(), _references.ToArray(), _resources.ToArray(), _defines.ToArray(), Output, Alignment, _errors.ToArray(), PdbOutput, Target, Win32Icon, Win32Resource);
        }
    }

    public class CompilerArguments
    {
        private readonly string[] _files; 
        private readonly string[] _references;
        private readonly string[] _resources;
        private readonly string[] _defines;
        private readonly ParsingError[] _errors;
        private readonly string _output;
        private readonly int _alignment;
        private readonly string _pdbOutput;
        private readonly string _target;
        private readonly string _win32Icon;
        private readonly string _win32Resource;
        public CompilerArguments(string[] files, string[] references, string[] resources, string [] defines, 
                                 string output, int alignment, ParsingError[] errors, string pdbOutput, 
                                 string target, string win32Icon, string win32Resource)
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
    }
}
