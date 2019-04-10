using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace OrderCreation
{
    
    [TestModule("D1430E56-BCDD-49BC-BE78-BF03251CD60C", ModuleType.UserCode, 1)]
    public class CreateShopifyOrder : ITestModule
    {      	
    	
    	int _OrderLimit = 1;
    	[TestVariable("6fa5363b-3ddd-4772-9143-070f5370a637")]
    	public int OrderLimit
    	{
    		get { return _OrderLimit; }
    		set { _OrderLimit = value; }
    	}
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            int NumberOfShopifyOrders = 1;
            
            //	INITIALIZE NEW REPOSITORY INSTANCE
            OrderCreationRepository repo = new OrderCreationRepository();
            
            //	LAUNCH FIREFOX AND OPEN SHOPIFY WEBSITE
            Report.Log(ReportLevel.Info, "Website", "Opening web site 'https://joanie-loves-tchotchke.myshopify.com/admin/orders' with browser 'firefox' in normal mode.", new RecordItemIndex(0));
            Host.Current.OpenBrowser("https://joanie-loves-tchotchke.myshopify.com/admin/orders", "firefox", "", false, false, false, false, false);
            Delay.Milliseconds(0);
            
            //	CREATE ORDERS UNTIL THE LIMIT IS MET
            while(NumberOfShopifyOrders <= _OrderLimit)
        	{	
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesOrdersShopi.CreateOrder' at Center.", repo.JoanieLovesTchotchkesOrdersShopi.CreateOrderInfo, new RecordItemIndex(1));
	            repo.JoanieLovesTchotchkesOrdersShopi.CreateOrder.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.BrowseProducts' at Center.", repo.JoanieLovesTchotchkesCreateOrder.BrowseProductsInfo, new RecordItemIndex(2));
	            repo.JoanieLovesTchotchkesCreateOrder.BrowseProducts.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.DivTagAllProducts' at Center.", repo.JoanieLovesTchotchkesCreateOrder.DivTagAllProductsInfo, new RecordItemIndex(3));
	            repo.JoanieLovesTchotchkesCreateOrder.DivTagAllProducts.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.UiStackUiStackAlignmentCenterUiS' at Center.", repo.JoanieLovesTchotchkesCreateOrder.UiStackUiStackAlignmentCenterUiSInfo, new RecordItemIndex(4));
	            repo.JoanieLovesTchotchkesCreateOrder.UiStackUiStackAlignmentCenterUiS.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.AddProducts' at Center.", repo.JoanieLovesTchotchkesCreateOrder.AddProductsInfo, new RecordItemIndex(5));
	            repo.JoanieLovesTchotchkesCreateOrder.AddProducts.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Wait", "Waiting 30s to exist. Associated repository item: 'JoanieLovesTchotchkesCreateOrder.AspectRatioAspectRatioSquareAspect'", repo.JoanieLovesTchotchkesCreateOrder.AspectRatioAspectRatioSquareAspectInfo, new ActionTimeout(30000), new RecordItemIndex(6));
	            repo.JoanieLovesTchotchkesCreateOrder.AspectRatioAspectRatioSquareAspectInfo.WaitForExists(30000);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalBtn' at Center.", repo.JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalBtnInfo, new RecordItemIndex(7));
	            repo.JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalBtn.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalContentHeading' at Center.", repo.JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalContentHeadingInfo, new RecordItemIndex(8));
	            repo.JoanieLovesTchotchkesCreateOrder.MarkAsPaidModalContentHeading.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.PaymentGatewayId' at Center.", repo.JoanieLovesTchotchkesCreateOrder.PaymentGatewayIdInfo, new RecordItemIndex(9));
	            repo.JoanieLovesTchotchkesCreateOrder.PaymentGatewayId.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Firefox.CashOnDeliveryCOD' at Center.", repo.Firefox.CashOnDeliveryCODInfo, new RecordItemIndex(10));
	            repo.Firefox.CashOnDeliveryCOD.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.CreateOrder' at Center.", repo.JoanieLovesTchotchkesCreateOrder.CreateOrderInfo, new RecordItemIndex(11));
	            repo.JoanieLovesTchotchkesCreateOrder.CreateOrder.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.WebElementUse' at Center.", repo.JoanieLovesTchotchkesCreateOrder.WebElementUseInfo, new RecordItemIndex(12));
	            repo.JoanieLovesTchotchkesCreateOrder.WebElementUse.Click();
	            Delay.Milliseconds(200);
	            
	            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesCreateOrder.Orders' at Center.", repo.JoanieLovesTchotchkesCreateOrder.OrdersInfo, new RecordItemIndex(1));
            	repo.JoanieLovesTchotchkesCreateOrder.Orders.Click();
            	Delay.Milliseconds(200);
            	
            	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'JoanieLovesTchotchkesOrdersShopi.CreateOrder' at Center.", repo.JoanieLovesTchotchkesOrdersShopi.CreateOrderInfo, new RecordItemIndex(1));
            	repo.JoanieLovesTchotchkesOrdersShopi.CreateOrder.MoveTo();
	            Delay.Milliseconds(200);
	            
	            NumberOfShopifyOrders++;
            }
            
            //	CLOSE BROWSER AFTER ORDER CREATION 
            Report.Log(ReportLevel.Info, "Application", "Closing application containing item 'JoanieLovesTchotchkesOrdersShopi.PageTabList'.", repo.JoanieLovesTchotchkesOrdersShopi.PageTabListInfo, new RecordItemIndex(1));
            Host.Current.CloseApplication(repo.JoanieLovesTchotchkesOrdersShopi.CreateOrder, 30000);
            Delay.Milliseconds(0);
    	}
	}
}
