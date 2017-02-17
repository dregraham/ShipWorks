using System.Collections.Generic;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Row selector for the entity grid
    /// </summary>
    public static class EntityGridRowSelector
    {
        private static IEntityGridRowSelector first = new FirstEntityGridRowSelector();
        private static IEntityGridRowSelector all = new AllEntityGridRowSelector();

        /// <summary>
        /// Select first entity row
        /// </summary>
        public static IEntityGridRowSelector First => first;

        /// <summary>
        /// Select all entity rows
        /// </summary>
        public static IEntityGridRowSelector All => all;

        /// <summary>
        /// Select specific entities
        /// </summary>
        public static IEntityGridRowSelector SpecificEntities(IEnumerable<long> keys) =>
            new SpecificEntityGridRowSelector(keys);
    }
}
