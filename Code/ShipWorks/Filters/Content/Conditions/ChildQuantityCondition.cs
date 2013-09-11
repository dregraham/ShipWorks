using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Allows for counting the number of children 
    /// </summary>
    public abstract class ChildQuantityCondition : ContainerCondition
    {
        // Private condition for transfering our values to and from editors
        class QuantityCondition : NumericCondition<int>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public QuantityCondition()
            {
                // Can't have a a count lower than zero
                minimumValue = 0;
            }

            /// <summary>
            /// Generate the SQL
            /// </summary>
            public override string GenerateSql(SqlGenerationContext context)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Our own special version of GenerateSql that forces in the valueExpression
            /// </summary>
            public string GenerateSql(SqlGenerationContext context, string valueExpression)
            {
                string result = base.GenerateSql(valueExpression, context);

                return result;
            }
        }

        QuantityCondition condition = new QuantityCondition();

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            Func<string, string> quantityAdorner = (valueExpression) => { return condition.GenerateSql(context, valueExpression); };

            if (EntityType != null)
            {
                string targetScopeChildPredicate = GetTargetScopeChildPredicate(context);

                if (targetScopeChildPredicate != null)
                {
                    using (SqlGenerationScope scope = context.PushScope(EntityType.Value, targetScopeChildPredicate, SqlGenerationScopeType.QuantityOfChild, quantityAdorner))
                    {
                        return scope.Adorn(base.GenerateSql(context));
                    }
                }
                else
                {
                    using (SqlGenerationScope scope = context.PushScope(EntityType.Value, SqlGenerationScopeType.QuantityOfChild, quantityAdorner))
                    {
                        return scope.Adorn(base.GenerateSql(context));
                    }
                }
            }
            else
            {
                using (SqlGenerationScope scope = context.PushScope(CurrentScopeField, TargetScopeField, SqlGenerationScopeType.QuantityOfChild, quantityAdorner))
                {
                    return scope.Adorn(base.GenerateSql(context));
                }
            }
        }

        /// <summary>
        /// Gets the EntityType this scope represents.  can be null if CurrentScopeField and TargetScopeField are used.
        /// </summary>
        protected virtual EntityType? EntityType
        {
            get { return null; }
        }

        /// <summary>
        /// A custom child predicate used to get to the target scope.  EntityType needs to be non-null
        /// </summary>
        protected virtual string GetTargetScopeChildPredicate(SqlGenerationContext context)
        {
            return null;
        }

        /// <summary>
        /// Paired with TargetScopeField, and use when there is no direct releation between the current scope and EntityType.  EntityType should remain null if using this.
        /// </summary>
        protected virtual EntityField2 CurrentScopeField
        {
            get { return null; }
        }

        /// <summary>
        /// Paired with CurrentScopeField, and use when there is no direct releation between the current scope and EntityType.  EntityType should remain null if using this.
        /// </summary>
        protected virtual EntityField2 TargetScopeField
        {
            get { return null; }
        }


        /// <summary>
        /// Primary value to be evaluated.
        /// </summary>
        public int Value1
        {
            get
            {
                return condition.Value1;
            }
            set
            {
                condition.Value1 = value;
            }
        }

        /// <summary>
        /// Second value to be evaluated.  Only used for the Between \ NotBetween operators.
        /// </summary>
        public int Value2
        {
            get
            {
                return condition.Value2;
            }
            set
            {
                condition.Value2 = value;
            }
        }

        /// <summary>
        /// The operator used for the comparison.
        /// </summary>
        public NumericOperator Operator
        {
            get
            {
                return condition.Operator;
            }
            set
            {
                condition.Operator = value;
            }
        }


        /// <summary>
        /// Create the editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new NumericValueEditor<int>(condition);
        }
    }
}
