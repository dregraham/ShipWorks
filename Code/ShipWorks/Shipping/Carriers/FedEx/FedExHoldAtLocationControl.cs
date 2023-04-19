using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.UI.Controls;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using System.Collections.Generic;
using Interapptive.Shared;
using ShipWorks.Shipping.ShipEngine.DTOs;
using Rebex.Security.Cryptography.Pkcs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExHoldAtLocationControl : UserControl
    {
        // a member/attribute to reference the FedEx API object representing a selected location 
        private ServicePoint fedExLocationDetails;
        private IEnumerable<ShipmentEntity> shipments;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHoldAtLocationControl" /> class.
        /// </summary>
        public FedExHoldAtLocationControl()
        {
            InitializeComponent();
            fedExLocationDetails = new ServicePoint();

            EnableDetailControls(false);
        }

        /// <summary>
        /// Saves to shipment entity.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        [NDependIgnoreLongMethod]
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
            if (shipmentEntity.FedEx.FedExHoldAtLocationEnabled && !locationDetails.MultiValued && !string.IsNullOrEmpty(fedExLocationDetails.ServicePointId))
            {
                // Get a handle to the entity, contact, and address so we don't have to walk
                // the object graph each time
                FedExShipmentEntity fedEx = shipmentEntity.FedEx;

                fedEx.HoldLocationId = fedExLocationDetails.ServicePointId;

                fedEx.HoldCompanyName = fedExLocationDetails.CompanyName;
                fedEx.HoldPhoneNumber = fedExLocationDetails.Phone;
                fedEx.HoldStreet1 = fedExLocationDetails.AddressLine1;
                fedEx.HoldStreet2 = fedExLocationDetails.AddressLine2;
                fedEx.HoldStreet3 = fedExLocationDetails.AddressLine3;
                fedEx.HoldCity = fedExLocationDetails.City;
                fedEx.HoldStateOrProvinceCode = fedExLocationDetails.StateProvince;
                fedEx.HoldCountryCode = fedExLocationDetails.CountryCode;
                fedEx.HoldPostalCode = fedExLocationDetails.PostalCode;
            }
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
            fedExLocationDetails = new ServicePoint();
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
                fedExLocationDetails = new ServicePoint
                {
                    ServicePointId = fedExShipment.HoldLocationId,
                    AddressLine1 = fedExShipment.HoldStreet1,
                    AddressLine2 = fedExShipment.HoldStreet2,
                    AddressLine3 = fedExShipment.HoldStreet3,
                    City = fedExShipment.HoldCity,
                    CountryCode = fedExShipment.HoldCountryCode,
                    PostalCode = fedExShipment.HoldPostalCode,
                    StateProvince = fedExShipment.HoldStateOrProvinceCode,
                    CompanyName = fedExShipment.HoldCompanyName,
                    Phone = fedExShipment.HoldPhoneNumber
                };
            }
        }

        /// <summary>
        /// Updates the location details text box with the .
        /// </summary>
        private string FormatLocationDetails()
        {
            StringBuilder formattedLocation = new StringBuilder();

            if (!string.IsNullOrEmpty(fedExLocationDetails.CompanyName))
            {
                formattedLocation.AppendFormat("{0}{1}", fedExLocationDetails.CompanyName, Environment.NewLine);
            }

            // Convert the address into a single string

            var streetAddress = fedExLocationDetails.AddressLine1;

            if (!string.IsNullOrEmpty(fedExLocationDetails.AddressLine2))
            {
                streetAddress += $"\n{fedExLocationDetails.AddressLine2}";
            }

            if (!string.IsNullOrEmpty(fedExLocationDetails.AddressLine3))
            {
                streetAddress += $"\n{fedExLocationDetails.AddressLine3}";
            }

            if (!string.IsNullOrEmpty(streetAddress))
            {
                formattedLocation.AppendFormat("{0}{1}", streetAddress, Environment.NewLine);
            }

            // Write out the city, state, and postal code
            if (!string.IsNullOrEmpty(fedExLocationDetails.City))
            {
                formattedLocation.AppendFormat("{0}", fedExLocationDetails.City);
            }

            if (!string.IsNullOrEmpty(fedExLocationDetails.StateProvince))
            {
                formattedLocation.AppendFormat(", {0}", fedExLocationDetails.StateProvince);
            }

            if (!string.IsNullOrEmpty(fedExLocationDetails.PostalCode))
            {
                formattedLocation.AppendFormat(" {0}", fedExLocationDetails.PostalCode);
            }

            if (!string.IsNullOrEmpty(fedExLocationDetails.Phone))
            {
                formattedLocation.AppendFormat("{0}{1}", Environment.NewLine, fedExLocationDetails.Phone);
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
                    if (result == DialogResult.OK && searchDialog.SelectedServicePoint != null)
                    {
                        fedExLocationDetails = searchDialog.SelectedServicePoint;
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
