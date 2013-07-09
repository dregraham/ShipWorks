using Divelements.SandGrid.Rendering;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Customized grid row for displaying Windows services.
    /// </summary>
    public class WindowsServiceGridRow : PagedEntityGrid.PagedEntityGridRow
    {
        /// <summary>
        /// Draws an error highlight if the service is stopped and there are eligible actions targeting the machine.
        /// </summary>
        /// <param name="context"></param>
        protected override void DrawRowBackground(RenderingContext context)
        {
            base.DrawRowBackground(context);

            var service = Entity as WindowsServiceEntity;

            if (null != service && service.GetStatus() != ServiceStatus.Running && service.IsRequiredToRun())
            {
                context.Graphics.FillRectangle(Brushes.Pink, Bounds);
            }
        }
    }
}
