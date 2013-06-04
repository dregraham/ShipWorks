using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Delegate to use for the EmailSettingsResolve event
    /// </summary>
    public delegate void EmailSettingsResolveEventHandler(object sender, EmailSettingsResolveEventArgs e);

    /// <summary>
    /// EventArgs for the EmailSettingsResolve event
    /// </summary>
    public class EmailSettingsResolveEventArgs : CancelEventArgs
    {
        TemplateType templateType;
        List<long> stores;
        CustomerEntity customer;

        bool useMostRecentOrder;
        bool useMostRecentOrderForRest = false;
        long? useSpecificStoreID;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSettingsResolveEventArgs(TemplateType templateType, List<long> stores, CustomerEntity customer)
        {
            this.templateType = templateType;
            this.stores = stores;
            this.customer = customer;

            useMostRecentOrder = customer != null;
            useSpecificStoreID = (customer != null) ? (long?) null : stores[0];
        }

        /// <summary>
        /// The type of template that was selected to be emailed.
        /// </summary>
        public TemplateType TemplateType
        {
            get { return templateType; }
        }

        /// <summary>
        /// The list of store ID's that the user has to choose from
        /// </summary>
        public List<long> Stores
        {
            get { return stores; }
        }

        /// <summary>
        /// The customer that is being sent email that has orders from more than one store.  Could be null for label or report templates.  If it
        /// is null, the UseMostRecentOrder options should not be used.
        /// </summary>
        public CustomerEntity Customer
        {
            get { return customer; }
        }

        /// <summary>
        /// Indicates if the store of the most recent order of the customer should be used.
        /// </summary>
        public bool UseMostRecentOrder
        {
            get { return useMostRecentOrder; }
            set { useMostRecentOrder = value; }
        }

        /// <summary>
        /// If UseMostRecentOrder is true, indiciates that this answer should be used for any remaining selected customers.
        /// </summary>
        public bool UseMostRecentOrderForRest
        {
            get { return useMostRecentOrderForRest; }
            set { useMostRecentOrderForRest = value; }
        }

        /// <summary>
        /// Indicates that the specified StoreID should be used.
        /// </summary>
        public long? UseSpecificStoreID
        {
            get { return useSpecificStoreID; }
            set { useSpecificStoreID = value; }
        }
    }
}
