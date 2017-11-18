using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fluffy.Ini
{
    public class FluffyReader
    {
        private const string SECTION_REGEX = @"\[(.*)\]";
        private const string ATTRIBUTE_REGEX = @"(.*)=(.*)";

        public bool TryGetSection(string line, out string value)
        {
            Match sectionMatch = Regex.Match(line, SECTION_REGEX);
            value = null;

            if (!sectionMatch.Success)
                return false;

            value = sectionMatch?.Groups[1].Value;
            return true;
        }

        public bool IsSection(string line)
        {
            return Regex.IsMatch(line, SECTION_REGEX);
        }

        public IEnumerable<KeyValuePair<string, string>> GetAttributes(string line)
        {
            MatchCollection matches = Regex.Matches(line, ATTRIBUTE_REGEX);
            foreach (Match att in matches)
            {
                if (att.Groups.Count != 3)
                    throw new Exception($"Expected 3 groups, got {att.Groups.Count}");

                yield return new KeyValuePair<string, string>(att.Groups[1].Value, att.Groups[2].Value);
            }
        }
    }
}
