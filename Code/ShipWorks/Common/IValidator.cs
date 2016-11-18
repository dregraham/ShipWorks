using Interapptive.Shared.Utility;

namespace ShipWorks.Common
{
    /// <summary>
    /// Basic validator interface
    /// </summary>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validate the input
        /// </summary>
        Result Validate(T input);
    }
}
