using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StUtil.Console
{
    public class ConsoleArgument
    {
        /// <summary>
        /// Set of allowed characters (-/) that an argument may be prefixed with
        /// </summary>
        public static char[] FlagCharacters = { '-', '/' };

        /// <summary>
        /// The name of the argument
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The description of the argument
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// If the argument is required
        /// </summary>
        public bool Required { get; private set; }

        /// <summary>
        /// If the argument has an associated value or if it is just a flag
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// If this argument can be specified multiple times
        /// </summary>
        public bool AllowMultiple { get; private set; }

        /// <summary>
        /// The character the argument should be prefixed with
        /// </summary>
        public char[] AllowedFlagCharacters { get; private set; }

        /// <summary>
        /// The different aliases for this argument e.g. w, wi, width
        /// </summary>
        public string[] Aliases { get; private set; }

        /// <summary>
        /// Convert the input string to the actual value
        /// </summary>
        public Func<string, object> GetValue { get; private set; }

        public ConsoleArgument(string name, string description, bool required, bool hasValue, bool allowMultiple, char[] allowedFlagCharacter, Func<string, object> getValue, params string[] aliases)
        {
            this.Name = name;
            this.Description = description;
            this.Required = required;
            this.HasValue = hasValue;
            this.AllowMultiple = allowMultiple;
            this.AllowedFlagCharacters = allowedFlagCharacter;
            this.GetValue = getValue;
            this.Aliases = aliases == null ? new string[] { name.ToLower().Replace(" ", "") } : aliases;
        }

        public ConsoleArgument(string name, string description, bool required, Func<string, object> getValue, params string[] aliases)
            : this(name, description, required, true, false, FlagCharacters, getValue, aliases)
        {
        }

        public ConsoleArgument(string name, string description, bool required, params string[] aliases)
            : this(name, description, required, true, false, FlagCharacters, s => s, aliases)
        {
        }

        public bool Matches(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            bool startWith = false;
            foreach (char c in AllowedFlagCharacters)
            {
                if (input[0] == c)
                {
                    startWith = true;
                    break;
                }
            }
            if (!startWith) return false;
            input = input.Substring(1);

            foreach (string alias in Aliases)
            {
                if (input.ToLower() == alias.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public static long ConvertInputNumber(string number)
        {
            if (number.StartsWith("0x"))
            {
                return long.Parse(number.Substring(2), NumberStyles.AllowHexSpecifier, null);
            }
            else
            {
                return long.Parse(number);
            }
        }
    }
}
