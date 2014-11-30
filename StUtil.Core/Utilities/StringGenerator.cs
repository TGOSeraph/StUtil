using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Utilities
{
    /// <summary>
    /// Class for generating random strings
    /// </summary>
    public class StringGenerator
    {
        /// <summary>
        /// The casing that is allowed
        /// </summary>
        public enum Case
        {
            Both,
            Upper,
            Lower
        }

        /// <summary>
        /// The random number generator
        /// </summary>
        [ThreadStatic]
        private Random random = new Random();

        /// <summary>
        /// Gets or sets the allowed cases.
        /// </summary>
        /// <value>The allow case.</value>
        public Case AllowCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether letters are allowed.
        /// </summary>
        /// <value><c>true</c> if letters are allowed; otherwise, <c>false</c>.</value>
        public bool AllowLetters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether numbers are allowed
        /// </summary>
        /// <value><c>true</c> if numbers are allowed; otherwise, <c>false</c>.</value>
        public bool AllowNumbers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether symbols are allowed.
        /// </summary>
        /// <value><c>true</c> if symbols are allowed; otherwise, <c>false</c>.</value>
        public bool AllowSymbols { get; set; }

        /// <summary>
        /// Gets or sets the letters to use in the random string.
        /// </summary>
        /// <value>The letters.</value>
        public string Letters { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of the string.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the string.
        /// </summary>
        /// <value>The minimum length.</value>
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of letters to use.
        /// </summary>
        /// <value>The minimum letters.</value>
        public int MinLetters { get; set; }

        /// <summary>
        /// Gets or sets the minimum numbers to use.
        /// </summary>
        /// <value>The minimum numbers.</value>
        public int MinNumbers { get; set; }

        /// <summary>
        /// Gets or sets the minimum symbols to use.
        /// </summary>
        /// <value>The minimum symbols.</value>
        public int MinSymbols { get; set; }

        /// <summary>
        /// Gets or sets the numbers to use in the string.
        /// </summary>
        /// <value>The numbers.</value>
        public string Numbers { get; set; }

        /// <summary>
        /// Gets or sets the symbols to use in the string.
        /// </summary>
        /// <value>The symbols.</value>
        public string Symbols { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringGenerator" /> class.
        /// </summary>
        public StringGenerator()
        {
            this.Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.Numbers = "0123456789";
            this.AllowLetters = true;
            this.AllowNumbers = true;
            this.AllowSymbols = false;
            this.AllowCase = Case.Both;
        }

        /// <summary>
        /// Generate a random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GenerateString(int length)
        {
            return GenerateString(length, length);
        }

        /// <summary>
        /// Generate a random string.
        /// </summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string GenerateString(int minLength, int maxLength)
        {
            return new StringGenerator() { MinLength = minLength, MaxLength = maxLength }.Generate();
        }

        /// <summary>
        /// Generates a new random string.
        /// </summary>
        /// <returns></returns>
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

            for (int i = output.Length; i < length; i++)
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