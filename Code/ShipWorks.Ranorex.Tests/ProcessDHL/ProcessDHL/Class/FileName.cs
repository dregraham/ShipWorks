/*
 * Created by Ranorex
 * User: SMadke
 * Date: 4/2/2018
 * Time: 3:47 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.IO;

namespace ProcessDHL
{
	/// <summary>
	/// This class is a static class. The class variable pdfname is used to 
	/// store the PDF file name. When a PDF file is being saved, the name of 
	/// the file is retrieved from this variable.
	/// </summary>
	public static class FileName
	{
		public static string pdfFolderPath = "C:\\Label_PDFs\\";
		public static string pdfname;
		public static string currentUser = Environment.UserName;        	
		//public static string pdfFolder = "PDF_Labels_" + System.DateTime.Now.ToString("MMdd_HHmmss");
		public static string pdfFolder = "PDFLabels" + System.DateTime.Now.ToString("MMddHHmmss");
	}
	
	public class PDFName
	{		
    	public void ChangeName()
    	{
    		string BullZipConfigPath = @"C:\Users\" + FileName.currentUser + @"\AppData\Roaming\PDF Writer\Bullzip PDF Printer\Option Sets\auto_print.ini";        
    		string[] PDFPrinterConfig = File.ReadAllLines(BullZipConfigPath);
	    	
    		PDFPrinterConfig[1] = @"  output=C:\printpdflabels\" + FileName.pdfFolder + "\\" + FileName.pdfname + "_" + System.DateTime.Now.ToString("MMddyyyyHHmmssffffff") + ".pdf";
        	File.WriteAllLines(BullZipConfigPath,PDFPrinterConfig);    		
    	}
	}
}
