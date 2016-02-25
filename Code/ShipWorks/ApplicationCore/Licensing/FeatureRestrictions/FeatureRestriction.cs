using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public abstract class FeatureRestriction
    {
        public virtual bool Handle(IWin32Window owner, object data)
        {
            return false;
        }
    }
}
