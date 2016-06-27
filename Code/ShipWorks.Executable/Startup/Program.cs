using System;

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
