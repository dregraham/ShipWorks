using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Filters
{
    [ConditionElement("ProStores Authorization", "ProStoresOrder.Authorization")]
    [ConditionStoreType(StoreTypeCode.ProStores)]
    public class ProStoresAuthorizationCondition : DateCondition
    {
        ProStoresAuthorizationStatus authorizedStatus = ProStoresAuthorizationStatus.Authorized;

        /// <summary>
        /// Create the custom editor for the condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new ProStoresAuthorizationConditionEditor(this);
        }

        /// <summary>
        /// Controls if the user wants to filter on if it is or is not authorized
        /// </summary>
        public ProStoresAuthorizationStatus AuthorizationStatus
        {
            get { return authorizedStatus; }
            set { authorizedStatus = value; }
        }

        /// <summary>
        /// Generate the SQL for the column
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> ProStoresOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ProStoresOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                switch (authorizedStatus)
                {
                    case ProStoresAuthorizationStatus.Authorized:
                        {
                            return scope.Adorn(string.Format("{0} IS NOT NULL", context.GetColumnReference(ProStoresOrderFields.AuthorizedDate)));
                        }

                    case ProStoresAuthorizationStatus.NotAuthorized:
                        {
                            return scope.Adorn(string.Format("{0} IS NULL", context.GetColumnReference(ProStoresOrderFields.AuthorizedDate)));
                        }

                    case ProStoresAuthorizationStatus.AuthorizedOn:
                        {
                            return scope.Adorn(GenerateSql(context.GetColumnReference(ProStoresOrderFields.AuthorizedDate), context));
                        }
                }

                throw new InvalidOperationException("Invalid authorizationStatus value: " + authorizedStatus);
            }
        }
    }
}
