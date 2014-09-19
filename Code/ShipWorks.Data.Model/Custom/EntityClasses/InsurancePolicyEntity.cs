using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// /// Partial class extention of the LLBLGen InsurancePolicyEntity
    /// </summary>
    public partial class InsurancePolicyEntity
    {
        // Use a field, so we can initialize the value to non-null
        private string claimStatus = string.Empty;

        /// <summary>
        /// Gets or sets the claim status of the policy.
        /// </summary>
        public string ClaimStatus
        {
            get { return claimStatus; }
            set { claimStatus = value; }
        }
    }
}
