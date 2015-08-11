using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Email;

namespace ShipWorks.Tests.Email
{
    public class EmailExceptionTest
    {
        [Fact]
        public void RetryAllowed_ReturnsTrue_WhenDefaultConstructor()
        {
            EmailException ex = new EmailException();
            Assert.IsTrue(ex.RetryAllowed);
        }

        [Fact]
        public void RetryAllowed_ReturnsTrue_ForSpecificValues()
        {
            EmailException ex = new EmailException("test true", EmailExceptionErrorNumber.None);
            Assert.IsTrue(ex.RetryAllowed, "None should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.LogonFailed);
            Assert.IsTrue(ex.RetryAllowed, "LogonFailed should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.DelaySending);
            Assert.IsTrue(ex.RetryAllowed, "DelaySending should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.MaxEmailsPerHourReached);
            Assert.IsTrue(ex.RetryAllowed, "MaxEmailsPerHourReached should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.EmailAccountChanged);
            Assert.IsTrue(ex.RetryAllowed, "EmailAccountChanged should return true.");
        }

        [Fact]
        public void RetryAllowed_ReturnsFalse_ForSpecificValues()
        {
            EmailException ex = new EmailException("test true", EmailExceptionErrorNumber.MissingToField);
            Assert.IsFalse(ex.RetryAllowed, "MissingToField should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.MissingFromField);
            Assert.IsFalse(ex.RetryAllowed, "MissingFromField should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.EmailBodyProcessingFailed);
            Assert.IsFalse(ex.RetryAllowed, "EmailBodyProcessingFailed should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidEmailAddress);
            Assert.IsFalse(ex.RetryAllowed, "InvalidEmailAddress should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidTemplateSelected);
            Assert.IsFalse(ex.RetryAllowed, "InvalidTemplateSelected should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.IndeterminateTemplateSettings);
            Assert.IsFalse(ex.RetryAllowed, "IndeterminateTemplateSettings should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidEmailAccount);
            Assert.IsFalse(ex.RetryAllowed, "InvalidEmailAccount should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.NoEmailAccountsConfigured);
            Assert.IsFalse(ex.RetryAllowed, "NoEmailAccountsConfigured should return false.");
        }
    }
}
