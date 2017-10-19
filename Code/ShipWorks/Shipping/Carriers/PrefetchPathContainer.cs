using System.Collections.Generic;
using System.Collections.Immutable;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Container for prefetch paths that make merging them easier
    /// </summary>
    public class PrefetchPathContainer
    {
        private readonly IImmutableList<PrefetchPathContainer> children;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// This is internal because the preferable method of interacting with this 
        /// is through the ToContainer and WithChild extension methods.
        /// </remarks>
        internal PrefetchPathContainer(IPrefetchPathElement2 path, IImmutableList<PrefetchPathContainer> children)
        {
            Path = path;
            this.children = children;

            PathKey = path.PropertyName.GetHashCode() ^
                path.Relation.HintTargetNameLeftOperand.GetHashCode() ^
                path.Relation.HintTargetNameRightOperand.GetHashCode();
        }

        /// <summary>
        /// Key that identifies a path
        /// </summary>
        /// <remarks>Unfortunately, LLBLgen prefetch paths do not have consistent hash codes, so we'll rely on this.</remarks>
        public int PathKey { get; }

        /// <summary>
        /// Prefetch path
        /// </summary>
        public IPrefetchPathElement2 Path { get; }

        /// <summary>
        /// Children for this container
        /// </summary>
        public IEnumerable<PrefetchPathContainer> Children => children;

        /// <summary>
        /// Get a new container with the specified child added
        /// </summary>
        public PrefetchPathContainer WithChild(IPrefetchPathElement2 child) =>
            new PrefetchPathContainer(Path, children.Add(child.ToContainer()));

        /// <summary>
        /// Get a new container with the specified child added
        /// </summary>
        public PrefetchPathContainer WithChild(PrefetchPathContainer child) =>
            new PrefetchPathContainer(Path, children.Add(child));
    }
}
