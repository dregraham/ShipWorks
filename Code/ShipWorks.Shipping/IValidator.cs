using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for classes that need to validate something
    /// </summary>
    /// <typeparam name="TEntity">The object to be validated.</typeparam>
    public interface IValidator<TEntity>
    {
        /// <summary>
        /// Validate an object asychronously.
        /// </summary>
        Task ValidateAsync(TEntity entity);
    }
}
