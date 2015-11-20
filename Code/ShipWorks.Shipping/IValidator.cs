using System.Threading.Tasks;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for classes that need to validate something
    /// </summary>
    /// <typeparam name="TEntity">The object to be validated.</typeparam>
    public interface IValidator<in TEntity>
    {
        /// <summary>
        /// Validate an object asynchronously.
        /// </summary>
        Task ValidateAsync(TEntity entity);
    }
}
