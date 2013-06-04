using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Helper for checking and displaying fields are required.
    /// </summary>
    public class RequiredFieldChecker
    {
        List<string> missingFields = new List<string>();

        /// <summary>
        /// Check that the given field has a value
        /// </summary>
        public void Check(string fieldName, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                missingFields.Add(fieldName);
            }
        }

        /// <summary>
        /// Validate that there were no missing required fields
        /// </summary>
        public bool Validate(IWin32Window owner)
        {
            if (missingFields.Count > 0)
            {
                StringBuilder message = new StringBuilder("The following fields are required:\n");

                foreach (string field in missingFields)
                {
                    message.AppendLine("    " + field);
                }

                MessageHelper.ShowMessage(owner, message.ToString());

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
