using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// Wizard page for entering an address to use with One Balance
    /// </summary>
    public partial class OneBalanceAccountAddressPage : WizardPage
    {
        private readonly IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity;
        private readonly IMessageHelper messageHelper;
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAccountAddressPage(IOneBalanceUpsAccountRegistrationActivity accountRegistrationActivity, IMessageHelper messageHelper, UpsAccountEntity upsAccount)
        {
            this.accountRegistrationActivity = accountRegistrationActivity;
            this.messageHelper = messageHelper;
            this.upsAccount = upsAccount;
            InitializeComponent();

            StepNext += OnStepNext;
        }

        /// <summary>
        /// When trying to proceed past this wizard page, check that all required fields are filled in and that all
        /// fields are less than their maximum length. If validation passes, register the UPS account with One Balance.
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // validate required fields
            if (!personControl.ValidateRequiredFields())
            {
                e.NextPage = this;
                return;
            }

            // create PersonAdapter from control
            PersonAdapter personAdapter = new PersonAdapter();
            personControl.SaveToEntity(personAdapter);

            // Validate field lengths
            Result validationResult = ValidateFieldLengths(personAdapter);
            if (validationResult.Failure)
            {
                messageHelper.ShowError(validationResult.Message);
                e.NextPage = this;
                return;
            }

            // populate the account with the given address info
            PersonAdapter.Copy(personAdapter, new PersonAdapter(upsAccount, ""));

            // execute account registration activity
            Result result = accountRegistrationActivity.Execute(upsAccount);
            if (result.Failure)
            {
                messageHelper.ShowError(this, result.Message);
                e.NextPage = this;
            }
        }

        /// <summary>
        /// Ensure that all fields are less than their maximum length
        /// </summary>
        private Result ValidateFieldLengths(PersonAdapter personAdapter)
        {
            List<Result> validationResults = new List<Result>();

            validationResults.Add(ValidateFieldLength("Name", personAdapter.ParsedName.FullName.Length, 20));
            validationResults.Add(ValidateFieldLength("Company", personAdapter.Company.Length, 30));
            validationResults.Add(ValidateFieldLength("Street Line 1", personAdapter.Street1.Length, 30));
            validationResults.Add(ValidateFieldLength("Street Line 2", personAdapter.Street2.Length, 30));
            validationResults.Add(ValidateFieldLength("Street Line 3", personAdapter.Street3.Length, 30));
            validationResults.Add(ValidateFieldLength("City", personAdapter.City.Length, 30));
            validationResults.Add(ValidateFieldLength("State", personAdapter.StateProvCode.Length, 2));
            validationResults.Add(ValidateFieldLength("Postal Code", personAdapter.PostalCode.Length, 5));
            validationResults.Add(ValidateFieldLength("Country", personAdapter.CountryCode.Length, 2));

            var errors = validationResults.Where(r => r.Failure);

            ComparisonResult comparisonResult = errors.CompareCountTo(1);

            switch (comparisonResult)
            {
                case ComparisonResult.Equal:
                    return Result.FromError(errors.FirstOrDefault().Message);
                case ComparisonResult.More:
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("The following errors occured");
                    foreach (Result error in errors)
                    {
                        stringBuilder.AppendLine($"\t- {error.Message}");
                    }

                    return Result.FromError(stringBuilder.ToString());
                }
                default:
                    return Result.FromSuccess();
            }
        }

        /// <summary>
        /// Check if a field is less than it's maximum length
        /// </summary>
        private Result ValidateFieldLength(string fieldName, int fieldLength, int maxFieldLength)
        {
            return fieldLength <= maxFieldLength ?
                Result.FromSuccess() :
                Result.FromError($"\"{fieldName}\" must be less than {maxFieldLength} characters");
        }
    }
}
