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
                Id = SectionLayoutIDs.From,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.USPSAccountSelector, Name = "USPS - Account Selector" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Origin, Name = "Origin"},
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Company, Name = "Company" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.City, Name = "City" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.PostalCode, Name = "Postal Code" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Country, Name = "Country" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Email, Name = "Email" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Phone, Name = "Phone" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Fax, Name = "Fax" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Website, Name = "Website" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExResidentialCommercialAddress, Name = "FedEx - Residential/Commercial Address" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "To Address",
                Id = SectionLayoutIDs.To,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Company, Name = "Company" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.City, Name = "City" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.PostalCode, Name = "Postal Code" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Country, Name = "Country" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Email, Name = "Email" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Phone, Name = "Phone" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.USPSRequireFullAddressValidation, Name = "USPS - Require full address validation" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.AddressType, Name = "Address Type" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Items",
                Id = SectionLayoutIDs.Items,
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options",
                Id = SectionLayoutIDs.LabelOptions,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsShipDate, Name = "Ship Date" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsUspsHideStealth, Name = "USPS - Stealth Postage" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsRequestedLabelFormat, Name = "Requested Label Format" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Shipment Details",
                Id = SectionLayoutIDs.ShipmentDetails,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsRequestedShipping, Name = "Req. Shipping" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsProvider, Name = "Provider" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsService, Name = "Service" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsConfirmation, Name = "Confirmation" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsPackaging, Name = "Packaging" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsMultiPackageShipment, Name = "Package Add/Delete" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsNonStandardPackaging, Name = "Non-Standard Packaging" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsWeight, Name = "Weight" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsDimensions, Name = "Dimensions" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsAddToWeight, Name = "Add to weight" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.ShipmentDetailsInsurance, Name = "Insurance" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "USPS - Reference",
                Id = SectionLayoutIDs.USPSReference,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.USPSReferenceMemo1, Name = "Memo 1" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.USPSReferenceMemo2, Name = "Memo 2" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.USPSReferenceMemo3, Name = "Memo 3" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "FedEx - Signature and Reference",
                Id = SectionLayoutIDs.FedExSignatureAndReference,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExSignatureAndReferenceSignatureRequired, Name = "Signature Required" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExSignatureAndReferenceReferenceNumber, Name = "Reference Number" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExSignatureAndReferenceInvoiceNumber, Name = "Invoice Number" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExSignatureAndReferencePostOfficeNumber, Name = "Post Office Number" },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FedExSignatureAndReferenceIntegrity, Name = "Integrity" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "FedEx - Email Notifications",
                Id = SectionLayoutIDs.FedExEmailNotifications,
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "UPS - Reference",
                Id = SectionLayoutIDs.UPSReference,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.UPSReferenceReferenceNumber, Name = "Reference Number",  },
                    new SectionFieldLayout() { Id = SectionLayoutFieldIDs.UPSReferenceReferenceNumber2, Name = "Reference Number 2" }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "UPS - Quantum View Notify",
                Id = SectionLayoutIDs.UPSQuantumViewNotify,
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Rates",
                Id = SectionLayoutIDs.Rates,
                SectionFields = new List<SectionFieldLayout>()
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Customs",
                Id = SectionLayoutIDs.Customs,
                SectionFields = new List<SectionFieldLayout>()
            });

            return sectionLayouts;
        }
    }
}