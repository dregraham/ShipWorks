using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    public class LinkControl : Label
    {
        public LinkControl()
        {
            ForeColor = Color.Blue;
            Font = new Font("Tahoma", Font.Size, FontStyle.Underline);
            Cursor = Cursors.Hand;
        }
    }
}
