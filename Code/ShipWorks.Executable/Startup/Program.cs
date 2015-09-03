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
        static void Main(string[] args)
        {
            ContainerInitializer.Initialize();

            ShipWorks.Program.Main();
        }
    }
}
