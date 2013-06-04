using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;
using System.Xml.Serialization;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that provide the user with a list of choices from which to choose a value
    /// </summary>
    public abstract class BooleanCondition : ValueChoiceCondition<bool>
    {
        string trueText;
        string falseText;

        /// <summary>
        /// Construct an instance where the given text is displayed to the user representing true and false conditions
        /// </summary>
        protected BooleanCondition(string trueText, string falseText)
        {
            if (string.IsNullOrWhiteSpace(trueText))
            {
                throw new ArgumentException("trueText cannot be null or whitespace", "trueText");
            }

            if (string.IsNullOrWhiteSpace(falseText))
            {
                throw new ArgumentException("falseText cannot be null or whitespace", "falseText");
            }

            this.trueText = trueText;
            this.falseText = falseText;
        }

        /// <summary>
        /// Provides the choices for the user to choose from. This is TrueText and FalseText.
        /// </summary>
        public override ICollection<ValueChoice<bool>> ValueChoices
        {
            get
            {
                List<ValueChoice<bool>> valueChoices = new List<ValueChoice<bool>>();

                valueChoices.Add(new ValueChoice<bool>(trueText, true));
                valueChoices.Add(new ValueChoice<bool>(falseText, false));

                return valueChoices;
            }
        }
    }
}
