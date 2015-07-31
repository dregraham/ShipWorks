using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Dialog allowing customer to choose hold location close to the distination address.
    /// </summary>
    public partial class FedExHoldAtLocationSearchDlg : Form
    {
        private readonly ShipmentEntity shipment;

        private List<RadioButton> addressRadioButtons;

        private DistanceAndLocationDetail[] distanceAndLocationDetails;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHoldAtLocationSearchDlg" /> class.
        /// </summary>
        public FedExHoldAtLocationSearchDlg(ShipmentEntity shipment)
        {
            InitializeComponent();

            this.shipment = shipment;
        }

        /// <summary>
        /// Gets the selected location.
        /// </summary>
        /// <value>The selected location.</value>
        public DistanceAndLocationDetail SelectedLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Shows the form as a modal dialog box with the specified owner.
        /// </summary>
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            // Just stubbed out so hold at location control can be wired up
            base.ShowDialog(owner);

            return DialogResult.OK;
        }

        /// <summary>
        /// Formats the address result for display.
        /// </summary>
        public string FormattedAddress(DistanceAndLocationDetail distanceAndLocation)
        {
            Contact contact = distanceAndLocation.LocationDetail.LocationContactAndAddress.Contact;
            Address address = distanceAndLocation.LocationDetail.LocationContactAndAddress.Address;
            string phoneLine = string.Empty;

            if (!string.IsNullOrWhiteSpace(contact.TollFreePhoneNumber))
            {
                phoneLine = contact.TollFreePhoneNumber;
            }
            else if (!string.IsNullOrWhiteSpace(contact.PhoneNumber))
            {
                phoneLine = contact.PhoneNumber;
            }

            return string.Format("{1} ({2} miles) {0}{3}{0}{4}, {5} {6}{7}",
                Environment.NewLine,
                contact.CompanyName,
                Math.Round(distanceAndLocation.Distance.Value, 2),
                string.Join(Environment.NewLine, address.StreetLines),
                address.City,
                address.StateOrProvinceCode,
                address.PostalCode,
                !string.IsNullOrWhiteSpace(phoneLine) ? string.Format("{0}{1}", Environment.NewLine, phoneLine) : string.Empty);
        }

        /// <summary>
        /// Resizes the dialog.
        /// </summary>
        private void ResizeDialog()
        {
            const int maxHeight = 300;

            addressPanel.Width = addressRadioButtons.Max(button => button.Width) + 10;

            if (addressRadioButtons.Last().Bottom < maxHeight)
            {
                addressPanel.Height = addressRadioButtons.Last().Bottom;
            }
            else
            {
                addressPanel.Height = maxHeight;
                addressPanel.Width += 20;
            }

            Height = addressPanel.Bottom + 82;
            Width = addressPanel.Width + 32;
        }

        /// <summary>
        /// Determines whether the user previously had selected this address.
        /// </summary>
        private bool IsSelected(DistanceAndLocationDetail distanceAndLocationDetail)
        {
            string[] returnedStreetArray = distanceAndLocationDetail.LocationDetail.LocationContactAndAddress.Address.StreetLines;

            // The first street lines don't match
            if (returnedStreetArray[0] != shipment.FedEx.HoldStreet1)
            {
                return false;
            }

            // The first street lines match and there are no second street lines
            if (returnedStreetArray.Length == 1 && string.IsNullOrEmpty(shipment.FedEx.HoldStreet2))
            {
                return true;
            }

            // The shipment has a second line, but the returned location does not
            if (returnedStreetArray.Length == 1)
            {
                return false;
            }

            // The first and second street lines match
            if (returnedStreetArray[1] == shipment.FedEx.HoldStreet2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clicks the ok button.
        /// </summary>
        private void ClickOkButton(object sender, EventArgs e)
        {
            if (addressRadioButtons != null)
            {
                for (int index = 0; index < addressRadioButtons.Count; index++)
                {
                    RadioButton addressRadioButton = addressRadioButtons[index];

                    if (addressRadioButton.Checked)
                    {
                        SelectedLocation = distanceAndLocationDetails[index];
                    }
                }
            }

            Close();
        }

        /// <summary>
        /// Loads address in the dialog asynchronously. This is show the form will display
        /// and tell user we are downloading addresses while downloading addresses.
        /// </summary>
        private void LoadDialog(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += RequestAddresses;
            worker.RunWorkerCompleted += RequestAddressesComplete;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Gets the addresses form FedEx.
        /// </summary>
        private void RequestAddresses(object sender, DoWorkEventArgs e)
        {
            IFedExShippingClerk fedExShippingClerk = FedExShippingClerkFactory.CreateShippingClerk(shipment, new FedExSettingsRepository());

            e.Result = fedExShippingClerk.PerformHoldAtLocationSearch(shipment);
        }

        /// <summary>
        /// Addresses retrieved. Time to display them.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void RequestAddressesComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                CarrierException carrierException = e.Error as CarrierException;
                if (carrierException != null)
                {
                    MessageHelper.ShowMessage(this,carrierException.Message);

                    Close();
                    return;
                }
                // if not carrier exception, throw
                throw new Exception("Error returned finding drop off locations", e.Error);
            }

            distanceAndLocationDetails = (DistanceAndLocationDetail[])e.Result;

            addressRadioButtons = new List<RadioButton>();

            try
            {
                RadioButton previousAddress = null;
                foreach (DistanceAndLocationDetail distanceAndLocationDetail in distanceAndLocationDetails)
                {
                    int position = 0;

                    if (previousAddress != null)
                    {
                        position = previousAddress.Location.Y + previousAddress.Height + 10;
                    }

                    RadioButton currentAddress = new RadioButton
                    {
                        Text = FormattedAddress(distanceAndLocationDetail),
                        Top = position,
                        AutoSize = true,
                        Checked = IsSelected(distanceAndLocationDetail)
                    };

                    addressRadioButtons.Add(currentAddress);
                    addressPanel.Controls.Add(currentAddress);

                    previousAddress = currentAddress;
                }

                topLabel.Text = "Select a Hold At FedEx Location:";
                ResizeDialog();
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);

                Close();
            }
        }
    }
}
