using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class UspsAccountEntity
    {
        /// <summary>
        /// Gets a friendly description of the Usps account
        /// </summary>
        public string Description
        {
            get
            {
                // Express1 uses terribly long account numbers
                if (UspsReseller != 1)
                {
                    return Username;
                }

                string descriptionBase = Username;

                // only shorten so long as we know they're still using long account numbers.
                if (descriptionBase.Length == 36)
                {
                    descriptionBase = string.Empty;
                }

                StringBuilder description = new StringBuilder(descriptionBase);

                if (!string.IsNullOrEmpty(Street1))
                {
                    if (description.Length > 0)
                    {
                        description.Append(", ");
                    }

                    description.Append(Street1);
                }

                if (!string.IsNullOrEmpty(PostalCode))
                {
                    if (description.Length > 0)
                    {
                        description.Append(", ");
                    }

                    description.Append(PostalCode);
                }

                return description.ToString();
            }
        }
    }
}
