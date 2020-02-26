using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Xunit;
using Moq;
using log4net;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Tests.Interapptive.Shared.Net
{
    public class NetworkUtilityTests
    {
         Mock<ILog> logger;

         private NetworkUtility testObject;

         public NetworkUtilityTests()
         {
             var mock = AutoMockExtensions.GetLooseThatReturnsMocks();

             logger = mock.Mock<ILog>();
             Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
             repo.Setup(x => x(It.IsAny<Type>()))
                 .Returns(logger.Object);

             testObject = new NetworkUtility(repo.Object);
         }

         [Fact]
         public void GetIPAddress_LogsIPAddress()
         {
             testObject.GetIPAddress();

             //Constructor was called in initialize method, so just need to verify log
             logger.Verify(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
         }

    }
}
