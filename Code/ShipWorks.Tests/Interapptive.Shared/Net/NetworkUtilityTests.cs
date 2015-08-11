using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Xunit;
using Moq;
using log4net;

namespace ShipWorks.Tests.Interapptive.Shared.Net
{
    public class NetworkUtilityTests
    {
         Mock<ILog> logger;

         private NetworkUtility testObject;

         [TestInitialize]
         public void Initialize()
         {
             logger = new Mock<ILog>();
             logger.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()));

             testObject = new NetworkUtility(logger.Object);
         }

         [Fact]
         public void GetIPAddress_LogsIPAddress_Test()
         {
             testObject.GetIPAddress();

             //Constructor was called in initialize method, so just need to verify log
             logger.Verify(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
         }

    }
}
