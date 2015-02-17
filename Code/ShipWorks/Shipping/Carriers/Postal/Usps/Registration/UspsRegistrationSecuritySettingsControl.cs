using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Control for collecting username, password, and security questions for a Stamps.com registration.
    /// </summary>
    public partial class UspsRegistrationSecuritySettingsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationSecuritySettingsControl"/> class.
        /// </summary>
        public UspsRegistrationSecuritySettingsControl()
        {
            InitializeComponent();

            LoadSecurityQuestions(firstCodewordType);
            LoadSecurityQuestions(secondCodewordType);
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username 
        { 
            get { return username.Text; } 
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password
        {
            get { return password.Text; }
        }

        /// <summary>
        /// Gets the first type of the security question.
        /// </summary>
        public CodewordType2 FirstSecurityQuestionType
        {
            get { return GetSelectedCodewordType(firstCodewordType); }
        }

        /// <summary>
        /// Gets the first security question answer.
        /// </summary>
        public string FirstSecurityQuestionAnswer
        {
            get { return firstCodewordValue.Text; }
        }

        /// <summary>
        /// Gets the type of the second security question.
        /// </summary>
        public CodewordType2 SecondSecurityQuestionType
        {
            get { return GetSelectedCodewordType(secondCodewordType); }
        }

        /// <summary>
        /// Gets the second security question answer.
        /// </summary>
        public string SecondSecurityQuestionAnswer
        {
            get { return secondCodewordValue.Text; }
        }

        /// <summary>
        /// Gets the type of the selected codeword.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <returns>The CodewordType.</returns>
        private CodewordType2 GetSelectedCodewordType(ComboBox comboBox)
        {
            CodewordDropdownItem selectedItem = comboBox.SelectedItem as CodewordDropdownItem;

            if (selectedItem == null)
            {
                throw new InvalidOperationException("A codeword was not selected.");
            }

            return selectedItem.CodewordType;
        }


        private void LoadSecurityQuestions(ComboBox comboBox)
        {
            List<CodewordDropdownItem> securityQuestions = new List<CodewordDropdownItem>();
            
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.BirthCity, "What is your city of birth?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.Last4DriversLicense, "What are the last 4 digits of your driver's license number?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.Last4SocialSecurityNumber, "What are the last 4 digits of your social security number?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.MothersMaidenName, "What is your mother's maiden name?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.PetsName, "What is your pet's name?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.FathersBirthplace, "What is your father's birthplace?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.FirstCarsMakeModel, "What is the make and model of your first car?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.FirstSchoolsName, "What is name of the first school you attended?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.HighSchoolMascot, "What was your high school mascot?"));
            securityQuestions.Add(new CodewordDropdownItem(CodewordType2.StreetName, "What is your street name?"));

            comboBox.Items.Add("Choose one");
            comboBox.Items.AddRange(securityQuestions.ToArray());

            comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>A collection of RegistrationValidationError objects if the data is not valid. An empty 
        /// collection indicates that everything is valid.</returns>
        public IEnumerable<RegistrationValidationError> ValidateRegistrationSettings()
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();

            // Just perform light validation here: we have a username, password, different security questions, 
            // and that we have answers to the questions
            username.Text = username.Text.Trim();

            if (string.IsNullOrEmpty(username.Text))
            {
                errors.Add(new RegistrationValidationError("A username is required."));
            }

            if (string.IsNullOrEmpty(password.Text))
            {
                errors.Add(new RegistrationValidationError("A password is required."));
            }

            if (password.Text != retypePassword.Text)
            {
                errors.Add(new RegistrationValidationError("The password and the re-typed password do not match."));
            }

            if (firstCodewordType.SelectedItem is CodewordDropdownItem && secondCodewordType.SelectedItem is CodewordDropdownItem)
            {
                // Selections have been made for the security questions, so make sure they are different
                if (firstCodewordType.SelectedItem == secondCodewordType.SelectedItem)
                {
                    errors.Add(new RegistrationValidationError("Security questions 1 and 2 must be different."));
                }
            }
            else
            {
                // A selection hasn't been made for the security question - the selected item is the "Choose One" string
                errors.Add(new RegistrationValidationError("You must select two security questions."));
            }

            if (string.IsNullOrEmpty(firstCodewordValue.Text.Trim()) || string.IsNullOrEmpty(secondCodewordValue.Text.Trim()))
            {
                errors.Add(new RegistrationValidationError("Answers must be provided for both security questions."));
            }

            return errors;
        }
    }
}
