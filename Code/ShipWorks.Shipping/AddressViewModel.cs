using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Core.UI;
using System;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping
{

    /// <summary>
    /// View model for use by AddressControl
    /// </summary>
    public class AddressViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string fullName;
        private string phone;
        private string email;
        private string countryCode;
        private string postalCode;
        private string stateProvCode;
        private string city;
        private string street;
        private string company;

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
        public string Company
        {
            get { return company; }
            set { handler.Set(nameof(Company), ref company, value); }
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
        public string StateProvCode
        {
            get { return stateProvCode; }
            set { handler.Set(nameof(StateProvCode), ref stateProvCode, value); }
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
        public string CountryCode
        {
            get { return countryCode; }
            set { handler.Set(nameof(CountryCode), ref countryCode, value); }
        }

        /// <summary>
        /// EmailAddress
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Email
        {
            get { return email; }
            set { handler.Set(nameof(Email), ref email, value); }
        }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Phone
        {
            get { return phone; }
            set { handler.Set(nameof(Phone), ref phone, value); }
        }

        /// <summary>
        /// Load the person
        /// </summary>
        public void Load(PersonAdapter person)
        {
            FullName = new PersonName(person).FullName;
            Company = person.Company;
            Street = person.StreetAll;
            City = person.City;
            StateProvCode = Geography.GetStateProvName(person.StateProvCode);
            PostalCode = person.PostalCode;
            CountryCode = person.CountryCode;
            Email = person.Email;
            Phone = person.Phone;
        }

        /// <summary>
        /// Save the current values to the specified person adapter
        /// </summary>
        public void SaveToEntity(PersonAdapter person)
        {
            SaveStreet(person, Street);
            SaveFullName(person, FullName);
            person.Company = Company;
            person.City = City;
            person.PostalCode = PostalCode;
            person.StateProvCode = Geography.GetStateProvCode(StateProvCode);
            person.CountryCode = CountryCode;
            person.Email = Email;
            person.Phone = Phone;
        }

        /// <summary>
        /// Save the street to the specified adapter
        /// </summary>
        private static void SaveStreet(PersonAdapter person, string value)
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
