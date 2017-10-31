using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Tests.Shared.ExtensionMethods
{
    /// <summary>
    /// Shorten the standard It.Is[T] methods
    /// </summary>
    public static class ParameterShorteners
    {
        /// <summary>
        /// Any string
        /// </summary>
        public static string AnyString => It.IsAny<string>();

        /// <summary>
        /// Any bool
        /// </summary>
        public static bool AnyBool => It.IsAny<bool>();

        /// <summary>
        /// Any long
        /// </summary>
        public static long AnyLong => It.IsAny<long>();

        /// <summary>
        /// Any int
        /// </summary>
        public static int AnyInt => It.IsAny<int>();

        /// <summary>
        /// Any date
        /// </summary>
        public static DateTime AnyDate => It.IsAny<DateTime>();

        /// <summary>
        /// Any object
        /// </summary>
        public static object AnyObject => It.IsAny<object>();

        /// <summary>
        /// Any shipment
        /// </summary>
        public static ShipmentEntity AnyShipment => It.IsAny<ShipmentEntity>();

        /// <summary>
        /// Any shipment
        /// </summary>
        public static IShipmentEntity AnyIShipment => It.IsAny<IShipmentEntity>();

        /// <summary>
        /// Checks a parameter for a shipment with given tracking number
        /// </summary>
        public static ShipmentEntity ShipmentWithTrackingNumber(string trackingNumber) =>
            It.Is<ShipmentEntity>(s => s.TrackingNumber == trackingNumber);

        /// <summary>
        /// Checks that a parameter is an enumerable that contains the specified items in any order
        /// </summary>
        public static IEnumerable<T> UnorderedEnumerable<T>(params T[] actual) =>
            It.Is<IEnumerable<T>>(l => l.UnorderedSequenceEquals(actual));
    }
}
