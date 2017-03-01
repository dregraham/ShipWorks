﻿#pragma warning disable S125 // Sections of code should not be "commented out"
//using Autofac.Extras.Moq;
//using Interapptive.Shared.Utility;
//using log4net;
//using Moq;
//using ShipWorks.Messaging.Messages.SingleScan;
//using ShipWorks.Tests.Shared;
//using System;
//using System.Threading.Tasks;
//using Xunit;

//namespace ShipWorks.SingleScan.Tests
//{
//    public class LoggableAutoPrintServiceTest : IDisposable
//    {
//        readonly AutoMock mock;
//        readonly Mock<IAutoPrintService> autoPrintService;
//        readonly Mock<ILog> logger;
//        readonly LoggableAutoPrintService testObject;

//        public LoggableAutoPrintServiceTest()
//        {
//            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
//            autoPrintService = mock.Mock<IAutoPrintService>();
//            logger = mock.Mock<ILog>();

//            testObject = mock.Create<LoggableAutoPrintService>();
//        }

//        [Fact]
//        public void AllowAutoPrint_DelegatesToAutoPrintService_AllowAutoPrint()
//        {
//            ScanMessage scanMessage = new ScanMessage();
//            testObject.AllowAutoPrint(scanMessage);

//            autoPrintService.Verify(a => a.AllowAutoPrint(scanMessage));
//        }

//        [Fact]
//        public async void Print_DelegatesToAutoPrintService_Prit()
//        {
//            AutoPrintServiceDto autoprintdto = new AutoPrintServiceDto();
//            await testObject.Print(new AutoPrintServiceDto());

//            autoPrintService.Verify(a => a.Print(autoprintdto));
//        }

//        [Fact]
//        public async void Print_LogsError_WhenAutoPrintServiceReturnsFailure()
//        {
//            string error = "something went wrong";
//            autoPrintService.Setup(a => a.Print(It.IsAny<AutoPrintServiceDto>())).Returns(Task.FromResult(GenericResult.FromError<string>(error)));

//            await testObject.Print(new AutoPrintServiceDto());

//            logger.Verify(l => l.Error(error));
//        }

//        public void Dispose()
//        {
//            mock?.Dispose();
//        }
//    }
//}
