using Autofac;
using Autofac.Features.OwnedInstances;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build a carrier account
    /// </summary>
    public class CarrierAccountEntityBuilder<T> : EntityBuilder<T> where T : EntityBase2, ICarrierAccount, new()
    {
        /// <summary>
        /// Save the carrier account
        /// </summary>
        /// <remarks>After saving, we need to make sure the account repository updates itself</remarks>
        public override T Save(SqlAdapter adapter)
        {
            T entity = base.Save(adapter);
            ICarrierAccountRepository<T> repository = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<ICarrierAccountRepository<T>>>().Value;
            repository.CheckForChangesNeeded();
            return entity;
        }
    }
}