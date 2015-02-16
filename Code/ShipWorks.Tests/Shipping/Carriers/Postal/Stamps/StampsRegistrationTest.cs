using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using System.Text.RegularExpressions;
using log4net;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsRegistrationTest
    {
        StampsRegistration testObject;
        Mock<IUspsRegistrationValidator> mockedValidator;
        Mock<IStampsRegistrationGateway> mockedGateway;
        private Mock<IRegistrationPromotion> promotion;

        [TestInitialize]
        public void Initialize()
        {
            // Setup a validator so our test object always passes validation
            mockedValidator = new Mock<IUspsRegistrationValidator>();
            mockedValidator.Setup(v => v.Validate(It.IsAny<StampsRegistration>())).Returns(new List<RegistrationValidationError>());

            mockedGateway = new Mock<IStampsRegistrationGateway>();
            mockedGateway.Setup(g => g.Register(It.IsAny<StampsRegistration>()));

            promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode()).Returns("shipworks");

            testObject = new StampsRegistration(mockedValidator.Object, mockedGateway.Object, promotion.Object);
        }

        [TestMethod]
        public void Submit_DelegatesToValidator_Test()
        {
            testObject.Submit();

            mockedValidator.Verify(v => v.Validate(testObject), Times.Once());
        }
        
        [TestMethod]
        [ExpectedException(typeof(StampsRegistrationException))]
        public void Submit_ThrowsRegistrationException_WhenValidationFails_Test()
        {
            // Setup our mocked validator to return two errors
            mockedValidator.Setup(v => v.Validate(It.IsAny<StampsRegistration>()))
                .Returns
                (
                    new List<RegistrationValidationError>() 
                    { 
                        new RegistrationValidationError("Mocked failure 1"),
                        new RegistrationValidationError("Mocked failure 2")
                    }
                );

            testObject.Submit();
        }

        [TestMethod]
        public void Submit_DelegatesToGateway_WhenValidationPasses_Test()
        {
            // Using a mocked validator, so there shouldn't be any validation errors
            testObject.Submit();

            mockedGateway.Verify(g => g.Register(It.IsAny<StampsRegistration>()), Times.Once());
        }

        [TestMethod]
        public void Submit_DoesNotDelegateToGateway_WhenValidationFails_Test()
        {
            // Setup our mocked validator to return two errors
            mockedValidator.Setup(v => v.Validate(It.IsAny<StampsRegistration>()))
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
            catch (StampsRegistrationException)
            { 
                // Catch the exception here since we just want to make sure
                // the gateway was not called
            }

            // The gateway should not be called when there are validation errors
            mockedGateway.Verify(g => g.Register(It.IsAny<StampsRegistration>()), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(StampsRegistrationException))]
        public void Submit_ThrowsStampsException_WhenRegistrationGatewayThrowsException_Test()
        {
            // Setup up our mocked gateway to throw an execption
            mockedGateway.Setup(g => g.Register(It.IsAny<StampsRegistration>())).Throws(new StampsRegistrationException());
            testObject.Submit();
        }


        [TestMethod]
        public void Constructor_AssignsVersion4IPAddress_Test()
        {
            //Constructor was called in initialize method, so just need to verify log

            // Make sure we have a version 4 formatted IP address
            string ipExpression = "\\b\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\b";
            Assert.IsTrue(Regex.IsMatch(testObject.MachineInfo.IPAddress, ipExpression));
        }
    }
}
