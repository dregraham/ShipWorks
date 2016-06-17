using Interapptive.Shared.Business;
using ShipWorks.Data.Utility;
using System;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Extensions for the person adapter
    /// </summary>
    /// <remarks>These are not directly in the person adapter because they make use of functionality
    /// that does not exist in the lowest level assembly</remarks>
    public static class PersonAdapterExtensions
    {
        /// <summary>
        /// Save the street to the specified adapter
        /// </summary>
        public static void SaveStreet(this PersonAdapter person, string value)
        {
            int maxStreet1 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet1);
            int maxStreet2 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet2);
            int maxStreet3 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet3);

            string[] lines = value?.Split(new[] { Environment.NewLine }, StringSplitOptions.None) ?? new string[0];

            string line1 = lines.Length > 0 ? lines[0] : string.Empty;
            string line2 = lines.Length > 1 ? lines[1] : string.Empty;
            string line3 = lines.Length > 2 ? lines[2] : string.Empty;

            if (line1.Length > maxStreet1)
            {
                line2 = line1.Substring(maxStreet1) + " " + line2;
                line1 = line1.Substring(0, maxStreet1);
            }

            if (line2.Length > maxStreet2)
            {
                line3 = line2.Substring(maxStreet2) + " " + line3;
                line2 = line2.Substring(0, maxStreet2);
            }

            if (line3.Length > maxStreet3)
            {
                line3 = line3.Substring(0, maxStreet3);
            }

            person.Street1 = line1;
            person.Street2 = line2;
            person.Street3 = line3;
        }

        /// <summary>
        /// Save the full name to the specified person
        /// </summary>
        public static void SaveFullName(this PersonAdapter person, string value)
        {
            PersonName name = PersonName.Parse(value);

            int maxFirst = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonFirst);
            if (name.First.Length > maxFirst)
            {
                name.Middle = name.First.Substring(maxFirst) + name.Middle;
                name.First = name.First.Substring(0, maxFirst);
            }

            int maxMiddle = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonMiddle);
            if (name.Middle.Length > maxMiddle)
            {
                name.Last = name.Middle.Substring(maxMiddle) + name.Last;
                name.Middle = name.Middle.Substring(0, maxMiddle);
            }

            int maxLast = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonLast);
            if (name.Last.Length > maxLast)
            {
                name.Last = name.Last.Substring(0, maxLast);
            }

            person.FirstName = name.First;
            person.MiddleName = name.Middle;
            person.LastName = name.LastWithSuffix;
            person.UnparsedName = name.UnparsedName;
            person.NameParseStatus = name.ParseStatus;
        }
    }
}
