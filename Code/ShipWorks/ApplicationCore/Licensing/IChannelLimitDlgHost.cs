using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitDlgHost : IDisposable
    {
        DialogResult ShowDialog(IWin32Window owner);
    }
}

