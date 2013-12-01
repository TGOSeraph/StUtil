using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Console
{
    public class ConsoleArgsParser
    {
        public List<string> LastUnmatched { get; private set; }
        public ConsoleArgument[] Arguments { get; private set; }

        public ConsoleArgsParser(params ConsoleArgument[] arguments)
        {
            this.Arguments = arguments;
        }

        private void CheckStoredValue(ref string current, Dictionary<ConsoleArgument, object> values, List<ConsoleArgument> args)
        {
            if (LastUnmatched.Count > 0)
            {
                foreach (ConsoleArgument arg in args)
                {
                    if (arg.Matches(LastUnmatched.Last()))
                    {
                        LastUnmatched.RemoveAt(LastUnmatched.Count - 1);
                        if (arg.HasValue)
                        {
                            values.Add(arg, arg.GetValue(current));
                            if (!arg.AllowMultiple)
                            {
                                current = "";
                                args.Remove(arg);
                                return;
                            }
                        }
                    }
                }
            }
            LastUnmatched.Add(current);
            current = "";
        }

        public Dictionary<ConsoleArgument, object> Parse(string[] input)
        {
            LastUnmatched = new List<string>();

            Dictionary<ConsoleArgument, object> values = new Dictionary<ConsoleArgument, object>();
            List<ConsoleArgument> currentArgs = Arguments.ToList();
            string prev = input[0];
            for (int i = 1; i < input.Length; i++)
            {
                CheckStoredValue(ref prev, values, currentArgs);
                prev = input[i];
            }
            CheckStoredValue(ref prev, values, currentArgs);

            return values;
        }

        public Dictionary<ConsoleArgument, object> Parse(string input)
        {
            LastUnmatched = new List<string>();

            Dictionary<ConsoleArgument, object> values = new Dictionary<ConsoleArgument, object>();
            List<ConsoleArgument> currentArgs = Arguments.ToList();

            char inString = '\0';
            string current = "";
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (inString != '\0')
                {
                    if (c == inString)
                    {
                        inString = '\0';
                        CheckStoredValue(ref current, values, currentArgs);
                    }
                }
                else
                {
                    if (c == '\'' || c == '"')
                    {
                        inString = c;
                        CheckStoredValue(ref current, values, currentArgs);
                    }
                    else if (c == ' ')
                    {
                        CheckStoredValue(ref current, values, currentArgs);
                    }
                    else
                    {
                        current += c;
                    }
                }
            }

            CheckStoredValue(ref current, values, currentArgs);

            foreach (ConsoleArgument arg in Arguments)
            {
                if (arg.Required && !values.Any(v => v.Key == arg))
                {
                    throw new KeyNotFoundException("Required argument '" + arg.Name + "' not found");
                }
            }

            return values;
        }
    }

}
