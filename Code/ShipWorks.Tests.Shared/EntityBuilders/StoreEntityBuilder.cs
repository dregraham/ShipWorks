using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Builder to create a new store
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoreEntityBuilder<T> : EntityBuilder<T> where T : StoreEntity, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreEntityBuilder(T store) : base(store)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreEntityBuilder()
        {
            Set(x => x.SetupComplete, true);
        }

        /// <summary>
        /// Set the address on the store
        /// </summary>
        public StoreEntityBuilder<T> WithAddress(string address1, string address2, string city, string state, string postalCode, string country) =>
            WithAddress(x => x.Address, address1, address2, city, state, postalCode, country) as StoreEntityBuilder<T>;

        /// <summary>
        /// Save the entity
        /// </summary>
        public override T Save(SqlAdapter adapter)
        {
            T value = base.Save(adapter);

            // Make sure the new store is seen by the store manager
            StoreManager.CheckForChanges();

            return value;
        }
    }
}