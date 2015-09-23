using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExHoldAtLocationControl : UserControl
    {
        // a member/attribute to reference the FedEx API object representing a selected location 
        private DistanceAndLocationDetail fedExLocationDetails;
        private IEnumerable<ShipmentEntity> shipments;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHoldAtLocationControl" /> class.
        /// </summary>
        public FedExHoldAtLocationControl()
        {
            InitializeComponent();
            fedExLocationDetails = new DistanceAndLocationDetail();

            EnableDetailControls(false);
        }

        /// <summary>
        /// Saves to shipment entity.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        public void SaveToShipment(ShipmentEntity shipmentEntity)
        {
            holdAtLocation.ReadMultiCheck(c => shipmentEntity.FedEx.FedExHoldAtLocationEnabled = c);

            if (!shipmentEntity.FedEx.FedExHoldAtLocationEnabled)
            {
                // Clear out the hold contact/address fields
                shipmentEntity.FedEx.HoldLocationType = null;
                
                shipmentEntity.FedEx.HoldTitle = null;
                shipmentEntity.FedEx.HoldCompanyName = null;
                shipmentEntity.FedEx.HoldStreet1 = null;
                shipmentEntity.FedEx.HoldStreet2 = null;
                shipmentEntity.FedEx.HoldStreet3 = null;
                shipmentEntity.FedEx.HoldCity = null;
                shipmentEntity.FedEx.HoldStateOrProvinceCode = null;
                shipmentEntity.FedEx.HoldPostalCode = null;
                shipmentEntity.FedEx.HoldCountryCode = null;
                shipmentEntity.FedEx.HoldUrbanizationCode = null;

                shipmentEntity.FedEx.HoldPersonName = null;
                shipmentEntity.FedEx.HoldContactId = null;
                shipmentEntity.FedEx.HoldEmailAddress = null;
                shipmentEntity.FedEx.HoldFaxNumber = null;
                shipmentEntity.FedEx.HoldLocationId = null;
                shipmentEntity.FedEx.HoldPagerNumber = null;
                shipmentEntity.FedEx.HoldPhoneExtension = null;
                shipmentEntity.FedEx.HoldPhoneNumber = null;
                shipmentEntity.FedEx.HoldResidential = null;
            }

            // Don't save anything if the option is selected and the location details were never set
            if (shipmentEntity.FedEx.FedExHoldAtLocationEnabled && fedExLocationDetails.LocationDetail != null)
            {
                // Don't save multi-valued location details otherwise location details will get overwritten
                if (!locationDetails.MultiValued)
                {
                    // Get a handle to the entity, contact, and address so we don't have to walk
                    // the object graph each time
                    FedExShipmentEntity fedEx = shipmentEntity.FedEx;
                    Contact contactInfo = GetContact();
                    Address address = GetAddress();

                    fedEx.HoldLocationId = fedExLocationDetails.LocationDetail.LocationId;

                    if (contactInfo != null)
                    {
                        fedEx.HoldContactId = contactInfo.ContactId;
                        fedEx.HoldPersonName = contactInfo.PersonName;
                        fedEx.HoldTitle = contactInfo.Title;
                        fedEx.HoldCompanyName = contactInfo.CompanyName;

                        fedEx.HoldPhoneNumber = contactInfo.PhoneNumber;
                        fedEx.HoldPhoneExtension = contactInfo.PhoneExtension;
                        fedEx.HoldPagerNumber = contactInfo.PagerNumber;
                        fedEx.HoldFaxNumber = contactInfo.FaxNumber;
                        fedEx.HoldEmailAddress = contactInfo.EMailAddress;
                    }

                    if (address != null)
                    {
                        fedEx.HoldStreet1 = GetStreetLineFromFedExAddress(address, 0);
                        fedEx.HoldStreet2 = GetStreetLineFromFedExAddress(address, 1);
                        fedEx.HoldStreet3 = GetStreetLineFromFedExAddress(address, 2);

                        fedEx.HoldCity = address.City;
                        fedEx.HoldStateOrProvinceCode = address.StateOrProvinceCode;
                        fedEx.HoldPostalCode = address.PostalCode;
                        fedEx.HoldCountryCode = address.CountryCode;
                        fedEx.HoldResidential = address.Residential;
                        fedEx.HoldUrbanizationCode = address.UrbanizationCode;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the street line from FedEx address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <returns>The text of the specified address line.</returns>
        private string GetStreetLineFromFedExAddress(Address address, int lineNumber)
        {
            string line = string.Empty;

            if (address.StreetLines.Length > lineNumber)
            {
                if (!string.IsNullOrEmpty(address.StreetLines[lineNumber]))
                {
                    line = address.StreetLines[lineNumber];
                }
            }

            return line;
        }

        /// <summary>
        /// Loads from shipment entity.
        /// </summary>
        /// <param name="loadedShipments">The shipments.</param>
        public void LoadFromShipment(IEnumerable<ShipmentEntity> loadedShipments)
        {
            // Retain a local reference to the shipment so we can provide it to the search dialog.
            this.shipments = loadedShipments;

            // Populate the FedExAPI object of the selected location and load/format the details into the text box
            ClearLocationDetails();

            foreach (ShipmentEntity shipment in shipments)
            {
                LoadFedExLocationDetails(shipment.FedEx);
                locationDetails.ApplyMultiText(FormatLocationDetails());
            }

            // Only enable the search link if there is a single shipment
            searchLink.Enabled = shipments.Count() == 1;
        }

        /// <summary>
        /// Clears the location details.
        /// </summary>
        private void ClearLocationDetails()
        {
            // Create a new location details object to clear out any pre-existing data
            fedExLocationDetails = new DistanceAndLocationDetail();
            locationDetails.Clear();
        }

        /// <summary>
        /// Loads the Fed Ex location details from a FedExHoldAtLocationEntity.
        /// </summary>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        private void LoadFedExLocationDetails(FedExShipmentEntity fedExShipment)
        {
            holdAtLocation.ApplyMultiCheck(fedExShipment.FedExHoldAtLocationEnabled);

            if (fedExShipment.FedExHoldAtLocationEnabled)
            {
                fedExLocationDetails.LocationDetail = new LocationDetail
                {
                    LocationId = fedExShipment.HoldLocationId,
                    LocationContactAndAddress = new LocationContactAndAddress
                    {
                        Address = new Address
                        {
                            City = fedExShipment.HoldCity,
                            CountryCode = fedExShipment.HoldCountryCode,
                            PostalCode = fedExShipment.HoldPostalCode,
                            Residential = fedExShipment.HoldResidential ?? false,
                            StateOrProvinceCode = fedExShipment.HoldStateOrProvinceCode,
                            StreetLines = new[] { fedExShipment.HoldStreet1, fedExShipment.HoldStreet2, fedExShipment.HoldStreet3 },
                            UrbanizationCode = fedExShipment.HoldUrbanizationCode
                        },
                        Contact = new Contact
                        {
                            ContactId = fedExShipment.HoldContactId,
                            CompanyName = fedExShipment.HoldCompanyName,
                            EMailAddress = fedExShipment.HoldEmailAddress,
                            FaxNumber = fedExShipment.HoldFaxNumber,
                            PagerNumber = fedExShipment.HoldPagerNumber,
                            PersonName = fedExShipment.HoldPersonName,
                            PhoneExtension = fedExShipment.HoldPhoneExtension,
                            PhoneNumber = fedExShipment.HoldPhoneNumber,
                            Title = fedExShipment.HoldTitle
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Helper method to get a handle to the contact object within the location details object.
        /// </summary>
        /// <returns>A Contact object.</returns>
        private Contact GetContact()
        {
            Contact contactInfo = null;
            
            if (fedExLocationDetails.LocationDetail != null)
            {
                if (fedExLocationDetails.LocationDetail.LocationContactAndAddress != null)
                {
                    contactInfo = fedExLocationDetails.LocationDetail.LocationContactAndAddress.Contact;
                }
            }

            return contactInfo;
        }

        /// <summary>
        /// Helper method to get a handle to the address object within the location details object.
        /// </summary>
        /// <returns>An Address object.</returns>
        private Address GetAddress()
        {
            Address address = null;

            if (fedExLocationDetails.LocationDetail != null)
            {
                if (fedExLocationDetails.LocationDetail.LocationContactAndAddress != null)
                {
                    address = fedExLocationDetails.LocationDetail.LocationContactAndAddress.Address;
                }
            }

            return address;
        }

        /// <summary>
        /// Updates the location details text box with the .
        /// </summary>
        private string FormatLocationDetails()
        {
            StringBuilder formattedLocation = new StringBuilder();
            
            // Get a handle to the contanct and address within the fedExLocationDetails object
            Contact contactInfo = GetContact();
            Address address = GetAddress();

            if (contactInfo != null)
            {
                // Write the contact info
                if (!string.IsNullOrEmpty(contactInfo.PersonName))
                {   
                    formattedLocation.AppendFormat("{0}{1}", contactInfo.PersonName, Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(contactInfo.CompanyName))
                {
                    formattedLocation.AppendFormat("{0}{1}", contactInfo.CompanyName, Environment.NewLine);
                }
            }

            if (address != null)
            {
                // We have an address populated in the FedEx location details object

                // Convert the address array into a single string
                string streetAddress = string.Join(Environment.NewLine, address.StreetLines.Where(s => !string.IsNullOrEmpty(s)));
                if (!string.IsNullOrEmpty(streetAddress))
                {
                    formattedLocation.AppendFormat("{0}{1}", streetAddress, Environment.NewLine);
                }

                // Write out the city, state, and postal code
                if (!string.IsNullOrEmpty(address.City))
                {
                    formattedLocation.AppendFormat("{0}", address.City);
                }

                if (!string.IsNullOrEmpty(address.StateOrProvinceCode))
                {
                    formattedLocation.AppendFormat(", {0}", address.StateOrProvinceCode);
                }

                if (!string.IsNullOrEmpty(address.PostalCode))
                {
                    formattedLocation.AppendFormat(" {0}", address.PostalCode);
                }
            }

            if (contactInfo != null)
            {
                // Write the contact phone number
                if (!string.IsNullOrEmpty(contactInfo.TollFreePhoneNumber))
                {
                    formattedLocation.AppendFormat("{0}{1}", Environment.NewLine, contactInfo.TollFreePhoneNumber);
                }
                if (!string.IsNullOrEmpty(contactInfo.PhoneNumber))
                {
                    formattedLocation.AppendFormat("{0}{1}", Environment.NewLine, contactInfo.PhoneNumber);
                }
            }

            // The location should be formatted based on the location details now
            return formattedLocation.ToString();
        }

        /// <summary>
        /// Called when [select location clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs" /> instance containing the event data.</param>
        private void OnSelectLocationClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                using (FedExHoldAtLocationSearchDlg searchDialog = new FedExHoldAtLocationSearchDlg(shipments.First()))
                {
                    DialogResult result = searchDialog.ShowDialog(this);
                    if (result == DialogResult.OK && searchDialog.SelectedLocation != null)
                    {
                        fedExLocationDetails = searchDialog.SelectedLocation;
                        SaveToShipment(shipments.First());
                    }
                }
                
                // Update the text box with the selected location data
                locationDetails.Text = FormatLocationDetails();
            }
            catch (CarrierException exception)
            {
                throw new FedExException("ShipWorks was unable to obtain a list of locations from FedEx.", exception);
            }
        }

        /// <summary>
        /// Called when [hold at location changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnHoldAtLocationChanged(object sender, EventArgs e)
        {            
            EnableDetailControls(holdAtLocation.CheckState == CheckState.Checked);
        }

        /// <summary>
        /// Enables the detail controls.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void EnableDetailControls(bool enabled)
        {
            locationDetails.Enabled = enabled;
            locationLabel.Enabled = enabled;
            searchLink.Enabled = enabled;
        }
    }
}
