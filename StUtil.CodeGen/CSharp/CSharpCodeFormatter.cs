using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CSharp
{
    public class CSharpCodeFormatter : IDotNetCodeFormatter
    {
        public string Format(string code)
        {
            int depth = 0;

            string str = "";
            char instring = '\0';
            int inescapedstring = 0;
            char lastChar = '\0';
            for (int i = 0; i < code.Length; i++)
            {
                bool added = false;
                char c = code[i];
                switch (c)
                {
                    case '\'':
                        if (instring != '\0')
                        {
                            if (lastChar != '\\')
                            {
                                instring = '\0';
                            }
                        }
                        else
                        {
                            instring = c;
                        }
                        break;
                    case '"':
                        if (instring != '\0')
                        {
                            if (inescapedstring > 0)
                            {
                                inescapedstring++;
                                if (inescapedstring % 2 == 0 && code[i + 1] != c)
                                {
                                    inescapedstring = 0;
                                    instring = '\0';
                                }
                            }
                            else
                            {
                                if (lastChar != '\\')
                                {
                                    instring = '\0';
                                }
                            }
                        }
                        else
                        {
                            if (lastChar == '@')
                            {
                                inescapedstring = 1;
                            }
                            instring = c;
                        }
                        break;
                    case '{':
                        if (instring == '\0')
                        {
                            depth += 1;
                        }
                        break;
                    case '}':
                        if (instring == '\0')
                        {
                            depth -= 1;
                            str = str.Substring(0, str.Length - 1);
                        }
                        break;
                    case '\n':
                        if (code[i + 1] != '\r')
                        {
                            added = true;
                            str += c + "".PadLeft(depth, '\t');
                        }
                        break;
                    case '\r':
                        added = true;
                        str += c + "".PadLeft(depth, '\t');
                        break;
                }
                if (!added)
                {
                    str += c;
                }
            }

            return str;
        }
    }
}
