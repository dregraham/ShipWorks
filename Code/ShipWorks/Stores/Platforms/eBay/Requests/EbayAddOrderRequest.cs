using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// Implements the eBay AddOrder API, which is used to combine orders
    /// </summary>
    public class EbayAddOrderRequest : EbayRequest<long, AddOrderRequestType, AddOrderResponseType>
    {
        AddOrderRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayAddOrderRequest"/> class.
        /// </summary>
        /// <param name="transactionsToCombine">The transactions to combine.</param>
        /// <param name="orderTotal">The order total.</param>
        /// <param name="paymentMethods">The payment methods.</param>
        /// <param name="shippingDetails">The shipping details.</param>
        /// <param name="salesTaxPercent">The sales tax percent.</param>
        /// <param name="taxState">State of the tax.</param>
        /// <param name="isShippingTaxed">if set to <c>true</c> [is shipping taxed].</param>
        [NDependIgnoreTooManyParams]
        public EbayAddOrderRequest(EbayToken token, IEnumerable<TransactionType> transactionsToCombine, double orderTotal, IEnumerable<BuyerPaymentMethodCodeType> paymentMethods, decimal shippingCost, string shippingCountryCode, string shippingService, decimal salesTaxPercent, string taxState, bool isShippingTaxed)
            : base(token, "AddOrder")
        {
            if (transactionsToCombine == null || transactionsToCombine.Count() < 2)
            {
                throw new EbayException("There must be at least two orders to combine orders through eBay.");
            }

            if (orderTotal < 0)
            {
                throw new EbayException("The order total of the combined orders cannot be less than zero.");
            }

            if (salesTaxPercent < 0)
            {
                throw new EbayException("The sales tax rate of the combined orders cannot be less than zero.");
            }

            if (shippingCost < 0)
            {
                throw new EbayException("The shipping cost of the combined orders cannot be less than zero.");
            }

            request = new AddOrderRequestType()
                {
                    Order = new OrderType()
                    {
                        CreatingUserRole = TradingRoleCodeType.Seller,
                        CreatingUserRoleSpecified = true,
                    
                        Total = new AmountType() 
                            { 
                                currencyID = CurrencyCodeType.USD,
                                Value = orderTotal
                            },

                        ShippingDetails = new ShippingDetailsType()
                        {
                            ShippingType = ShippingTypeCodeType.Flat,
                            ShippingTypeSpecified = true
                        }
                    },
                };

            request.Order.TransactionArray = transactionsToCombine.ToArray();
            request.Order.PaymentMethods = paymentMethods.ToArray();

            ConfigureShippingDetails(shippingService, shippingCost, shippingCountryCode);
            ConfigureSalesTax(salesTaxPercent, taxState, isShippingTaxed);
        }

        /// <summary>
        /// Combines the orders
        /// </summary>
        public override long Execute()
        {
            AddOrderResponseType response = SubmitRequest();

            return long.Parse(response.OrderID);
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
                request.Order.ShippingDetails.SalesTax = new SalesTaxType()
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
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
