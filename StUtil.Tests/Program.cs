using System;
using System.Collections.Generic;
using System.Linq;
using StUtil.CodeGen;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.Tests
{
    static class Program
    {
        public enum TokenType
        {
            None,
            Function,
            Parameters,
            Parameter,
            ParameterSplitter,
            PopupTimeout,
            SetVariableString,
            SetVariableFunction,
            SetVariableValue
        }
        public class Token
        {
            public TokenType Type { get; set; }
            public string Value { get; set; }
            public Token(string value, TokenType type)
            {
                this.Type = type;
                this.Value = value;
            }
            public override string ToString()
            {
                return "[" + Type.ToString() + "] " + Value;
            }
        }

        static Dictionary<string, string> GetScriptArguments()
        {
            return new Dictionary<string, string>() { 
                {"objecttype", "contact"},
                {"objectid", "1234"}
            };
        }

        static Dictionary<string, Func<string>> GetReplacements()
        {
            return new Dictionary<string, Func<string>>() { 
                {"#", () => 
                    {
                        //lookup window name
                        return "w_...";
                    }
                },
                {"%", () => 
                    {
                        //lookup dw name
                        return "d_...";
                    }
                },
                {"@", () => 
                    {
                        //lookup window name
                        return "active window handle";
                    }
                },
                {"0", () => 
                    {
                        //blank for blank
                        return "";
                    }
                }
            };
        }

        static List<Token> FirstParse(string line)
        {
            string current = "";
            TokenType type = TokenType.None;
            List<Token> tokens = new List<Token>();
            int bracketDepth = 0;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                switch (c)
                {
                    case '(':
                        bracketDepth++;
                        if (type == TokenType.None)
                        {
                            type = TokenType.Function;
                            tokens.Add(new Token(current, type));
                            type = TokenType.Parameters;
                            current = "";
                        }
                        else
                        {
                            current += c;
                        }
                        break;
                    case ')':
                        if (--bracketDepth == 0)
                        {
                            tokens.Add(new Token(current, type));
                            current = "";
                            type = TokenType.None;
                        }
                        else
                        {
                            current += c;
                        }
                        break;
                    case '~':
                        if (current != "")
                        {
                            if (type == TokenType.None)
                            {
                                type = TokenType.Function;
                            }
                            tokens.Add(new Token(current, type));
                        }
                        type = TokenType.None;
                        current = "";
                        break;
                    default:
                        if (type == TokenType.None)
                        {
                            if (c == ':' && line[i + 1] == '=')
                            {
                                type = TokenType.SetVariableString;
                                tokens.Add(new Token(current, type));
                                current = "";
                                type = TokenType.SetVariableValue;
                                i++;
                                break;
                            }
                            else if (c == '@' && line[i + 1] == '=')
                            {
                                type = TokenType.SetVariableFunction;
                                tokens.Add(new Token(current, type));
                                current = "";
                                type = TokenType.SetVariableValue;
                                i++;
                                break;
                            }
                            else if (c == '[' && i == 0)
                            {
                                type = TokenType.ParameterSplitter;
                                tokens.Add(new Token(line[++i].ToString(), type));
                                current = "";
                                type = TokenType.None;
                                i++;
                                break;
                            }
                        }
                        current += c;
                        break;
                }
            }
            if (current.Length > 0)
            {
                if (type == TokenType.None)
                {
                    throw new Exception();
                }
                tokens.Add(new Token(current, type));
            }
            return tokens;
        }

        static bool ParseParameter(ref int vtmpId, string arg, Dictionary<string, string> scriptArgs, List<Token> tokens, int index)
        {
            bool changed = false;

            if (arg.StartsWith("[") && arg.EndsWith("]"))
            {
                changed = true;
                tokens.Insert(index, new Token(scriptArgs[arg.Substring(1, arg.Length - 2)], TokenType.Parameter));
            }
            else if (arg.StartsWith("{") && arg.EndsWith("}"))
            {
                string action = arg.Substring(1, arg.Length - 2);
                if (!action.StartsWith("$"))
                {
                    changed = true;
                    tokens.RemoveAt(index);
                    ParseFunctionParameter(ref vtmpId, action, tokens, index);
                }
            }

            return changed;
        }

        static string GetNextVariableName(ref int vtmpId)
        {
            return "__tmp_" + (++vtmpId).ToString();
        }
        static string GetVariableName(int vtmpId)
        {
            return "__tmp_" + vtmpId.ToString();
        }

        static void ParseFunctionParameter(ref int vtmpId, string action, List<Token> tokens, int index)
        {
            tokens.Insert(index - 1, new Token(GetNextVariableName(ref vtmpId), TokenType.SetVariableFunction));
            tokens.Insert(index, new Token(action, TokenType.SetVariableValue));
            tokens.Insert(index + 2, new Token("{$" + GetVariableName(vtmpId) + "}", TokenType.Parameter));
        }

        static bool ReParse(ref int vtmpId, Dictionary<string, string> scriptArgs, List<Token> tokens)
        {
            bool changed = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];
                switch (token.Type)
                {
                    case TokenType.SetVariableFunction:
                        changed = true;
                        token.Type = TokenType.SetVariableString;
                        token = tokens[++i];
                        string action = token.Value;
                        token.Value = "{^}";
                        foreach (Token t in FirstParse(action))
                        {
                            tokens.Insert(i++ - 1, t);
                        }
                        break;
                    case TokenType.Parameter:
                        if (ParseParameter(ref vtmpId, token.Value, scriptArgs, tokens, i))
                        {
                            changed = true;
                        }
                        break;
                    case TokenType.Parameters:
                        bool split = i - 2 >= 0 && tokens.ElementAt(i - 2).Type == TokenType.ParameterSplitter;
                        char paramsplit = split ? tokens.ElementAt(i - 2).Value[0]
                            : ',';
                        string[] args = token.Value.Split(paramsplit);
                        if (split)
                        {
                            tokens.RemoveAt(i - 2);
                        }
                        changed = true;
                        tokens.Replace(i - 1, args.Select(a => new Token(a, TokenType.Parameter)).ToArray());
                        break;
                }
            }
            return changed;
        }

        [STAThread]
        static void Main()
        {

            StUtil.Native.PE.WindowsPE pe = new Native.PE.WindowsPE(@"C:\Projects\~Other\APIs\StUtil.Git\StUtil.Core\bin\Debug\StUtil.Core.dll");


            Dictionary<string, string> ScriptArguments = GetScriptArguments();
            Dictionary<string, Func<string>> Replacements = GetReplacements();

            string line = "[']A({B()})";
            List<Token> tokens = FirstParse(line);

            int vtmpId = 0;

            bool changed = true;
            while (changed)
            {
                changed = ReParse(ref vtmpId, ScriptArguments, tokens);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
