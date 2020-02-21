using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Form for allowing users to agree with Insurance terms and conditions
    /// </summary>
    public partial class InsuranceAgreementDlg : Form, IForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceAgreementDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Insurance agreement has been shown
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Task.Factory.StartNew(() =>
                {
                    WebClient webClient = new WebClient();
                    return webClient.DownloadString(InsuranceUtility.OnlineCustomerAgreementFile);
                })
            .ContinueWith(ant =>
                {
                    Cursor = Cursors.Default;

                    if (ant.Exception != null)
                    {
                        if (WebHelper.IsWebException(ant.Exception.InnerException))
                        {
                            textBox.Text = ant.Exception.InnerException.Message;
                        }
                        else
                        {
                            throw ant.Exception;
                        }
                    }
                    else
                    {
                        textBox.Text = ant.Result;
                        agree.Enabled = true;
                    }
                }, 
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Change acceptance of UPIC agreement
        /// </summary>
        private void OnChangeAgreement(object sender, EventArgs e)
        {
            ok.Enabled = agree.Checked;
        }
    }
}
