using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public partial class YahooApiAccountSettingsHost : UserControl
    {
        private YahooApiAccountSettingsViewModel viewModel;


        public YahooApiAccountSettingsHost()
        {
            InitializeComponent();
            Load += OnPageLoad;
        }

        private void OnPageLoad(object sender, EventArgs e)
        {
            viewModel = IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<YahooApiAccountSettingsViewModel>>().Value;
            YahooApiAccountSettings page = new YahooApiAccountSettings
            {
                DataContext = viewModel
            };
            ControlHost.Child = page;
        }
    }
}
