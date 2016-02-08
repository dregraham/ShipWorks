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
        public NeweggAccountSettingsValidatorTest()
        {
        }

        [Fact]
        public void IsSellerIdValid_ReturnsFalse_WhenEmptyString()
        {
            string sellerId = string.Empty;
            Assert.False(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }
                
        [Fact]
        public void IsSellerIdValid_ReturnsFalse_WhenOnlyWhitespace()
        {
            string sellerId = "        ";
            Assert.False(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenNoWhiteSpaceCharacters()
        {
            string sellerId = "MySellerId";
            Assert.True(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenAlphaNumericAndWhiteSpaceCharacters()
        {
            string sellerId = "My Seller ID";
            Assert.True(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSellerIdValid_ReturnsTrue_WhenUsingSandboxedSellerId()
        {
            // This test uses the actual seller ID that Newegg provided to us
            // for our sandboxed seller account. This test is similar to the 
            // case for no white space characters, but tests/documents that
            // any future changes to IsSellerIdValid method still returns
            // true for a "real" seller ID
            string sellerId = "A09V";
            Assert.True(NeweggAccountSettingsValidator.IsSellerIdValid(sellerId));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsFalse_WhenEmptyString()
        {
            string secretKey = string.Empty;
            Assert.False(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsFalse_WhenOnlyWhitespace()
        {
            string secretKey = "        ";
            Assert.False(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenNoWhiteSpaceCharacters()
        {
            string secretKey = "MySecretKeyValue";
            Assert.True(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenAlphaNumericAndWhiteSpaceCharacters()
        {
            string secretKey = "My Secret Key Value";
            Assert.True(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenInGuidFormat()
        {
            // Since the secret key value appears to be a GUID based on Newegg documentation, 
            // this test is to confirm that a GUID string is a valid secret key
            string secretKey = Guid.NewGuid().ToString();
            Assert.True(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }

        [Fact]
        public void IsSecretKeyValid_ReturnsTrue_WhenUsingSandboxedSecretKey()
        {
            // This test uses the actual secret key that Newegg provided to us
            // for our sandboxed seller account. This test is similar to the 
            // case for no white space characters, but tests/documents that
            // any future changes to IsSecretKeyValid method still returns
            // true for a "real" secret key
            string secretKey = ": E09799F3-A8FD-46E0-989F-B8587A1817E0";
            Assert.True(NeweggAccountSettingsValidator.IsSecretKeyValid(secretKey));
        }


        [Fact]
        public void Validate_ReturnsValidationError_WhenSellerIdIsInvalid()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = string.Empty;
            storeEntity.SecretKey = Guid.NewGuid().ToString();

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.Equal(1, errors.Count);
        }

        [Fact]
        public void Validate_ReturnsValidationError_WhenSecretKeyIsInvalid()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = "A09V";
            storeEntity.SecretKey = string.Empty;

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.Equal(1, errors.Count);
        }

        [Fact]
        public void Validate_ValidationErrorListIsEmpty_WhenStoreEntityIsValid()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = "A09V";
            storeEntity.SecretKey = Guid.NewGuid().ToString();

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.Equal(0, errors.Count);
        }

        [Fact]
        public void Validate_ReturnsMultipleValidationErrors_WhenSecretKeyAndSellerIdAreInvalid()
        {
            NeweggStoreEntity storeEntity = new NeweggStoreEntity();
            storeEntity.SellerID = string.Empty;
            storeEntity.SecretKey = string.Empty;

            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(storeEntity));
            Assert.Equal(2, errors.Count);
        }


        [Fact]
        public void Validate_ReturnsValidationError_WhenStoreIsNull()
        {
            List<ValidationError> errors = new List<ValidationError>(NeweggAccountSettingsValidator.Validate(null));

            Assert.Equal(1, errors.Count);
        }

    }
}
