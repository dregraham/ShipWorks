using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Responsible for loading or creating customer records based on an incoming order
    /// </summary>
    public static class CustomerProvider
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(CustomerProvider));

        /// <summary>
        /// Creates or loads a customer record based on configuration and the specified order.
        /// </summary>
        public static async Task<CustomerEntity> AcquireCustomer(OrderEntity order, StoreType storeType, ISqlAdapter adapter)
        {
            CustomerEntity customer = null;

            // This section of code can only be executed by a single thread - across all computers running shipworks - at a time.
            using (CustomerAcquisitionLock customerLock = new CustomerAcquisitionLock())
            {
                Stopwatch sw = Stopwatch.StartNew();

                if (order.CustomerID > 0)
                {
                    customer = DataProvider.GetEntity(order.CustomerID, adapter, true) as CustomerEntity;
                }
                else
                {
                    // Find the customer using its online identifier
                    customer = await FindExistingCustomer(order, storeType, adapter);
                }

                TimeSpan searchTime = sw.Elapsed;

                // If we still don't have a customer, we have to create one
                if (customer == null)
                {
                    log.InfoFormat("  Creating new customer.");

                    customer = await CreateCustomer(order, adapter).ConfigureAwait(false);
                }
                // We found it, we may need to update it
                else
                {
                    IConfigurationEntity config = ConfigurationData.FetchReadOnly();

                    // Have to update
                    if (config.CustomerUpdateShipping || config.CustomerUpdateBilling)
                    {
                        if (config.CustomerUpdateBilling)
                        {
                            PersonAdapter.Copy(order, customer, "Bill");
                        }

                        if (config.CustomerUpdateShipping)
                        {
                            PersonAdapter.Copy(order, customer, "Ship");
                        }

                        if (config.CustomerUpdateBilling || config.CustomerUpdateShipping || customer.IsDirty)
                        {
                            // Save it back
                            await adapter.SaveEntityAsync(customer);
                        }
                    }
                }

                log.InfoFormat("Customer acquisition: {0} (Search: {1} [{2:0.00}%])",
                    sw.Elapsed.TotalSeconds,
                    searchTime.TotalSeconds,
                    100 * searchTime.TotalSeconds / sw.Elapsed.TotalSeconds);
            }

            return customer;
        }

        /// <summary>
        /// Find an existing customer that matches the properties of the given order for the specified store type
        /// </summary>
        public static async Task<CustomerEntity> FindExistingCustomer(OrderEntity order, StoreType storeType, ISqlAdapter adapter)
        {
            CustomerEntity customer = await FindCustomerByOnlineIdentifier(adapter, order, storeType);

            if (customer != null)
            {
                log.InfoFormat("  Customer {0} found by online identifier.", customer.CustomerID);
                return customer;
            }

            IConfigurationEntity config = ConfigurationData.FetchReadOnly();

            return await FindExistingCustomer(order.BillPerson, config.CustomerCompareEmail, config.CustomerCompareAddress, adapter);
        }

        /// <summary>
        /// See if there is an existing customer that matches the specified customer using the configuration in the options
        /// </summary>
        public static Task<CustomerEntity> FindExistingCustomer(CustomerEntity customer, bool compareEmail, bool compareMailing, ISqlAdapter adapter) =>
            FindExistingCustomer(customer.BillPerson, compareEmail, compareMailing, adapter);

        /// <summary>
        /// Find a customer with the given criteria
        /// </summary>
        private static Task<CustomerEntity> FindCustomer(ISqlAdapter adapter, IPredicate predicate, params IEntityRelation[] relations)
        {
            if (predicate == null)
            {
                return Task.FromResult<CustomerEntity>(null);
            }

            QueryFactory factory = new QueryFactory();
            IJoinOperand source = factory.Customer
                .InnerJoin(OrderEntity.Relations.CustomerEntityUsingCustomerID);

            foreach (IEntityRelation relation in relations)
            {
                source = source.InnerJoin(relation);
            }

            var query = factory.Create<CustomerEntity>()
                .From(source)
                .Where(predicate);

            return adapter.FetchFirstAsync(query);
        }

        /// <summary>
        /// Find an existing customer based on the address information in the given entity
        /// </summary>
        private static async Task<CustomerEntity> FindExistingCustomer(PersonAdapter billPerson, bool compareEmail, bool compareMailing, ISqlAdapter adapter)
        {
            CustomerEntity customer = null;

            // Find the customer using the billing email address
            if (compareEmail)
            {
                // Find a match in the order table
                customer = await FindCustomer(adapter, GetCompareOrderEmailPredicate(billPerson));

                // Find a match in the customer table
                if (customer == null)
                {
                    customer = await FindCustomer(adapter, GetCompareCustomerEmailPredicate(billPerson));
                }
            }

            // Find the customer using the billing mailing address
            if (customer == null && compareMailing)
            {
                // Find it in the order table
                customer = await FindCustomer(adapter, GetCompareOrderAddressPredicate(billPerson));

                // Then look in the customer table
                if (customer == null)
                {
                    customer = await FindCustomer(adapter, GetCompareCustomerAddressPredicate(billPerson));
                }
            }

            return customer;
        }

        /// <summary>
        /// Attempt to find a customer using online identifier.  If no customer is found, null is returned.
        /// </summary>
        private static async Task<CustomerEntity> FindCustomerByOnlineIdentifier(ISqlAdapter adapter, OrderEntity order, StoreType storeType)
        {
            bool instanceLookup;
            IEnumerable<IEntityField2> identifierFields = storeType.CreateCustomerIdentifierFields(out instanceLookup);

            // If nothing to identify by, then nothing to do
            if (identifierFields == null)
            {
                return null;
            }

            PredicateExpression customerExpression = BuildCustomerIdentifierPredicate(order, storeType, identifierFields);

            // There were no fields, or there were no non-null fields: nothing to do
            if (customerExpression.Count == 0)
            {
                return null;
            }

            if (instanceLookup)
            {
                customerExpression.And(OrderFields.StoreID == order.StoreID);
                return await FindCustomer(adapter, customerExpression);
            }

            customerExpression.And(StoreFields.TypeCode == (int) storeType.TypeCode);
            return await FindCustomer(adapter, customerExpression, OrderEntity.Relations.StoreEntityUsingStoreID);
        }

        /// <summary>
        /// Build the customer identifier predicate
        /// </summary>
        private static PredicateExpression BuildCustomerIdentifierPredicate(OrderEntity order, StoreType storeType, IEnumerable<IEntityField2> identifierFields)
        {
            PredicateExpression customerExpression = new PredicateExpression();

            // Build the search criteria
            foreach (IEntityField2 identifierField in identifierFields)
            {
                IEntityField2 field = order.Fields[identifierField.Name];

                if (field != null)
                {
                    object value = field.CurrentValue;

                    // If its null we can't use it
                    if (value != null)
                    {
                        customerExpression.AddWithOr(new FieldCompareValuePredicate(identifierField, null, ComparisonOperator.Equal, value));
                    }
                }
                else
                {
                    // It's only OK to not find the identifier on manual orders - which may be of the OrderEntity base type and not the specific store type
                    if (!order.IsManual)
                    {
                        throw new InvalidOperationException(string.Format("Should have found store-specific field for non-manual order. {0} {1}", identifierField.Name, storeType.StoreTypeName));
                    }
                }
            }

            return customerExpression;
        }

        /// <summary>
        /// Get the predicate to use to match to an existing order by billing email
        /// </summary>
        private static IPredicate GetCompareOrderEmailPredicate(PersonAdapter billPerson)
        {
            // Don't look for email if its blank
            if (billPerson.Email.Length == 0)
            {
                return null;
            }

            return OrderFields.BillEmail == billPerson.Email;
        }

        /// <summary>
        /// Get the predicate to use to match to an existing order by mailing address
        /// </summary>
        private static IPredicate GetCompareOrderAddressPredicate(PersonAdapter billPerson)
        {
            return
                OrderFields.BillFirstName == billPerson.FirstName &
                OrderFields.BillLastName == billPerson.LastName &
                OrderFields.BillStreet1 == billPerson.Street1 &
                OrderFields.BillStreet2 == billPerson.Street2 &
                OrderFields.BillPostalCode == billPerson.PostalCode;
        }

        /// <summary>
        /// Get the predicate to use to match to an existing customer by billing email
        /// </summary>
        private static IPredicate GetCompareCustomerEmailPredicate(PersonAdapter billPerson)
        {
            // Don't look for email if its blank
            if (billPerson.Email.Length == 0)
            {
                return null;
            }

            return CustomerFields.BillEmail == billPerson.Email;
        }

        /// <summary>
        /// Get the predicate to use to match to an existing order by mailing address
        /// </summary>
        private static IPredicate GetCompareCustomerAddressPredicate(PersonAdapter billPerson)
        {
            return
                CustomerFields.BillFirstName == billPerson.FirstName &
                CustomerFields.BillLastName == billPerson.LastName &
                CustomerFields.BillStreet1 == billPerson.Street1 &
                CustomerFields.BillStreet2 == billPerson.Street2 &
                CustomerFields.BillPostalCode == billPerson.PostalCode;
        }

        /// <summary>
        /// Create a new customer record based on the properties of the order
        /// </summary>
        private static async Task<CustomerEntity> CreateCustomer(OrderEntity order, ISqlAdapter adapter)
        {
            CustomerEntity customer = new CustomerEntity();

            PersonAdapter.Copy(order, customer, "Bill");
            PersonAdapter.Copy(order, customer, "Ship");

            customer.RollupOrderCount = 0;
            customer.RollupOrderTotal = 0;
            customer.RollupNoteCount = 0;

            await adapter.SaveEntityAsync(customer).ConfigureAwait(false);

            return customer;
        }
    }
}
