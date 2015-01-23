using System.Linq;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using StUtil.Native.Hook;
using System.Collections.Generic;
using StUtil.Utilities;

namespace StUtil.Dev.ConsoleTest
{
    public class Program
    {
        class WindowSelector
        {

        }
        public static void Main(string[] args)
        {
            string selector = "html:5>";

            CssSelectorParser.Parse(selector);

        }

        public class CssSelectableNode
        {
            public CssSelectableNode Parent { get; set; }
            public string Id { get; set; }
            public string Class { get; set; }
            public List<CssSelectableNode> Children { get; set; }
            public int Index
            {
                get
                {
                    return Parent == null
                        ? 0
                        : Parent.Children.IndexOf(this);
                }
            }

            public int NumberOfSiblings
            {
                get
                {
                    return this.Parent == null
                        ? 0
                        : Parent.Children.Count - 1;
                }
            }

            public CssSelectableNode NextSibling
            {
                get
                {
                    return this.Parent == null
                        ? null
                        : (this.Parent.Children.Count == Index + 1
                            ? null
                            : this.Parent.Children[Index + 1]);
                }
            }

            public CssSelectableNode PreviousSibling
            {
                get
                {
                    return this.Parent == null
                        ? null
                        : (this.Index == 0
                            ? null
                            : this.Parent.Children[Index - 1]);
                }
            }

        }

       

        public class CssSelectorParser
        {
            public static CssSelectorParseTree Parse(string selector)
            {
                List<CssSelectorNode> nodes = new List<CssSelectorNode>();
                CssSelectorParseTree parsed = new CssSelectorParseTree(nodes);
                Parser parser = new Parser { Input = selector };

                while (parser.Input.Length > 0)
                {
                    string read = parser.ReadUntil('>', ' ', '+');

                    if (parser.Input.Length > 0)
                    {
                        parser.Read(1);
                        parser.ReadWhile(' ', '\t');
                    }
                    if (read.Length == 0) continue;

                    if (read.Contains(':'))
                    {
                        //Handle pseudoclass
                        Parser pseudoParser = new Parser() { Input = read };
                        read = pseudoParser.ReadUntil(':');
                        pseudoParser.Read(1);
                        string type = pseudoParser.ReadUntil('(');
                        string args = null;
                        if (pseudoParser.Input.Length > 0)
                        {
                            pseudoParser.Read(1);
                            args = pseudoParser.ReadUntil(')');
                            pseudoParser.Read(1);
                        }
                        nodes.Add(new PseudoCssSelector(read, type, args));
                    }
                    else
                    {
                        nodes.Add(new CoreCssSelector(read));
                    }
                }

                return parsed;
            }
        }

        public class CssSelectorParseTree
        {
            public IEnumerable<CssSelectorNode> Nodes { get; private set; }
            public CssSelectorParseTree(List<CssSelectorNode> nodes)
            {
                this.Nodes = nodes;
            }
        }

        public abstract class CssSelectorNode
        {
        }

        public class CoreCssSelector : CssSelectorNode
        {
            public string Class { get; set; }
            public string Id { get; set; }

            public CoreCssSelector(string selector)
            {
                Id = Class = "";

                bool id = false;
                foreach (char c in selector)
                {
                    if (c == '.')
                    {
                        id = true;
                    }
                    else if (c == '#')
                    {
                        id = false;
                    }
                    else
                    {
                        if (id)
                        {
                            Id += c;
                        }
                        else
                        {
                            Class += c;
                        }
                    }
                }
            }
        }
        public class PseudoCssSelector : CoreCssSelector
        {
            public string PseudoClass { get; set; }
            public string PseudoArgs { get; set; }

            public PseudoCssSelector(string selector, string pseudoClass, string pseudoArgs)
                : base(selector)
            {
                this.PseudoArgs = pseudoArgs;
                this.PseudoClass = pseudoClass;
            }
        }

    }
}