using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for classes that need to load something
    /// </summary>
    /// <typeparam name="TResult">The resulting type of a load operation.</typeparam>
    /// <typeparam name="TEntity">The object from which something should be loaded.</typeparam>
    public interface ILoader<TResult, TEntity>
    {
        /// <summary>
        /// Asyc load method.
        /// </summary>
        Task<TResult> LoadAsync(TEntity entity);
    }
}
