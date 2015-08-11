using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Crashes;

namespace ShipWorks.Tests.ApplicationCore.Crashes
{
    public class CrashSubmitterTest
    {
        private Mock<Exception> testException;
        private Mock<Exception> testInnerException;
        private Mock<IOException> testIoException;
        private Mock<IOException> testIoInnerException;
        private string version = "V0.0.0.0";
        private string exceptionMessage = "ExceptionMessage";
        private string innerExceptionMessage = "InnerExceptionMessage";
        private string stackTrace = "StackTrace";
        private string innerStackTrace = "InnerStackTrace";

        [TestInitialize]
        public void Initialize()
        {
            Assembly assembly = typeof(CrashSubmitter).Assembly;
            version = string.Format("V{0}", assembly.GetName().Version.ToString());

            testException = new Mock<Exception>();
            testException.Setup(e => e.Message).Returns(exceptionMessage);
            testException.Setup(e => e.StackTrace).Returns(stackTrace);

            testInnerException = new Mock<Exception>();
            testInnerException.Setup(e => e.Message).Returns(innerExceptionMessage);
            testInnerException.Setup(e => e.StackTrace).Returns(innerStackTrace);


            testIoException = new Mock<IOException>();
            testIoException.Setup(e => e.Message).Returns(exceptionMessage);
            testIoException.Setup(e => e.StackTrace).Returns(stackTrace);

            testIoInnerException = new Mock<IOException>();
            testIoInnerException.Setup(e => e.Message).Returns(innerExceptionMessage);
            testIoInnerException.Setup(e => e.StackTrace).Returns(innerStackTrace);
        }

        [Fact]
        public void GetIdentifier_NoInnerException_ReturnsCorrectIdentifier_Test()
        {
            string expectedIdentifier = string.Format("{0} {1}{2},None{3}", version, testException.Object.GetType().Name, exceptionMessage, stackTrace);

            string returnedIdentifier = CrashSubmitter.GetIdentifier(testException.Object);

            Assert.AreEqual(expectedIdentifier, returnedIdentifier);
        }

        [Fact]
        public void GetIdentifier_WithInnerException_ReturnsCorrectIdentifier_Test()
        {

            Exception testInnerException = new Exception(innerExceptionMessage);
            Exception testException = new Exception(exceptionMessage, testInnerException);

            string expectedIdentifier = string.Format("{0} {1}{2},{3}", version, testException.GetType().Name, exceptionMessage, innerExceptionMessage);

            string returnedIdentifier = CrashSubmitter.GetIdentifier(testException);

            Assert.AreEqual(expectedIdentifier, returnedIdentifier);
        }

        [Fact]
        public void GetIdentifier_NoInnerExceptionAndIsIoException_ReturnsCorrectIdentifier_Test()
        {
            string expectedIdentifier = string.Format("{0} {1}{2},None{3}", version, testIoException.Object.GetType().Name, exceptionMessage, stackTrace);

            string returnedIdentifier = CrashSubmitter.GetIdentifier(testIoException.Object);

            Assert.AreEqual(expectedIdentifier, returnedIdentifier);
        }

        [Fact]
        public void GetIdentifier_WithInnerExceptionAndIsIoException_ReturnsCorrectIdentifier_Test()
        {
            IOException testInnerException = new IOException(innerExceptionMessage);
            IOException testException = new IOException(exceptionMessage, testInnerException);

            string expectedIdentifier = string.Format("{0} {1}{2},{3}", version, testException.GetType().Name, exceptionMessage, innerExceptionMessage);

            string returnedIdentifier = CrashSubmitter.GetIdentifier(testException);

            Assert.AreEqual(expectedIdentifier, returnedIdentifier);
        }
    }
}
