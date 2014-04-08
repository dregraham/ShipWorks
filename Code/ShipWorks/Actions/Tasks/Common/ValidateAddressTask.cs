using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

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
                
                PersonAdapter originalShippingAddress = new PersonAdapter();
                PersonAdapter.Copy(order, "Ship", originalShippingAddress);
                

                // If the address has already been validated, don't bother validating it again
                if (order == null ||
                    (order.ShipAddressValidationStatus != (int)AddressValidationStatusType.NotChecked &&
                     order.ShipAddressValidationStatus != (int)AddressValidationStatusType.Pending))
                {
                    return;
                }

                try
                {
                    validator.Validate(order, "Ship", (originalAddress, suggestedAddresses) =>
                    {
                        ValidatedAddressManager.DeleteExistingAddresses(context, order.OrderID);
                        ValidatedAddressManager.SaveOrderAddress(context, order, originalAddress, true);

                        foreach (AddressEntity address in suggestedAddresses)
                        {
                            ValidatedAddressManager.SaveOrderAddress(context, order, address, false);
                        }

                        order.ShipAddressValidationSuggestionCount = suggestedAddresses.Count();
                        context.CommitWork.AddForSave(order);

                        ValidatedAddressManager.PropagateAddressChangesToShipments(order.OrderID, originalShippingAddress, new PersonAdapter(order, "Ship"), context);
                    });
                }
                catch (AddressValidationException ex)
                {
                    throw new ActionTaskRunException("Error validating address", ex);
                }
            }
        }
    }
}