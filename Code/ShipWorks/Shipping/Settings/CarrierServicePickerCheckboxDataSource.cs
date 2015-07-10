using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Settings
{
    [CLSCompliant(false)]
    public class CarrierServicePickerCheckBoxDataSource<T> where T: struct, IConvertible
    {
        public CarrierServicePickerCheckBoxDataSource(T serviceType)
        {
            if (!typeof(T).IsEnum)
            {
                // This is to address the fact that a generic cannot be created specifically
                // for an enumeration
                throw new InvalidOperationException("T must be an enumeration");
            }

            Value = serviceType;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return EnumHelper.GetDescription(Value as Enum);
        }

        public T Value { get; private set; }
    }
}
