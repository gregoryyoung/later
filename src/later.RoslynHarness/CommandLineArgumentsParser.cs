using System.Collections.Generic;

namespace later.RoslynHarness
{
    public class CommandLineArgumentsParser
    {
        private const string REFERENCEARG = "/reference:";
        private const string REFERENCEARGSHORT = "/r:";
        private const string RESOURCEARG = "/resource:";
        private const string RESOURCEARGSHORT = "/res:";
        private const string DEFINEARG = "/define:";
        private const string DEFINEARGSHORT = "/d:";
        private const string OUTPUTARG = "/out:";
        private const string ALIGNARG = "/filealign:";
        private const string PDBARG = "/pdb:";
        private const string TARGETARG = "/target:";
        private const string WIN32ICONARG = "/win32icon:";
        private const string WIN32RESOURCEARG = "/win32res:";
        private const string MAINARG = "/main:";
        private const string PLATFORMARG = "/platform:";
        private const string OPTIMIZEARG = "/optimize";
        private const string NOSTDLIBARG = "/nostdlib";
        private const string CHECKEDARG = "/checked";
        private const string DEBUGARG = "/debug";
        private const string NOWARNARG = "/nowarn:";
        private const string NOCONFIGARG = "/noconfig";
        private const string WARNLEVELARG = "/warn:";
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
                if(lower.StartsWith(REFERENCEARG) || lower.StartsWith(REFERENCEARGSHORT))
                {
                    ret.AddReference(GetPossiblyQuotedString(current, pieces));
                }
                else if (lower.StartsWith(RESOURCEARG) || lower.StartsWith(RESOURCEARGSHORT))
                {
                    ret.AddResource(GetPossiblyQuotedString(current, pieces));
                }
                else if (lower.StartsWith(DEFINEARG) || lower.StartsWith(DEFINEARGSHORT))
                {
                    ret.AddDefine(GetPossiblyQuotedString(current, pieces));
                }
                else if (lower.StartsWith(OUTPUTARG))
                {
                    ret.Output = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(PDBARG))
                {
                    ret.PdbOutput = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(ALIGNARG))
                {
                    int align = 0;
                    if(!int.TryParse(GetPossiblyQuotedString(current, pieces), out align))
                    {
                        ret.AddError(new ParsingError("Alignment must be a numeric value"));
                    }
                    ret.Alignment = align;
                }
                else if (lower.StartsWith(WARNLEVELARG))
                {
                    int level = 0;
                    if(!int.TryParse(GetPossiblyQuotedString(current, pieces), out level))
                    {
                        ret.AddError(new ParsingError("Warning Level must be a numeric value"));
                    }
                    ret.WarningLevel = level;
                }
                else if (lower.StartsWith(TARGETARG))
                {
                    ret.Target = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(WIN32ICONARG))
                {
                    ret.Win32Icon = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(WIN32RESOURCEARG))
                {
                    ret.Win32Resource = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(MAINARG))
                {
                    ret.Main = GetPossiblyQuotedString(current, pieces);
                }
                else if (lower.StartsWith(PLATFORMARG))
                {
                    ret.Platform = GetPossiblyQuotedString(current, pieces);
                }
                else if(lower.StartsWith(OPTIMIZEARG))
                {
                    ret.Optimize = lower.Contains("+") || lower.Equals(OPTIMIZEARG);
                }
                else if (lower.StartsWith(CHECKEDARG))
                {
                    ret.IsChecked = lower.Contains("+") || lower.Equals(CHECKEDARG);
                }
                else if (lower.StartsWith(NOWARNARG))
                {
                    ret.AddWarningFilter(GetPossiblyQuotedString(current, pieces).Split(',', ';'));
                }
                else if (lower.StartsWith(NOSTDLIBARG))
                {
                    ret.NoStdLib = lower.Contains("+") || lower.Equals(NOSTDLIBARG);
                }
                else if (lower.StartsWith(NOCONFIGARG))
                {
                    ret.NoConfig = true;
                }
                else if (lower.StartsWith(DEBUGARG))
                {
                    if (lower.Contains("+") || lower.Equals(DEBUGARG))
                    {
                        ret.DebugLevel = "FULL";
                    }
                    else if (lower.Equals(DEBUGARG + "-"))
                    {
                        ret.DebugLevel = "NONE";
                    } else if(lower.Contains(":"))
                    {
                        ret.DebugLevel = GetPossiblyQuotedString(current, pieces).ToUpper();
                    }
                }
                else
                {
                    ret.AddFile(GetPossiblyQuotedString(current, pieces));
                }
            }
            return ret.ToCompilerArguments();
        }

        private static string GetPossiblyQuotedString(string first, Queue<string> args)
        {
            if (first == null) return "";
            if(first.Contains("/"))
            {
                var colon = first.IndexOf(":");
                first = first.Substring(colon + 1, first.Length - colon -1);
            }
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
}
