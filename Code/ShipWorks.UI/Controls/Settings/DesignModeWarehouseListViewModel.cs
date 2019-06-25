using System.Collections.Generic;
using System.Reflection;
using ShipWorks.ApplicationCore.Settings.Warehouse;

namespace ShipWorks.UI.Controls.Settings
{
    public class DesignModeWarehouseListViewModel
    {
        public DesignModeWarehouseListViewModel()
        {
            Warehouses = new List<WarehouseViewModel>
            {
                new WarehouseViewModel { Id = "abc-123", Name = "Saint Louis Main", City = "St. Louis", IsAlreadyLinked = false, State = "MO", Street = "1 Memorial Dr.", Zip = "63102-1234" },
                new WarehouseViewModel { Id = "hij-456", Name = "Secondary", City = "Beverly Hills", IsAlreadyLinked = true, State = "CA", Street = "123 Bar", Zip = "90210" },
                new WarehouseViewModel { Id = "xyz-789", Name = "Backup", City = "Atlanta", IsAlreadyLinked = false, State = "GA", Street = "123 Foo", Zip = "43218" },
            };
        }

        /// <summary>
        /// Warehouse that's been selected
        /// </summary>
        [Obfuscation]
        public IWarehouseViewModel SelectedWarehouse { get; set; }

        /// <summary>
        /// List of warehouses from which to choose
        /// </summary>
        [Obfuscation]
        public IEnumerable<IWarehouseViewModel> Warehouses { get; set; }
    }
}
