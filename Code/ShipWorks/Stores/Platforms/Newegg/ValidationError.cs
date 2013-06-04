using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Describes a validation error that occurred.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="errorMessage">The error message.</param>
        public ValidationError(NeweggStoreEntity store, string errorMessage)
        {
            this.Store = store;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public NeweggStoreEntity Store { get; set; }
    }
}
