using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

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
                if (order == null)
                {
                    continue;
                }

                validator.Validate(order, "Ship", (originalAddress, suggestedAddresses) =>
                {
                    DeleteExistingAddresses(context, order);
                    SaveAddress(context, order, originalAddress, true);

                    foreach (AddressEntity address in suggestedAddresses)
                    {
                        SaveAddress(context, order, address, false);
                    }

                    context.CommitWork.AddForSave(order);
                });
            }
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        private static void DeleteExistingAddresses(ActionStepContext context, OrderEntity order)
        {
            List<ValidatedAddressEntity> addressesToDelete;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                // Retrieve the addresses 
                LinqMetaData metaData = new LinqMetaData(adapter);
                addressesToDelete = metaData.ValidatedAddress.Where(x => x.ConsumerID == order.OrderID).ToList();
            }

            // Mark each address for deletion
            addressesToDelete.ForEach(x =>
            {
                context.CommitWork.AddForDelete(x);
                context.CommitWork.AddForDelete(new AddressEntity { AddressID = x.AddressID, IsNew = false });
            });
        }

        /// <summary>
        /// Save a validated address
        /// </summary>
        private static void SaveAddress(ActionStepContext context, OrderEntity order, AddressEntity address, bool isOriginalAddress)
        {
            // If the address is null, we obviously don't need to save it
            if (address == null)
            {
                return;
            }

            ValidatedAddressEntity validatedAddressEntity = new ValidatedAddressEntity
            {
                ConsumerID = order.OrderID,
                Address = address,
                IsOriginal = isOriginalAddress
            };

            context.CommitWork.AddForSave(validatedAddressEntity);
        }
    }
}
