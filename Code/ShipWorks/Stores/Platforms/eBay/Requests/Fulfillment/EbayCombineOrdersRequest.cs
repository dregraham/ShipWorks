using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment
{
    /// <summary>
    /// An implementation of the ICombineOrdersRequest interface that is responsible for
    /// making requests to eBay to combine a group of items/transactions into a single order.
    /// </summary>
    public class EbayCombineOrdersRequest : EbayRequest, ICombineOrdersRequest
    {
        private AddOrderRequestType request;


        /// <summary>
        /// Initializes a new instance of the <see cref="EbayCombineOrdersRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayCombineOrdersRequest(TokenData tokenData)
            : base(tokenData, "AddOrder")
        {
            request = new AddOrderRequestType()
            {
                Order = new OrderType()
                {
                    CreatingUserRole = TradingRoleCodeType.Seller,
                    CreatingUserRoleSpecified = true,
                    
                    Total = new AmountType() 
                    { 
                        currencyID = CurrencyCodeType.USD 
                    },

                    ShippingDetails = new ShippingDetailsType()
                    {
                        ShippingType = ShippingTypeCodeType.Flat,
                        ShippingTypeSpecified = true
                    }
                },
            };
        }


        /// <summary>
        /// Combines the orders in the IEnumerable of transactions provided.
        /// </summary>
        /// <param name="transactionsToCombine">The transactions to combine.</param>
        /// <param name="orderTotal">The order total.</param>
        /// <param name="paymentMethods">The payment methods.</param>
        /// <param name="shippingDetails">The shipping details.</param>
        /// <param name="salesTaxPercent">The sales tax percent.</param>
        /// <param name="taxState">State of the tax.</param>
        /// <param name="isShippingTaxed">if set to <c>true</c> [is shipping taxed].</param>
        /// <returns>An AddOrderResponseType object.</returns>
        public AddOrderResponseType CombineOrders(IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed)
        {
            request.Order.TransactionArray = transactionsToCombine.ToArray();
            request.Order.PaymentMethods = paymentMethods.ToArray();
            request.Order.Total.Value = orderTotal;

            ConfigureShippingDetails(shippingService, shippingCost, shippingCountryCode);
            ConfigureSalesTax(salesTaxPercent, taxState, isShippingTaxed);

            AddOrderResponseType response = SubmitRequest() as AddOrderResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to combine orders through eBay.");
            }

            return response;
        }

        /// <summary>
        /// A helper method to configure the shipping details of the request.
        /// </summary>
        /// <param name="shippingService">The shipping service.</param>
        /// <param name="shippingCost">The shipping cost.</param>
        /// <param name="shippingCountryCode">The shipping country code.</param>
        private void ConfigureShippingDetails(string shippingService, decimal shippingCost, string shippingCountryCode)
        {
            if (shippingCountryCode.ToUpper() == "US")
            {
                // configure the domestic options
                ShippingServiceOptionsType domesticOptions = new ShippingServiceOptionsType
                {
                    ShippingServiceCost = new AmountType()
                    {
                        currencyID = CurrencyCodeType.USD,
                        Value = Convert.ToDouble(shippingCost)
                    },
                    ShippingService = shippingService,
                    ShippingServicePriority = 1,
                    ShippingServicePrioritySpecified = true
                };

                request.Order.ShippingDetails.ShippingServiceOptions = new ShippingServiceOptionsType[] { domesticOptions };
            }
            else
            {
                // configure the international options
                InternationalShippingServiceOptionsType internationalOptions = new InternationalShippingServiceOptionsType
                {
                    ShippingServiceCost = new AmountType()
                    {
                        currencyID = CurrencyCodeType.USD,
                        Value = Convert.ToDouble(shippingCost)
                    },
                    ShippingService = shippingService,
                    ShippingServicePriority = 1,
                    ShippingServicePrioritySpecified = true,
                    ShipToLocation = new string[] { shippingCountryCode}
                };

                request.Order.ShippingDetails.InternationalShippingServiceOption = new InternationalShippingServiceOptionsType[] { internationalOptions };
            }
        }

        /// <summary>
        /// A helper method to build the sales tax object.
        /// </summary>
        /// <param name="salesTaxPercent">The sales tax percent.</param>
        /// <param name="taxState">State of the tax.</param>
        /// <param name="isShippingTaxed">if set to <c>true</c> [is shipping taxed].</param>
        /// <returns>A SalesTaxType object; a null value is return if salesTaxPercent is less than/equal to zero.</returns>
        private void ConfigureSalesTax(decimal salesTaxPercent, string taxState, bool isShippingTaxed)
        {
            if (salesTaxPercent > 0)
            {
                request.Order.ShippingDetails.SalesTax = new SalesTaxType
                {
                    SalesTaxPercent = (float)salesTaxPercent,
                    SalesTaxPercentSpecified = true,
                    SalesTaxState = taxState,
                    ShippingIncludedInTax = isShippingTaxed,
                    ShippingIncludedInTaxSpecified = true
                };
            }
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "AddOrder";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An AddOrderRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
