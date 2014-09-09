using System;
using System.Collections.Generic;
using System.Drawing;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// A fake web client for internal testing purposes to simulate calls to Tango that may not yet be 
    /// implemented on the Tango side. This is just to streamline the development side of things on the
    /// ShipWorks side without having to mess with Fiddler and all of the certificate inspection that
    /// goes along with it when trying to setup specific test cases.
    /// </summary>
    public class FakeTangoWebClient : TangoWebClientWrapper, ITangoWebClient
    {
        /// <summary>
        /// Gets the nudges.
        /// </summary>
        /// <returns>A couple of fake nudges for testing purposes.</returns>
        public override IEnumerable<Nudge> GetNudges(IEnumerable<StoreEntity> stores)
        {
            // Build up a couple of dummy nudges for testing purposes. Null is being configured as the INudgeAction 
            // until the actual implementations are ready. Null is a good test to ensure that this is accounted 
            // for, however.
            List<Nudge> nudges = new List<Nudge>
            {
                new Nudge(1, NudgeType.ShipWorksUpgrade, new Uri("http://www.shipworks.com"), new List<NudgeOption> { new NudgeOption(0, "First Button", null), new NudgeOption(1, "Second Button", null) }, new Size(625, 575)),
                new Nudge(2, NudgeType.ShipWorksUpgrade, new Uri("http://www.google.com"), new List<NudgeOption> { new NudgeOption(0, "Close", null) }, new Size(300, 500))
            };

            return nudges;
        }

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        public override void LogNudgeOption(NudgeOption option)
        {
            // Do nothing to simulate a successful nudge being logged to Tango
        }
    }
}
