using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using System.Diagnostics;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using System.Linq;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Utility;
using Interapptive.Shared.Business;
using System.ComponentModel.DataAnnotations;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Controls
{
    /// <summary>
    /// Control for editing the details of a person
    /// </summary>
    public partial class PersonControl : UserControl
    {
        // Controls if it's always editable, or can go into edit-mode.
        PersonEditStyle editStyle = PersonEditStyle.Normal;

        // Controls what fields are avialable
        PersonFields availableFields = PersonFields.All;

        // Controls what fields are required
        PersonFields requiredFields = PersonFields.None;

        // Indicates if the control is currently in readonly mode.
        bool isReadonly = false;

        /// <summary>
        /// The user has typed something into the control.  This is kind of like TextChanged for a text box, 
        /// except for the whole thing.
        /// </summary>
        public event EventHandler ContentChanged;

        /// <summary>
        /// The user has changed the destination state or country
        /// </summary>
        public event EventHandler DestinationChanged;

        // The adapter from which the current data was loaded
        List<PersonAdapter> loadedPeople = null;

        // List of controls on the form and the person fields they are utilized by
        List<ControlFieldMap> controlFieldMappings;

        // Maps a control to the label that labels it on the form
        Dictionary<Control, Label> labelMap;

        class ControlFieldMap
        {
            public ControlFieldMap(Control control, PersonFields fields)
            {
                Control = control;
                Fields = fields;
            }

            public Control Control { get; set; }
            public PersonFields Fields { get; set; }
        }

        /// <summary>
        /// Specialied BindingSource that automatically adds non-existing countries to the bound list so we don't just default to Albania
        /// </summary>
        class CountryBindingSource : BindingSource
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public CountryBindingSource() :
                base(CreateCountryView(), "")
            {

            }

            /// <summary>
            /// Get a DataView representing the list of countries
            /// </summary>
            private static DataView CreateCountryView()
            {
                DataTable newSource = new DataTable();
                newSource.Columns.Add("Name", typeof(string));

                foreach (string name in Geography.Countries)
                {
                    newSource.Rows.Add(name);
                }

                return newSource.DefaultView;
            }

            /// <summary>
            /// Searches for the index of the item that has the given PropertyDescriptor
            /// </summary>
            public override int Find(PropertyDescriptor prop, object key)
            {
                int index = base.Find(prop, key);

                // The value wasn't found - we need to add it
                if (index < 0)
                {
                    DataView view = List as DataView;
                    view.Table.Rows.Add(key);

                    // repeat the search
                    index = base.Find(prop, key);
                }

                return index;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonControl()
        {
            InitializeComponent();

            country.TextChanged += this.OnComboBoxTextChanged;
            state.TextChanged += this.OnComboBoxTextChanged;

            state.TextChanged += this.OnDestinationChanged;

            InitializeFieldMappings();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            EnsureCountryInitialized();

            UpdateAvailableFieldsUI();
        }

        /// <summary>
        /// Ensure the control gets initialized one time
        /// </summary>
        private void EnsureCountryInitialized()
        {
            if (country.DataSource != null)
            {
                return;
            }

            // Country drop down
            country.DisplayMember = "Name";
            country.ValueMember = "Name";
            country.DataSource = new CountryBindingSource();

            // Default to use
            country.SelectedValue = Geography.GetCountryName("US");
        }

        /// <summary>
        /// Initialize all of the field mappings
        /// </summary>
        private void InitializeFieldMappings()
        {
            // Control to label mappings
            labelMap = new Dictionary<Control, Label>();
            labelMap[fullName] = labelFullName;
            labelMap[company] = labelCompany;
            labelMap[street] = labelStreet;
            labelMap[city] = labelCity;
            labelMap[state] = labelStateProv;
            labelMap[postalCode] = labelPostalCode;
            labelMap[country] = labelCountry;
            labelMap[email] = labelEmail;
            labelMap[phone] = labelPhone;
            labelMap[fax] = labelFax;
            labelMap[website] = labelWebsite;

            // Maps controls to the fields they are controlled by
            controlFieldMappings = new List<ControlFieldMap>
                {
                    new ControlFieldMap(labelName, PersonFields.Name | PersonFields.Company),
                    new ControlFieldMap(fullName, PersonFields.Name),
                    new ControlFieldMap(company, PersonFields.Company),
                    new ControlFieldMap(labelAddress, PersonFields.Street | PersonFields.City | PersonFields.Street | PersonFields.Postal | PersonFields.Country | PersonFields.Residential),
                    new ControlFieldMap(street, PersonFields.Street),
                    new ControlFieldMap(city, PersonFields.City),
                    new ControlFieldMap(state, PersonFields.State),
                    new ControlFieldMap(postalCode, PersonFields.Postal),
                    new ControlFieldMap(country, PersonFields.Country),
                    new ControlFieldMap(labelContactInfo, PersonFields.Email | PersonFields.Phone | PersonFields.Fax | PersonFields.Website),
                    new ControlFieldMap(email, PersonFields.Email),
                    new ControlFieldMap(phone, PersonFields.Phone),
                    new ControlFieldMap(fax, PersonFields.Fax),
                    new ControlFieldMap(website, PersonFields.Website)
                };
        }

        /// <summary>
        /// Controls if the address is always editable, or if it needs to go into edit mode.
        /// </summary>
        [DefaultValue(PersonEditStyle.Normal)]
        public PersonEditStyle EditStyle
        {
            get
            {
                return editStyle;
            }
            set
            {
                if (editStyle == value)
                {
                    return;
                }

                editStyle = value;

                if (editStyle == PersonEditStyle.Normal && isReadonly)
                {
                    MakeEditable();
                }

                if (editStyle == PersonEditStyle.ReadOnly && !isReadonly)
                {
                    MakeReadonly();
                }
            }
        }

        /// <summary>
        /// Fields to Validate as not empty when ValidateRequiredFields is called.
        /// IMPORTANT: This should only be used when not in MultiValued mode.
        /// </summary>
        [DefaultValue(PersonFields.None)]
        [Category("Misc")]
        public PersonFields RequiredFields
        {
            get
            {
                return requiredFields;
            }
            set
            {
                requiredFields = value;
            }
        }

        /// <summary>
        /// Set which fields are available for editing
        /// </summary>
        [DefaultValue(PersonFields.All)]
        public PersonFields AvailableFields
        {
            get
            {
                return availableFields;
            }
            set
            {
                if (value == availableFields)
                {
                    return;
                }

                availableFields = value;

                UpdateAvailableFieldsUI();
            }
        }

        /// <summary>
        /// The Maximum number of lines in the street field. (Between 1-3)
        /// </summary>
        [DefaultValue(3)]
        [Range(1, 3)]
        [Browsable(true)]
        [Category("Misc")]
        public int MaxStreetLines
        {
            get
            {
                return street.MaxLines;
            }
            set
            {
                street.MaxLines = value;
                street.Height = (13 * MaxStreetLines) + 8;
                UpdateAvailableFieldsUI();
            }
        }

        /// <summary>
        /// Validate that RequiredFields have data entered.
        /// IMPORTANT: This should only be used when not in MultiValued mode.
        /// </summary>
        public bool ValidateRequiredFields()
        {
            if (RequiredFields == PersonFields.None)
            {
                return true;
            }

            List<string> emptyFieldNames = new List<string>();
            
            foreach (ControlFieldMap controlFieldMap in controlFieldMappings)
            {
                if (controlFieldMap.Control is Label || !RequiredField(controlFieldMap.Fields))
                {
                    continue;
                }

                bool fieldIsEmpty = false;

                MultiValueTextBox multiValueTextBox = controlFieldMap.Control as MultiValueTextBox;
                if (multiValueTextBox != null)
                {
                    if (multiValueTextBox.MultiValued)
                    {
                        throw new InvalidOperationException("ValidateRequiredFields not valid in a MultiValued scenario.");
                    }
                 
                    if (string.IsNullOrEmpty(multiValueTextBox.Text))
                    {
                        fieldIsEmpty = true;
                    }
                }

                StreetEditor streetEditor = controlFieldMap.Control as StreetEditor;
                if (streetEditor != null)
                {
                    if (streetEditor.MultiValued)
                    {
                        throw new InvalidOperationException("ValidateRequiredFields not valid in a MultiValued scenario.");
                    }

                    if (String.IsNullOrEmpty(streetEditor.Line1))
                    {
                        fieldIsEmpty = true;
                    }
                }

                MultiValueComboBox multiValueComboBox = controlFieldMap.Control as MultiValueComboBox;
                if (multiValueComboBox != null)
                {
                    if (multiValueComboBox.MultiValued)
                    {
                        throw new InvalidOperationException("ValidateRequiredFields not valid in a MultiValued scenario.");
                    }

                    if (string.IsNullOrEmpty(multiValueComboBox.Text))
                    {
                        fieldIsEmpty = true;
                    }
                }

                if (fieldIsEmpty)
                {
                    emptyFieldNames.Add(labelMap[controlFieldMap.Control].Text.Replace(":", ""));
                }
            }

            // If any were missing
            if (emptyFieldNames.Count > 0)
            {
                string message = "The following fields are required:\n";

                foreach (string name in emptyFieldNames)
                {
                    message += string.Format("\n    {0}", name);
                }

                MessageHelper.ShowError(this, message);

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update the UI based on what fields are set to be available.
        /// </summary>
        private void UpdateAvailableFieldsUI()
        {
            int nextTop = labelName.Top;

            // Update control positioning and availablility
            foreach (ControlFieldMap map in controlFieldMappings)
            {
                bool show = ShowField(map.Fields);

                Control control = map.Control;

                // In ReadOnly mode, ComboBox's are tagged with their readonly text box that is shown.
                TextBox readOnlyBox = map.Control.Tag as TextBox;
                if (readOnlyBox != null)
                {
                    readOnlyBox.Visible = show;

                    control = readOnlyBox;
                }

                // Show it if its available (And their is not a readonly version)
                else
                {
                    control.Visible = show;
                }

                // There are 7 pixels shorter for readonly textboxes than normal (multilines stay the same)
                bool offsetReadOnly = control is TextBox && !((TextBox) control).Multiline && isReadonly;

                if (show)
                {
                    control.Top = nextTop;

                    // Account for 3 of the 7 pixels.  The number is 3 b\c that's how far it offsets the box in MakeReadOnly
                    if (offsetReadOnly)
                    {
                        control.Top += 3;
                    }

                    nextTop = control.Bottom + 6;

                    // Offset the reamining 4 pixels.
                    if (offsetReadOnly)
                    {
                        nextTop += 4;
                    }
                }

                Label label;
                if (labelMap.TryGetValue(map.Control, out label))
                {
                    label.Visible = show;

                    int offset = (control is CheckBox) ? 1 : (offsetReadOnly ? 0 : 3);
                    label.Top = control.Top + offset;
                }
            }
        }

        /// <summary>
        /// Idicates if the given field is required.
        /// </summary>
        private bool RequiredField(PersonFields field)
        {
            return (RequiredFields & field) != 0;
        }

        /// <summary>
        /// Indicates if the given field should be displayed
        /// </summary>
        private bool ShowField(PersonFields field)
        {
            return (availableFields & field) != 0;
        }

        /// <summary>
        /// Indicates if the control is currently in the readonly state
        /// </summary>
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool IsReadonly
        {
            get
            {
                return isReadonly;
            }
        }

        /// <summary>
        /// Clear the contents of the controls, and any entities cached in the control
        /// </summary>
        public void Clear()
        {
            if (loadedPeople != null)
            {
                loadedPeople.Clear();
                loadedPeople = null;
            }

            fullName.Text = "";
            company.Text = "";

            street.Line1 = "";
            street.Line2 = "";
            street.Line3 = "";

            city.Text = "";
            state.SelectedIndex = -1;
            state.Text = "";
            postalCode.Text = "";
            country.SelectedIndex = -1;

            email.Text = "";
            phone.Text = "";
            fax.Text = "";
            website.Text = "";
        }

        /// <summary>
        /// Load the data from the given person into the controls
        /// </summary>
        public void LoadEntity(PersonAdapter person)
        {
            LoadEntities(new List<PersonAdapter> { person });
        }

        /// <summary>
        /// Load the given list of entities into the control.
        /// </summary>
        public void LoadEntities(List<PersonAdapter> list)
        {
            // Ensure we are created and initialized
            EnsureCountryInitialized();

            loadedPeople = list;

            if (list == null || list.Count == 0)
            {
                Clear();
                return;
            }

            using (MultiValueScope scope = new MultiValueScope())
            {
                // Go through each additional person, and see if there are any conflicts
                foreach (PersonAdapter person in loadedPeople)
                {
                    fullName.ApplyMultiText(new PersonName(person).FullName);
                    company.ApplyMultiText(person.Company);

                    street.ApplyMultiText(StreetEditor.CombinLines(person.Street1, person.Street2, person.Street3));

                    city.ApplyMultiText(person.City);
                    state.ApplyMultiText(Geography.GetStateProvName(person.StateProvCode, person.CountryCode));
                    postalCode.ApplyMultiText(person.PostalCode);
                    country.ApplyMultiValue(Geography.GetCountryName(person.CountryCode));

                    email.ApplyMultiText(person.Email);
                    phone.ApplyMultiText(person.Phone);
                    fax.ApplyMultiText(person.Fax);
                    website.ApplyMultiText(person.Website);
                }
            }
        }

        /// <summary>
        /// Save the data from the controls to the loaded persons
        /// </summary>
        public void SaveToEntity()
        {
            if (loadedPeople == null)
            {
                return;
            }

            SaveToEntities(loadedPeople);
        }

        /// <summary>
        /// Save the data from the controls into the given entity. The original list of loaded people is
        /// left alone.
        /// </summary>
        public void SaveToEntity(PersonAdapter person)
        {
            SaveToEntities(new List<PersonAdapter> { person });
        }

        /// <summary>
        /// Save the data from the controls into the given list.  The original list of loaded people is
        /// left alone.
        /// </summary>
        public void SaveToEntities(IEnumerable<PersonAdapter> list)
        {
            foreach (PersonAdapter person in list)
            {
                fullName.ReadMultiText(value =>
                {
                    PersonName name = PersonName.Parse(value);

                    int maxFirst = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonFirst);
                    if (name.First.Length > maxFirst)
                    {
                        name.Middle = name.First.Substring(maxFirst) + name.Middle;
                        name.First = name.First.Substring(0, maxFirst);
                    }

                    int maxMiddle = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonMiddle);
                    if (name.Middle.Length > maxMiddle)
                    {
                        name.Last = name.Middle.Substring(maxMiddle) + name.Last;
                        name.Middle = name.Middle.Substring(0, maxMiddle);
                    }

                    int maxLast = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonLast);
                    if (name.Last.Length > maxLast)
                    {
                        name.Last = name.Last.Substring(0, maxLast);
                    }                 

                    person.FirstName = name.First;
                    person.MiddleName = name.Middle;
                    person.LastName = name.LastWithSuffix;
                    person.UnparsedName = name.UnparsedName;
                    person.NameParseStatus = name.ParseStatus;
                });
                company.ReadMultiText(value => person.Company = value);

                street.ReadMultiText(value =>
                {
                    int maxStreet1 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet1);
                    int maxStreet2 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet2);
                    int maxStreet3 = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonStreet3);

                    string line1 = street.Line1;
                    string line2 = street.Line2;
                    string line3 = street.Line3;

                    if (line1.Length > maxStreet1)
                    {
                        line2 = line1.Substring(maxStreet1) + " " + line2;
                        line1 = line1.Substring(0, maxStreet1);
                    }

                    if (line2.Length > maxStreet2)
                    {
                        line3 = line2.Substring(maxStreet2) + " " + line3;
                        line2 = line2.Substring(0, maxStreet2);
                    }

                    if (line3.Length > maxStreet3)
                    {
                        line3 = line3.Substring(0, maxStreet3);
                    }

                    person.Street1 = line1;
                    person.Street2 = line2;
                    person.Street3 = line3;
                });

                city.ReadMultiText(value => person.City = value);
                state.ReadMultiText(value => person.StateProvCode = Geography.GetStateProvCode(value));
                postalCode.ReadMultiText(value => person.PostalCode = value);
                country.ReadMultiText(value => person.CountryCode = Geography.GetCountryCode(country.Text));

                email.ReadMultiText(value => person.Email = value);
                phone.ReadMultiText(value => person.Phone = value);
                fax.ReadMultiText(value => person.Fax = value);
                website.ReadMultiText(value => person.Website = value);
            }
        }

        /// <summary>
        /// Copy all the address data from the other address control
        /// </summary>
        public void CopyFrom(PersonControl other)
        {
            fullName.Text = other.fullName.Text;
            company.Text = other.company.Text;

            street.Line1 = other.street.Line1;
            street.Line2 = other.street.Line2;
            street.Line3 = other.street.Line3;

            city.Text = other.city.Text;
            state.Text = other.state.Text;
            postalCode.Text = other.postalCode.Text;
            country.Text = other.country.Text;

            email.Text = other.email.Text;
            phone.Text = other.phone.Text;
            fax.Text = other.fax.Text;
            website.Text = other.website.Text;
        }

        /// <summary>
        /// The full name of the person as entered in the control, or null if multiple values.
        /// </summary>
        public string FullName
        {
            get
            {
                if (fullName.MultiValued)
                {
                    return null;
                }

                return fullName.Text;
            }
            set
            {
                fullName.Text = value;
            }
        }

        /// <summary>
        /// The selected country code, or null if not available.
        /// </summary>
        public string CountryCode
        {
            get
            {
                if (country.MultiValued)
                {
                    return null;
                }

                return Geography.GetCountryCode(country.Text);
            }
        }

        /// <summary>
        /// Make all the controls look readonly
        /// </summary>
        private void MakeReadonly()
        {
            if (isReadonly)
            {
                return;
            }

            isReadonly = true;

            foreach (ComboBox comboBox in Controls.OfType<ComboBox>().ToList())
            {
                TextBox textBox = new TextBox();
                textBox.Text = comboBox.Text;
                textBox.Location = comboBox.Location;

                Controls.Add(textBox);
                Controls.SetChildIndex(textBox, Controls.IndexOf(comboBox));

                comboBox.Visible = false;
                comboBox.Tag = textBox;
            }

            foreach (TextBox textBox in Controls.OfType<TextBox>())
            {
                textBox.BorderStyle = BorderStyle.None;
                textBox.Location = new Point(textBox.Left + 3, textBox.Top + 3);

                textBox.ReadOnly = true;
                textBox.BackColor = BackColor;
            }

            foreach (CheckBox checkBox in Controls.OfType<CheckBox>())
            {
                checkBox.Enabled = false;
            }

            UpdateAvailableFieldsUI();
        }

        /// <summary>
        /// Make all the controls look editable
        /// </summary>
        private void MakeEditable()
        {
            if (!isReadonly)
            {
                return;
            }

            isReadonly = false;

            foreach (ComboBox comboBox in Controls.OfType<ComboBox>().ToList())
            {
                TextBox textBox = (TextBox) comboBox.Tag;
                Controls.Remove(textBox);

                comboBox.Visible = true;
                comboBox.Tag = null;
            }

            foreach (TextBox textBox in Controls.OfType<TextBox>())
            {
                textBox.BorderStyle = BorderStyle.Fixed3D;
                textBox.Location = new Point(textBox.Left - 3, textBox.Top - 3);

                textBox.ReadOnly = false;
                textBox.BackColor = new TextBox().BackColor;
            }

            foreach (CheckBox checkBox in Controls.OfType<CheckBox>())
            {
                checkBox.Enabled = true;
            }

            UpdateAvailableFieldsUI();
        }

        /// <summary>
        /// The text of one of the comboboxes has changed
        /// </summary>
        private void OnComboBoxTextChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox) sender;
            TextBox textBox = comboBox.Tag as TextBox;

            // Update the associated text box so when we switch to readonly mode its synced
            if (textBox != null)
            {
                textBox.Text = comboBox.Text;
            }
        }

        /// <summary>
        /// When the state drops down, make sure the list is appropriate to the current country
        /// </summary>
        private void OnStateDropDown(object sender, EventArgs e)
        {
            // Save the text that is in there now
            bool isMultiValue = state.MultiValued;
            string stateProv = state.Text;

            // Get the country
            string countryCode = Geography.GetCountryCode(country.Text);

            if (countryCode == "US" && state.Items.Count != Geography.States.Count)
            {
                state.Items.Clear();
                state.Items.AddRange(Geography.States.ToArray());
            }
            else if (countryCode == "CA" && state.Items.Count != Geography.Provinces.Count)
            {
                state.Items.Clear();
                state.Items.AddRange(Geography.Provinces.ToArray());
            }
            else if (countryCode != "US" && countryCode != "CA")
            {
                state.Items.Clear();
            }

            // Set the text back
            if (!isMultiValue)
            {
                state.Text = stateProv;
            }
            else
            {
                state.MultiValued = true;
            }
        }

        /// <summary>
        /// A value in the control has changed
        /// </summary>
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The selected country or state has changed
        /// </summary>
        private void OnDestinationChanged(object sender, EventArgs e)
        {
            if (DestinationChanged != null)
            {
                DestinationChanged(this, EventArgs.Empty);
            }
        }
    }
}
