using ShipWorks.Shipping.Carriers;
using SandMenu = Divelements.SandRibbon.Menu;
using SandMenuItem = Divelements.SandRibbon.MenuItem;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Class to manage printing ShipEngine manifests
    /// </summary>
    public interface IShipEngineManifestUtility
    {
        /// <summary>
        /// Populate the Create Manifest menu
        /// </summary>
        void PopulateCreateManifestMenu(SandMenuItem menu, ICarrierAccountRetriever accountRetriever);

        /// <summary>
        /// Populate the Print Manifest menu
        /// </summary>
        void PopulatePrintManifestMenu(SandMenu menu, ICarrierAccountRetriever accountRetriever);
    }
}
