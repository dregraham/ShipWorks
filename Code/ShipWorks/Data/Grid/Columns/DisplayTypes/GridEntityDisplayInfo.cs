using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Used for displaying entity data in the entity column
    /// </summary>
    public class GridEntityDisplayInfo
    {
        public GridEntityDisplayInfo()
        {

        }

        public GridEntityDisplayInfo(long entityID, EntityType? entityType, string displayText)
        {
            this.EntityID = entityID;
            this.EntityType = entityType;
            this.DisplayText = displayText;
        }

        public long EntityID
        {
            get;
            set;
        }

        public EntityType? EntityType 
        { 
            get; set; 
        }
        public string DisplayText 
        { 
            get; set; 
        }

        public bool IsDeleted
        {
            get;
            set;
        }
    }
}
