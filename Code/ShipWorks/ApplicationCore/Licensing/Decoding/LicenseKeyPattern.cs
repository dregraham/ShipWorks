using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Licensing.Decoding
{
    class LicenseKeyPattern : List<LicenseKeyPatternType>
	{
        /// <summary>
        /// Possible pattern types
        /// </summary>
        private static LicenseKeyPatternType[,] patternKeyTypes = new LicenseKeyPatternType[27, 3];

        /// <summary>
        /// Each letter in the pattern represents 3 types.  This there need to be
        /// 27 keys, as each key can represent 3*3*3 combination of types.
        /// </summary>
        private static char[] patternKeys = new char[27]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
                'K', 'L', 'M', 'N', 'O', 'Q', 'S', 'T', 'U',
                'X', 'Z', '1', '2', '3', '4', '5', '6', '7'
            };

        /// <summary>
        /// Static contstructor, Initialize the pattern data
        /// </summary>
        static LicenseKeyPattern()
        {
            int index = 0;

            for (int slot1 = 0; slot1 < 3; slot1++)
            {
                for (int slot2 = 0; slot2 < 3; slot2++)
                {
                    for (int slot3 = 0; slot3 < 3; slot3++)
                    {
                        patternKeyTypes[index, 0] = (LicenseKeyPatternType) Enum.Parse(typeof(LicenseKeyPatternType), slot1.ToString());
                        patternKeyTypes[index, 1] = (LicenseKeyPatternType) Enum.Parse(typeof(LicenseKeyPatternType), slot2.ToString());
                        patternKeyTypes[index, 2] = (LicenseKeyPatternType) Enum.Parse(typeof(LicenseKeyPatternType), slot3.ToString());
                        
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Cannot directly create an instance of this class
        /// </summary>
		private LicenseKeyPattern()
		{

        }

        /// <summary>
        /// Create a new LicenseKeyPattern with the given pattern string
        /// </summary>
        public static LicenseKeyPattern Decode(string pattern)
        {
            // Quick validity check
            if (pattern.Length != 5)
            {
                throw new LicenseKeyPatternException("Invalid Pattern");
            }

            LicenseKeyPattern newPattern = new LicenseKeyPattern();

            // Look up each key in the pattern
            for (int i = 0; i < pattern.Length; i++)
            {
                // Find the index of the key
                for (int index = 0; index < 27; index++)
                {
                    // If this is the index, add in the patterns
                    if (patternKeys[index] == pattern[i])
                    {
                        newPattern.Add(patternKeyTypes[index, 0]);
                        newPattern.Add(patternKeyTypes[index, 1]);
                        newPattern.Add(patternKeyTypes[index, 2]);

                        break;
                    }
                }
            }

            // Quick sanity check
            if (pattern != newPattern.Code)
            {
                throw new LicenseKeyPatternException("Invalid Pattern");
            }

            return newPattern;
        }

        /// <summary>
        /// Returns a string representation of the pattern.
        /// </summary>
        public string Code
        {
            get
            {
                if (Count != 15)
                {
                    throw new LicenseKeyPatternException("Invalid Pattern");
                }

                // This will end up holding the code
                StringBuilder code = new StringBuilder(5, 5);

                // Take the chunks of the pattern by three's
                for (int i = 0; i < 15; i += 3)
                {
                    // Look to see what key this maps to
                    for (int key = 0; key < 27; key++)
                    {
                        if (patternKeyTypes[key, 0] == (LicenseKeyPatternType) this[i + 0] &&
                            patternKeyTypes[key, 1] == (LicenseKeyPatternType) this[i + 1] &&
                            patternKeyTypes[key, 2] == (LicenseKeyPatternType) this[i + 2])
                        {
                            code.Append(patternKeys[key]);
                            break;
                        }
                    }
                }

                // Make sure we have the proper code
                if (code.Length != 5)
                {
                    throw new LicenseKeyPatternException("Invalid Pattern");
                }

                return code.ToString();
            }
        }
	}
}
