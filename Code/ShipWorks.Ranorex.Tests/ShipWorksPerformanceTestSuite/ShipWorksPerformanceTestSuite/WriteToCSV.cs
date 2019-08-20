using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace ShipWorksPerformanceTestSuite
{
	[TestModule("59FD8557-4C46-4E5C-BEEB-2241D5A4CB2E", ModuleType.UserCode, 1)]
	public class WriteToCSV : ITestModule
	{
		string currentDir,currentFile;
		
		public WriteToCSV()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			currentDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),@"..\..\"));			
			currentDir = currentDir + @"PerformanceGraphData\";		
			
			WriteHighVolumeBatchGrid();
			WriteECommerceIBatchGrid();
			WriteECommerceIIBatchGrid();
			WriteHighVolumeOLM();			
		}
		
		void WriteHighVolumeBatchGrid()
		{
			currentFile = currentDir + "HighVolumeBatchGrid.csv";
			
			using (StreamWriter sw = new StreamWriter(currentFile,false))
			{
				sw.WriteLine("LoadOrders,ApplyProfile,ProcessShipments,VoidShipments");
				sw.WriteLine(Timing.totalLoad500Time+","+Timing.totalApplyProfile500Time+","+Timing.totalProcessTime+","+Timing.totalVoid500Time);
			}
		}
		
		void WriteECommerceIBatchGrid()
		{
			currentFile = currentDir + "ECommerceIBatchGrid.csv";
			
			using (StreamWriter sw = new StreamWriter(currentFile,false))
			{
				sw.WriteLine("LoadOrders,ApplyProfile,ProcessShipments,VoidShipments");
				sw.WriteLine(Timing.totalLoad100SRTime+","+Timing.totalApplyProfile100SRTime+","+Timing.totalProcess100SRTime+","+Timing.totalVoid100SRTime);
			}
		}
		
		void WriteECommerceIIBatchGrid()
		{				
			currentFile = currentDir + "ECommerceIIBatchGrid.csv";
			
			using (StreamWriter sw = new StreamWriter(currentFile,false))
			{
				sw.WriteLine("LoadOrders,ApplyProfile,ProcessShipments,VoidShipments");
				sw.WriteLine(Timing.totalLoad100Time+","+Timing.totalApplyProfile100Time+","+Timing.totalProcess100Time+","+Timing.totalVoid100Time);
			}
		}
		
		void WriteHighVolumeOLM()
		{
			currentFile = currentDir + "HighVolumeOLM.csv";
			
			using (StreamWriter sw = new StreamWriter(currentFile,false))
			{
				sw.WriteLine("SingleScanTime(ms)");
				sw.WriteLine(Timing.singleScanTime);
			}			
		}
	}
}
