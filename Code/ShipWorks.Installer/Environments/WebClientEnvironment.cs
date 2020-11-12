namespace ShipWorks.Installer.Environments
{
    /// <summary>
    /// Hub web client environment
    /// </summary>
    public class WebClientEnvironment
    {
        /// <summary>
        /// Environment name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL for Warehouse
        /// </summary>
        public string WarehouseUrl { get; set; }

        /// <summary>
        /// URL for Inno Installer
        /// </summary>
        public string InnoInstallerUrl { get; set; }
    }
}
