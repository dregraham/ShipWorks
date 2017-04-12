using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;

namespace ShipWorks.Shipping.UI.Tests.Carriers.UPS.LocalRating
{
    public class FakeUpsLocalRatingViewModel : UpsLocalRatingViewModel
    {
        public FakeUpsLocalRatingViewModel(IUpsLocalRateTable rateTable,
            Func<ISaveFileDialog> saveFileDialogFactory,
            Func<IOpenFileDialog> openFileDialogFactory,
            IMessageHelper messageHelper,
            Func<Type, ILog> logFactory)
            : base(rateTable, saveFileDialogFactory, openFileDialogFactory, messageHelper, logFactory)
        {
        }

        /// <summary>
        /// Calls the upload rating file.
        /// </summary>
        public async Task CallUploadRatingFile() => await UploadRatingFile();
    }
}
