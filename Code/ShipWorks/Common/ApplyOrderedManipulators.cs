using System;
using System.Linq;
using Interapptive.Shared.Collections;

namespace ShipWorks.Common
{
    /// <summary>
    /// Apply an ordered set of manipulators
    /// </summary>
    public class ApplyOrderedManipulators<T, K> : IApplyOrderedManipulators<T, K> where T : IManipulator<K>
    {
        private readonly IOrderedEnumerable<T> manipulators;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyOrderedManipulators(IOrderedEnumerable<T> manipulators)
        {
            this.manipulators = manipulators;
        }

        /// <summary>
        /// Apply the given manipulators
        /// </summary>
        public K Apply(K input)
        {
            if (manipulators.None())
            {
                throw new InvalidOperationException($"No manipulators were registered for type {typeof(T).Name}");
            }

            return manipulators.Aggregate(input, (acc, manipulator) => manipulator.Manipulate(acc));
        }
    }
}
