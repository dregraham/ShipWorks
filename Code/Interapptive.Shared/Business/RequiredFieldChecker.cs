using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Helper for checking and displaying fields are required.
    /// </summary>
    public class RequiredFieldChecker
    {
        private List<string> missingFields = new List<string>();

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
        /// Validate that there were no missing required fields and displays message to user
        /// </summary>
        public bool Validate(IWin32Window owner)
        {
            GenericResult<string> result = Validate();

            if (result.Success)
            {
                return true;
            }
            else
            {
                MessageHelper.ShowMessage(owner, result.Message.ToString());

                return false;
            }
        }

        /// <summary>
        /// Validate that there were no missing required fields
        /// </summary>
        public GenericResult<string> Validate()
        {
            if (!missingFields.Any())
            {
                return GenericResult.FromSuccess(string.Empty);
            }
            else
            {
                StringBuilder message = new StringBuilder("The following fields are required:\n");

                foreach (string field in missingFields)
                {
                    message.AppendLine("    " + field);
                }

                return GenericResult.FromError<string>(message.ToString());
            }
        }
    }
}
