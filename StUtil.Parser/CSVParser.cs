using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Parser
{
    public class CSVParser : StUtil.Parser.BoundedStringParser<List<string>>
    {
        public string Delim { get; set; }

        public CSVParser(string delim = ",")
        {
            this.Delim = delim;
        }

        public override char? HandleCharacter(char c)
        {
            char? nc = base.HandleCharacter(c);
            if (nc.HasValue)
            {
                if (base.CurrentBounding == null)
                {
                    if (nc.Value == Delim[0])
                    {
                        bool matches = true;
                        for (int i = 1; i < Delim.Length; i++)
                        {
                            if (NextCharacter(i + 1, false) != Delim[i])
                            {
                                matches = false;
                                break;
                            }
                        }
                        if (matches)
                        {
                            ConsumeCharacters(Delim.Length - 1);
                            StoreCurrentToken();
                            AppendToCurrentToken(nc.Value);
                            StoreCurrentToken("DELIM");
                            return null;
                        }
                    }
                }
            }

            return nc;
        }

        public override List<string> GetResults()
        {
            List<string> results = new List<string>();
            string current = "";
            foreach (StUtil.Parser.Token token in Tokens)
            {
                if (token.Type == "DELIM")
                {
                    results.Add(current);
                    current = "";
                }
                else
                {
                    current += token.Value;
                }
            }
            results.Add(current.TrimStart());
            return results;
        }
    }
}
