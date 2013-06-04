using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// A static class for validating Newegg account settings.
    /// </summary>
    public static class NeweggAccountSettingsValidator
    {
        /// <summary>
        /// Determines whether the seller ID value provided is valid.
        /// </summary>
        /// <param name="sellerId">The seller ID.</param>
        /// <returns>
        ///   <c>true</c> if the seller ID value is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSellerIdValid(string sellerId)
        {
            bool isValid = false;
            
            if (!string.IsNullOrWhiteSpace(sellerId))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Determines whether the secret key value provided is valid.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>
        ///   <c>true</c> if the secret key value is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSecretKeyValid(string secretKey)
        {
            bool isValid = false;

            // Based on the Newegg API documentation, the secret key is a GUID,
            // but we treat it as a normal string value here since the docs don't 
            // come out and say it *is* a GUID
            if (!string.IsNullOrWhiteSpace(secretKey))
            {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// Validates the account setting (seller ID and secret key) for the full store given by executing each of the individual 
        /// validate methods.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>An IEnumerable of ValidationErrors containing any items that triggered validation to fail.</returns>
        public static IEnumerable<ValidationError> Validate(NeweggStoreEntity store)
        {
            List<ValidationError> errors = new List<ValidationError>();

            if (store != null)
            {
                if (!IsSellerIdValid(store.SellerID))
                {
                    errors.Add(new ValidationError(store, "ShipWorks was unable to validate your seller ID. Please check your seller ID and try again."));
                }

                if (!IsSecretKeyValid(store.SecretKey))
                {
                    errors.Add(new ValidationError(store, "ShipWorks was unable to validate your secret key. Please check your secret key and try again."));
                }
            }
            else
            {
                errors.Add(new ValidationError(store, "There was an attempt to validate null store value."));
            }

            return errors;
        }
    }
}
