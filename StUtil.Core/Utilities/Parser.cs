using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StUtil.Utilities
{
    public class Parser
    {
        private string input;
        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                Offset = 0;
            }
        }
        public int Offset { get; private set; }

        public Parser(string input)
        {
            this.Input = input;
        }

        public Parser()
        {
        }

        public char Peek()
        {
            return input[0];
        }

        public string Peek(int count)
        {
            return input.Substring(0, count);
        }

        public bool CheckAhead(string text)
        {
            return Peek(text.Length) == text;
        }

        public char Read()
        {
            return Read(1)[0];
        }

        public string Read(int count)
        {
            string outp = input.Substring(0, count);
            input = input.Substring(count);
            Offset += count;
            return outp;
        }

        public string ReadLine()
        {
            string ret = ReadUntil('\n').TrimEnd('\r');
            Read();
            return ret;
        }

        public string Read(string value)
        {
            string v = Read(value.Length);
            if (v != value)
            {
                throw new FormatException("Expected '" + value + "' found '" + v + "'");
            }
            return v;
        }

        public string ReadLine()
        {
            string ret = ReadUntil('\n').TrimEnd('\r');
            Read();
            return ret;
        }

        public string ReadWhile(Func<char, bool> condition)
        {
            return ReadWhile((c, i) => condition(c));
        }

        public string ReadWhile(Func<int, bool> condition)
        {
            return ReadWhile((c, i) => condition(i));
        }

        public string ReadWhile(Func<char, int, bool> condition)
        {
            string outp = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (!condition(input[i], i))
                {
                    break;
                }
                outp += input[i];
            }
            input = input.Substring(outp.Length);
            Offset += outp.Length;
            return outp;
        }

        public string ReadWhile(params char[] chars)
        {
            return ReadWhile((c, i) => Array.IndexOf(chars, c) != -1);
        }

        public string ReadUntil(Regex pattern)
        {
            return ReadWhile((c, i) => !pattern.IsMatch(input.Substring(i)));
        }

        public string ReadUntil(params char[] chars)
        {
            return ReadWhile((c, i) => Array.IndexOf(chars, c) == -1);
        }

        public string ReadUntil(params string[] words)
        {
            char[] find = words.Select(t => t[0]).Distinct().ToArray();
            string read = "";

            read = ReadUntil(find);
            var match = words.Where(w => w[0] == Peek()).FirstOrDefault(w => CheckAhead(w));
            if (match != null)
            {
                return read;
            }
            else
            {
                return read + Read() + ReadUntil(words);
            }
        }

        public string ReadUntil(Func<char, bool> condition)
        {
            return ReadUntil((c, i) => condition(c));
        }

        public string ReadUntil(Func<int, bool> condition)
        {
            return ReadUntil((c, i) => condition(i));
        }

        public string ReadUntil(Func<char, int, bool> condition)
        {
            return ReadWhile((c, i) => !condition(c, i));
        }

        public string ReadWhitespace()
        {
            return ReadWhile(char.IsWhiteSpace);
        }

        public string ReadToEnd()
        {
            string ret = this.input;
            this.input = "";
            Offset += ret.Length;
            return ret;
        }
    }
}
