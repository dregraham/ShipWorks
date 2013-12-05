using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Wraps an entity to expose its person and address details
    /// </summary>
    public class PersonAdapter
    {
        // If there is not an entity loaded, but rather our own local values, then this holds that data
        Dictionary<string, object> localValues = null;

        // If there is an entity loaded which we represent, then this holds that data
        EntityBase2 entity;
        string fieldPrefix;

        /// <summary>
        /// Creates a new instance of the adapter that maintains its own values, and has no backing entity.
        /// </summary>
        public PersonAdapter()
        {
            localValues = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a new instance of the adapter for the entity.  All fields must
        /// be named to standard, with the optional given prefix in front of them.
        /// </summary>
        public PersonAdapter(EntityBase2 entity, string fieldPrefix)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (fieldPrefix == null)
            {
                fieldPrefix = string.Empty;
            }

            this.entity = entity;
            this.fieldPrefix = fieldPrefix;
        }

        /// <summary>
        /// Apply the default values to the fields of the entity
        /// </summary>
        public static void ApplyDefaults(EntityBase2 entity, string fieldPrefix)
        {
            PersonAdapter adapter = new PersonAdapter(entity, fieldPrefix);

            adapter.OriginID = 0;

            adapter.NameParseStatus = PersonNameParseStatus.Unknown;
            adapter.UnparsedName = "";
            adapter.FirstName = "";
            adapter.MiddleName = "";
            adapter.LastName = "";

            adapter.Company = "";

            adapter.Street1 = "";
            adapter.Street2 = "";
            adapter.Street3 = "";

            adapter.City = "";
            adapter.StateProvCode = "";
            adapter.PostalCode = "";
            adapter.CountryCode = "";

            adapter.Phone = "";
            adapter.Fax = "";
            adapter.Email = "";
            adapter.Website = "";
        }

        /// <summary>
        /// Copy the person\address values from the fromEntity to the corresponding fields of the toEntity.
        /// </summary>
        public static void Copy(EntityBase2 fromEntity, EntityBase2 toEntity, string fieldPrefix)
        {
            Copy(fromEntity, fieldPrefix, toEntity, fieldPrefix);
        }

        /// <summary>
        /// Copy the person\address values from the fromEntity to the given PersonAdapter
        /// </summary>
        public static void Copy(EntityBase2 fromEntity, string fromPrefix, PersonAdapter toAdapter)
        {
            PersonAdapter fromAdapter = new PersonAdapter(fromEntity, fromPrefix);

            Copy(fromAdapter, toAdapter);
        }

        /// <summary>
        /// Copy the person\address values from the fromEntity to the corresponding fields of the toEntity.
        /// </summary>
        public static void Copy(EntityBase2 fromEntity, string fromPrefix, EntityBase2 toEntity, string toPrefix)
        {
            PersonAdapter fromAdapter = new PersonAdapter(fromEntity, fromPrefix);
            PersonAdapter toAdapter = new PersonAdapter(toEntity, toPrefix);

            Copy(fromAdapter, toAdapter);
        }

        /// <summary>
        /// Copy the person\address values from the from adapter to the corresponding fields of the to adapter
        /// </summary>
        public static void Copy(PersonAdapter fromAdapter, PersonAdapter toAdapter)
        {
            // Only copy the origin of there is one to copy from
            if (fromAdapter.HasField("OriginID")) toAdapter.OriginID = fromAdapter.OriginID;

            toAdapter.NameParseStatus = fromAdapter.NameParseStatus;
            toAdapter.UnparsedName = fromAdapter.UnparsedName;
            toAdapter.FirstName = fromAdapter.FirstName;
            toAdapter.MiddleName = fromAdapter.MiddleName;
            toAdapter.LastName = fromAdapter.LastName;

            toAdapter.Company = fromAdapter.Company;

            toAdapter.Street1 = fromAdapter.Street1;
            toAdapter.Street2 = fromAdapter.Street2;
            toAdapter.Street3 = fromAdapter.Street3;

            toAdapter.City = fromAdapter.City;
            toAdapter.StateProvCode = fromAdapter.StateProvCode;
            toAdapter.PostalCode = fromAdapter.PostalCode;
            toAdapter.CountryCode = fromAdapter.CountryCode;

            toAdapter.Phone = fromAdapter.Phone;
            toAdapter.Fax = fromAdapter.Fax;
            toAdapter.Email = fromAdapter.Email;
            toAdapter.Website = fromAdapter.Website;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            PersonAdapter other = obj as PersonAdapter;
            if ((object) other == null)
            {
                return false;
            }

            return
                this.OriginID == other.OriginID &&
                this.FirstName == other.FirstName &&
                this.MiddleName == other.MiddleName &&
                this.LastName == other.LastName &&
                this.Company == other.Company &&
                this.Street1 == other.Street1 &&
                this.Street2 == other.Street2 &&
                this.Street3 == other.Street3 &&
                this.City == other.City &&
                this.StateProvCode == other.StateProvCode &&
                this.PostalCode == other.PostalCode &&
                this.CountryCode == other.CountryCode &&
                this.Phone == other.Phone &&
                this.Fax == other.Fax &&
                this.Email == other.Email &&
                this.Website == other.Website;
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(PersonAdapter left, PersonAdapter right)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) left == null) || ((object) right == null))
            {
                return false;
            }


            return left.Equals(right);
        }

        /// <summary>
        /// Operator!=
        /// </summary>
        public static bool operator !=(PersonAdapter left, PersonAdapter right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return (OriginID.ToString() + FirstName + MiddleName + LastName + Company + Street1 + Street2 + Street3 + City + StateProvCode + PostalCode + CountryCode +
                Phone + Fax + Email + Website).GetHashCode();
        }

        /// <summary>
        /// Get the value of the field with the given field name.  If no field exists then the default of the type is returned
        /// </summary>
        private T GetField<T>(string fieldName)
        {
            if (localValues != null)
            {
                object value;
                if (localValues.TryGetValue(fieldName, out value))
                {
                    return (T) value;
                }
                else
                {
                    return GetDefault<T>();
                }
            }
            else
            {
                IEntityField2 field = entity.Fields[fieldPrefix + fieldName];

                if (field == null || field.CurrentValue == null)
                {
                    return GetDefault<T>();
                }

                return (T) field.CurrentValue;
            }
        }

        /// <summary>
        /// Get our default value representation for the type
        /// </summary>
        private T GetDefault<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (T) (object) "";
            }

            return default(T);
        }

        /// <summary>
        /// Set the value of the given field name.  If the field does not exist, nothing is done.
        /// </summary>
        private void SetField(string fieldName, object value)
        {
            if (localValues != null)
            {
                localValues[fieldName] = value;
            }
            else
            {
                IEntityField2 field = entity.Fields[fieldPrefix + fieldName];

                if (field == null)
                {
                    return;
                }

                entity.SetNewFieldValue(fieldPrefix + fieldName, value);
            }
        }

        /// <summary>
        /// Indicates if the adapter supports getting\setting the specified field
        /// </summary>
        private bool HasField(string fieldName)
        {
            if (localValues != null)
            {
                return localValues.ContainsKey(fieldName);
            }
            else
            {
                return entity.Fields[fieldPrefix + fieldName] != null;
            }
        }

        /// <summary>
        /// OriginID value.  Usually only applies to and exists for shipments.
        /// </summary>
        public long OriginID
        {
            get { return GetField<long>("OriginID"); }
            set { SetField("OriginID", value); }
        }

        /// <summary>
        /// Status indicating how well ShipWorks was able to recognize the unparsed name string.
        /// </summary>
        public PersonNameParseStatus NameParseStatus
        {
            get 
            {
                if (HasField("NameParseStatus"))
                {
                    return (PersonNameParseStatus)GetField<int>("NameParseStatus");
                }
                else
                {
                    return PersonNameParseStatus.Unknown;
                }
            }
            set { SetField("NameParseStatus", (int)value); }
        }

        /// <summary>
        /// Original, unparsed nanme
        /// </summary>
        public string UnparsedName
        {
            get { return GetField<string>("UnparsedName"); }
            set { SetField("UnparsedName", value); }
        }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName
        {
            get { return GetField<string>("FirstName");  }
            set { SetField("FirstName", value); }
        }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName
        {
            get { return GetField<string>("MiddleName"); }
            set { SetField("MiddleName", value); }
        }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName
        {
            get { return GetField<string>("LastName"); }
            set { SetField("LastName", value); }
        }

        /// <summary>
        /// Company
        /// </summary>
        public string Company
        {
            get { return GetField<string>("Company"); }
            set { SetField("Company", value); }
        }

        /// <summary>
        /// Street line 1
        /// </summary>
        public string Street1
        {
            get { return GetField<string>("Street1"); }
            set { SetField("Street1", value); }
        }

        /// <summary>
        /// Street line 2
        /// </summary>
        public string Street2
        {
            get { return GetField<string>("Street2"); }
            set { SetField("Street2", value); }
        }

        /// <summary>
        /// Street line 3
        /// </summary>
        public string Street3
        {
            get { return GetField<string>("Street3"); }
            set { SetField("Street3", value); }
        }

        /// <summary>
        /// All 3 streets combined, seperated by new lines, but only non-blank ones
        /// </summary>
        public string StreetAll
        {
            get
            {
                StringBuilder street = new StringBuilder();
                if (!string.IsNullOrEmpty(Street1))
                {
                    street.Append(Street1);
                }

                if (!string.IsNullOrEmpty(Street2))
                {
                    if (street.Length > 0)
                    {
                        street.AppendLine();
                    }

                    street.Append(Street2);
                }

                if (!string.IsNullOrEmpty(Street3))
                {
                    if (street.Length > 0)
                    {
                        street.AppendLine();
                    }

                    street.Append(Street3);
                }

                return street.ToString();
            }
        }

        /// <summary>
        /// Get the street lines for the address.  Only non-blank lines are returned.
        /// </summary>
        public string[] StreetLines
        {
            get
            {
                List<string> lines = new List<string>();

                if (!string.IsNullOrEmpty(Street1))
                {
                    lines.Add(Street1);
                }

                if (!string.IsNullOrEmpty(Street2))
                {
                    lines.Add(Street2);
                }

                if (!string.IsNullOrEmpty(Street3))
                {
                    lines.Add(Street3);
                }

                return lines.ToArray();
            }
        }

        /// <summary>
        /// City
        /// </summary>
        public string City
        {
            get { return GetField<string>("City"); }
            set { SetField("City", value); }
        }

        /// <summary>
        /// The state or province code
        /// </summary>
        public string StateProvCode
        {
            get { return GetField<string>("StateProvCode"); }
            set { SetField("StateProvCode", value); }
        }

        /// <summary>
        /// The postal or zip code
        /// </summary>
        public string PostalCode
        {
            get { return GetField<string>("PostalCode"); }
            set { SetField("PostalCode", value); }
        }

        /// <summary>
        /// If PostalCode is zip code, this is the first 5 digits
        /// </summary>
        public string PostalCode5
        {
            get
            {
                return PersonUtility.GetZip5(PostalCode);
            }
        }

        /// <summary>
        /// If postal code is a zip code, this is the last 5 digits
        /// </summary>
        public string PostalCode4
        {
            get
            {
                return PersonUtility.GetZip4(PostalCode);
            }
        }

        /// <summary>
        /// The country code
        /// </summary>
        public string CountryCode
        {
            get { return GetField<string>("CountryCode"); }
            set { SetField("CountryCode", value); }
        }

        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone
        {
            get { return GetField<string>("Phone"); }
            set { SetField("Phone", value); }
        }

        /// <summary>
        /// Phone number with all extra punctuation removed, and all letters converted to numbers
        /// </summary>
        public string Phone10Digits
        {
            get
            {
                return PersonUtility.GetPhoneDigits10(Phone);
            }
        }

        /// <summary>
        /// Fax
        /// </summary>
        public string Fax
        {
            get { return GetField<string>("Fax"); }
            set { SetField("Fax", value); }
        }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email
        {
            get { return GetField<string>("Email"); }
            set { SetField("Email", value); }
        }

        /// <summary>
        /// Website URL
        /// </summary>
        public string Website
        {
            get { return GetField<string>("Website"); }
            set { SetField("Website", value); }
        }
    }
}
