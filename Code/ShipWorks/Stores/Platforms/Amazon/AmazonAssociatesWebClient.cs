using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using System.Net;
using System.Web.Services.Protocols;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using Microsoft.Web.Services3;
using System.Security.Cryptography;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Amazon Web Services client.
    /// </summary>
    public static class AmazonAssociatesWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonAssociatesWebClient));
						  
        /// <summary>
        /// Tests the connection to Amazon
        /// </summary>
        public static void TestConnection(string accessKeyID, ClientCertificate certificate)
        {
            SellerLookup lookup = new SellerLookup();
            lookup.AWSAccessKeyId = accessKeyID;

            SellerLookupRequest request = new SellerLookupRequest();
            request.ResponseGroup = new string[] { "Seller" };
            request.SellerId = new string[] { "A2OIXR4TA6XKODX" };

            lookup.Request = new SellerLookupRequest[] { request };

            try
            {
                AWSECommerceService aws = CreateService(certificate, "TestConnection");
                SellerLookupResponse response = aws.SellerLookup(lookup);

                CheckResponseErrors(response.OperationRequest);
            }
            catch (AsynchronousOperationException ex)
            {
                log.Error("TestConnection failed.", ex);
                throw new AmazonException(typeof(AWSECommerceService), ex);
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("TestConnection failed.", ex);
                    throw new AmazonException(typeof(AWSECommerceService), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Look for errors and expose them as Amazon related if they do exist
        /// </summary>
        private static void CheckResponseErrors(OperationRequest operationRequest)
        {
            // See if there were any errors
            if (operationRequest.Errors != null && operationRequest.Errors.Length > 0)
            {
                ErrorsError error = operationRequest.Errors[0];

                if (error.Message.IndexOf("AWSAccessKeyId") != -1)
                {
                    throw new AmazonException(typeof(AmazonAssociatesWebClient), "The specified Access Key ID is invalid.");
                }
                else
                {
                    throw new AmazonException(typeof(AmazonAssociatesWebClient), error.Message);
                }
            }
        }

        /// <summary>
        /// Creates the web service proxy to connect to Amazon Associates (formerly AWS)
        /// </summary>
        private static AWSECommerceService CreateService(ClientCertificate certificate, string logName)
        {
            AWSECommerceService service = new AWSECommerceService();

            // configure wse security
            AmazonWse.ConfigureWse(service, certificate, new ApiLogEntry(ApiLogSource.Amazon, logName));

            return service;
        }

        /// <summary>
        /// Queries Amazon for product information for the provided ASIN
        /// </summary>
        public static AmazonItemDetail GetItemDetail(AmazonStoreEntity amazonStore, string asin)
        {
            // import the binary serialized certificate
            ClientCertificate certificate = new ClientCertificate();
            certificate.Import(amazonStore.Certificate);

            // construct the Amazon request objects
            ItemLookup lookup = new ItemLookup();
            lookup.AWSAccessKeyId = amazonStore.AccessKeyID;

            ItemLookupRequest request = new ItemLookupRequest();
            request.ItemId = new string[] { asin };
            request.ResponseGroup = new string[] { "ItemAttributes", "Images" };

            lookup.Request = new ItemLookupRequest[] { request };

            try
            {
                // create the web proxy and execute the lookup
                AWSECommerceService aws = CreateService(certificate, "GetItemDetail");
                ItemLookupResponse response = aws.ItemLookup(lookup);

                // raise errors
                CheckResponseErrors(response.OperationRequest);

                if (response.Items.Length == 0 || response.Items[0].Item == null || response.Items[0].Item.Length == 0)
                {
                    return null;
                }

                Item item = response.Items[0].Item[0];
                AmazonItemDetail itemDetail = new AmazonItemDetail() { Asin = asin };

                PopulateWeightValue(AmazonWeights.GetWeightsPriority(amazonStore), item, itemDetail);

                if (item.MediumImage != null)
                {
                    itemDetail.ItemUrl = item.MediumImage.URL;
                }

                return itemDetail;
            }
            catch (CryptographicException ex)
            {
                throw new AmazonException("There is a problem with your Amazon Certificate:\n\n" + ex.Message, ex);
            }
            catch (AsynchronousOperationException ex)
            {
                throw new AmazonException(typeof(AWSECommerceService), ex);
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    throw new AmazonException(typeof(AWSECommerceService), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Fills the Item Detail weight from downloaded Amazon information based on user/store preference.
        /// </summary>
        private static void PopulateWeightValue(List<AmazonWeightField> weightPriority, Item item, AmazonItemDetail itemDetail)
        {
            // look through the user-defined weight priorities looking for an weight value to use
            foreach (AmazonWeightField field in weightPriority)
            {
                switch (field)
                {
                    case AmazonWeightField.PackagingWeight:
                        if (item.ItemAttributes.PackageDimensions != null)
                        {
                            itemDetail.Weight = GetWeightValue(item.ItemAttributes.PackageDimensions.Weight);
                        }
                        break;

                    case AmazonWeightField.ItemWeight:
                        if (item.ItemAttributes.ItemDimensions != null)
                        {
                            itemDetail.Weight = GetWeightValue(item.ItemAttributes.ItemDimensions.Weight);
                        }
                        break;
                    case AmazonWeightField.TotalMetalWeight:
                        if (item.ItemAttributes.TotalMetalWeight != null)
                        {
                            itemDetail.Weight = GetWeightValue(item.ItemAttributes.TotalMetalWeight);
                        }
                        break;
                    case AmazonWeightField.TotalGemWeight:
                        if (item.ItemAttributes.TotalGemWeight != null)
                        {
                            itemDetail.Weight = GetWeightValue(item.ItemAttributes.TotalGemWeight);
                        }
                        break;
                    case AmazonWeightField.TotalDiamondWeight:
                        if (item.ItemAttributes.TotalDiamondWeight != null)
                        {
                            itemDetail.Weight = GetWeightValue(item.ItemAttributes.TotalDiamondWeight);
                        }
                        break;
                    default:
                        break;
                }

                if (itemDetail.Weight > 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Get the weight value, taking units into consideration
        /// </summary>
        private static double GetWeightValue(DecimalWithUnits weight)
        {
            if (weight == null)
            {
                return 0;
            }

            double original = weight.Value;

            switch (weight.Units)
            {
                case "hundredths-pounds":
                    return original / 100;

                case "grams":
                    return original * 0.00220462262;

                default:
                    log.ErrorFormat("Invalid Amazon Weight Unit: {0}", weight.Units);
                    break;
            }

            return original;
        }
    }
}
