using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Class for parsing and formatting people names
    /// </summary>
    public class PersonName
    {
        // Setup regular expression to find prefixes.
        static Regex prefixRegex = new Regex(@"\b(mr\.? |mrs\.? |miss |ms\.? |dr\.? |sgt\.? )", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Setup regular expression to find suffixes.
        static Regex suffixRegex = new Regex(@"([ ]*(,|-)[ ]*)?(?<suffix> i| ii| iii| iv| jr\.?| sr\.?| esq\.?| phd\.?| md| m\.d\.| dvm| d\.v\.m\.)($|\.)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // for testing business suffixes
        static Regex businessSuffixRegex = new Regex(@"([ ]*(,|-)[ ]*)?(?<suffix> corp\.?| llc\.?| inc\.?| ltd\.?| co\.?)($|\.)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // for counting words
        static Regex wordCountRegex = new Regex(@"\b\w", RegexOptions.Compiled);

        // Split the name into substrings.
        static Regex spacesRegex = new Regex("[ ]+", RegexOptions.Compiled);

        // the status of how/if the name was parsed
        PersonNameParseStatus parseStatus = PersonNameParseStatus.Unknown;

        string prefix = "";
        string first = "";
        string middle = "";
        string last = "";
        string suffix = "";
        string unparsed = "";

        /// <summary>
        /// Default constructor
        /// </summary>
        public PersonName()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonName(string first, string middle, string last, string unparsed, PersonNameParseStatus parseStatus)
        {
            this.first = first;
            this.middle = middle;
            this.last = last;
            this.parseStatus = parseStatus;
            this.unparsed = unparsed;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonName(string first, string middle, string last)
            : this(first, middle, last, "", PersonNameParseStatus.Simple)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonName(PersonAdapter person)
            : this(person.FirstName, person.MiddleName, person.LastName, person.UnparsedName, person.NameParseStatus)
        {

        }

        /// <summary>
        /// Returns the number of words in the input string
        /// </summary>
        private static int GetWordCount(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }
            else
            {
                return wordCountRegex.Matches(input).Count;
            }
        }

        /// <summary>
        /// Parses a string to a FullName object
        /// </summary>
        public static PersonName Parse(string input)
        {
            PersonName personName = new PersonName();
            personName.unparsed = input;

            // Blank name
            if (string.IsNullOrWhiteSpace(input))
            {
                return personName;
            }

            input = input.Trim();

            // number of commas involved
            int commaCount = input.Count(c => c == ',');

            // Handle commas
            if (commaCount == 1)
            {
                string[] parts = input.Split(',');

                // if there are multiple words on both sides of the comma
                if (GetWordCount(parts[0]) > 1 && GetWordCount(parts[1]) > 1)
                {
                    personName.parseStatus = PersonNameParseStatus.Unparsed;
                }
            }
            else if (commaCount > 1)
            {
                personName.parseStatus = PersonNameParseStatus.Unparsed;
            }

            TryParseCompanyName(input, personName);

            if (personName.parseStatus == PersonNameParseStatus.Unknown)
            {
                // strip out the suffix first if it is there
                Match suffixMatch = suffixRegex.Match(input);
                if (suffixMatch.Success)
                {
                    // Save the suffix.
                    personName.suffix = suffixMatch.Groups["suffix"].Value.Trim();

                    // Remove the suffix from the name.
                    input = input.Remove(suffixMatch.Index, suffixMatch.Length);

                    // update the comma count
                    commaCount = input.Count(c => c == ',');
                }

                // Flip for comma
                if (commaCount == 1)
                {
                    string[] parts = input.Split(',');
                    input = parts[1] + " " + parts[0];
                }

                // Find a prefix
                Match prefixMatch = prefixRegex.Match(input);
                if (prefixMatch.Success)
                {
                    // Save the prefix.
                    personName.prefix = prefixMatch.Value.Trim();

                    // Remove the prefix from the name.
                    input = input.Remove(prefixMatch.Index, prefixMatch.Length);

                    // the parsed name includes prefix
                    personName.parseStatus = PersonNameParseStatus.PrefixFound;
                }

                input = input.Trim();

                // Process the rest of the name.
                if (input.Length > 0)
                {
                    string[] nameParts = spacesRegex.Split(input);

                    // Save the given name.
                    personName.first = nameParts[0];

                    if (nameParts.Length > 1)
                    {
                        for (int i = 1; i <= nameParts.Length - 2; i++)
                        {
                            if (i > 1)
                            {
                                personName.middle += " ";
                            }

                            personName.middle += nameParts[i];
                        }

                        personName.last = nameParts[nameParts.Length - 1];
                    }
                }

                if (personName.parseStatus == PersonNameParseStatus.Unparsed)
                {
                    personName.parseStatus = PersonNameParseStatus.Simple;
                }
            }
            else
            {
                // pick the middle comma and go
                string[] segments = input.Split(',');
                int splitComma = (commaCount + ((commaCount % 2) == 0 ? 0 : 1)) / 2;

                string firstPortion = string.Join(",", segments.Take(splitComma));
                string lastPortion = string.Join(",", segments.Skip(splitComma));

                // still populae first/last fields for the grid
                personName.first = firstPortion.Trim();
                personName.last = lastPortion.Trim();
            }

            return personName;
        }

        /// <summary>
        /// Try to parse the company name
        /// </summary>
        private static void TryParseCompanyName(string input, PersonName personName)
        {
            // look for business names (inc, corp, llc) and fallback
            if (personName.parseStatus != PersonNameParseStatus.Unknown)
            {
                return;
            }

            Match suffixMatch = businessSuffixRegex.Match(input);
            if (suffixMatch.Success)
            {
                personName.parseStatus = PersonNameParseStatus.CompanyFound;
            }
        }

        /// <summary>
        /// Gets the unparsed name
        /// </summary>
        public string UnparsedName
        {
            get { return unparsed; }
            set { unparsed = value; }
        }

        /// <summary>
        /// Gets whether or not the name was parsed
        /// </summary>
        public PersonNameParseStatus ParseStatus
        {
            get { return parseStatus; }
            set { parseStatus = value; }
        }

        /// <summary>
        /// The prefix of the name, such as Mr, Mrs, etc.
        /// </summary>
        public string Prefix
        {
            get { return prefix; }
            set { prefix = value ?? ""; }
        }

        /// <summary>
        /// First (given) name
        /// </summary>
        public string First
        {
            get { return first; }
            set { first = value ?? ""; }
        }

        /// <summary>
        /// Middle name of the person
        /// </summary>
        public string Middle
        {
            get { return middle; }
            set { middle = value ?? ""; }
        }

        /// <summary>
        /// Last (Family) name
        /// </summary>
        public string Last
        {
            get { return last; }
            set { last = value ?? ""; }
        }

        /// <summary>
        /// The suffix of the name, such as Jr, Sr, etc.
        /// </summary>
        public string Suffix
        {
            get { return suffix; }
            set { suffix = value ?? ""; }
        }

        /// <summary>
        /// Returns the last name along with the suffix, if any
        /// </summary>
        public string LastWithSuffix
        {
            get
            {
                string result = last;

                if (suffix.Length != 0)
                {
                    if (result.Length != 0)
                    {
                        result += " ";
                    }

                    result += suffix;
                }

                return result;
            }
        }

        /// <summary>
        /// Returns a string that represents the concatenated full name
        /// </summary>
        public string FullName
        {
            get
            {
                if (parseStatus == PersonNameParseStatus.Simple || parseStatus == PersonNameParseStatus.Unknown)
                {
                    StringBuilder name = new StringBuilder(prefix);

                    if (first.Length > 0)
                    {
                        name.AppendFormat("{0}{1}", name.Length == 0 ? string.Empty : " ", first);
                    }

                    if (middle.Length > 0)
                    {
                        name.AppendFormat("{0}{1}", name.Length == 0 ? string.Empty : " ", middle);
                    }

                    if (LastWithSuffix.Length > 0)
                    {
                        name.AppendFormat("{0}{1}", name.Length == 0 ? string.Empty : " ", LastWithSuffix);
                    }

                    return name.ToString();
                }
                else
                {
                    // return the original, unparsed string
                    return unparsed;
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the concatenated full name
        /// </summary>
        public override string ToString()
        {
            return FullName;
        }
    }
}
