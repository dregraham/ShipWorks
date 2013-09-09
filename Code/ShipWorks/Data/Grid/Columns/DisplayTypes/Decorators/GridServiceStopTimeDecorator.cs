using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;


namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Decorates a service last stopped time.
    /// </summary>
    public class GridServiceStopTimeDecorator : GridColumnDisplayDecorator
    {
        /// <summary>
        /// Grays out the column value if the LastStopDateTime is before the LastStartDateTime.
        /// </summary>
        public override void ApplyDecoration(GridColumnFormattedValue formattedValue)
        {
            var service = (ServiceStatusEntity)formattedValue.Entity;

            if (service.LastStopDateTime < service.LastStartDateTime)
            {
                formattedValue.ForeColor = Color.DarkGray;
            }
        }
    }
}
