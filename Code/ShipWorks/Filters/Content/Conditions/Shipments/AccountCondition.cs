using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.FactoryClasses;
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
    public class AccountCondition : ValueChoiceCondition<string>
    {
        private readonly IShippingManager shippingManager;
        private readonly ILifetimeScope scope;
        private readonly ISqlAdapterFactory sqlAdapter;

        public AccountCondition()
        {
            scope = IoC.BeginLifetimeScope();
            shippingManager = scope.Resolve<IShippingManager>();
            sqlAdapter = new SqlAdapterFactory();

            Value = string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountCondition(IShippingManager shippingManager, ISqlAdapterFactory sqlAdapter)
        {

            this.shippingManager = shippingManager;
            this.sqlAdapter = sqlAdapter;
            Value = string.Empty;
        }

        ///// <summary>
        ///// Provides the choices for the user to choose from  This is a list of all shipping accounts that currently
        ///// exist in the system.
        ///// </summary>
        public override ICollection<ValueChoice<string>> ValueChoices
        {
            get
            {
                var sort = ShipmentFields.CarrierAccount.Ascending();
                var query = new QueryFactory().Shipment
                    .Select(() => ShipmentFields.CarrierAccount.ToValue<string>())
                    .Distinct()
                    .OrderBy(sort);
                var values = sqlAdapter.Create().FetchQuery(query);

                List<ValueChoice<string>> choices = new List<ValueChoice<string>>();
                foreach(var account in values)
                {
                    if (!string.IsNullOrEmpty(account))
                    {
                        choices.Add(new ValueChoice<string>(account, account));
                    }                    
                }
                scope?.Dispose();
                return choices;
            }
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Register the parameters
            string accountParam = context.RegisterParameter(Value);
            string processedParam = context.RegisterParameter(true);

            return $"{context.GetColumnReference(ShipmentFields.CarrierAccount)} {GetSqlOperator()} {accountParam} and {context.GetColumnReference(ShipmentFields.Processed)} = {processedParam}";
        }       
    }
}

