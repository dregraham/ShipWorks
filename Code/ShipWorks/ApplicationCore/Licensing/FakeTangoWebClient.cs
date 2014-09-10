using System;
using System.Collections.Generic;
using System.Drawing;
using log4net;
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
                new Nudge(1, NudgeType.ShipWorksUpgrade, new Uri("http://www.shipworks.com"), new Size(625, 575)),
                new Nudge(2, NudgeType.ShipWorksUpgrade, new Uri("http://www.google.com"), new Size(300, 500))
            };

            // Add a couple of options to the first nudge
            nudges[0].AddNudgeOption(new NudgeOption(0, "First Button", nudges[0], null, "CloseClicked"));
            nudges[0].AddNudgeOption(new NudgeOption(1, "Second Button", nudges[0], null, "CloseClicked"));

            // Add one option to the second nudge in the list
            nudges[1].AddNudgeOption(new NudgeOption(0, "Close", nudges[1], null, "CloseClicked"));

            return nudges;
        }

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        public override void LogNudgeOption(NudgeOption option)
        {
            // Just log the option that was selected to disk to simulate a call to Tango
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The {0} option result was selected for nudge ID {1}", option.Result, option.Owner.NudgeID);
        }
    }
}
