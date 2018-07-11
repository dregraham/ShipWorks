/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/16/2018
 * Time: 12:11 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmokeTest
{
	/// <summary>
	/// Description of DisappearingWindow.
	/// </summary>
	public partial class DisappearingWindow : Form
    {
        private Timer myTimer;

        public DisappearingWindow()
        {
            InitializeComponent();
        }

        private void Install_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DisappearingWindow_Load(object sender, EventArgs e)
        {

            myTimer = new Timer();
            myTimer.Interval = (10000); // 10 mins
            myTimer.Tick += MyTimer_Tick;
            myTimer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            myTimer.Stop();
            DialogResult = DialogResult.OK;
        }
    }
}
