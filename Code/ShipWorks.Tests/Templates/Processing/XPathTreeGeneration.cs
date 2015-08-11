using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Templates.Processing;
using ShipWorks.Data;
using ShipWorks.Templates;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using System.Transactions;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using System.Diagnostics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Templates.Processing
{
    public class XPathTreeGeneration
    {
        [TestInitialize]
        public void Intitialize()
        {
            DatabaseManager.Initialize(@"BRIANPC\Development", "ShipWorksLocal");

            DatabaseManager.StartTiming(false);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            DatabaseManager.Cleanup();
        }

        [Fact]
        [Ignore]
        public void SelectTrackingNumber()
        {
            TemplateResult result = ProcessTokens("<xsl:for-each select='//Shipment//@* | //Shipment//*'>[<xsl:value-of select='.' />] </xsl:for-each>", 107006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }


        [Fact]
        [Ignore]
        public void SelectGenerated()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Generated}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectTemplateContentHeight()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Template/Output/ContentHeight}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectUsername()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/User/Username}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectStoreName()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Store/StoreName}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectStoreLastDownloaded()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Store/LastDownload}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectStoreCity()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Store/Address[@type != 'Whatever']/City}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectCustomerCity()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Customer[1]/Address[@type = 'bill']/City}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderNumber()
        {
            TemplateResult result = ProcessTokens("{//Order/Number}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectSumOrderCharges()
        {
            TemplateResult result = ProcessTokens("{sum(//Charge/Amount)}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectUsernameTest()
        {
            TemplateResult result = ProcessTokens("{//Note[@ID = 7044]}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }


        [Fact]
        [Ignore]
        public void SelectOrderDate()
        {
            TemplateResult result = ProcessTokens("{//Order/Date}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectMivaAdd1()
        {
            TemplateResult result = ProcessTokens("{//Order/Miva/Sebenza/Add1}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderItemCode2()
        {
            TemplateResult result = ProcessTokens("{//Order/Item[2]/Code}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderItemCodes()
        {
            TemplateResult result = ProcessTokens("<xsl:for-each select='/ShipWorks/Customer/Order/Item'>[<xsl:value-of select='.' />] </xsl:for-each>", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderStoreName()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Store[@ID = 7005]/../Customer/Order/Number}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }


        [Fact]
        [Ignore]
        public void SelectOrderItemName()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/Name}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderEmail()
        {
            TemplateResult result = ProcessTokens("{//Order/Address[@type='bill']/Email}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectCustomerNotes()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Customer/Note[User/Username = 'Brian']/Text}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectCustomer2xNotes()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Customer/Notes}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectReference()
        {
            TemplateResult result = ProcessTokens("{//Reference}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderItemAttributeDescription()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/Option/Description}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderChargeDescription()
        {
            TemplateResult result = ProcessTokens("{//Order/Charge/Description}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderChargeAmounts()
        {
            TemplateResult result = ProcessTokens("<xsl:for-each select='/ShipWorks/Customer/Order/Charge'>[<xsl:value-of select='Amount' />] </xsl:for-each>", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderPaymentCCExpiration()
        {
            TemplateResult result = ProcessTokens("{//Order/Payment/Detail[2]/Value}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderNotesCount()
        {
            TemplateResult result = ProcessTokens("{count(/ShipWorks/Customer/Order/Note)}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectOrderNoteText()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Customer/Order/Note/Text}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderItemTotal()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/Total}", 56022006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderItemPayPalTransaction()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/eBay/PayPal/TransactionID}", 56022006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderItemFeedbackBuyerLeft()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/eBay/FeedbackBuyerLeft}", 56022006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderItemSellingRecord()
        {
            TemplateResult result = ProcessTokens("{//Order/Item/eBay/SellingManager/RecordNumber}", 56022006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderBuyer()
        {
            TemplateResult result = ProcessTokens("{//Order/eBay/BuyerID}", 56022006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderFeedbackBuyerLeft()
        {
            TemplateResult result = ProcessTokens("{//Order/eBay/FeedbackBuyerLeft}", 56006006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectEbayOrderPayPalTransaction()
        {
            TemplateResult result = ProcessTokens("{//Order/eBay/SellingManager/RecordNumber}", 56006006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectShipmentTrackingNumber()
        {
            TemplateResult result = ProcessTokens("{//Order/Shipment/TrackingNumber}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectShipmentCustomsItem()
        {
            TemplateResult result = ProcessTokens("{//Order/Shipment/CustomsItem/Weight}", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectShipmentStandardLabels()
        {
            TemplateResult result = ProcessTokens("<xsl:for-each select=\"/ShipWorks/Customer/Order/Shipment[Status = 'Processed']/Labels[@type='image']/Package/node()[name() = 'Primary' or name() = 'Supplemental']/Label[@orientation = 'wide']\">[<xsl:value-of select='.' />] </xsl:for-each>", 29006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        [Fact]
        [Ignore]
        public void SelectStoreSpecifics()
        {
            TemplateResult result = ProcessTokens("{/ShipWorks/Customer/Order/MarketplaceAdvisor/.}", 41006);

            Debug.WriteLine(result.XPathSource.OuterXml);
            Debug.WriteLine("RESULT: " + result.ReadResult());
        }

        // Customer/Order/Shipment[Status = 'Processed']/Labels[@type='image']/Package/node()[name() = 'Primary' or name() = 'Supplemental']/Label[@orientation = 'wide']
        // Customer/Order/Shipment[Status = 'Processed']/Labels[@type='thermal']/Package/node()[name() = 'Primary' or name() = 'Supplemental']/Label

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

            TemplateTranslationContext context = new TemplateTranslationContext(new TemplateEntity { Name = "TestName", ParentFolder = new TemplateFolderEntity { Name = "TestFolder" }, PageHeight = 11, PageWidth = 8.5 }, 
                input, 
                new ProgressItem(""));

            TemplateXPathNavigator xpath = new TemplateXPathNavigator(context);
            TemplateResult result = templateXsl.Transform(xpath);

            return result;
        }
    }
}
