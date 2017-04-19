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
    /// <summary>
    /// This class Inherits from UpsLocalRatingViewModel and allows 
    /// one to call the protected method UploadRatingFile().
    /// </summary>
    public class FakeUpsLocalRatingViewModel : UpsLocalRatingViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeUpsLocalRatingViewModel"/> class.
        /// </summary>
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
