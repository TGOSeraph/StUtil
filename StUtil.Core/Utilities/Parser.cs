using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Utilities
{
    public class Parser
    {
        public string Input { get; set; }

        public Parser(string input)
        {
            this.Input = input;
        }

        public Parser()
        {
        }

        public char Peek()
        {
            return Input[0];
        }

        public string Peek(int count)
        {
            return Input.Substring(0, count);
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
            string outp = Input.Substring(0, count);
            Input = Input.Substring(count);
            return outp;
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
            for (int i = 0; i < Input.Length; i++)
            {
                if (!condition(Input[i], i))
                {
                    break;
                }
                outp += Input[i];
            }
            Input = Input.Substring(outp.Length);
            return outp;
        }

        public string ReadWhile(params char[] chars)
        {
            return ReadWhile((c, i) => Array.IndexOf(chars, c) != -1);
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
    }
}
