using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Escalator
{
    public partial class Escalator : ServiceBase
    {
        public Escalator()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"\hereIam.txt"))
            {
                file.WriteLine(DateTime.Now.ToString());
            }
        }

        protected override void OnStop()
        {
        }
    }
}
