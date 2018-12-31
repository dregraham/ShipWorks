using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Syncfusion.XlsIO;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Class for writing product excel spreadsheets.
    /// </summary>
    [Component]
    public class ProductExcelWriter : IProductExcelWriter
    {
        /// <summary>
        /// Write the data to the spreadsheet
        /// </summary>
        public GenericResult<string> WriteDataToFile(DataTable data, string filename)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //Import DataTable to the worksheet.
                    worksheet.ImportDataTable(data, true, 1, 1);

                    workbook.SaveAs(filename, ",");
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<string>(ex);
            }

            return GenericResult.FromSuccess("Success");
        }
    }
}
