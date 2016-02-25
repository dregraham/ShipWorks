using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public abstract class FeatureRestriction
    {
        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public virtual bool Handle(IWin32Window owner, object data) => false;
    }
}
