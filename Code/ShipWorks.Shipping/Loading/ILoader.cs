using System.Threading.Tasks;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Interface for classes that need to load something
    /// </summary>
    /// <typeparam name="TResult">The resulting type of a load operation.</typeparam>
    public interface ILoader<TResult> 
    {
        /// <summary>
        /// Asyc load method.
        /// </summary>
        Task<TResult> LoadAsync(long entityID);
    }
}
