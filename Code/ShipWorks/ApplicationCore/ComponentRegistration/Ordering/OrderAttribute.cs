using System;
using System.ComponentModel.Composition;

namespace ShipWorks.ApplicationCore.ComponentRegistration.Ordering
{
    /// <summary>
    /// Constants for use with the OrderAttribute
    /// </summary>
    public static class Order
    {
        /// <summary>
        /// Order is not specified
        /// </summary>
        /// <remarks>This is necessary because there are times when we care about the order of some
        /// dependencies, but not all. </remarks>
        public const int Unordered = int.MaxValue;
    }

    /// <summary>
    /// Specifies a registration order of a component
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="order"></param>
        public OrderAttribute(int order, Type forType)
        {
            Order = order;
            ForType = forType;
        }

        /// <summary>
        /// Order of registration
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Type of registration for which to apply the order
        /// </summary>
        public Type ForType { get; set; }
    }
}
