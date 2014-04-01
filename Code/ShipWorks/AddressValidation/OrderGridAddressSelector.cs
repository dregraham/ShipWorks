using System.Windows.Forms;
using ShipWorks.Data.Grid;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Responsible for displaying possible validated addresses
    /// </summary>
    public class OrderGridAddressSelector
    {
        /// <summary>
        /// Display the list of available addresses
        /// </summary>
        public void ShowAddressOptionMenu(object sender, GridHyperlinkClickEventArgs e)
        {
            //TODO: This is just example code (WIP)
            ContextMenu menu = new ContextMenu(new[]
            {
                new MenuItem("1 Memorial Drive, Suite 2000, St. Louis, MO 63102 (Original)", (s, args) => { }),
                new MenuItem("-"), 
                new MenuItem("One Memorial Drive, Ste 2000, St. Louis, MO 63102", (s, args) => { })
            });

            using (menu)
            {
                menu.Show((Control)sender, e.MouseArgs.Location);   
            }
        }
    }
}
