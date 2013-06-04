using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that provide options from a list enum values
    /// </summary>
    public abstract class EnumCondition<T> : ValueChoiceCondition<T> where T : struct
    {
        /// <summary>
        /// Get the value choices the user will be provided with
        /// </summary>
        public override ICollection<ValueChoice<T>> ValueChoices
        {
            get
            {
                return EnumHelper.GetEnumList<T>().Select(e => new ValueChoice<T>(e.Description, e.Value)).ToList();
            }
        }
    }
}
