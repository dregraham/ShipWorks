using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ShipWorks.ApplicationCore.Licensing.Decoding
{
    /// <summary>
    /// Used to read a license key and create a RawLicense from it
    /// </summary>
    static class LicenseDecoder
    {
        /// <summary>
        /// Decodes the given license string.  Returns null on error.
        /// </summary>
        public static RawLicense Decode(string code, string salt)
        {
            try
            {
                // First we need to get pattern data and the "other" data
                Regex regex = new Regex(@"^(\w{5,5})-(\w{5,5})-(\w{5,5})-(\w{5,5})(?:-(.+))?$");
                Match match = regex.Match(code);

                if (!match.Success)
                {
                    return null;
                }

                string pattern = match.Groups[1].Value;
                string encoded = match.Groups[2].Value + match.Groups[3].Value + match.Groups[4].Value;
                string plainText = (match.Groups.Count == 6) ? match.Groups[5].Value : "";

                // Get the pattern
                LicenseKeyPattern patternCode = LicenseKeyPattern.Decode(pattern);

                StringBuilder hash = new StringBuilder(5);
                StringBuilder data1 = new StringBuilder(5);
                StringBuilder data2 = new StringBuilder(5);

                // Now use the pattern to separate out the data
                foreach (LicenseKeyPatternType patternType in patternCode)
                {
                    switch (patternType)
                    {
                        case LicenseKeyPatternType.HashData:
                            hash.Append(encoded[0]);
                            break;

                        case LicenseKeyPatternType.Data1:
                            data1.Append(encoded[0]);
                            break;

                        case LicenseKeyPatternType.Data2:
                            data2.Append(encoded[0]);
                            break;
                    }

                    encoded = encoded.Remove(0, 1);
                }

                // Compute the hash
                string ourHash = RawLicense.GetHashData(data1.ToString() + data2.ToString() + plainText + salt);

                // Valid!
                if (ourHash == hash.ToString())
                {
                    return new RawLicense(code, data1.ToString(), data2.ToString(), plainText);
                }
                else
                {
                    return null;
                }
            }
            catch (LicenseKeyPatternException)
            {
                return null;
            }
        }
    }
}
