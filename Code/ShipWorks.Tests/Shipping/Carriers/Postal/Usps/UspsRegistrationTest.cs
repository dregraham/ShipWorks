﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsRegistrationTest
    {
        UspsRegistration testObject;
        Mock<IUspsRegistrationValidator> mockedValidator;
        Mock<IUspsRegistrationGateway> mockedGateway;
        private Mock<IRegistrationPromotion> promotion;

        public UspsRegistrationTest()
        {
            // Setup a validator so our test object always passes validation
            mockedValidator = new Mock<IUspsRegistrationValidator>();
            mockedValidator.Setup(v => v.Validate(It.IsAny<UspsRegistration>())).Returns(new List<RegistrationValidationError>());

            mockedGateway = new Mock<IUspsRegistrationGateway>();
            mockedGateway.Setup(g => g.Register(It.IsAny<UspsRegistration>()));

            promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode()).Returns("shipworks");

            testObject = new UspsRegistration(mockedValidator.Object, mockedGateway.Object, promotion.Object);
        }

        [Fact]
        public void Submit_DelegatesToValidator()
        {
            testObject.Submit();

            mockedValidator.Verify(v => v.Validate(testObject), Times.Once());
        }

        [Fact]
        public void Submit_ThrowsRegistrationException_WhenValidationFails()
        {
            // Setup our mocked validator to return two errors
            mockedValidator.Setup(v => v.Validate(It.IsAny<UspsRegistration>()))
                .Returns
                (
                    new List<RegistrationValidationError>()
                    {
                        new RegistrationValidationError("Mocked failure 1"),
                        new RegistrationValidationError("Mocked failure 2")
                    }
                );

            Assert.Throws<UspsRegistrationException>(() => testObject.Submit());
        }

        [Fact]
        public void Submit_DelegatesToGateway_WhenValidationPasses()
        {
            // Using a mocked validator, so there shouldn't be any validation errors
            testObject.Submit();

            mockedGateway.Verify(g => g.Register(It.IsAny<UspsRegistration>()), Times.Once());
        }

        [Fact]
        public void Submit_DoesNotDelegateToGateway_WhenValidationFails()
        {
            // Setup our mocked validator to return two errors
            mockedValidator.Setup(v => v.Validate(It.IsAny<UspsRegistration>()))
                .Returns
                (
                    new List<RegistrationValidationError>()
                    {
                        new RegistrationValidationError("Mocked failure 1"),
                        new RegistrationValidationError("Mocked failure 2")
                    }
                );

            try
            {
                testObject.Submit();
            }
            catch (UspsRegistrationException)
            {
                // Catch the exception here since we just want to make sure
                // the gateway was not called
            }

            // The gateway should not be called when there are validation errors
            mockedGateway.Verify(g => g.Register(It.IsAny<UspsRegistration>()), Times.Never());
        }

        [Fact]
        public void Submit_ThrowsUspsException_WhenRegistrationGatewayThrowsException()
        {
            // Setup up our mocked gateway to throw an execption
            mockedGateway.Setup(g => g.Register(It.IsAny<UspsRegistration>())).Throws(new UspsRegistrationException());
            Assert.Throws<UspsRegistrationException>(() => testObject.Submit());
        }


        [Fact]
        public void Constructor_AssignsVersion4IPAddress()
        {
            //Constructor was called in initialize method, so just need to verify log

            // Make sure we have a version 4 formatted IP address
            string ipExpression = "\\b\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\b";
            Assert.True(Regex.IsMatch(testObject.MachineInfo.IPAddress, ipExpression));
        }
    }
}
