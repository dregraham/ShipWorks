using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation.Enums;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for 'Address' elements
    /// </summary>
    public class AddressOutline : ElementOutline
    {
        string type;
        bool includeName;

        PersonAdapter person;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public AddressOutline(TemplateTranslationContext context, string type, bool includeName)
            : base(context)
        {
            this.type = type;
            this.includeName = includeName;

            AddAttribute("type", type);

            if (includeName)
            {
                AddElement("FullName", () => person.UnparsedName);
                AddElement("FirstName", () => person.FirstName);
                AddElement("MiddleName", () => person.MiddleName);
                AddElement("LastName", () => person.LastName);
            }

            AddElement("Company", () => person.Company);

            AddElement("Line1", () => person.Street1);
            AddElement("Line2", () => person.Street2);
            AddElement("Line3", () => person.Street3);

            AddElement("City", () => person.City);

            AddElement("StateCode", () => person.StateProvCode);
            AddElement("StateName", () => Geography.GetStateProvName(person.StateProvCode, person.CountryCode));

            AddElement("PostalCode", () => person.PostalCode);

            AddElement("CountryCode", () => person.CountryCode);
            AddElement("CountryName", () => Geography.GetCountryName(person.CountryCode));

            AddElement("Phone", () => person.Phone);
            AddElement("Fax", () => person.Fax);
            AddElement("Email", () => person.Email);
            AddElement("Website", () => person.Website);

            AddElement("AddressValidationStatus",
                () => EnumHelper.GetDescription((AddressValidationStatusType) person.AddressValidationStatus));

            AddElement("ResidentialStatus", () => GetValidationDetailStatusDesc(person.ResidentialStatus));
            AddElement("POBox", () => GetValidationDetailStatusDesc(person.POBox));
            AddElement("USTerritory", () => GetValidationDetailStatusDesc(person.USTerritory));
            AddElement("MilitaryAddress", () => GetValidationDetailStatusDesc(person.MilitaryAddress));
        }

        /// <summary>
        /// Helper function to get address validation status description from an int which correspondes to 
        /// </summary>
        private string GetValidationDetailStatusDesc(int statusType)
        {
            return EnumHelper.GetDescription((ValidationDetailStatusType) statusType);
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new AddressOutline(Context, type, includeName) { person = (PersonAdapter) data };
        }
    }
}
