using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Represents the initial download policy of a store.
    /// </summary>
    public class InitialDownloadPolicy
    {
        InitialDownloadRestrictionType restriction;

        /// <summary>
        /// Constructor
        /// </summary>
        public InitialDownloadPolicy(InitialDownloadRestrictionType restriction)
        {
            this.restriction = restriction;

            MaxDaysBack = null;
            DefaultDaysBack = 30;
            DefaultStartingOrderNumber = 0;
        }

        /// <summary>
        /// The type of restriction
        /// </summary>
        public InitialDownloadRestrictionType RestrictionType
        {
            get { return restriction; }
        }

        /// <summary>
        /// If the restriction type is DaysBack, this controls the max days back allowed.
        /// </summary>
        public int? MaxDaysBack
        {
            get;
            set;
        }

        /// <summary>
        /// The default number of days to go back when the restriction is DaysBack
        /// </summary>
        public int DefaultDaysBack
        {
            get;
            set;
        }

        /// <summary>
        /// The default order number to use as the starting point for order number based downloads
        /// </summary>
        public long DefaultStartingOrderNumber
        {
            get;
            set;
        }
    }
}
