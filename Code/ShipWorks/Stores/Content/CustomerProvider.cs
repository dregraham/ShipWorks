using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Diagnostics;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityInterfaces;

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
        public static CustomerEntity AcquireCustomer(OrderEntity order, StoreType storeType, SqlAdapter adapter, bool persist)
        {
            CustomerEntity customer = null;

            // This section of code can only be executed by a single thread - accross all computers running shipworks - at a time.
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
                    customer = FindExistingCustomer(order, storeType, adapter);
                }

                TimeSpan searchTime = sw.Elapsed;

                // If we still don't have a customer, we have to create one
                if (customer == null)
                {
                    log.InfoFormat("  Creating new customer.");

                    customer = CreateCustomer(order, adapter, persist);
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
                            adapter.SaveEntity(customer);
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
        /// Find a CustomerID, searching using the specified predicate
        /// </summary>
        private static CustomerEntity FindCustomer(IPredicate predicate, SqlAdapter adapter)
        {
            if (predicate == null)
            {
                return null;
            }

            return FindCustomer(new RelationPredicateBucket(predicate), adapter);
        }

        /// <summary>
        /// Find a CustomerID, searching using the specified predicate
        /// </summary>
        private static CustomerEntity FindCustomer(RelationPredicateBucket bucket, SqlAdapter adapter)
        {
            if (bucket == null)
            {
                return null;
            }

            CustomerEntity customer = null;
            CustomerCollection customerCollection = new CustomerCollection();
            bucket.Relations.Add(OrderEntity.Relations.CustomerEntityUsingCustomerID);

            adapter.FetchEntityCollection(customerCollection, bucket);

            customer = customerCollection?.FirstOrDefault();

            return customer;
        }

        /// <summary>
        /// Find an existing customer that matches the properties of the given order for the specified store type
        /// </summary>
        public static CustomerEntity FindExistingCustomer(OrderEntity order, StoreType storeType, SqlAdapter adapter)
        {
            CustomerEntity customer = FindCustomer(GetCompareOnlineIdentifierPredicate(order, storeType), adapter);

            if (customer != null)
            {
                log.InfoFormat("  Customer {0} found by online identifier.", customer.CustomerID);
                return customer;
            }

            IConfigurationEntity config = ConfigurationData.FetchReadOnly();

            return FindExistingCustomer(order, config.CustomerCompareEmail, config.CustomerCompareAddress, adapter);
        }

        /// <summary>
        /// Find an existing customer based on the address information in the given entity
        /// </summary>
        private static CustomerEntity FindExistingCustomer(EntityBase2 entity, bool compareEmail, bool compareMailing, SqlAdapter adapter)
        {
            CustomerEntity customer = null;

            PersonAdapter billPerson = new PersonAdapter(entity, "Bill");

            // Find the customer using the billing email address
            if (compareEmail)
            {
                // Find a match in the order table
                customer = FindCustomer(GetCompareOrderEmailPredicate(billPerson), adapter);

                // Find a match in the customer table
                if (customer == null)
                {
                    customer = FindCustomer(GetCompareCustomerEmailPredicate(billPerson), adapter);
                }
            }

            // Find the customer using the billing mailing address
            if (customer == null && compareMailing)
            {
                // Find it in the order table
                customer = FindCustomer(GetCompareOrderAddressPredicate(billPerson), adapter);

                // Then look in the customer table
                if (customer == null)
                {
                    customer = FindCustomer(GetCompareCustomerAddressPredicate(billPerson), adapter);
                }
            }

            return customer;
        }

        /// <summary>
        /// See if there is an existing customer that matches the specified customer using the configuration in the options
        /// </summary>
        public static CustomerEntity FindExistingCustomer(CustomerEntity customer, bool compareEmail, bool compareMailing, SqlAdapter adapter)
        {
            return FindExistingCustomer((EntityBase2) customer, compareEmail, compareMailing, adapter);
        }

        /// <summary>
        /// Attempt to find a customer using online identifier.  If no customer is found, null is returned.
        /// </summary>
        private static RelationPredicateBucket GetCompareOnlineIdentifierPredicate(OrderEntity order, StoreType storeType)
        {
            bool instanceLookup;
            IEnumerable<IEntityField2> identifierFields = storeType.CreateCustomerIdentifierFields(out instanceLookup);
            
            // If nothing to identify by, then nothing to do
            if (identifierFields == null)
            {
                return null;
            }

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

            // There were no fields, or there were no non-null fields: nothing to do
            if (customerExpression.Count == 0)
            {
                return null;
            }
            else
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket();

                if (instanceLookup)
                {
                    bucket.PredicateExpression.Add(OrderFields.StoreID == order.StoreID);
                }
                else
                {
                    bucket.PredicateExpression.Add(StoreFields.TypeCode == (int) storeType.TypeCode);
                    bucket.Relations.Add(OrderEntity.Relations.StoreEntityUsingStoreID);
                }

                bucket.PredicateExpression.AddWithAnd(customerExpression);

                return bucket;
            }
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
        private static CustomerEntity CreateCustomer(OrderEntity order, SqlAdapter adapter, bool persist)
        {
            CustomerEntity customer = new CustomerEntity();

            PersonAdapter.Copy(order, customer, "Bill");
            PersonAdapter.Copy(order, customer, "Ship");

            customer.RollupOrderCount = 0;
            customer.RollupOrderTotal = 0;
            customer.RollupNoteCount = 0;

            if (persist)
            {
                adapter.SaveEntity(customer);
            }

            return customer;
        }
    }
}
