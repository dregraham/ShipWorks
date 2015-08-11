using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class NeweggAccountSettingsValidatorTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [Fact]
        public void IsSellerIdValid_ReturnsFalse_WhenEmptyString_Test()
        {
            string sellerId = string.Empty;
            Assert.IsFalse(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }
                
        [Fact]
        public void IsSellerIdValid_ReturnsFalse_WhenOnlyWhitespace_Test()
        {
            string sellerId = "        ";
            Assert.IsFalse(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenNoWhiteSpaceCharacters_Test()
        {
            string sellerId = "MySellerId";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenAlphaNumericAndWhiteSpaceCharacters_Test()
        {
            string sellerId = "My Seller ID";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenUsingSandboxedSellerId_Test()
        {
            // This test uses the actual seller ID that Newegg provided to us
            // for our sandboxed seller account. This test is similar to the 
            // case for no white space characters, but tests/documents that
            // any future changes to IsSellerIdValid method still returns
            // true for a "real" seller ID
            string sellerId = "A09V";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsFalse_WhenEmptyString_Test()
        {
            string secretKey = string.Empty;
            Assert.IsFalse(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsFalse_WhenOnlyWhitespace_Test()
        {
            string secretKey = "        ";
            Assert.IsFalse(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenNoWhiteSpaceCharacters_Test()
        {
            string secretKey = "MySecretKeyValue";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenAlphaNumericAndWhiteSpaceCharacters_Test()
        {
            string secretKey = "My Secret Key Value";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenInGuidFormat_Test()
        {
            // Since the secret key value appears to be a GUID based on Newegg documentation, 
            // this test is to confirm that a GUID string is a valid secret key
            string secretKey = Guid.NewGuid().ToString();
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenUsingSandboxedSecretKey_Test()
        {
            // This test uses the actual secret key that Newegg provided to us
            // for our sandboxed seller account. This test is similar to the 
            // case for no white space characters, but tests/documents that
            // any future changes to IsSecretKeyValid method still returns
            // true for a "real" secret key
            string secretKey = ": E09799F3-A8FD-46E0-989F-B8587A1817E0";
            Assert.IsTrue(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }


        [Fact]
        public void Validate_ReturnsValidationError_WhenSellerIdIsInvalid_Test()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = string.Empty;
            storeEntity.SecretKey = Guid.NewGuid().ToString();

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.AreEqual(1, errors.Count);
        }

        [Fact]
        public void Validate_ReturnsValidationError_WhenSecretKeyIsInvalid_Test()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = "A09V";
            storeEntity.SecretKey = string.Empty;

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.AreEqual(1, errors.Count);
        }

        [Fact]
        public void Validate_ValidationErrorListIsEmpty_WhenStoreEntityIsValid_Test()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = "A09V";
            storeEntity.SecretKey = Guid.NewGuid().ToString();

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.AreEqual(0, errors.Count);
        }

        [Fact]
        public void Validate_ReturnsMultipleValidationErrors_WhenSecretKeyAndSellerIdAreInvalid_Test()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = string.Empty;
            storeEntity.SecretKey = string.Empty;

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.AreEqual(2, errors.Count);
        }


        [Fact]
        public void Validate_ReturnsValidationError_WhenStoreIsNull_Test()
        {
            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(null));

            Assert.AreEqual(1, errors.Count);
        }

    }
}
