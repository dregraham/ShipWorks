using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Content.Panels.CoreExtensions.Grid
{
    /// <summary>
    /// Custom DisplayType for displaying Edit or View for shipments
    /// </summary>
    class GridShipmentEditViewDisplayType : GridActionDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridShipmentEditViewDisplayType()
            : base(string.Empty, GridLinkAction.Edit)
        {

        }

        /// <summary>
        /// Get the value to use for the entity
        /// </summary>
        protected override object GetEntityValue(EntityBase2 entity)
        {
            return entity;
        }

        /// <summary>
        /// Get the text to display in the column
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            ShipmentEntity entity = value as ShipmentEntity;
            if (entity == null)
            {
                return string.Empty;
            }

            // During shutdown, Security can be null
            if (UserSession.Security?.HasPermission(PermissionType.ShipmentsCreateEditProcess, entity.OrderID) != true)
            {
                return "View";
            }

            return (entity.Processed || entity.Voided) ? "View" : "Ship";
        }
    }
}
