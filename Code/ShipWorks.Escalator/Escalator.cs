using System;
using System.ServiceProcess;
using System.IO;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Logic of the Escalator service
    /// </summary>
    public class Escalator : ServiceBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Escalator()
        {
            this.ServiceName = ShipWorks.Escalator.ServiceName.Resolve();
        }

        /// <summary>
        /// Code that runs when the service starts
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            using (StreamWriter file = new StreamWriter(@"hereIam.txt"))
            {
                file.WriteLine(DateTime.Now.ToString());
            }
        }

        /// <summary>
        /// Code that runs when the service stops
        /// </summary>
        protected override void OnStop()
        {
            // Do nothing
        }
    }
}
