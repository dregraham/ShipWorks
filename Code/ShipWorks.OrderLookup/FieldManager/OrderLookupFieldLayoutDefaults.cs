using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Class for retrieving default order lookup field layouts
    /// </summary>
    [Component]
    public class OrderLookupFieldLayoutDefaults : IOrderLookupFieldLayoutDefaults
    {
        /// <summary>
        /// Load default layouts
        /// </summary>
        public IEnumerable<SectionLayout> GetDefaults()
        {
            List<SectionLayout> sectionLayouts = new List<SectionLayout>();

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address",
                Id = "FromAddress",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "USPSAccountSelector", Name = "USPS - Account Selector" },
                    new SectionFieldLayout() { Id = "Origin", Name = "Origin"},
                    new SectionFieldLayout() { Id = "FullName", Name = "Full Name" },
                    new SectionFieldLayout() { Id = "Company", Name = "Company" },
                    new SectionFieldLayout() { Id = "Street", Name = "Street" },
                    new SectionFieldLayout() { Id = "City", Name = "City" },
                    new SectionFieldLayout() { Id = "StateProvince", Name = "State Province" },
                    new SectionFieldLayout() { Id = "PostalCode", Name = "Postal Code" },
                    new SectionFieldLayout() { Id = "Country", Name = "Country" },
                    new SectionFieldLayout() { Id = "Email", Name = "Email" },
                    new SectionFieldLayout() { Id = "Phone", Name = "Phone" },
                    new SectionFieldLayout() { Id = "Fax", Name = "Fax" },
                    new SectionFieldLayout() { Id = "Website", Name = "Website" },
                    new SectionFieldLayout() { Id = "FedExResidentialCommercialAddress", Name = "FedEx - Residential/Commercial Address" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "To Address",
                Id = "ToAddress",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "FullName", Name = "Full Name" },
                    new SectionFieldLayout() { Id = "Company", Name = "Company" },
                    new SectionFieldLayout() { Id = "Street", Name = "Street" },
                    new SectionFieldLayout() { Id = "City", Name = "City" },
                    new SectionFieldLayout() { Id = "StateProvince", Name = "State Province" },
                    new SectionFieldLayout() { Id = "PostalCode", Name = "Postal Code" },
                    new SectionFieldLayout() { Id = "Country", Name = "Country" },
                    new SectionFieldLayout() { Id = "Email", Name = "Email" },
                    new SectionFieldLayout() { Id = "Phone", Name = "Phone" },
                    new SectionFieldLayout() { Id = "USPSRequireFullAddressValidation", Name = "USPS - Require full address validation" },
                    new SectionFieldLayout() { Id = "AddressType", Name = "Address Type" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Items",
                Id = "Items",
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options",
                Id = "LabelOptions",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "ShipDate", Name = "Ship Date" },
                    new SectionFieldLayout() { Id = "USPSStealthPostage", Name = "USPS - Stealth Postage" },
                    new SectionFieldLayout() { Id = "RequestedLabelFormat", Name = "Requested Label Format" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Shipment Details",
                Id = "ShipmentDetails",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "RequestedShipping", Name = "Req. Shipping" },
                    new SectionFieldLayout() { Id = "Provider", Name = "Provider" },
                    new SectionFieldLayout() { Id = "Service", Name = "Service" },
                    new SectionFieldLayout() { Id = "Confirmation", Name = "Confirmation" },
                    new SectionFieldLayout() { Id = "Packaging", Name = "Packaging" },
                    new SectionFieldLayout() { Id = "MultiPackageShipment", Name = "Package Add/Delete" },
                    new SectionFieldLayout() { Id = "NonStandardPackaging", Name = "Non-Standard Packaging" },
                    new SectionFieldLayout() { Id = "Weight", Name = "Weight" },
                    new SectionFieldLayout() { Id = "Dimensions", Name = "Dimensions" },
                    new SectionFieldLayout() { Id = "AddToWeight", Name = "Add to weight" },
                    new SectionFieldLayout() { Id = "Insurance", Name = "Insurance" },
                    new SectionFieldLayout() { Id = "OnTracSaturdayDelivery", Name = "OnTrac - Saturday Delivery" },
                    new SectionFieldLayout() { Id = "OnTracSignatureRequired", Name = "OnTrac - Signature Required" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "USPS - Reference",
                Id = "USPSReference",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "Memo1", Name = "Memo 1" },
                    new SectionFieldLayout() { Id = "Memo2", Name = "Memo 2" },
                    new SectionFieldLayout() { Id = "Memo3", Name = "Memo 3" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "FedEx - Signature and Reference",
                Id = "FedExSignatureAndReference",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "SignatureRequired", Name = "Signature Required" },
                    new SectionFieldLayout() { Id = "ReferenceNumber", Name = "Reference Number" },
                    new SectionFieldLayout() { Id = "InvoiceNumber", Name = "Invoice Number" },
                    new SectionFieldLayout() { Id = "PostOfficeNumber", Name = "Post Office Number" },
                    new SectionFieldLayout() { Id = "Integrity", Name = "Integrity" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "FedEx - Email Notifications",
                Id = "FedExEmailNotifications",
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "UPS - Reference",
                Id = "UPSReference",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "ReferenceNumber", Name = "Reference Number",  },
                    new SectionFieldLayout() { Id = "ReferenceNumber2", Name = "Reference Number 2" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "UPS - Quantum View Notify",
                Id = "UPSQuantumView",
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "OnTrac - Reference and Instructions",
                Id = "OnTracReferenceAndInstructions",
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "Reference", Name = "Reference" },
                    new SectionFieldLayout() { Id = "Reference2", Name = "Reference 2" },
                    new SectionFieldLayout() { Id = "Instructions", Name = "Instructions" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Rates",
                Id = "Rates",
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Customs",
                Id = "Customs",
                SectionFields = new List<SectionFieldLayout>()
            });

            return sectionLayouts;
        }
    }
}