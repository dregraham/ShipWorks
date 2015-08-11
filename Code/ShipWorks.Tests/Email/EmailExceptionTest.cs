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
            Assert.True(ex.RetryAllowed);
        }

        [Fact]
        public void RetryAllowed_ReturnsTrue_ForSpecificValues()
        {
            EmailException ex = new EmailException("test true", EmailExceptionErrorNumber.None);
            Assert.True(ex.RetryAllowed, "None should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.LogonFailed);
            Assert.True(ex.RetryAllowed, "LogonFailed should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.DelaySending);
            Assert.True(ex.RetryAllowed, "DelaySending should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.MaxEmailsPerHourReached);
            Assert.True(ex.RetryAllowed, "MaxEmailsPerHourReached should return true.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.EmailAccountChanged);
            Assert.True(ex.RetryAllowed, "EmailAccountChanged should return true.");
        }

        [Fact]
        public void RetryAllowed_ReturnsFalse_ForSpecificValues()
        {
            EmailException ex = new EmailException("test true", EmailExceptionErrorNumber.MissingToField);
            Assert.False(ex.RetryAllowed, "MissingToField should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.MissingFromField);
            Assert.False(ex.RetryAllowed, "MissingFromField should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.EmailBodyProcessingFailed);
            Assert.False(ex.RetryAllowed, "EmailBodyProcessingFailed should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidEmailAddress);
            Assert.False(ex.RetryAllowed, "InvalidEmailAddress should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidTemplateSelected);
            Assert.False(ex.RetryAllowed, "InvalidTemplateSelected should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.IndeterminateTemplateSettings);
            Assert.False(ex.RetryAllowed, "IndeterminateTemplateSettings should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.InvalidEmailAccount);
            Assert.False(ex.RetryAllowed, "InvalidEmailAccount should return false.");

            ex = new EmailException("test true", EmailExceptionErrorNumber.NoEmailAccountsConfigured);
            Assert.False(ex.RetryAllowed, "NoEmailAccountsConfigured should return false.");
        }
    }
}
