/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/24/2017
 * Time: 1:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.XlsIO;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;


namespace PerformanceTesting
{
	/// <summary>
	/// Description of DurationResults.
	/// </summary>
	public static class DurationResults
	{

		static List<List<TimeSpan>> Results = new List<List<TimeSpan>>();
		static ExcelEngine excelEngine = new ExcelEngine();
		static int runNumber = 0;
		static DateTime StartTime = DateTime.Now;
		static IWorkbook workbook;
		static IWorksheet worksheet;
		static string Version;
		
		
		static DurationResults()
		{
				excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;
				workbook = excelEngine.Excel.Workbooks.Create(1);
				worksheet = workbook.Worksheets[0];
		}
		
		public static void GetVersion(string SendVersion)
		{
			Version = SendVersion;
		}
		
		public static void StartRun()
		{
			Results.Add(new List<TimeSpan>());
		}
		
		public static void AddDuration(TimeSpan duration)
		{
			Results.Last().Add(duration);
		}
		
		public static void WriteData()
		{
		
		            
                List<TimeSpan> run = Results[runNumber]; 
                worksheet.Range[1, runNumber + 1].Value = "Run Number " + (runNumber + 1).ToString();
                for (int durationNumber = 0; durationNumber < run.Count; durationNumber++) 
                { 
                    TimeSpan duration = run[durationNumber]; 
                    worksheet.Range[durationNumber + 2, runNumber + 1].Value2 = (duration.TotalSeconds).ToString("N1");
                } 
                runNumber++;
            
            
            

                workbook.SaveAs("\\\\intfs01\\Development\\Testing\\Automated Performance Results\\"+ "ShipWorks_" + Version + "_Computer_" + Environment.MachineName + "_Time_" + StartTime.ToString("yyyy_MM_dd_HH_mm_ss") + "_Performance_Results.xls");

//" + DateTime.Now.ToString() + " 
					
			
			
			
			
		}
		public static void ExtractFiles()
		{
			string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			//using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read("C:\\Generic Performance Files.zip"))
			using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read("Generic Performance Files.zip"))
			{
					zip1.ExtractAll(userPath + "\\Desktop\\",
					Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
			} 
			
			
		}
		
	}
}
