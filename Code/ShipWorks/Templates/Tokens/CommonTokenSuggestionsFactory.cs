using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Provides common token suggestions based on where the token is going to be used.
    /// </summary>
    [Component]
    public class CommonTokenSuggestionsFactory : ITokenSuggestionFactory
    {
        private readonly TokenSuggestion[] genericSuggestions;
        private readonly TokenSuggestion[] shippingSuggestions;
        private readonly TokenSuggestion[] emailAddressSuggestions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonTokenSuggestionsFactory" /> class.
        /// </summary>
        public CommonTokenSuggestionsFactory()
        {
            genericSuggestions = new TokenSuggestion[]
            {
                new TokenSuggestion("{//Order/Number}",
                    "Order Number"),

                new TokenSuggestion("{//Order/Item/Name}",
                    "Item Name (1st Only)"),

                new TokenSuggestion("{//Order/Address[@type='bill']/LastName}, {//Order/Address[@type='bill']/FirstName}",
                    "Billing Person Name"),
            };

            emailAddressSuggestions = new TokenSuggestion[]
            {
                new TokenSuggestion("{//Order/Address[@type='bill']/Email}",
                    "Billing Email Address"),

                new TokenSuggestion("{//Order/Address[@type='ship']/Email}",
                    "Shipping Email Address"),
            };


            shippingSuggestions = new TokenSuggestion[]
            {
                new TokenSuggestion("{//Order/Number}",
                    "Order Number"),

                new TokenSuggestion("{//Order/Item/Name}",
                    "Item Name (1st Only)"),

                new TokenSuggestion("{//Order/Item/Quantity}",
                    "Item Quantity (1st Only)"),

                new TokenSuggestion(
                    "<xsl:for-each select=\"//Order/Item\">\r\n" +
                    "    {Name}\r\n" +
                    "    <xsl:if test=\"position() !=  last()\">, </xsl:if>\r\n" +
                    "</xsl:for-each>",
                    "Items Names"),

                new TokenSuggestion(
                    "<xsl:for-each select=\"//Order/Item\">\r\n" +
                    "    ({Quantity}) - {Name}\r\n" +
                    "    <xsl:if test=\"position() !=  last()\">, </xsl:if>\r\n" +
                    "</xsl:for-each>",
                    "Item Quantity and Name"),

                new TokenSuggestion(
                    "<xsl:for-each select=\"//Order/Item\">\r\n" +
                    "    ({Quantity}) - {Code} - {Location}\r\n" +
                    "    <xsl:if test=\"position() !=  last()\">, </xsl:if>\r\n" +
                    "</xsl:for-each>",
                    "Item Quantity and Locations")
            };


        }

        //public TokenSuggestion[] GetSuggestions(TokenUsage usage)
        //{
        //    return GetSuggestions(usage, new List<OrderEntity>());
        //}

        /// <summary>
        /// Gets an array of token suggestions for a particular usage.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <returns>An array of TokenSuggestion objects.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid TokenUsage passed to GetSuggestions.</exception>
        public TokenSuggestion[] GetSuggestions(TokenUsage usage)
        {
            switch (usage)
            {
                case TokenUsage.ShippingReference:
                    return shippingSuggestions;

                case TokenUsage.EmailAddress:
                    return emailAddressSuggestions;

                case TokenUsage.Generic:
                case TokenUsage.FileName:
                case TokenUsage.EmailSubject:
                    return genericSuggestions;

                case TokenUsage.OrderStatus:
                    return StatusPresetManager.GetAllPresets(StatusPresetTarget.Order).Select(s => new TokenSuggestion(s)).ToArray();
            }

            throw new InvalidOperationException("Invalid TokenUsage passed to GetSuggestions. " + usage);
        }

    }
}