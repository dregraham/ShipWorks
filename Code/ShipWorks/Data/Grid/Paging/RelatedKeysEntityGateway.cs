using System;
using System.Collections.Generic;
using ShipWorks.Data.Model;
using ShipWorks.Data.Utility;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// A PagedEntityGateway that provides support for specifying the query to use
    /// </summary>
    public class RelatedKeysEntityGateway : PagedEntityGateway
    {
        List<long> sourceKeys = new List<long>();
        EntityType relatedKeyType;

        /// <summary>
        /// Constructor
        /// </summary>
        public RelatedKeysEntityGateway(long sourceKey, EntityType relatedKeyType)
            : this(new List<long> { sourceKey }, relatedKeyType)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RelatedKeysEntityGateway(List<long> sourceKeys, EntityType relatedKeyType)
            : base(relatedKeyType)
        {
            if (sourceKeys == null)
            {
                throw new ArgumentNullException("sourceKeys");
            }

            this.sourceKeys = sourceKeys;
            this.relatedKeyType = relatedKeyType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected RelatedKeysEntityGateway(RelatedKeysEntityGateway copy)
            : base(copy)
        {
            this.sourceKeys = copy.sourceKeys;
            this.relatedKeyType = copy.relatedKeyType;
        }

        /// <summary>
        /// Make an exact clone of the state of the gateway
        /// </summary>
        public override IEntityGateway Clone()
        {
            return new RelatedKeysEntityGateway(this);
        }

        /// <summary>
        /// Get the sorted list of all keys for the configured gateway
        /// </summary>
        protected override PagedSortedKeys QuerySortedKeys(SortDefinition sortDefinition)
        {
            return new PagedSortedKeys(sourceKeys, relatedKeyType, sortDefinition);
        }
    }
}
