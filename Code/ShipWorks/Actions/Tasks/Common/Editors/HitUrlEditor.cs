using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class HitUrlEditor : TemplateBasedTaskEditor
    {
        private HitUrlTask task;

        public HitUrlEditor(HitUrlTask task)
        {
            InitializeComponent();

            this.task = task;
        }

    }
}
