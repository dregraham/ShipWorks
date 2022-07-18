using System.Windows.Forms;

namespace ShipWorks.Stores.Services
{
    /// <summary>
    /// Class to convert Amazon MWS stores to SP
    /// </summary>
    public interface IAmazonMwsToSpConverter
    {
        /// <summary>
        /// Convert Amazon MWS stores to SP
        /// </summary>
        void ConvertStores(IWin32Window owner);
    }
}
