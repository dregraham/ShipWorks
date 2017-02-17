using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common
{
    /// <summary>
    /// Apply a set of validators
    /// </summary>
    public class CompositeValidator<T, K> : ICompositeValidator<T, K> where T : IValidator<K>
    {
        readonly IEnumerable<T> validators;

        /// <summary>
        /// Constructor
        /// </summary>
        public CompositeValidator(IEnumerable<T> validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Apply the validators, collecting the results
        /// </summary>
        public ICompositeValidatorResult Apply(K input)
        {
            if (validators.None())
            {
                throw new InvalidOperationException($"No validators were registered for type {typeof(T).Name}");
            }

            IEnumerable<Result> results = validators.Select(x => x.Validate(input)).Where(x => x.Failure).ToArray();

            return results.Any() ?
                new CompositeValidatorResult(false, results.Select(x => x.Message)) :
                new CompositeValidatorResult(true, Enumerable.Empty<string>());
        }
    }
}
