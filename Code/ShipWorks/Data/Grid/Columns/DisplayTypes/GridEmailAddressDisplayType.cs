using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Email;
using Rebex.Mime.Headers;
using Rebex.Mime;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display types for columns that show email addresses
    /// </summary>
    public class GridEmailAddressDisplayType : GridColumnDisplayType
    {
        EmailDisplayFormat format = EmailDisplayFormat.NameOnly;

        /// <summary>
        /// Create the editor for controlling email display settings
        /// </summary>
        public override GridColumnDisplayEditor CreateEditor()
        {
            return new GridEmailDisplayEditor(this);
        }

        /// <summary>
        /// The format that will be used to display the email address
        /// </summary>
        public EmailDisplayFormat EmailDisplayFormat
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// Get the text to display based on the configured formatting
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            string addressList = (string) value;
            if (string.IsNullOrEmpty(addressList))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(addressList.Length);

            // Break each email down
            foreach (string addressItem in EmailUtility.SplitAddressList(addressList))
            {
                string display = "";

                try
                {
                    // Parse it into its parts
                    MailAddress address = new MailAddress(addressItem);

                    // Add the DisplayName if configured
                    if (format != EmailDisplayFormat.AddressOnly && !string.IsNullOrEmpty(address.DisplayName))
                    {
                        display += address.DisplayName;
                    }

                    // Show the address part if configured to do so - or if there was no name to display
                    if (format != EmailDisplayFormat.NameOnly || display.Length == 0)
                    {
                        string addressFormat = (display.Length == 0) ? "{0}" : " [{0}]";

                        display += string.Format(addressFormat, address.Address);
                    }
                }
                catch (MimeException)
                {
                    display = addressItem;
                }

                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }

                sb.Append(display);
            }

            return sb.ToString();
        }
    }
}
