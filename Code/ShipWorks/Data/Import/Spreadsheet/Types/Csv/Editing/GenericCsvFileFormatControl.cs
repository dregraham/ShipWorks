using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// UserControl for editing generic csv file format settings
    /// </summary>
    public partial class GenericCsvFileFormatControl : UserControl
    {
        public event EventHandler FileFormatChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvFileFormatControl()
        {
            InitializeComponent();

            LoadDelimiters();
            LoadQuotes();
            LoadEncodings();

            OnChangeQuotes(null, EventArgs.Empty);
        }
        
        /// <summary>
        /// The delimiter
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char Delimiter
        {
            get { return (char) delimiter.SelectedValue; }
            set { delimiter.SelectedValue = value; }
        }

        /// <summary>
        /// The field quotes
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char Quotes
        {
            get { return (char) textQualifier.SelectedValue; }
            set { textQualifier.SelectedValue = value; }
        }

        /// <summary>
        /// The escape for intra field literal quote characters
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char QuoteEscape
        {
            get { return (char) quoteEscapes.SelectedValue; }
            set { quoteEscapes.SelectedValue = value; }
        }

        /// <summary>
        /// The encoding to read the input file
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Encoding
        {
            get { return (string) fileEncoding.SelectedValue; }
            set { fileEncoding.SelectedValue = value; }
        }

        /// <summary>
        /// Load the choices for delimiters
        /// </summary>
        private void LoadDelimiters()
        {
            var delimiters = new ArrayList
                {
                    new { Name = GenericCsvUtility.GetCharacterDescription(','),  Value = ',' },
                    new { Name = GenericCsvUtility.GetCharacterDescription(';'),  Value = ';' },
                    new { Name = GenericCsvUtility.GetCharacterDescription('\t'), Value = '\t' },
                };

            delimiter.DisplayMember = "Name";
            delimiter.ValueMember = "Value";
            delimiter.DataSource = delimiters;

            delimiter.SelectedIndex = 0;
        }

        /// <summary>
        /// Load the choices for quotes
        /// </summary>
        private void LoadQuotes()
        {
            var qualifiers = new ArrayList
                {
                    new { Name = GenericCsvUtility.GetCharacterDescription('"'),  Value = '"' },
                    new { Name = GenericCsvUtility.GetCharacterDescription('\''), Value = '\'' },
                    new { Name = GenericCsvUtility.GetCharacterDescription('\0'), Value = '\0'}
                };

            textQualifier.DisplayMember = "Name";
            textQualifier.ValueMember = "Value";
            textQualifier.DataSource = qualifiers;

            textQualifier.SelectedIndex = 0;
        }

        /// <summary>
        /// Load the list of valid escapes for literal quotes
        /// </summary>
        private void LoadQuoteEscapes()
        {
            quoteEscapes.SelectedIndexChanged -= this.OnChangeEscape;

            int selectedIndex = quoteEscapes.SelectedIndex;

            char quotes = (char) textQualifier.SelectedValue;

            var escapes = new ArrayList
                {
                    new { Name = GenericCsvUtility.GetCharacterDescription(quotes),  Value = quotes },
                    new { Name = GenericCsvUtility.GetCharacterDescription('\\'), Value = '\\' },
                };

            quoteEscapes.DisplayMember = "Name";
            quoteEscapes.ValueMember = "Value";
            quoteEscapes.DataSource = escapes;

            quoteEscapes.SelectedIndex = selectedIndex;
            if (quoteEscapes.SelectedIndex == -1)
            {
                quoteEscapes.SelectedIndex = 0;
            }

            quoteEscapes.SelectedIndexChanged += this.OnChangeEscape;
            OnChangeEscape(quoteEscapes, EventArgs.Empty);
        }

        /// <summary>
        /// Load the options for encodings
        /// </summary>
        private void LoadEncodings()
        {
            fileEncoding.DisplayMember = "Display";
            fileEncoding.ValueMember = "Value";
            fileEncoding.DataSource = new ArrayList {
                new { Display = "Automatic",  Value = "" },
                new { Display = "Unicode (UTF-7)",  Value = "utf-7" },
                new { Display = "Unicode (UTF-8)",  Value = "utf-8" },
                new { Display = "Unicode (UTF-16)", Value = "utf-16" },
                new { Display = "Unicode (UTF-32)", Value = "utf-32" },
                new { Display = "Windows (ISO-8859-1)", Value = "iso-8859-1" },
                new { Display = "ASCII",            Value = "us-ascii" }};

            fileEncoding.SelectedIndex = 0;
        }

        /// <summary>
        /// Changing the selected quote option
        /// </summary>
        private void OnChangeQuotes(object sender, EventArgs e)
        {
            panelQuoteEscapes.Enabled = ((char) textQualifier.SelectedValue) != '\0';

            LoadQuoteEscapes();
        }

        /// <summary>
        /// Delimiter has changed
        /// </summary>
        private void OnChangeDelimiter(object sender, EventArgs e)
        {
            RaiseFileFormatChanged();
        }

        /// <summary>
        /// Escape has changed
        /// </summary>
        private void OnChangeEscape(object sender, EventArgs e)
        {
            RaiseFileFormatChanged();
        }

        /// <summary>
        /// The selected encoding has changed
        /// </summary>
        private void OnChangeEncoding(object sender, EventArgs e)
        {
            RaiseFileFormatChanged();
        }

        /// <summary>
        /// Raise the file format changed event
        /// </summary>
        private void RaiseFileFormatChanged()
        {
            if (FileFormatChanged != null)
            {
                FileFormatChanged(this, EventArgs.Empty);
            }
        }
    }
}
