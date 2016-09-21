using System;
using System.ComponentModel;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extend functionality of the generated FilterNodeEntity
    /// </summary>
    public partial class FilterNodeEntity
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterNodeEntity));

        /// <summary>
        /// The filter the FilterNode represents.
        /// </summary>
        public FilterEntity Filter
        {
            get
            {
                return FromFilterSequence(x => x.Filter);
            }
        }

        /// <summary>
        /// The filter the FilterNode represents.
        /// </summary>
        public long FilterID
        {
            get
            {
                return FromFilterSequence(x => x.FilterID);
            }
        }

        /// <summary>
        /// Gets a property from the the FilterSequence property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retrievalAction"></param>
        /// <returns></returns>
        private T FromFilterSequence<T>(Func<FilterSequenceEntity, T> retrievalAction)
        {
            FilterSequenceEntity sequence = FilterSequence;

            if (sequence != null)
            {
                return retrievalAction(sequence);
            }

            log.Warn("FilterSequence is null");
            return default(T);
        }

        /// <summary>
        /// A related entity is being set
        /// </summary>
        protected override void OnRelatedEntitySet(IEntityCore relatedEntity, string fieldName)
        {
            base.OnRelatedEntitySet(relatedEntity, fieldName);

            if (fieldName == "FilterSequence" || fieldName == "ParentNode")
            {
                // We have to maintain the Filter\FilterSequence relationship ourselves, since we don't prefetch using
                // that relationship.
                if (ParentNode != null && FilterSequence != null)
                {
                    if (!ParentNode.Filter.ChildSequences.Contains(FilterSequence))
                    {
                        ParentNode.Filter.ChildSequences.Add(FilterSequence);
                        ParentNode.Filter.ChildSequences.Sort((int) FilterSequenceFieldIndex.Position, ListSortDirection.Ascending);
                    }
                }
            }
        }

        /// <summary>
        /// A related entity is being unset
        /// </summary>
        protected override void OnRelatedEntityUnset(IEntityCore relatedEntity, string fieldName)
        {
            base.OnRelatedEntityUnset(relatedEntity, fieldName);

            if (fieldName == "FilterSequence")
            {
                // We have to maintain the Filter\FilterSequence relationship ourselves, since we don't prefetch using
                // that relationship.
                if (ParentNode != null)
                {
                    FilterSequenceEntity sequence = (FilterSequenceEntity) relatedEntity;

                    if (sequence.NodesUsingSequence.Count == 0)
                    {
                        ParentNode.Filter.ChildSequences.Remove(sequence);
                    }
                }
            }

            if (fieldName == "ParentNode")
            {
                if (FilterSequence != null)
                {
                    ((FilterNodeEntity) relatedEntity).Filter.ChildSequences.Remove(FilterSequence);
                }
            }
        }
    }
}
