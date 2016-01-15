using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Wraps an entity to expose its address details
    /// </summary>
    public class AddressAdapter : EntityAdapter, IAddressAdapter
    {
        /// <summary>
        /// Creates a new instance of the adapter that maintains its own values, and has no backing entity.
        /// </summary>
        public AddressAdapter() : base()
        {

        }

        /// <summary>
        /// Creates a new instance of the adapter for the entity.  All fields must
        /// be named to standard, with the optional given prefix in front of them.
        /// </summary>
        public AddressAdapter(IEntity2 entity, string fieldPrefix) 
            : base(entity, fieldPrefix)
        {

        }

        /// <summary>
        /// Apply the default values to the fields of the entity
        /// </summary>
        public static void ApplyDefaults(IEntity2 entity, string fieldPrefix)
        {
            AddressAdapter adapter = new AddressAdapter(entity, fieldPrefix);

            adapter.OriginID = 0;

            adapter.Street1 = "";
            adapter.Street2 = "";
            adapter.Street3 = "";

            adapter.City = "";
            adapter.StateProvCode = "";
            adapter.PostalCode = "";
            adapter.CountryCode = "";

            adapter.AddressValidationStatus = 0;
            adapter.AddressValidationSuggestionCount = 0;

            adapter.ResidentialStatus = 0;
            adapter.POBox = 0;
            adapter.USTerritory = 0;
            adapter.MilitaryAddress = 0;
        }

        /// <summary>
        /// Copy the address values from the fromEntity to the corresponding fields of the toEntity.
        /// </summary>
        public static void Copy(IEntity2 fromEntity, IEntity2 toEntity, string fieldPrefix)
        {
            Copy(fromEntity, fieldPrefix, toEntity, fieldPrefix);
        }

        /// <summary>
        /// Copy the address values from the fromEntity to the given PersonAdapter
        /// </summary>
        public static void Copy(IEntity2 fromEntity, string fromPrefix, AddressAdapter toAdapter)
        {
            AddressAdapter fromAdapter = new AddressAdapter(fromEntity, fromPrefix);

            Copy(fromAdapter, toAdapter);
        }

        /// <summary>
        /// Copy the address values from the fromEntity to the corresponding fields of the toEntity.
        /// </summary>
        public static void Copy(IEntity2 fromEntity, string fromPrefix, IEntity2 toEntity, string toPrefix)
        {
            AddressAdapter fromAdapter = new AddressAdapter(fromEntity, fromPrefix);
            AddressAdapter toAdapter = new AddressAdapter(toEntity, toPrefix);

            Copy(fromAdapter, toAdapter);
        }

        /// <summary>
        /// Copy the address values from the from adapter to the corresponding fields of the to adapter
        /// </summary>
        public static void Copy(AddressAdapter fromAdapter, AddressAdapter toAdapter)
        {
            // Only copy the origin of there is one to copy from
            if (fromAdapter.HasField("OriginID")) toAdapter.OriginID = fromAdapter.OriginID;

            toAdapter.Street1 = fromAdapter.Street1.Trim();
            toAdapter.Street2 = fromAdapter.Street2.Trim();
            toAdapter.Street3 = fromAdapter.Street3.Trim();

            toAdapter.City = fromAdapter.City.Trim();
            toAdapter.StateProvCode = fromAdapter.StateProvCode.Trim();
            toAdapter.PostalCode = fromAdapter.PostalCode.Trim();
            toAdapter.CountryCode = fromAdapter.CountryCode.Trim();

            toAdapter.ResidentialStatus = fromAdapter.ResidentialStatus;
            toAdapter.POBox = fromAdapter.POBox;
            toAdapter.USTerritory = fromAdapter.USTerritory;
            toAdapter.MilitaryAddress = fromAdapter.MilitaryAddress;
        }

        /// <summary>
        /// Copy the address values from this adapter to another entity
        /// </summary>
        public void CopyTo(IEntity2 entity, string prefix)
        {
            Copy(this, new AddressAdapter(entity, prefix));
        }

        /// <summary>
        /// Copy the address values from this adapter to another
        /// </summary>
        /// <param name="destinationAddress"></param>
        public void CopyTo(AddressAdapter destinationAddress)
        {
            Copy(this, destinationAddress);
        }

        /// <summary>
        /// Copies validation data from one address to another
        /// </summary>
        public static void CopyValidationData(AddressAdapter fromAddress, AddressAdapter toAddress)
        {
            toAddress.AddressValidationError = fromAddress.AddressValidationError;
            toAddress.AddressValidationStatus = fromAddress.AddressValidationStatus;
            toAddress.AddressValidationSuggestionCount = fromAddress.AddressValidationSuggestionCount;
            toAddress.AddressType = fromAddress.AddressType;
        }

        /// <summary>
        /// Copy the validation details from this adapter to another
        /// </summary>
        /// <param name="destinationAddress"></param>
        public void CopyValidationDataTo(AddressAdapter destinationAddress)
        {
            CopyValidationData(this, destinationAddress);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            AddressAdapter other = obj as AddressAdapter;
            if ((object) other == null)
            {
                return false;
            }

            return
                AreEqualTrimmed(Street1, other.Street1) &&
                AreEqualTrimmed(Street2, other.Street2) &&
                AreEqualTrimmed(Street3, other.Street3) &&
                AreEqualTrimmed(City, other.City) &&
                AreEqualTrimmed(StateProvCode, other.StateProvCode) &&
                AreEqualTrimmed(PostalCode, other.PostalCode) &&
                AreEqualTrimmed(CountryCode, other.CountryCode);
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(AddressAdapter left, AddressAdapter right)
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
        public static bool operator !=(AddressAdapter left, AddressAdapter right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return (OriginID + Street1 + Street2 + Street3 + City + StateProvCode + PostalCode + CountryCode + AddressValidationStatus).GetHashCode();
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
        /// ValidationStatus - relates to enum ValidationStatusType defined in ShipWorks project.
        /// </summary>
        public int AddressValidationStatus
        {
            get { return GetField<int>("AddressValidationStatus"); }
            set { SetField("AddressValidationStatus", value); }
        }

        /// <summary>
        /// AddressValidationSuggestionCount - keeps a count of how many suggestions an address has so that it doesn't have to be looked up each time.
        /// </summary>
        public int AddressValidationSuggestionCount
        {
            get { return GetField<int>("AddressValidationSuggestionCount"); }
            set { SetField("AddressValidationSuggestionCount", value); }
        }

        /// <summary>
        /// Gets or sets the address validation error.
        /// </summary>
        public string AddressValidationError
        {
            get { return GetField<string>("AddressValidationError"); }
            set { SetField("AddressValidationError", value); }
        }

        /// <summary>
        /// Whether the address is residential or commercial
        /// </summary>
        public int ResidentialStatus
        {
            get { return GetField<int>("ResidentialStatus"); }
            set { SetField("ResidentialStatus", value); }
        }

        /// <summary>
        /// Whether the address is a PO Box
        /// </summary>
        public int POBox
        {
            get { return GetField<int>("POBox"); }
            set { SetField("POBox", value); }
        }

        /// <summary>
        /// Whether the address is an US territory
        /// </summary>
        public int USTerritory
        {
            get { return GetField<int>("USTerritory"); }
            set { SetField("USTerritory", value); }
        }

        /// <summary>
        /// Whether the address is a military address
        /// </summary>
        public int MilitaryAddress
        {
            get { return GetField<int>("MilitaryAddress"); }
            set { SetField("MilitaryAddress", value); }
        }

        /// <summary>
        /// Checks whether two strings are equal after trimming
        /// </summary>
        private static bool AreEqualTrimmed(string left, string right)
        {
            string trimmedLeft = left == null ? string.Empty : left.Trim();
            string trimmedRight = right == null ? string.Empty : right.Trim();

            return string.Equals(trimmedLeft, trimmedRight, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Gets or sets the type of the address.
        /// </summary>
        public int AddressType
        {
            get { return GetField<int>("AddressType"); }
            set { SetField("AddressType", value); }
        }
    }
}