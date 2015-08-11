using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Registration
{
    public class UspsRegistrationValidatorTest
    {
        private readonly UspsRegistrationValidator testObject;

        private Mock<IRegistrationPromotion> promotion;
        private Mock<IUspsRegistrationGateway> gateway;

        public UspsRegistrationValidatorTest()
        {
            testObject = new UspsRegistrationValidator();
        }
        
        private UspsRegistration CreateValidUnitedStatesRegistration()
        {
            promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode()).Returns("valid promo code");

            gateway = new Mock<IUspsRegistrationGateway>();

            UspsRegistration registration = new UspsRegistration(testObject, gateway.Object, promotion.Object)
            {
                UserName = "RonMexico",
                Password = "H3RP35",
                Email = "ron@mexico.com",
                
                FirstCodewordType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CodewordType2.MothersMaidenName,
                FirstCodewordValue = "Dog",

                SecondCodewordType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CodewordType2.PetsName,
                SecondCodewordValue = "Fighting",

                PhysicalAddress = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.Address()
                {
                    Address1 = "123 Prescription Ln",
                    City = "Newport News",
                    State = "VA",
                    ZIPCode = "23601",
                    Country = "US",
                    PhoneNumber = "5555555555"
                },

                MachineInfo = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.MachineInfo() { IPAddress = "127.0.0.1" },
                CreditCard = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CreditCard()
            };

            return registration;
        }

        private UspsRegistration CreateValidInternationalRegistration()
        {
            promotion = new Mock<IRegistrationPromotion>();
            promotion.Setup(p => p.GetPromoCode()).Returns("valid promo code");

            UspsRegistration registration = new UspsRegistration(testObject, new Moq.Mock<IUspsRegistrationGateway>().Object, promotion.Object)
            {
                UserName = "RonMexico",
                Password = "H3RP35",
                Email = "ron@mexico.com",

                FirstCodewordType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CodewordType2.MothersMaidenName,
                FirstCodewordValue = "Dog",

                SecondCodewordType = ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CodewordType2.PetsName,
                SecondCodewordValue = "Fighting",

                PhysicalAddress = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.Address()
                {
                    Address1 = "123 Prescription Ln",
                    City = "Sidcup",
                    Province = "Kent",
                    PostalCode = "DA14 6DJ",
                    Country = "UK",
                    PhoneNumber = "5555555555"
                },

                MachineInfo = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.MachineInfo() { IPAddress = "127.0.0.1" },
                CreditCard = new ShipWorks.Shipping.Carriers.Postal.Usps.WebServices.CreditCard()
            };

            return registration;
        }


        [Fact]
        public void Validate_ReturnsEmptyList_WhenUnitedStatesRegistrationIsValid_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenInternationalRegistrationIsValid_Test()
        {
            UspsRegistration registration = CreateValidInternationalRegistration();
            registration.PhysicalAddress.ZIPCode = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        #region Required Fields Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenMissingIPAddress_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.MachineInfo.IPAddress = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Machine info is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenMachineInfoIsNull_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.MachineInfo = null;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Machine info is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPhysicalAddressIsNull_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress = null;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A physical address must be provided.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPhysicalAddressIsMissingCity_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.City = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A city is required for the physical address.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPhysicalAddressIsMissingAddress1_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.Address1 = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A street address is required for the physical address.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenUnitedStatesRegistrationIsMissingState_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.State = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A state is required for the physical address.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenUnitedStatesRegistrationIsMissingZipCode_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.ZIPCode = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A postal code is required for the physical address.", errors.First().Message);
        }

        
        [Fact]
        public void Validate_ReturnsOneError_WhenInternationalRegistrationIsMissingPostalCode_Test()
        {
            UspsRegistration registration = CreateValidInternationalRegistration();
            registration.PhysicalAddress.PostalCode = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A postal code is required for the physical address.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenInternationalRegistrationIsMissingProvince_Test()
        {
            UspsRegistration registration = CreateValidInternationalRegistration();
            registration.PhysicalAddress.Province = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("A province is required for the physical address.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenUsernameIsMissing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.UserName = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Username is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPasswordIsMissing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Password is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenFirstCodewordValueIsMissing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.FirstCodewordValue = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("First code word is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenSecondCodewordValueIsMissing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordValue = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Second code word is required.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenEmailIsMissing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Email = string.Empty;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);
            
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Email address is required.", errors.First().Message);
        }

        #endregion Required Fields Tests


        #region Username Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenUserNameIs15Characters_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.UserName = "123456789012345";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The username provided is too long. Usernames must be 14 characters or less.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenUserNameIsMoreThan15Characters_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.UserName = "1234567890123456";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The username provided is too long. Usernames must be 14 characters or less.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenUserNameMeetsUpperBound_Test()
        {
            // Set the username to 40 characters
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.UserName = "12345678901234";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        #endregion Username Tests


        #region Password Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenPasswordIsTooLong_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "ThisIs1LongPasswordThatShouldCauseAValidationError";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Passwords must be between 6 and 20 characters long and contain at least one letter and one number.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPasswordIsTooShort_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "abc1d";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Passwords must be between 6 and 20 characters long and contain at least one letter and one number.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPasswordOnlyContainsNumbers_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "123456789";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Passwords must be between 6 and 20 characters long and contain at least one letter and one number.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPasswordOnlyContainsLetters_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "missingNumber";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Passwords must be between 6 and 20 characters long and contain at least one letter and one number.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenPasswordIsValid_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "valid2Password";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());            
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenPasswordMeetsUpperBound_Test()
        {
            // Set the password to a strong enough password that is 20 characters
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "valid2Password123456";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenPasswordMeetsLowerBound_Test()
        {
            // Set the password to a strong enough password that is 6 characters long
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Password = "ab12cd";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        #endregion Password Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenCodewordTypesAreSame_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordType = registration.FirstCodewordType;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("You must select two different code word questions.", errors.First().Message);
        }

        #region First Code Word Value Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenFirstCodewordValueIsTooShort_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.FirstCodewordValue = "a";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The first code word must be between 2 and 33 characters long.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenFirstCodewordValueIsTooLong_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.FirstCodewordValue = "1234567890123456789012345678901234";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The first code word must be between 2 and 33 characters long.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenFirstCodeWordValueMeetsLowerBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.FirstCodewordValue = "ab";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenFirstCodeWordValueMeetsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.FirstCodewordValue = "123456789012345678901234567890123";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        #endregion First Code Word Value Tests

        #region Second Code Word Value Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenSecondCodewordValueIsTooShort_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordValue = "a";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The second code word must be between 2 and 33 characters long.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenSecondCodewordValueIsTooLong_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordValue = "1234567890123456789012345678901234";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("The second code word must be between 2 and 33 characters long.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenSecondCodeWordValueMeetsLowerBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordValue = "ab";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenSecondCodeWordValueMeetsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.SecondCodewordValue = "123456789012345678901234567890123";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        #endregion Second Code Word Value Tests

        [Fact]
        public void Validate_ReturnsOneError_WhenEmailExceedsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Email = "somebodyThatExceedsTheUpperBound@WhatBoundaries.com";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Email address is too long. Stamps.com only allows email addresses that are less than 41 characters long.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenEmailMeetsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Email = "somebody123456789@somewhere1234567890.com";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenEmailDoesNotMeetUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.Email = "somebody@somewhere.com";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenPromoCodeExceedsUpperBound_Test()
        {
            // Create a promo code of 51 characters (exceeds the upper bound by 1)
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            promotion.Setup(p => p.GetPromoCode()).Returns("123456789012345678901234567890123456789012345678901");
            
            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("An invalid promo code was provided. Stamps.com only recognizes promo codes that are 50 characters or less.", errors.First().Message);
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenPromoCodeMeetsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            promotion.Setup(p => p.GetPromoCode()).Returns("12345678901234567890123456789012345678901");

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsEmptyList_WhenPromoCodeDoesNotMeetsUpperBound_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            promotion.Setup(p => p.GetPromoCode()).Returns("123456789012345");

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(0, errors.Count());
        }

        [Fact]
        public void Validate_ReturnsOneError_WhenBothPaymentMethodsAreNull_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.CreditCard = null;
            registration.AchAccount = null;

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual("Stamps.com requires that either credit card or account be provided in the registration process.", errors.First().Message);
        }

        [Fact]
        public void Validate_CleansesPhoneNumber_WhenLongerThanTenCharacters_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.PhoneNumber = "555-555-5555";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual("5555555555", registration.PhysicalAddress.PhoneNumber);
        }

        [Fact]
        public void Validate_RetrunsOneError_WhenPhoneNumberIsLongerThanTenCharacters_AfterCleansing_Test()
        {
            UspsRegistration registration = CreateValidUnitedStatesRegistration();
            registration.PhysicalAddress.PhoneNumber = "1-555-555-5555";

            IEnumerable<RegistrationValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count());
        }
    }
}
