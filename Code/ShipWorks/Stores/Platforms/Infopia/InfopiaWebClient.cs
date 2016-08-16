using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Infopia.WebServices;
using System.Web.Services.Protocols;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using log4net;
using System.Net;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Web client for the Infopia Web Service
    /// </summary>
    public class InfopiaWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(InfopiaWebClient));
						  
        #region getOrdersCellsToPopulate

        static Cell[] getOrdersCellsToPopulate = new Cell[]
            {
                // *ORDER PRODUCT LINE*
                CreateCell("*ORDER PRODUCT LINE ID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE ORDER ID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE PRODUCT CODE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE MARKETPLACE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE MARKETPLACE ID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE BUYER MARKETPLACE ID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE TITLE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE BIN LOCATION*", "", ""),
                CreateCell("*ORDER PRODUCT LINE PRICE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE BID QUANTITY*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE QUANTITY*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE SHIP PRICE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE HANDLING FEE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE TAX*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE SURCHARGE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE COUPON DISCOUNT*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE VOLUME DISCOUNT*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE TOTAL PRICE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE COST*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE GHOST SKU*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE MARKETPLACE SALE TM*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LISTING FEE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE FEATURING FEE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE SELLING FEE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE IS BIN*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE DPS SKU*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE DPS GROUP ID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LIST CSID*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LIST DURATION*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LIST USE RESERVE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LIST PRICE TYPE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE LIST END TM*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE CURRENT BID PRICE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE MAX BID PRICE*", "", ""), 
                CreateCell("*ORDER PRODUCT LINE IS WINNING BIDDER*", "", ""), 

                // *ORDER LINE*
                CreateCell("*ORDER ID*", "", ""), 
                CreateCell("*ORDER CUSTOMER ID*", "", ""), 
                CreateCell("*ORDER STATUS*", "", ""), 
                CreateCell("*ORDER MALL SOURCE*", "", ""), 
                CreateCell("*ORDER EMAIL SENT TM*", "", ""), 
                CreateCell("*ORDER TM*", "", ""), 
                CreateCell("*ORDER EMAIL*", "", ""), 
                CreateCell("*ORDER BILLING COMPANY*", "", ""), 
                CreateCell("*ORDER BILLING FIRST NAME*", "", ""), 
                CreateCell("*ORDER BILLING LAST NAME*", "", ""), 
                CreateCell("*ORDER BILLING EMAIL*", "", ""), 
                CreateCell("*ORDER BILLING PHONE*", "", ""), 
                CreateCell("*ORDER BILLING FAX*", "", ""), 
                CreateCell("*ORDER BILLING ADDRESS TYPE*", "", ""), 
                CreateCell("*ORDER BILLING STREET*", "", ""), 
                CreateCell("*ORDER BILLING STREET2*", "", ""), 
                CreateCell("*ORDER BILLING CITY*", "", ""), 
                CreateCell("*ORDER BILLING STATE-REGION*", "", ""), 
                CreateCell("*ORDER BILLING ZIP-POSTAL CODE*", "", ""), 
                CreateCell("*ORDER BILLING COUNTRY*", "", ""), 
                CreateCell("*ORDER SHIPPING COMPANY*", "", ""), 
                CreateCell("*ORDER SHIPPING FIRST NAME*", "", ""), 
                CreateCell("*ORDER SHIPPING LAST NAME*", "", ""), 
                CreateCell("*ORDER SHIPPING EMAIL*", "", ""), 
                CreateCell("*ORDER SHIPPING PHONE*", "", ""), 
                CreateCell("*ORDER SHIPPING FAX*", "", ""), 
                CreateCell("*ORDER SHIPPING ADDRESS TYPE*", "", ""), 
                CreateCell("*ORDER SHIPPING STREET*", "", ""), 
                CreateCell("*ORDER SHIPPING STREET2*", "", ""), 
                CreateCell("*ORDER SHIPPING CITY*", "", ""), 
                CreateCell("*ORDER SHIPPING STATE-REGION*", "", ""), 
                CreateCell("*ORDER SHIPPING ZIP-POSTAL CODE*", "", ""), 
                CreateCell("*ORDER SHIPPING COUNTRY*", "", ""), 
                CreateCell("*ORDER NOTES*", "", ""), 
                CreateCell("*ORDER PAY TERM*", "", ""), 
                CreateCell("*ORDER SHIPPER*", "", ""), 
                CreateCell("*ORDER SHIP METHOD*", "", ""), 
                CreateCell("*ORDER BUYER FEEDBACK TYPE*", "", ""), 
                CreateCell("*ORDER BUYER FEEDBACK*", "", ""), 
                CreateCell("*ORDER COUPON CODE*", "", ""), 
                CreateCell("*ORDER TYPE*", "", ""), 
                CreateCell("*ORDER TRACKING ID*", "", ""), 
                CreateCell("*ORDER TRACKING SOURCE*", "", ""), 
                CreateCell("*ORDER TRACKING DSC*", "", ""), 
                CreateCell("*ORDER INSURANCE FEE*", "", ""), 
                CreateCell("*ORDER STATUS LAST CHANGED TM*", "", ""), 
                CreateCell("*ORDER IS NEW CUSTOMER*", "", ""), 
                CreateCell("*ORDER ACCOUNT CREDIT*", "", ""), 
                CreateCell("*ORDER HAD PAYPAL PAYMENT*", "", ""), 
                CreateCell("*ORDER LINKED ACCOUNT*", "", ""), 
                CreateCell("*ORDER PAYMENT RECEIVED AMOUNT*", "", ""), 
                CreateCell("*ORDER PAYMENT RECEIVED TM*", "", ""), 
                CreateCell("*ORDER SUPPLIER EMAIL SENT TM*", "", ""), 
                CreateCell("*ORDER SHIP CALCULATION TYPE*", "", ""), 
                CreateCell("*ORDER USER SOURCE*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD NUM*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD NAME ON CARD*", "", ""), 
                CreateCell("*ORDER CHARGE LINE MASKED CREDIT CARD NUM*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD EXP MONTH*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD EXP YEAR*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD SECURITY CODE*", "", ""), 

                // *ORDER CHARGE LINE*
                CreateCell("*ORDER CHARGE LINE ORDER ID*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CHARGE TYPE*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CHARGE STATUS*", "", ""), 
                CreateCell("*ORDER CHARGE LINE PAYMENT GATEWAY*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD AUTH CODE*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD INVOICE NUM*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD TRANSACTION NUM*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD CHARGE TM*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD AVS MATCH*", "", ""), 
                CreateCell("*ORDER CHARGE LINE CREDIT CARD ZIP AVS MATCH*", "", ""), 

                // *ORDER TRACKING LINE*
                CreateCell("*ORDER TRACKING LINE ORDER ID*", "", ""), 
                CreateCell("*ORDER TRACKING LINE SHIPPER*", "", ""), 
                CreateCell("*ORDER TRACKING LINE TRACKING NUM*", "", ""), 

                // *ORDER NOTE LINE*
                CreateCell("*ORDER NOTE LINE ID*", "", ""), 
                CreateCell("*ORDER NOTE LINE ORDER ID*", "", ""), 
                CreateCell("*ORDER NOTE LINE TYPE*", "", ""), 
                CreateCell("*ORDER NOTE LINE NOTE*", "", ""), 
                CreateCell("*ORDER NOTE LINE TM*", "", ""), 
                CreateCell("*ORDER NOTE LINE TRANSACTION ID*", "", ""), 

                // *ORDER ATTRIBUTE LINE*
                CreateCell("*ORDER ATTRIBUTE LINE ORDER PRODUCT LINE ID*", "", ""), 
                CreateCell("*ORDER ATTRIBUTE LINE NAME*", "", ""), 
                CreateCell("*ORDER ATTRIBUTE LINE VALUE*", "", ""), 
                CreateCell("*ORDER ATTRIBUTE LINE PRODUCT CODE*", "", ""), 
                CreateCell("*ORDER ATTRIBUTE LINE PRICE*", "", "")
            };

        #endregion

        #region getProductCellsToPopulate

        static Cell[] getProductCellsToPopulate = new Cell[]
            {
                CreateCell("*PRODUCT CODE*", "", ""),
                CreateCell("*IMAGE 1 URL*", "", ""),
                CreateCell("*WEIGHT LBS*", "", ""),
                CreateCell("*WEIGHT OZ*", "", ""),
                CreateCell("*MANUFACTURER*", "", ""),
                CreateCell("*MANUFACTURER ID*", "", ""),
                CreateCell("*ISBN*", "", ""),
                CreateCell("*UPC*", "", ""),
                CreateCell("*BIN LOCATION*", "", ""),
                CreateCell("*SUPPLIER*", "", "")
            };

        #endregion

        // search paging token
        int queryId = -1;

        // Results from the last GetOrders call
        XPathDocument ordersXml = null;

        /// <summary>
        /// Gets the results from the last GetOrders call
        /// </summary>
        public IXPathNavigable OrdersXml
        {
            get { return ordersXml; }
        }

        // Infopia User Token
        string tokenOverride = "";

        // Store entity
        InfopiaStoreEntity store;

        // Maps downloaded product codes to their product info
        static Dictionary<string, InfopiaProductInfo> productInfoMap = new Dictionary<string, InfopiaProductInfo>();

        /// <summary>
        /// Determines whether the live server is currently being used
        /// </summary>
        public static bool UseLiveServer
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaWebClient(InfopiaStoreEntity store, string tokenOverride)
        {
            this.tokenOverride = tokenOverride;
            this.store = store;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaWebClient(InfopiaStoreEntity store)
            : this(store, "")
        {
        }

        /// <summary>
        /// Tests the User Token by contact the Infopia servers
        /// </summary>
        public void TestConnection()
        {
            try
            {
                using (InfopiaWebService2 service = CreateWebService("TestConnection"))
                {
                    GetAvailableImportMasterHeaderTypesRequest request = new GetAvailableImportMasterHeaderTypesRequest();
                    request.user = User;

                    MasterHeaderTypesResponseWrapper responseWrapper = service.GetAvailableImportMasterHeaderTypes(request);
                    CheckResponse(responseWrapper.response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
            }
        }

        #region Orders

        /// <summary>
        /// Uploads tracking details to Infopia
        /// </summary>
        public void UploadShipmentDetails(long orderNumber, string shipperCode, string trackingNumber, decimal shippingCharge)
        {
            try
            {
                using (InfopiaWebService2 service = CreateWebService("UploadShipmentDetails - " + orderNumber.ToString()))
                {
                    if (trackingNumber.Length == 0)
                    {
                        trackingNumber = "no tracking";
                        log.InfoFormat("No tracking number specified for order {0}, sending 'no tracking'", orderNumber);
                    }

                    // create the lines/cells needed to update
                    Line updateLine = new Line();
                    updateLine.cells = new Cell[]
                        {
                            CreateCell("*ORDER TRACKING LINE ORDER ID*", orderNumber.ToString(), ""),
                            CreateCell("*ORDER TRACKING LINE SHIPPER*", shipperCode, ""),
                            CreateCell("*ORDER TRACKING LINE TRACKING NUM*", trackingNumber, "")
                        };

                    // define request
                    AddOrUpdateLinesRequest request = new AddOrUpdateLinesRequest()
                        {
                            user = User,
                            masterHeaderTypeId = "*ORDERS*",
                            lines = new Line[] { updateLine }
                        };

                    // validate the response
                    StatusesResponseWrapper responseWrapper = service.AddOrUpdateLines(request);
                    foreach (Status status in responseWrapper.response.statuses)
                    {
                        CheckStatus(status);
                    }

                    // now we update the shipping charge

                    // create the line and cells for updating
                    updateLine = new Line();
                    updateLine.cells = new Cell[]
                        {
                            CreateCell("*ORDER ID*", orderNumber.ToString(), ""),
                            CreateCell("*ORDER ACTUAL SHIPPING CHARGE*", shippingCharge.ToString("F2"), "")
                        };

                    // create the request
                    request = new AddOrUpdateLinesRequest()
                        {
                            user = User,
                            masterHeaderTypeId = "*ORDERS*",
                            lines = new Line[] { updateLine }
                        };

                    responseWrapper = service.AddOrUpdateLines(request);
                    foreach (Status status in responseWrapper.response.statuses)
                    {
                        CheckStatus(status);
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
            }
        }

        /// <summary>
        /// Updates an order with a new status
        /// </summary>
        public void UpdateOrderStatus(long orderNumber, string status)
        {
            using (InfopiaWebService2 service = CreateWebService("UpdateOrderStatus"))
            {
                // Create the lines/cells needed to update
                Line updateLine = new Line();
                updateLine.cells = new Cell[] 
                {
                    CreateCell("*ORDER ID*", orderNumber.ToString(), ""),
                    CreateCell("*ORDER STATUS*", status, "")
                };

                // define the request
                AddOrUpdateLinesRequest request = new AddOrUpdateLinesRequest()
                {
                    user = User,
                    masterHeaderTypeId = "*ORDERS*",
                    lines = new Line[] { updateLine }
                };

                try
                {
                    StatusesResponseWrapper responseWrapper = service.AddOrUpdateLines(request);
                    foreach (Status infopiaStatus in responseWrapper.response.statuses)
                    {
                        CheckStatus(infopiaStatus);
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
                }
            }
        }

        /// <summary>
        /// Retrieve the next page of orders, and returns whether or not there are more to download
        /// </summary>
        public bool GetNextOrdersPage(DateTime? lastModified)
        {
            Search searchOptions = new Search();

            // setup paging
            if (queryId == 0)
            {
                // the last call resulted in all being returned in the page
                ordersXml = null;
                return false;
            }
            if (queryId == -1)
            {
                // first call
                searchOptions.pageInfo = new QueryType() { Item = 1, ItemElementName = ItemChoiceType.pageNumber };
            }
            else
            {
                // use the last search result's token (queryid)
                searchOptions.pageInfo = new QueryType() { Item = queryId, ItemElementName = ItemChoiceType.queryId };
            }

            searchOptions.andOrType = "and";
            searchOptions.numberMasterObjectsPerPage = InfopiaUtility.DownloadBatchSize;
            searchOptions.orderByCell = new Cell() { headerId = "*ORDER STATUS LAST CHANGED TM*" };

            // search criteria
            searchOptions.searchByCells = GetSearchCriteria(lastModified).ToArray();

            // prepare the select list
            searchOptions.cellsToPopulate = getOrdersCellsToPopulate;

            // prepare the request
            GetLinesRequest request = new GetLinesRequest() { user = User, search = searchOptions, masterHeaderTypeId = "*ORDERS*" };

            try
            {
                // create the service and execute the request
                using (InfopiaWebService2 service = CreateWebService("GetOrders"))
                {
                    // execute the request
                    LinesResponseWrapper responseWrapper = service.GetLines(request);

                    // Check for Errors
                    CheckResponse(responseWrapper.response);

                    // save the query id for the next iteration/page
                    queryId = responseWrapper.response.queryId;

                    if (responseWrapper.response.lines.Length > 0)
                    {
                        ordersXml = ConvertOrderResponseToXml(responseWrapper.response.lines);
                    }
                    else
                    {
                        ordersXml = null;
                    }

                    // return the number of results returned
                    return responseWrapper.response.lines.Length > 0;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
            }
        }

        /// <summary>
        /// Gets a list of order IDs that need to be downloaded
        /// </summary>
        public int GetOrderCount(DateTime? lastModified)
        {
            Search searchOptions = new Search();
            searchOptions.pageInfo = new QueryType() { Item = 1, ItemElementName = ItemChoiceType.pageNumber };
            searchOptions.andOrType = "and";
            searchOptions.numberMasterObjectsPerPage = 1;
            searchOptions.orderByCell = new Cell() { headerId = "*ORDER STATUS LAST CHANGED TM*" };

            // select clause
            List<Cell> cellsToPopulate = new List<Cell>()
            {
                CreateCell("*ORDER ID*", "", ""),
                CreateCell("*ORDER STATUS LAST CHANGED TM*", "", "")
            };

            // criteria
            List<Cell> searchByCells = GetSearchCriteria(lastModified);

            searchOptions.cellsToPopulate = cellsToPopulate.ToArray();
            searchOptions.searchByCells = searchByCells.ToArray();

            // Prepare the request
            GetLinesRequest request = new GetLinesRequest() { user = User, search = searchOptions, masterHeaderTypeId="*ORDERS*" };

            try
            {
                using (InfopiaWebService2 service = CreateWebService("GetCount"))
                {
                    // execute the request
                    LinesResponseWrapper responseWrapper = service.GetLines(request);

                    // Check for errors
                    CheckResponse(responseWrapper.response);

                    return responseWrapper.response.estimatedNumberMasterObjectsFound;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
            }
        }

        /// <summary>
        /// Creates infopia search criteria cells
        /// </summary>
        private static List<Cell> GetSearchCriteria(DateTime? lastModified)
        {
            List<Cell> searchByCells = new List<Cell>()
            {
                CreateCell("*ORDER STATUS LAST CHANGED TM*", InfopiaUtility.FormatDate(InfopiaUtility.ConvertToInfopiaTimeZone(lastModified.Value)), ">"),
                CreateCell("*ORDER STATUS*", "Incomplete", "!="),
                CreateCell("*ORDER STATUS*", "Pending", "!=")
            };
            return searchByCells;
        }

        /// <summary>
        /// Determine the line type for line from an order
        /// </summary>
        private string GetOrderLineType(Line line)
        {
            switch (line.cells[0].headerId)
            {
                case "*ORDER ID*": return "Order";
                case "*ORDER PRODUCT LINE ID*": return "Product";
                case "*ORDER CHARGE LINE ORDER ID*": return "Charge";
                case "*ORDER TRACKING LINE ORDER ID*": return "Tracking";
                case "*ORDER NOTE LINE ID*": return "Note";
                case "*ORDER ATTRIBUTE LINE ORDER PRODUCT LINE ID*": return "Attribute";
            }

            throw new InfopiaException("Could not determine line type for Infopia line beginning with '" + line.cells[0].headerId + "'");
        }

        /// <summary>
        /// Convert the lines from a GetOrders call to a hierarchical XmlDocument
        /// </summary>
        [NDependIgnoreLongMethod]
        private XPathDocument ConvertOrderResponseToXml(Line[] lines)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("InfopiaOrders");

                // Go through the lines to group by order.  They are returned like:
                // 
                //   Order Line
                //      Product Lines or
                //      Charge Lines or 
                //      Tracking Lines or 
                //      Attribute Lines
                //   Order Line
                //      ...
                //      ...
                List<OrderLineContainer> groupedLines = new List<OrderLineContainer>();
                OrderLineContainer currentOrder = null;
                foreach (Line line in lines)
                {
                    string lineType = GetOrderLineType(line);
                    if (lineType == "Order")
                    {
                        // Moving to the next order
                        currentOrder = new OrderLineContainer(line);

                        groupedLines.Add(currentOrder);
                    }
                    else if (lineType == "Attribute")
                    {
                        currentOrder.AttributeLines.Add(line);
                    }
                    else
                    {
                        currentOrder.ChildLines.Add(line);
                    }
                }

                // write out each order and associated data
                foreach (OrderLineContainer container in groupedLines)
                {
                    xmlWriter.WriteStartElement("InfopiaOrder");

                    // First write the order data itself
                    WriteCellsToXml(container.OrderLine, xmlWriter);

                    foreach (Line line in container.ChildLines)
                    {
                        string lineType = GetOrderLineType(line);

                        // Write the open
                        xmlWriter.WriteStartElement(lineType);

                        // Write all the values
                        WriteCellsToXml(line, xmlWriter);

                        // If its a product line, we have to write all the attributes for it before we close
                        if (lineType == "Product")
                        {
                            // Get this product id
                            string productID = GetCell(line, "*ORDER PRODUCT LINE ID*");

                            // Write all the atributes for this product
                            foreach (Line attributeLine in container.AttributeLines)
                            {
                                if (productID == GetCell(attributeLine, "*ORDER ATTRIBUTE LINE ORDER PRODUCT LINE ID*"))
                                {
                                    xmlWriter.WriteStartElement("Attribute");
                                    WriteCellsToXml(attributeLine, xmlWriter);
                                    xmlWriter.WriteEndElement();
                                }
                            }
                        }

                        // Close LineType
                        xmlWriter.WriteEndElement();
                    }

                    // close the order
                    xmlWriter.WriteEndElement();
                }

                // close InfopiaOrders
                xmlWriter.WriteEndElement();

                // close the document
                xmlWriter.WriteEndDocument();

                using (StringReader reader = new StringReader(writer.ToString()))
                {
                    // re-load into xmldoc and return it
                    XPathDocument document = new XPathDocument(reader);
                    return document;
                }
            }
        }

        #endregion 

        #region Common

        /// <summary>
        /// Checks the web service response
        /// </summary>
        private void CheckResponse(BaseStatusResponse response)
        {
            CheckStatus(response.status);
        }

        /// <summary>
        /// Checks a status code response from infopia for failure
        /// </summary>
        private void CheckStatus(Status status)
        {
            if (status.statusCode < 0)
            {
                throw new InfopiaException(status.statusCode, status.statusMsg);
            }
        }

        /// <summary>
        /// Get the user object based on the store's token
        /// </summary>
        public User User
        {
            get
            {
                User user = new User();
                if (tokenOverride.Length > 0)
                {
                    user.userToken = tokenOverride;
                }
                else
                {
                    user.userToken = store.ApiToken;
                }

                return user;
            }
        }

        /// <summary>
        /// Create an Infopia Web Service instance.
        /// </summary>
        public InfopiaWebService2 CreateWebService(string logName)
        {
            InfopiaWebService2 service = new InfopiaWebService2(new ApiLogEntry(ApiLogSource.Infopia, logName));

            service.Url = UseLiveServer ?
                "https://app.infopia.com/InfopiaWebServices/InfopiaWS/2" :
                "https://test.infopia.com/InfopiaWebServices/InfopiaWS/2";

            return service;
        }

        /// <summary>
        /// Creates an Infopia cell object.
        /// </summary>
        private static Cell CreateCell(string header, string data, string op)
        {
            Cell cell = new Cell()
            {
                headerId = header,
                value = data,
                @operator = op
            };

            return cell;
        }

        /// <summary>
        /// Finds a cell identified by the provided header in the line
        /// </summary>
        private string GetCell(Line line, string header)
        {
            try
            {
                return line.cells.First<Cell>(c => c.headerId == header).value;
            }
            catch (InvalidOperationException ex)
            {
                throw new InfopiaException(string.Format("Could not find header '{0}' in Infopia response line.", header), ex);
            }
        }

        /// <summary>
        /// Write all the cells in the given line to XML
        /// </summary>
        private void WriteCellsToXml(Line line, XmlTextWriter xmlWriter)
        {
            foreach (Cell cell in line.cells)
            {
                xmlWriter.WriteStartElement("Cell");
                xmlWriter.WriteAttributeString("Name", cell.headerId);
                xmlWriter.WriteString(cell.value);
                xmlWriter.WriteEndElement();
            }
        }


        #endregion 
    
        #region SKUS

        /// <summary>
        /// Create the dictionary key for the product map
        /// </summary>
        private string CreateProductMapKey(string code)
        {
            return string.Format("{0}_{1}", store.StoreID, code);
        }

        /// <summary>
        /// Contacts infopia to download 
        /// </summary>
        /// <param name="productCodes"></param>
        [NDependIgnoreLongMethod]
        private void EnsureProductsInternal(List<string> productCodes)
        {
            if (productCodes.Count > 5)
            {
                throw new InvalidOperationException("Infopia does not allow querying by more than 5 criteria.");
            }

            List<string> toDownload = new List<string>();

            // We need to download all those that aren't already cached
            toDownload.AddRange(productCodes.Where(code => code.Length > 0 && !productInfoMap.ContainsKey(CreateProductMapKey(code))));

            // everything is cached
            if (toDownload.Count == 0)
            {
                return;
            }

            try
            {
                using (InfopiaWebService2 service = CreateWebService("GetProductInfos"))
                {
                    Search search = new Search();

                    search.andOrType = "or";
                    search.pageInfo = new QueryType() { Item = 1, ItemElementName = ItemChoiceType.pageNumber };

                    // one order won't exceed 500...
                    search.numberMasterObjectsPerPage = 500;

                    // setup the search criteria
                    List<Cell> searchCells = new List<Cell>();
                    searchCells.AddRange(toDownload.Select<string, Cell>(code => new Cell() { headerId = "*PRODUCT CODE*", value = code, @operator = "=" }));
                    search.searchByCells = searchCells.ToArray();

                    // set the cells we need
                    search.cellsToPopulate = getProductCellsToPopulate;

                    GetLinesRequest request = new GetLinesRequest()
                    {
                        search = search,
                        user = User,
                        masterHeaderTypeId = "*SKUS*"
                    };

                    // Make the getLines call
                    LinesResponseWrapper linesResponseWrapper = service.GetLines(request);

                    // Make sure it worked
                    CheckResponse(linesResponseWrapper.response);

                    // pull out the data
                    Line[] lines = linesResponseWrapper.response.lines;

                    // to keep track of which ones were successfull
                    List<string> fetchedProducts = new List<string>();
                    foreach (Line line in lines)
                    {
                        // we downloaded this one
                        string productCode = GetCell(line, "*PRODUCT CODE*");
                        fetchedProducts.Add(productCode);

                        // convert to Xml and load into an InfopiaProductInfo object
                        string xml = ConvertProductResponseToXml(line);
                        using (StringReader reader = new StringReader(xml))
                        {
                            // Create XPath document
                            XPathDocument xmlDocument = new XPathDocument(reader);

                            // Create navigator
                            XPathNavigator xpath = xmlDocument.CreateNavigator();
                            xpath.MoveToFirstChild();

                            productInfoMap[CreateProductMapKey(productCode)] = new InfopiaProductInfo(xpath);
                        }
                    }

                    // go through all those that weren't downloaded, and add blank productinfo entries to the cache
                    foreach (string missingCode in toDownload.Where<string>(s => !fetchedProducts.Contains(s)))
                    {
                        // Add a blank one
                        productInfoMap[CreateProductMapKey(missingCode)] = new InfopiaProductInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(InfopiaException));
            }
        }

        /// <summary>
        /// Makes a single call to Infopia to download and cache all those products that aren't already cached
        /// </summary>
        public void EnsureProducts(List<string> productCodes)
        {
            int batchSize = 5;
            int batches = (int)(productCodes.Count / batchSize) + ((productCodes.Count % batchSize) > 0 ? 1 : 0);

            for (int x = 0; x < batches; x++)
            {
                EnsureProductsInternal(productCodes.Skip(x * batchSize).Take(batchSize).ToList());
            }
        }

        /// <summary>
        /// Get the product info for the given product code.
        /// </summary>
        public InfopiaProductInfo GetProductInfo(string productCode)
        {
            string key = store.StoreID + "_" + productCode;

            // If it doesnt exist, we have to create it
            if (productInfoMap.ContainsKey(key))
            {
                return (InfopiaProductInfo)productInfoMap[key];
            }
            else
            {
                return new InfopiaProductInfo();
            }
        }

        /// <summary>
        /// Convert the lines from a GetOrders call to a hierarchical XmlDocument
        /// </summary>
        private string ConvertProductResponseToXml(Line productLine)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("InfopiaSku");

                // First line is the order line
                WriteCellsToXml(productLine, xmlWriter);

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();

                return writer.ToString();
            }
        }

        #endregion
    }
}