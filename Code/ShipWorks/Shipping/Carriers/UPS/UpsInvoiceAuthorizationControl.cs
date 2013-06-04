using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// A control for collecting the UPS invoice information that is required to authenticate an account with UPS.
    /// </summary>
    public partial class UpsInvoiceAuthorizationControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceAuthorizationControl" /> class.
        /// </summary>
        public UpsInvoiceAuthorizationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the invoice authorization data.
        /// </summary>
        /// <value>The invoice authorization data.</value>
        public UpsOltInvoiceAuthorizationData InvoiceAuthorizationData
        {
            get
            {
                UpsOltInvoiceAuthorizationData invoiceAuthorizationData = new UpsOltInvoiceAuthorizationData
                {
                    ControlID = authControlID.Text,
                    InvoiceAmount = authInvoiceAmount.Amount,
                    InvoiceDate = authInvoiceDate.Value,
                    InvoiceNumber = authInvoiceNumber.Text
                };

                return invoiceAuthorizationData;
            }
        }
    }
}
