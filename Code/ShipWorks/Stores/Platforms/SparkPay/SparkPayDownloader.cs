using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    class SparkPayDownloader : StoreDownloader
    {
        public SparkPayDownloader(StoreEntity store) : base(store)
        {

        }
        protected override void Download()
        {
            throw new NotImplementedException();
        }
    }
}
