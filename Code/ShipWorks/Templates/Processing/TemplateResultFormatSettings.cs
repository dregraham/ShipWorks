using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Holds the settings used to format a set of template results
    /// </summary>
    public class TemplateResultFormatSettings
    {
        // The next result index to be used for formatting
        int nextResultIndex = 0;

        // The type of template the results are from
        TemplateType templateType;

        // How the template is configured to be output
        TemplateOutputFormat outputFormat;

        // The label sheet, position, and calibration, if a label template
        LabelSheetEntity labelSheet;
        LabelPosition labelPosition;

        /// <summary>
        /// Standard constructor
        /// </summary>
        private TemplateResultFormatSettings()
        {

        }

        /// <summary>
        /// Creates the default settings based on the given template
        /// </summary>
        public static TemplateResultFormatSettings FromTemplate(TemplateEntity template)
        {
            TemplateResultFormatSettings settings = new TemplateResultFormatSettings();
            settings.templateType = (TemplateType) template.Type;
            settings.outputFormat = (TemplateOutputFormat) template.OutputFormat;

            if (settings.templateType == TemplateType.Label)
            {
                settings.labelSheet = LabelSheetManager.GetLabelSheet(template.LabelSheetID);
                settings.labelPosition = new LabelPosition(1, 1);
            }

            return settings;
        }

        /// <summary>
        /// Creates the default settings based on the print result
        /// </summary>
        public static TemplateResultFormatSettings FromPrintResult(PrintResultEntity printResult)
        {
            TemplateResultFormatSettings settings = new TemplateResultFormatSettings();
            settings.templateType = (TemplateType) printResult.TemplateType;
            settings.outputFormat = (TemplateOutputFormat) printResult.OutputFormat;

            if (settings.templateType == TemplateType.Label)
            {
                settings.labelSheet = LabelSheetManager.GetLabelSheet(printResult.LabelSheetID.Value);
                settings.labelPosition = new LabelPosition(1, 1);
            }

            return settings;
        }

        /// <summary>
        /// The type of template the results come from
        /// </summary>
        public TemplateType TemplateType
        {
            get { return templateType; }
            set { templateType = value; }
        }

        /// <summary>
        /// The configured output format of the template
        /// </summary>
        public TemplateOutputFormat OutputFormat
        {
            get { return outputFormat; }
            set { outputFormat = value; }
        }

        /// <summary>
        /// The configure label sheet of the template.  Only applies when TemplateType is Label.
        /// </summary>
        public LabelSheetEntity LabelSheet
        {
            get { return labelSheet; }
            set { labelSheet = value; }
        }

        /// <summary>
        /// The location that the first label on the first sheet will be output. Only applies when TemplateType is Label.  Once this is used,
        /// it is reset to (1, 1) by the output formatter.
        /// </summary>
        public LabelPosition LabelPosition
        {
            get { return labelPosition; }
            set { labelPosition = value; }
        }

        /// <summary>
        /// The next result index that will be used when generating output from the results.  This will be updated
        /// by the formatter automatically as it goes.
        /// </summary>
        public int NextResultIndex
        {
            get { return nextResultIndex; }
            set { nextResultIndex = value; }
        }
    }
}
