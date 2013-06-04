using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.ApplicationCore;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using System.Diagnostics;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Business;

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
        public static long AcquireCustomer(OrderEntity order, StoreType storeType, SqlAdapter adapter)
        {
            long? customerID = null;

            // This section of code can only be executed by a single thread - accross all computers running shipworks - at a time.
            using (CustomerAcquisitionLock customerLock = new CustomerAcquisitionLock())
            {
                Stopwatch sw = Stopwatch.StartNew();

                // Find the customer using its online identifier
                customerID = FindExistingCustomer(order, storeType, adapter);

                TimeSpan searchTime = sw.Elapsed;

                // If we still don't have a customer, we have to create one
                if (customerID == null)
                {
                    log.InfoFormat("  Creating new customer.");

                    customerID = CreateCustomer(order, adapter);
                }
                // We found it, we may need to update it
                else
                {
                    ConfigurationEntity config = ConfigurationData.Fetch();

                    // Have to update
                    if (config.CustomerUpdateShipping || config.CustomerUpdateBilling)
                    {
                        CustomerEntity customer = new CustomerEntity(customerID.Value);
                        adapter.FetchEntity(customer);

                        if (config.CustomerUpdateBilling)
                        {
                            PersonAdapter.Copy(order, customer, "Bill");
                        }

                        if (config.CustomerUpdateShipping)
                        {
                            PersonAdapter.Copy(order, customer, "Ship");
                        }

                        // Save it back
                        adapter.SaveEntity(customer);
                    }
                }

                log.InfoFormat("Customer acquisition: {0} (Search: {1} [{2:0.00}%])", 
                    sw.Elapsed.TotalSeconds, 
                    searchTime.TotalSeconds, 
                    100 * searchTime.TotalSeconds / sw.Elapsed.TotalSeconds);
            }

            return customerID.Value;
        }

        /// <summary>
        /// Find an existing customer that matches the properties of the given order for the specified store type
        /// </summary>
        public static long? FindExistingCustomer(OrderEntity order, StoreType storeType, SqlAdapter adapter)
        {
            long? customerID = FindCustomerID(OrderFields.CustomerID, GetCompareOnlineIdentifierPredicate(order, storeType), adapter);

            if (customerID != null)
            {
                log.InfoFormat("  Customer {0} found by online identifier.", customerID);
                return customerID;
            }

            ConfigurationEntity config = ConfigurationData.Fetch();

            return FindExistingCustomer(order, config.CustomerCompareEmail, config.CustomerCompareAddress, adapter);
        }

        /// <summary>
        /// See if there is an existing customer that matches the specified customer using the configuration in the options
        /// </summary>
        public static long? FindExistingCustomer(CustomerEntity customer, SqlAdapter adapter)
        {
            ConfigurationEntity config = ConfigurationData.Fetch();

            return FindExistingCustomer((EntityBase2) customer, config.CustomerCompareEmail, config.CustomerCompareAddress, adapter);
        }

        /// <summary>
        /// See if there is an existing customer that matches the specified customer using the configuration in the options
        /// </summary>
        public static long? FindExistingCustomer(CustomerEntity customer, bool compareEmail, bool compareMailing, SqlAdapter adapter)
        {
            return FindExistingCustomer((EntityBase2) customer, compareEmail, compareMailing, adapter);
        }

        /// <summary>
        /// Find an existing customer based on the address information in the given entity
        /// </summary>
        private static long? FindExistingCustomer(EntityBase2 entity, bool compareEmail, bool compareMailing, SqlAdapter adapter)
        {
            long? customerID = null;

            PersonAdapter billPerson = new PersonAdapter(entity, "Bill");

            // Find the customer using the billing email address
            if (compareEmail)
            {
                // Find a match in the order table
                customerID = FindCustomerID(OrderFields.CustomerID, GetCompareOrderEmailPredicate(billPerson), adapter);

                // Find a match in the customer table
                if (customerID == null)
                {
                    customerID = FindCustomerID(CustomerFields.CustomerID, GetCompareCustomerEmailPredicate(billPerson), adapter);
                }
            }

            // Find the customer using the billing mailing address
            if (customerID == null && compareMailing)
            {
                // Find it in the order table
                customerID = FindCustomerID(OrderFields.CustomerID, GetCompareOrderAddressPredicate(billPerson), adapter);

                // Then look in the customer table
                if (customerID == null)
                {
                    customerID = FindCustomerID(CustomerFields.CustomerID, GetCompareCustomerAddressPredicate(billPerson), adapter);
                }
            }

            return customerID;
        }

        /// <summary>
        /// Find a CustomerID, searching using the specified predicate
        /// </summary>
        private static long? FindCustomerID(EntityField2 customerIDField, IPredicate predicate, SqlAdapter adapter)
        {
            if (predicate == null)
            {
                return null;
            }

            return FindCustomerID(customerIDField, new RelationPredicateBucket(predicate), adapter);
        }

        /// <summary>
        /// Find a CustomerID, searching using the specified predicate
        /// </summary>
        private static long? FindCustomerID(EntityField2 customerIDField, RelationPredicateBucket bucket, SqlAdapter adapter)
        {
            if (bucket == null)
            {
                return null;
            }

            // We need to try to pull the CustomerID
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(customerIDField, 0, "CustomerID", "");

            // Do the fetch
            DataTable result = new DataTable();
            adapter.FetchTypedList(resultFields, result, bucket, 1, true);

            if (result.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return (long) result.Rows[0][0];
            }
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
        private static long CreateCustomer(OrderEntity order, SqlAdapter adapter)
        {
            CustomerEntity customer = new CustomerEntity();

            PersonAdapter.Copy(order, customer, "Bill");
            PersonAdapter.Copy(order, customer, "Ship");

            customer.RollupOrderCount = 0;
            customer.RollupOrderTotal = 0;
            customer.RollupNoteCount = 0;

            adapter.SaveEntity(customer);

            // Entity will be OutOfSync, so we have to get the field directly
            return (long) customer.Fields[(int) CustomerFieldIndex.CustomerID].CurrentValue;
        }
    }
}
