using Divelements.SandGrid.Rendering;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;

namespace ShipWorks.ApplicationCore.Services.UI
{
    /// <summary>
    /// Customized grid row for displaying services.
    /// </summary>
    public class ServiceGridRow : PagedEntityGrid.PagedEntityGridRow
    {
        /// <summary>
        /// Draws an error highlight if the service is stopped and there are eligible actions targeting the machine.
        /// </summary>
        /// <param name="context"></param>
        protected override void DrawRowBackground(RenderingContext context)
        {
            base.DrawRowBackground(context);

            var service = Entity as ServiceStatusEntity;

            if (null != service && service.GetStatus() != ServiceStatus.Running && service.IsRequiredToRun())
            {
                context.Graphics.FillRectangle(Brushes.Pink, Bounds);
            }
        }
    }
}
