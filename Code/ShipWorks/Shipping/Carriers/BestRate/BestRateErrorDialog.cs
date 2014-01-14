using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.Utility;
using ShipWorks.Properties;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateErrorDialog : Form
    {
        private readonly List<BrokerException> brokerExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateErrorDialog"/> class.
        /// </summary>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        public BestRateErrorDialog(IEnumerable<BrokerException> brokerExceptions)
        {
            InitializeComponent();

            // Sort the exceptions by severity level (highest to lowest)
            BrokerExceptionSeverityLevelComparer severityLevelComparer = new BrokerExceptionSeverityLevelComparer();
            this.brokerExceptions = brokerExceptions.OrderBy(ex => ex.SeverityLevel, severityLevelComparer).ToList();
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            errorGrid.Rows.Clear();

            foreach (BrokerException brokerException in brokerExceptions)
            {
                List<GridCell> cells = new List<GridCell>
                {
                    new GridCell(brokerException.SeverityLevel == BrokerExceptionSeverityLevel.Error ? Resources.error16 : Resources.warning16),
                    new GridCell(GetProviderLogo(brokerException.ShipmentType)),
                    new GridCell(brokerException.Message)
                };

                // Set the row height to zero, so it gets dynamically sized based on any 
                // text that gets wrapped to the next line
                GridRow row = new GridRow(cells.ToArray());
                row.Height = 0;

                errorGrid.Rows.Add(row);
            }
        }

        /// <summary>
        /// Gets the provider logo for the given shipment type.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <returns>An Image of the provider logo.</returns>
        private static Image GetProviderLogo(ShipmentType shipmentType)
        {
            Image providerLogo = EnumHelper.GetImage(shipmentType.ShipmentTypeCode);

            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.Other)
            {
                providerLogo = ShippingIcons.truck_blue;
            }

            return providerLogo;
        }

        /// <summary>
        /// Closes the dialog when the Close button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClose(object sender, EventArgs e)
        {
            Close();
        }
    }
}
