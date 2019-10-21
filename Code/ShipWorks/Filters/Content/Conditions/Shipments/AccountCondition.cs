using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Condition base on the carrier account of an shipment
    /// </summary>
    [ConditionElement("Account", "Shipment.Account")]
    public class AccountCondition : ValueChoiceCondition<long>
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingManager shippingManager;
        private readonly IShippingAccountListProvider shippingAccountListProvider;
        private readonly ILifetimeScope scope;

        public AccountCondition()
        {
            scope = IoC.BeginLifetimeScope();
            shipmentTypeManager = scope.Resolve<IShipmentTypeManager>();
            shippingManager = scope.Resolve<IShippingManager>();
            shippingAccountListProvider = scope.Resolve<IShippingAccountListProvider>();
            Value = -1;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountCondition(IShipmentTypeManager shipmentTypeManager, IShippingManager shippingManager)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingManager = shippingManager;
            Value = -1;
        }

        ///// <summary>
        ///// Provides the choices for the user to choose from  This is a list of all store-types that currently
        ///// exist in the system.
        ///// </summary>
        public override ICollection<ValueChoice<long>> ValueChoices
        {
            get
            {
                List<ValueChoice<long>> choices = new List<ValueChoice<long>>();
                var carriers = GetCarriers();
                foreach(var carrier in carriers)
                {
                    IEnumerable<ICarrierAccount> availableAccounts = shippingAccountListProvider.GetAvailableAccounts(carrier);
                    foreach(var account in availableAccounts)
                    {
                        if(account.AccountId != 0)
                        {
                            choices.Add(new ValueChoice<long>(account.AccountDescription, account.AccountId));
                        }                     
                    }
                }
                return choices;
            }
        }

        private ShipmentTypeCode[] GetCarriers()
        {
            var result = shipmentTypeManager.ShipmentTypes
                     .Where(t => t.ShipmentTypeCode != ShipmentTypeCode.BestRate)
                     .Where(
                         t =>
                             t.ShipmentTypeCode != ShipmentTypeCode.AmazonSFP ||
                             shippingManager.IsShipmentTypeConfigured(t.ShipmentTypeCode))
                     .Select(t => t.ShipmentTypeCode)
                     .ToArray();

            scope?.Dispose();

            return result;
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Register the parameters
            string accountParam = context.RegisterParameter(Value);
            string processedParam = context.RegisterParameter(true);

            return $"{context.GetColumnReference(ShipmentFields.CarrierAccountID)} {GetSqlOperator()} {accountParam} and {context.GetColumnReference(ShipmentFields.Processed)} = {processedParam}";
        }       
    }
}

