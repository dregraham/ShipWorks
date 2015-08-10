using System;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.UI;

namespace ShipWorks.Startup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IoC.Initialize(typeof(ShippingModule).Assembly);

            ShipWorks.Program.Main();
        }
    }
}
