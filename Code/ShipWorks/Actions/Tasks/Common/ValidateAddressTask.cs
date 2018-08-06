﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task that validates the shipping address of orders
    /// </summary>
    [ActionTask("Validate order shipping address", "ValidateAddress", ActionTaskCategory.UpdateLocally)]
    public class ValidateAddressTask : ActionTask
    {
        /// <summary>
        /// Create the editor for editing the settings of the task
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new ValidateAddressTaskEditor();
        }

        /// <summary>
        /// How to label the input selection for the task
        /// </summary>
        public override string InputLabel
        {
            get { return "Validate shipping address of:"; }
        }

        /// <summary>
        /// This task only operates on orders
        /// </summary>
        public override EntityType? InputEntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// Run the task over the given input
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            AddressValidator validator = new AddressValidator();

            foreach (long orderID in inputKeys)
            {
                OrderEntity order = DataProvider.GetEntity(orderID) as OrderEntity;

                // If the address has already been validated, don't bother validating it again
                if (order == null)
                {
                    continue;
                }

                StoreEntity store = StoreManager.GetRelatedStore(order.OrderID);
                if (store == null)
                {
                    continue;
                }

                if (!AddressValidationPolicy.ShouldManuallyValidate(store, new AddressAdapter(order, "Ship")))
                {
                    continue;
                }

                AddressAdapter originalShippingAddress = new AddressAdapter();
                AddressAdapter.Copy(order, "Ship", originalShippingAddress);

                try
                {
                    Task task = validator.ValidateAsync(order, store, "Ship", true, (originalAddress, suggestedAddresses) =>
                        ValidatedAddressManager.SaveValidatedOrder(context, new ValidatedOrderShipAddress(order, originalAddress, suggestedAddresses, originalShippingAddress)));
                    task.Wait();
                }
                catch (AddressValidationException ex)
                {
                    throw new ActionTaskRunException("Error validating address", ex);
                }
            }
        }

        /// <summary>
        /// Commit the database changes
        /// </summary>
        public override async Task Commit(List<long> inputKeys, ActionStepContext context, IDataAccessAdapter adapter)
        {
            try
            {
                await base.Commit(inputKeys, context, adapter).ConfigureAwait(false);
            }
            catch (ORMConcurrencyException ex)
            {
                // If the order has changed since validation begun, throw a task exception
                throw new ActionTaskRunException("Order was changed after validation begun. Try re-running the action.", ex);
            }
        }
    }
}