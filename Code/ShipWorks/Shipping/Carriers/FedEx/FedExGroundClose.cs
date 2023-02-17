using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.Threading;
using ShipWorks.Shipping.ShipEngine.Manifest;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Utility class for dealing with FedEx end of day stuff
    /// </summary>
    public static class FedExGroundClose
    {
        /// <summary>
        /// Process the end of day close.  Return true if there were any shipments to be closed
        /// </summary>
        public static List<long> ProcessClose()
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var closings = new List<long>();

                var shipEngineManifestUtility = lifetimeScope.Resolve<IShipEngineManifestUtility>();

                foreach (var account in FedExAccountManager.Accounts)
                {
                    var task = Task.Run(() => shipEngineManifestUtility.CreateManifestTask(account,
                        new ProgressItem("FedEx Ground Close"), new List<string>(),
                        new List<string>()));

                    task.Wait();

                    closings.AddRange(task.Result);
                }

                return closings;
            }
        }

        /// <summary>
        /// Print the close reports for all of the given closings.  The user will be prompted for print settings.
        /// </summary>
        public static void PrintCloseReports(IEnumerable<long> closings)
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var shipEngineManifestUtility = lifetimeScope.Resolve<IShipEngineManifestUtility>();

                foreach (var c in closings)
                {
                    shipEngineManifestUtility.Print(c);
                }
            }
        }
    }
}
