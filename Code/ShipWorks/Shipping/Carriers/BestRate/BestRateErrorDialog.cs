using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.Properties;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateErrorDialog : Form
    {
        private readonly IEnumerable<BrokerException> brokerExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateErrorDialog"/> class.
        /// </summary>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        public BestRateErrorDialog(IEnumerable<BrokerException> brokerExceptions)
        {
            InitializeComponent();

            this.brokerExceptions = brokerExceptions;
        }

        /// <summary>
        /// Called when [load].
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
                    new GridCell(brokerException.SeverityLevel == BrokerExceptionSeverityLevel.High ? Resources.error16 : Resources.warning16),
                    new GridCell(brokerException.Message)
                };

                GridRow row = new GridRow(cells.ToArray());
                errorGrid.Rows.Add(row);
            }
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
