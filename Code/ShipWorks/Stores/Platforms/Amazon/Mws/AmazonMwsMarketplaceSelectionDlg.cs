using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Windnow for allowing a user to select their Amazon marketplace
    /// </summary>
    public partial class AmazonMwsMarketplaceSelectionDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsMarketplaceSelectionDlg(List<AmazonMwsMarketplace> marketplaces, string defaultID)
        {
            InitializeComponent();

            foreach (var marketplace in marketplaces)
            {
                GridRow row = new GridRow(new string[]
                    {
                        marketplace.Name,
                        marketplace.MarketplaceID
                    });
                row.Tag = marketplace;

                sandGrid.Rows.Add(row);

                if (marketplace.MarketplaceID == defaultID)
                {
                    row.Selected = true;
                }
            }
        }

        /// <summary>
        /// Get the selected marketplace
        /// </summary>
        public AmazonMwsMarketplace SelectedMarketplace
        {
            get
            {
                if (sandGrid.SelectedElements.Count == 1)
                {
                    return (AmazonMwsMarketplace) sandGrid.SelectedElements[0].Tag;
                }

                return null;
            }
        }

        /// <summary>
        /// OK'ing the current selection
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (sandGrid.SelectedElements.Count == 0)
            {
                MessageHelper.ShowMessage(this, "Please select a marketplace from the list.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Double-clicking a row
        /// </summary>
        private void OnRowActivated(object sender, GridRowEventArgs e)
        {
            OnOK(ok, EventArgs.Empty);
        }
    }
}
