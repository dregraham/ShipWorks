using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    public delegate bool MigrationExecutionInvoker(ProgressProvider progress);
}
