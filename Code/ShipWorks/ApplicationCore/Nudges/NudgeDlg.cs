using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Nudges
{
    public partial class NudgeDlg : Form
    {
        private readonly Nudge nudge;

        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeDlg"/> class.
        /// </summary>
        /// <param name="nudge">The nudge.</param>
        public NudgeDlg(Nudge nudge)
        {
            InitializeComponent();

            this.nudge = nudge;
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Text = EnumHelper.GetDescription(nudge.NudgeType);
        }
    }
}
