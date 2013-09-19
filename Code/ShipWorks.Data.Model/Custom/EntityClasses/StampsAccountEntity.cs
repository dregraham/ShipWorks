﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class StampsAccountEntity
    {
        /// <summary>
        /// Gets a friendly description of the Stamps account
        /// </summary>
        public string Description
        {
            get
            {
                // Express1 uses terribly long account numbers
                if (!IsExpress1)
                {
                    return Username;
                    
                }

                string descriptionBase = Username;

                // only shorten so long as we know they're still using long account numbers.
                if (descriptionBase.Length == 36)
                {
                    descriptionBase = "Express1";
                }

                StringBuilder description = new StringBuilder(descriptionBase);

                if (Street1.Length > 0)
                {
                    if (description.Length > 0)
                    {
                        description.Append(", ");
                    }

                    description.Append(Street1);
                }

                if (PostalCode.Length > 0)
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
