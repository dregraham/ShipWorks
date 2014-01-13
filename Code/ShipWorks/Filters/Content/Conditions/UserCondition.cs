using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Users;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Condition base for conditions that compare against ShipWorks users
    /// </summary>
    public abstract class UserCondition : ValueChoiceCondition<long>
    {
        /// <summary>
        /// Provides the choices for the user to choose from  This is a list of all store-types that currently
        /// exist in the system.
        /// </summary>
        public override ICollection<ValueChoice<long>> ValueChoices
        {
            get
            {
                List<ValueChoice<long>> choices = new List<ValueChoice<long>>();

                // Add in all the users
                foreach (UserEntity user in UserManager.GetUsers(false))
                {
                    choices.Add(new ValueChoice<long>(user.Username, user.UserID));
                }

                // There will always be one user
                if (Value == 0)
                {
                    Value = choices[0].Value;
                }

                // Now they have to be sorted
                choices.Sort(new Comparison<ValueChoice<long>>(
                    delegate(ValueChoice<long> left, ValueChoice<long> right)
                    {
                        return left.Name.CompareTo(right.Name);
                    }));

                return choices;
            }
        }
    }
}
