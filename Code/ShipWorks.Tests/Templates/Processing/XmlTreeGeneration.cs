using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Templates.Processing;
using ShipWorks.Data;
using ShipWorks.Templates;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using System.Transactions;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using System.Diagnostics;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Templates.Processing
{
    public class XmlTreeGeneration : IDisposable
    {
        public XmlTreeGeneration()
        {
            DatabaseManager.Initialize(@"BRIANPC\Development", "ShipWorksLocal");

            // Do one simple one to get shit jitted
            SelectTrackingNumber();

            DatabaseManager.StartTiming(false);
        }
        
        public void Dispose()
        {
            DatabaseManager.Cleanup();
        }

        
        public void SelectGenerated()
        {
            ProcessTokens("{/ShipWorks/Generated}", 29006);
        }

        
        public void SelectTemplateContentHeight()
        {
            ProcessTokens("{/ShipWorks/Template/Output/ContentHeight}", 29006);
        }

        
        public void SelectUsername()
        {
            ProcessTokens("{/ShipWorks/User/Username}", 29006);
        }

        
        public void SelectStoreName()
        {
            ProcessTokens("{/ShipWorks/Store/StoreName}", 29006);
        }

        
        public void SelectStoreLastDownloaded()
        {
            ProcessTokens("{/ShipWorks/Store/LastDownload}, {/ShipWorks/Store/LastDownload}", 29006);
        }

        
        public void SelectStoreCity()
        {
            ProcessTokens("{/ShipWorks/Store/Address[@type != 'Whatever']/City}", 29006);
        }

        
        public void SelectCustomerCity()
        {
            ProcessTokens("{/ShipWorks/Customer[1]/Address[@type = 'bill']/City}", 29006);
        }

        
        public void SelectCustomerNotes()
        {
            ProcessTokens("<xsl:for-each select='/ShipWorks/Customer/Note/*'><xsl:value-of select='.' /></xsl:for-each>", 29006);
        }

        
        public void SelectCustomerNotesLegacy()
        {
            ProcessTokens("{/ShipWorks/Customer/Notes}", 29006);
        }

        
        public void SelectUsernames()
        {
            ProcessTokens("<xsl:for-each select='//Username'><xsl:value-of select='.' />, </xsl:for-each>", 29006);
        }

        
        public void SelectUsernameTest()
        {
            ProcessTokens("{(//Username)[3]/../../Text}", 29006);
        }

        
        public void SelectSumAllAmounts()
        {
            ProcessTokens("{sum(//Charge/@ID)}, {sum((//Amount)[position() &lt;= 5])}", 29006);
        }

        
        public void SelectOrderNotesLegacy()
        {
            ProcessTokens("{//Order/Notes}", 29006);
        }

        
        public void SelectOrderPaymentAll()
        {
            ProcessTokens("{//Order/Payment/.}", 29006);
        }

        
        public void SelectOrderChildren()
        {
            ProcessTokens("<xsl:for-each select='/ShipWorks/Customer/Order/*'><xsl:value-of select='.' /></xsl:for-each>", 29006);
        }

        
        public void SelectOrderNumber()
        {
            ProcessTokens("{//Number}", 29006);
        }

        
        public void SelectOrderDate()
        {
            ProcessTokens("{//Order/Date}", 29006);
        }

        
        public void SelectReference()
        {
            ProcessTokens("{//Reference}", 29006);
        }

        
        public void SelectSumOrderCharges()
        {
            ProcessTokens("{sum(//Charge/Amount)}", 29006);
        }

        
        public void SelectSumUnitPrice()
        {
            ProcessTokens("{sum(//UnitPrice)}", 29006);
        }

        
        public void SelectOrderItemCode2()
        {
            ProcessTokens("{sum((//Order/Item | //Order/Charge)/@ID)}", 29006);
        }

        
        public void SelectOrderItemCodes()
        {
            ProcessTokens("<xsl:for-each select='/ShipWorks/Customer/Order/Item'>[<xsl:value-of select='.' />] </xsl:for-each>", 29006);
        }

        
        public void SelectShipments()
        {
            // Other                       (21006)
            // Endicia domestic - Standard (35006)
            // Endicia internat - Standard (26006)
            // Postal domestic             (33006)
            // Postal internat             (41006)
            // UPS internat - Standard    (106006)
            // UPS domestic - Standard     (88006)
            // UPS multi - Standard        (93006)
            // FedEx GRD DOM COD - Stand   (23006)
            // FedEx EXP DOM COD - Stand   (22006)
            // FedEx Home Deliv  - Stand   (24006)
            // FedEx Intl       - Stand   (107006)
            ProcessTokens("<xsl:for-each select='//Shipment//@* | //Shipment//*'>[<xsl:value-of select='.' />] </xsl:for-each>", 107006);
        }

        
        public void LabelTesting()
        {
            ProcessTokens("{count(//Label)}", 107006);
        }

        
        public void SelectTrackingNumber()
        {
            ProcessTokens("{/ShipWorks/Customer/Order/Shipment/TrackingNumber}", 26006);
        }

        
        public void SelectStoreSpecifics()
        {
            ProcessTokens("{count(//RecordNumber)}", 96006);
        }

        
        public void SelectShipmentSpecifics()
        {
            ProcessTokens("{//Service} {//Carrier}", 26006);
        }

        /// <summary>
        /// Process the tokens against the given IDs
        /// </summary>
        private TemplateResult ProcessTokens(string tokenText, params long[] ids)
        {
            List<long> idList = ids.ToList();

            TemplateXsl templateXsl = TemplateXslProvider.FromToken(tokenText);

            TemplateInput input = new TemplateInput(idList, idList, idList.Count > 0 ?
                TemplateContextTranslator.ResolveContextFromEntityType(EntityUtility.GetEntityType(idList[0])) :
                TemplateInputContext.Customer);

            TemplateTranslationContext context = new TemplateTranslationContext(
                new TemplateEntity { Name = "TestName", ParentFolder = new TemplateFolderEntity { Name = "TestFolder" }, PageHeight = 11, PageWidth = 8.5 }, 
                input, 
                new ProgressItem(""));

            TemplateXPathNavigator xpath = new TemplateXPathNavigator(context);
            TemplateResult result = templateXsl.Transform(xpath);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());

            return result;
        }
    }
}
