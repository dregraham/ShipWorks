using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Dialog allowing customer to choose hold location close to the distination address.
    /// </summary>
    public partial class FedExHoldAtLocationSearchDlg : Form
    {
        private readonly ShipmentEntity shipment;

        private List<RadioButton> addressRadioButtons;

        private ServicePoint[] servicePoints;

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
        public ServicePoint SelectedServicePoint
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
        public string FormattedAddress(ServicePoint servicePoint)
        {

            string phoneLine = !string.IsNullOrWhiteSpace(servicePoint.Phone) ? servicePoint.Phone : string.Empty;
            var distanceInMiles = Math.Round(servicePoint.DistanceInMeters * 0.00062137, 2);

            var street = servicePoint.AddressLine1;

            if (!string.IsNullOrEmpty(servicePoint.AddressLine2))
            {
                street += $"\n{servicePoint.AddressLine2}";
            }

            if (!string.IsNullOrEmpty(servicePoint.AddressLine3))
            {
                street += $"\n{servicePoint.AddressLine3}";
            }
    

            return string.Format("{1} ({2} miles) {0}{3}{0}{4}, {5} {6}{7}",
                Environment.NewLine,
                servicePoint.CompanyName,
                distanceInMiles,
                street,
                servicePoint.City,
                servicePoint.StateProvince,
                servicePoint.PostalCode,
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
        private bool IsSelected(ServicePoint servicePoint) =>
            servicePoint.AddressLine1 == shipment.FedEx.HoldStreet1 &&
            servicePoint.AddressLine2 == shipment.FedEx.HoldStreet2 &&
            servicePoint.AddressLine3 == shipment.FedEx.HoldStreet3;


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
                        SelectedServicePoint = servicePoints[index];
                    }
                }
            }

            Close();
        }

        /// <summary>
        /// Loads address in the dialog asynchronously. This is show the form will display
        /// and tell user we are downloading addresses while downloading addresses.
        /// </summary>
        private async void LoadDialog(object sender, EventArgs e)
        {
            try
            {
                var result = await RequestAddresses().ConfigureAwait(true);
                RequestAddressesComplete(result);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);

                Close();
            }
        }

        /// <summary>
        /// Gets the addresses form FedEx.
        /// </summary>
        private async Task<ListServicePointsResponse> RequestAddresses()
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShipEngineWebClient shipEngineWebClient = lifetimeScope.Resolve<IShipEngineWebClient>();
                var fedExAccountRepo = lifetimeScope.Resolve<ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>>();

                var fedExAccount = fedExAccountRepo.GetAccount(shipment);

                var result = await shipEngineWebClient.ListServicePoints(fedExAccount.ShipEngineCarrierID, shipment).ConfigureAwait(false);

                if (result.Failure)
                {
                    throw result.Exception;
                }

                return result.Value;
            }
        }

        /// <summary>
        /// Addresses retrieved. Time to display them.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        private void RequestAddressesComplete(ListServicePointsResponse response)
        {
            servicePoints = response.ServicePoints;

            addressRadioButtons = new List<RadioButton>();

            RadioButton previousAddress = null;
            foreach (var servicePoint in servicePoints)
            {
                int position = 0;

                if (previousAddress != null)
                {
                    position = previousAddress.Location.Y + previousAddress.Height + 10;
                }

                RadioButton currentAddress = new RadioButton
                {
                    Text = FormattedAddress(servicePoint),
                    Top = position,
                    AutoSize = true,
                    Checked = IsSelected(servicePoint)
                };

                addressRadioButtons.Add(currentAddress);
                addressPanel.Controls.Add(currentAddress);

                previousAddress = currentAddress;
            }

            topLabel.Text = "Select a Hold At FedEx Location:";
            ResizeDialog();
        }
    }
}
