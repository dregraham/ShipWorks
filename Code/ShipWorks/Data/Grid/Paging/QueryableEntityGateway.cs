using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// A PagedEntityGateway that provides support for specifying the query to use
    /// </summary>
    public class QueryableEntityGateway : PagedEntityGateway
    {
        RelationPredicateBucket queryBucket = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryableEntityGateway(EntityType entityType, RelationPredicateBucket queryBucket)
            : this(new DataProviderEntityProvider(entityType), queryBucket)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public QueryableEntityGateway(IEntityProvider entityProvider, RelationPredicateBucket queryBucket)
            : base(entityProvider)
        {
            this.queryBucket = queryBucket;
        }

        /// <summary>
        /// Constructor for cloning
        /// </summary>
        private QueryableEntityGateway(QueryableEntityGateway copy) 
            : base(copy)
        {
            this.queryBucket = EntityUtility.ClonePredicateBucket(copy.queryBucket);
        }

        /// <summary>
        /// Make an exact clone of the state of the gateway
        /// </summary>
        public override IEntityGateway Clone()
        {
            return new QueryableEntityGateway(this);
        }

        /// <summary>
        /// Overridden to provide the query that the gateway will use to select rows
        /// </summary>
        protected override RelationPredicateBucket GetQueryFilter()
        {
            return queryBucket;
        }
    }
}
