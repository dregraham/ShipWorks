using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// TextBox that formats its content to look like money
    /// </summary>
    public class MoneyTextBox : MultiValueTextBox
    {
        // The last valid amount that was entered
        decimal currentAmount = 0.0m;

        // Raised whenever the value changes
        public event EventHandler AmountChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public MoneyTextBox()
        {

        }

        /// <summary>
        /// Get \ set the amount
        /// </summary>
        [Category("Appearance")]
        public decimal Amount
        {
            get
            {
                ParseAmount(Text);

                return currentAmount;
            }
            set
            {
                MultiValued = false;

                SetCurrentAmount(value);

                this.SelectionStart = base.Text.Length;
            }
        }

        /// <summary>
        /// Override Text so we can format when the user changes us.
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                MultiValued = false;

                ParseAmount(value);
            }
        }

        /// <summary>
        /// Parse the value in the text box
        /// </summary>
        private void ParseAmount(string value)
        {
            if (value.Length == 0)
            {
                SetCurrentAmount(0);
            }
            else
            {
                decimal amount;
                if (decimal.TryParse(value, NumberStyles.Currency, null, out amount))
                {
                    SetCurrentAmount(amount);
                }
                else
                {
                    FormatAmountText();
                }
            }

            this.SelectionStart = base.Text.Length;
        }

        /// <summary>
        /// Format the weight value
        /// </summary>
        private void FormatAmountText()
        {
            if (!MultiValued)
            {
                base.Text = currentAmount.ToString("c");
            }
        }

        /// <summary>
        /// Update the current amount that the control represents.
        /// </summary>
        private void SetCurrentAmount(decimal newAmount)
        {
            if (currentAmount != newAmount)
            {
                currentAmount = newAmount;
                FormatAmountText();

                OnAmountChanged();
            }
            else
            {
                FormatAmountText();
            }
        }

        /// <summary>
        /// Called whenever the amount changes.  Raises the AmountChanged event.
        /// </summary>
        protected virtual void OnAmountChanged()
        {
            if (AmountChanged != null)
            {
                AmountChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Trap the enter key
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                ParseAmount(Text);

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Control is losing focus
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            ParseAmount(Text);
        }
    }
}
