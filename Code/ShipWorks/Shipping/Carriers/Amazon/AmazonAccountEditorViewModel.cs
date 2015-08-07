using System;
using System.ComponentModel;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Model for the Amazon account editor
    /// </summary>
    public class AmazonAccountEditorViewModel
    {
        private readonly IAmazonAccountManager accountManager;

        public AmazonAccountEditorViewModel(IAmazonCredentials credentials, IAmazonAccountManager accountManager)
        {
            this.accountManager = accountManager;

            Credentials = credentials;
            Person = new PersonAdapter();
        }

        /// <summary>
        /// Load the given account into the view model
        /// </summary>
        public void Load(AmazonAccountEntity account)
        {
            MethodConditions.EnsureArgumentIsNotNull(account, () => account);

            Credentials.MerchantId = account.MerchantID;
            Credentials.AuthToken = account.AuthToken;

            PersonAdapter.Copy(account, string.Empty, Person);
        }

        /// <summary>
        /// Credentials for the related Amazon account
        /// </summary>
        public IAmazonCredentials Credentials { get; private set; }

        /// <summary>
        /// Person view of the related Amazon account
        /// </summary>
        public PersonAdapter Person { get; private set; }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(AmazonAccountEntity account)
        {
            MethodConditions.EnsureArgumentIsNotNull(account, () => account);

            Credentials.Validate();

            if (!Credentials.Success)
            {
                Success = false;
                Message = Credentials.Message;
                return;
            }

            Credentials.PopulateAccount(account);
            PersonAdapter.Copy(Person, new PersonAdapter(account, string.Empty));

            try
            {
                accountManager.SaveAccount(account);

                Success = true;
                Message = string.Empty;
            }
            catch (ORMConcurrencyException)
            {
                Success = false;
                Message = "Your changes cannot be saved because another use has deleted the account.";
            }
        }

        /// <summary>
        /// Was the result of the save operation successful?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message that was the result of the save operation
        /// </summary>
        public string Message { get; set; }
    }
}