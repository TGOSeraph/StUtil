using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Strings
{
    public sealed class RandomStringGenerator
    {
        public const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numbers = "0123456789";

        public enum Case
        {
            Both,
            Upper,
            Lower
        }

        public string Symbols { get; set; }

        public int MaxLength { get; set; }
        public int MinLength { get; set; }

        public bool AllowLetters { get; set; }
        public bool AllowNumbers { get; set; }
        public bool AllowSymbols { get; set; }

        public Case AllowCase {get; set;}

        public int MinLetters { get; set; }
        public int MinNumbers { get; set; }
        public int MinSymbols { get; set; }

        [ThreadStatic]
        private Random random = new Random();

        public RandomStringGenerator()
        {
            this.AllowLetters = true;
            this.AllowNumbers = true;
            this.AllowSymbols = false;
            this.AllowCase = Case.Both;
        }

        public string Generate()
        {
            string output = string.Empty;
            int length = random.Next(MinLength, MaxLength + 1);
            int minlength = (AllowLetters ? MinLetters : 0) + (AllowNumbers ? MinNumbers : 0) + (AllowSymbols && Symbols.Length > 0 ? MinSymbols : 0);
            if (length < minlength) length = minlength;

            string allowed = string.Empty;

            if (AllowLetters)
            {
                allowed = Letters;
                for (int i = 0; i < MinLetters; i++)
                {
                    if (AllowCase == Case.Both)
                    {
                        if (random.NextDouble() > 0.5)
                        {
                            output += Char.ToLower(Letters[random.Next(0, Letters.Length)]);
                        }
                        else
                        {
                            output += Char.ToUpper(Letters[random.Next(0, Letters.Length)]);
                        }
                    }
                    else if (AllowCase == Case.Upper)
                    {
                        output += Char.ToUpper(Letters[random.Next(0, Letters.Length)]);
                    }
                    else
                    {
                        output += Char.ToLower(Letters[random.Next(0, Letters.Length)]);
                    }
                }
            }

            if (AllowNumbers)
            {
                allowed += Numbers;
                for (int i = 0; i < MinNumbers; i++)
                {
                    output += Numbers[random.Next(0, Numbers.Length)];
                }
            }

            if (AllowSymbols && Symbols.Length > 0)
            {
                allowed += Symbols;
                for (int i = 0; i < MinSymbols; i++)
                {
                    output += Symbols[random.Next(0, Symbols.Length)];
                }
            }

            for (int i = output.Length; i < MaxLength; i++)
            {
                if (AllowCase == Case.Both)
                {
                    if (random.NextDouble() > 0.5)
                    {
                        output += Char.ToLower(allowed[random.Next(0, allowed.Length)]);
                    }
                    else
                    {
                        output += Char.ToUpper(allowed[random.Next(0, allowed.Length)]);
                    }
                }
                else if (AllowCase == Case.Upper)
                {
                    output += Char.ToUpper(allowed[random.Next(0, allowed.Length)]);
                }
                else
                {
                    output += Char.ToLower(allowed[random.Next(0, allowed.Length)]);
                }
            }

            return new string(output.ToCharArray().OrderBy(x => random.Next()).ToArray());
        }
    }
}
