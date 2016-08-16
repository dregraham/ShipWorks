using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using System.IO;
using Interapptive.Shared.UI;
using Interapptive.Shared.IO.Text.Csv;
using Divelements.SandGrid;
using ShipWorks.Properties;
using System.Collections;
using Interapptive.Shared;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// UserControl allow the user to read and load a sample CSV document for headings
    /// </summary>
    public partial class GenericCsvSourceSchemaControl : UserControl
    {
        List<GenericSpreadsheetSourceColumn> sourceColumns = new List<GenericSpreadsheetSourceColumn>();

        bool suspendLoadSample = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvSourceSchemaControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The sample file used to load the schema
        /// </summary>
        public string SampleFilename
        {
            get { return sampleFileName.Text; }
        }

        /// <summary>
        /// Gets the currently configured schema, or null if in error or not configured
        /// </summary>
        public GenericCsvSourceSchema CurrentSchema
        {
            get
            {
                if (sourceColumns.Count == 0)
                {
                    return null;
                }

                return new GenericCsvSourceSchema(
                    sourceColumns,
                    fileFormatControl.Delimiter,
                    fileFormatControl.Quotes,
                    fileFormatControl.QuoteEscape,
                    fileFormatControl.Encoding);
            }
        }

        /// <summary>
        /// Browse for the sample CSV file
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "CSV \\ Text Files (*.csv;*.txt)|*.csv;*.txt|All Files (*.*)|*.*";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    var attempts = 
                        new char[] { ',', '\t', ';' }.SelectMany(delimeter =>
                            new char[] { '"', '\'', '\0' }.SelectMany(quote =>
                                new char[] { quote, '\\' }.Select(escape =>
                                    new { Delimeter = delimeter, Quote = quote, Escape = escape}  )));

                    try
                    {
                        var results = attempts.Select(attempt =>
                            {
                                Exception error = null;
                                List<GenericSpreadsheetSourceColumn> columns = new List<GenericSpreadsheetSourceColumn>();

                                try
                                {
                                    columns = LoadColumnData(dlg.FileName, attempt.Delimeter, attempt.Quote, attempt.Escape, fileFormatControl.Encoding, 20);
                                }
                                catch (GenericSpreadsheetException ex)
                                {
                                    error = ex;

                                    // If it's an IOEXception, its a file problem, and we just stop
                                    if (ex.InnerException is IOException)
                                    {
                                        throw;
                                    }
                                    else
                                    {
                                        error = ex;
                                    }
                                }

                                return new { Attempt = attempt, Error = error, Columns = columns.Count, CleanFields = DetermineCleanFields(columns) };

                            });

                        var bestResult = results.Where(result => result.Columns > 0).OrderByDescending(result => result.Columns).ThenByDescending(result => result.CleanFields).FirstOrDefault();

                        // If there is a best result, load it's options into the UI
                        if (bestResult != null)
                        {
                            suspendLoadSample = true;

                            fileFormatControl.Delimiter = bestResult.Attempt.Delimeter;
                            fileFormatControl.Quotes = bestResult.Attempt.Quote;
                            fileFormatControl.QuoteEscape = bestResult.Attempt.Escape;

                            suspendLoadSample = false;
                        }

                        LoadCsvSample(dlg.FileName);
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// One of the text file format options has changed
        /// </summary>
        private void OnChangeFileFormatOption(object sender, EventArgs e)
        {
            if (sampleFileName.Text.Length > 0)
            {
                LoadCsvSample(sampleFileName.Text);
            }
        }

        /// <summary>
        /// Load the CSV sample from the given file
        /// </summary>
        private void LoadCsvSample(string filename)
        {
            if (suspendLoadSample)
            {
                return;
            }

            panelOptions.Visible = true;

            sourceColumns.Clear();
            sampleFileName.Text = filename;

            try
            {
                sourceColumns = LoadColumnData(filename, fileFormatControl.Delimiter, fileFormatControl.Quotes, fileFormatControl.QuoteEscape, fileFormatControl.Encoding, 3);
            }
            catch (GenericSpreadsheetException ex)
            {
                MessageHelper.ShowError(this, "There was an error reading the sample file:\n\n" + ex.Message);

                sourceColumns.Clear();
            }

            // Load the grid
            columnGrid.LoadColumns(sourceColumns);
        }

        /// <summary>
        /// Load the column data from the given file with the specified options
        /// </summary>
        [NDependIgnoreTooManyParams]
        private static List<GenericSpreadsheetSourceColumn> LoadColumnData(string filename, char delimiter, char quote, char escape, string encodingName, int maxSamples)
        {
            try
            {
                List<GenericSpreadsheetSourceColumn> columns = new List<GenericSpreadsheetSourceColumn>();

                // Get the encoding to use
                Encoding encoding = GenericCsvUtility.GetEncoding(encodingName);

                using (StreamReader streamReader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding ?? Encoding.UTF8, encoding == null))
                {
                    using (CsvReader reader = new CsvReader(streamReader, true, delimiter, quote, escape, '#', true))
                    {
                        try
                        {
                            // Create a row for each header
                            foreach (string header in reader.GetFieldHeaders())
                            {
                                columns.Add(new GenericSpreadsheetSourceColumn(header));
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            throw new MalformedCsvException("The file does not contain header columns, or contains header columns with duplicate names.", ex);
                        }

                        // Add in some sample data
                        int counter = 0;
                        while (reader.ReadNextRecord())
                        {
                            foreach (var sourceColumn in columns)
                            {
                                string sampleValue = reader[sourceColumn.Name];

                                if (!string.IsNullOrWhiteSpace(sampleValue))
                                {
                                    sourceColumn.Samples.Add(sampleValue);
                                }
                            }

                            if (++counter == maxSamples)
                            {
                                break;
                            }
                        }
                    }
                }

                return columns;
            }
            catch (Exception ex)
            {
                if (!(ex is IOException ||
                      ex is MalformedCsvException ||
                      ex is MissingFieldCsvException))
                {
                    throw;
                }

                throw new GenericSpreadsheetException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Determine the total number of 'Clean' feilds - that is fields that don't start or end with a potential field quote character.
        /// </summary>
        private static int DetermineCleanFields(List<GenericSpreadsheetSourceColumn> columns)
        {
            int clean = 0;

            char[] quotes = { '"', '\'' };

            foreach (var column in columns)
            {
                foreach (var sample in column.Samples)
                {
                    bool isDirty = false;

                    foreach (char quote in quotes)
                    {
                        if (sample.StartsWith(quote.ToString()) && sample.EndsWith(quote.ToString()))
                        {
                            isDirty = true;
                        }
                    }

                    if (!isDirty)
                    {
                        clean++;
                    }
                }
            }

            return clean;
        }
    }
}
