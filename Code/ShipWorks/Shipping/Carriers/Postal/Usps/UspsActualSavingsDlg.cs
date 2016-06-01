using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsActualSavingsDlg : Form
    {
        private readonly List<RateResult> originalRates;
        private readonly List<RateResult> discountedRates;

        public UspsActualSavingsDlg(List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            InitializeComponent();

            this.originalRates = originalRates;
            this.discountedRates = discountedRates;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            sandGrid.Rows.Clear();

            // When we loop through a "parent" rate (of like Priority, where Delivery is the real rate), this lets us get back to the parent if we need it.
            RateResult parentRate = null;

            // Go through each rate
            foreach (RateResult originalRate in originalRates)
            {
                PostalRateSelection originalRateDetail = (PostalRateSelection)originalRate.OriginalTag;

                // If it's an express1 saving rate, replace it with the actual express1 rate
                if (originalRateDetail != null)
                {
                    RateResult discountedRate = discountedRates.Where(e1r => e1r.Selectable).FirstOrDefault(e1r =>
                            ((PostalRateSelection)e1r.OriginalTag).ServiceType == originalRateDetail.ServiceType && ((PostalRateSelection)e1r.OriginalTag).ConfirmationType == originalRateDetail.ConfirmationType);

                    if (discountedRate != null)
                    {
                        //if (Express1Utilities.IsPostageSavingService(originalRateDetail.ServiceType) && discountedRate.Amount <= originalRate.Amount)
                        //{
                            // If we need to add in the parent, add it
                            if (parentRate != null)
                            {
                                sandGrid.Rows.Add(new GridRow(parentRate.Description));
                                parentRate = null;
                            }

                            AddRateLine(originalRate, discountedRate);
                        //}
                    }
                }
                else
                {
                    parentRate = originalRate;
                }
            }
        }

        /// <summary>
        /// Add a line for the given rate
        /// </summary>
        private void AddRateLine(RateResult originalRate, RateResult discountedRate)
        {
            GridRow row = new GridRow(new string[]
                    {
                        originalRate.Description,
                        originalRate.AmountOrDefault.ToString("c"),
                        discountedRate.AmountOrDefault.ToString("c"),
                        (originalRate.AmountOrDefault - discountedRate.AmountOrDefault).ToString("c")
                    });


            sandGrid.Rows.Add(row);
        }
    }
}
