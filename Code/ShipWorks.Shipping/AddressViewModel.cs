using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Core.UI;
using System;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.UI
{

    /// <summary>
    /// View model for use by AddressControl
    /// </summary>
    public class AddressViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string fullName;
        private string phoneNumber;
        private string emailAddress;
        private string country;
        private string postalCode;
        private string state;
        private string city;
        private string street;
        private string companyName;

        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddressViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Full name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FullName
        {
            get { return fullName; }
            set { handler.Set(nameof(FullName), ref fullName, value); }
        }

        /// <summary>
        /// Company name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CompanyName
        {
            get { return companyName; }
            set { handler.Set(nameof(CompanyName), ref companyName, value); }
        }

        /// <summary>
        /// Street
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Street
        {
            get { return street; }
            set { handler.Set(nameof(Street), ref street, value); }
        }

        /// <summary>
        /// City
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string City
        {
            get { return city; }
            set { handler.Set(nameof(City), ref city, value); }
        }

        /// <summary>
        /// State
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string State
        {
            get { return state; }
            set { handler.Set(nameof(State), ref state, value); }
        }

        /// <summary>
        /// PostalCode
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PostalCode
        {
            get { return postalCode; }
            set { handler.Set(nameof(PostalCode), ref postalCode, value); }
        }

        /// <summary>
        /// Country
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Country
        {
            get { return country; }
            set { handler.Set(nameof(Country), ref country, value); }
        }

        /// <summary>
        /// EmailAddress
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string EmailAddress
        {
            get { return emailAddress; }
            set { handler.Set(nameof(EmailAddress), ref emailAddress, value); }
        }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { handler.Set(nameof(PhoneNumber), ref phoneNumber, value); }
        }

        /// <summary>
        /// Load the person
        /// </summary>
        public void Load(PersonAdapter person)
        {
            FullName = new PersonName(person).FullName;
            CompanyName = person.Company;
            Street = person.StreetAll;
            City = person.City;
            State = Geography.GetStateProvName(person.StateProvCode);
            PostalCode = person.PostalCode;
            Country = person.CountryCode;
            EmailAddress = person.Email;
            PhoneNumber = person.Phone;
        }

        /// <summary>
        /// Save the current values to the specified person adapter
        /// </summary>
        public void SaveToEntity(PersonAdapter person)
        {
            SaveStreet(person, Street);
            SaveFullName(person, FullName);
            person.Company = CompanyName;
            person.City = City;
            person.PostalCode = PostalCode;
            person.StateProvCode = Geography.GetStateProvCode(State);
            person.CountryCode = Country;
            person.Email = EmailAddress;
            person.Phone = PhoneNumber;
        }

        /// <summary>
        /// Save the street to the specified adapter
        /// </summary>
        private static void SaveStreet(PersonAdapter person, string value)
        {
            int maxStreet1 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet1);
            int maxStreet2 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet2);
            int maxStreet3 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet3);

            string[] lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

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
                line3 = line3.Substring(09, maxStreet3);
            }

            person.Street1 = line1;
            person.Street2 = line2;
            person.Street3 = line3;
        }

        /// <summary>
        /// Save the full name to the specified person
        /// </summary>
        private static void SaveFullName(PersonAdapter person, string value)
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
