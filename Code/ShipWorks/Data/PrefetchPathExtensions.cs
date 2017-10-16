using System.Collections.Immutable;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Data
{
    /// <summary>
    /// Extensions on Prefetch Paths
    /// </summary>
    public static class PrefetchPathExtensions
    {
        /// <summary>
        /// Get a PrefetchPathContainer for the given path
        /// </summary>
        public static PrefetchPathContainer ToContainer(this IPrefetchPathElement2 path) =>
            new PrefetchPathContainer(path, ImmutableList.Create<PrefetchPathContainer>());

        /// <summary>
        /// Get a PrefetchPathContainer with the given child for the given path
        /// </summary>
        public static PrefetchPathContainer WithChild(this IPrefetchPathElement2 path, IPrefetchPathElement2 child) =>
            new PrefetchPathContainer(path, ImmutableList.Create<PrefetchPathContainer>(child.ToContainer()));

        /// <summary>
        /// Get a PrefetchPathContainer with the given child for the given path
        /// </summary>
        public static PrefetchPathContainer WithChild(this IPrefetchPathElement2 path, PrefetchPathContainer child) =>
            new PrefetchPathContainer(path, ImmutableList.Create<PrefetchPathContainer>(child));
    }
}
