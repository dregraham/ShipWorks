using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Tokens;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// An implementation of the ITokenSuggestionFactory that is specific to i-parcel.
    /// </summary>
    public class iParcelTokenSuggestionFactory : ITokenSuggestionFactory
    {
        private const string DefaultTokenXsl = "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>";

        private readonly List<ShipmentEntity> shipments;
        private readonly IiParcelRepository repository;

        private readonly TokenSuggestion defaultSkuQuantitySuggestion;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTokenSuggestionFactory" /> class.
        /// </summary>
        public iParcelTokenSuggestionFactory()
            : this(new iParcelDatabaseRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTokenSuggestionFactory" /> class.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        public iParcelTokenSuggestionFactory(List<ShipmentEntity> shipments)
            : this(shipments, new iParcelDatabaseRepository())
        { }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTokenSuggestionFactory" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public iParcelTokenSuggestionFactory(IiParcelRepository repository)
            : this(new List<ShipmentEntity>(), repository)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelTokenSuggestionFactory" /> class.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="repository">The repository.</param>
        public iParcelTokenSuggestionFactory(List<ShipmentEntity> shipments, IiParcelRepository repository)
        {
            this.shipments = shipments;
            this.repository = repository;

            defaultSkuQuantitySuggestion = new TokenSuggestion(DefaultTokenXsl, "All item SKUs and Quantities in a delimited list");
        }

        /// <summary>
        /// Gets an array of token suggestions for a particular usage. This is specific to i-parcel
        /// and will only return a single suggestion for building a delimited list of SKUs and Quantities.
        /// </summary>
        /// <param name="usage">The usage.</param>
        /// <returns>An array of TokenSuggestion objects.</returns>
        public TokenSuggestion[] GetSuggestions(TokenUsage usage)
        {
            try
            {
                // We only have the need for returning shipping suggestions in the context of i-parcel, 
                // so we ignore the token usage in this implementation
                List<TokenSuggestion> suggestions = new List<TokenSuggestion>();
                
                if (shipments.Count == 1 && shipments.First() != null)
                {
                    // Since there is only one shipment selected, opulate the order details so the full list of order items
                    // is available to us in order to build out a list of suggestions for each order item to let the user 
                    // to quickly pick from a list in the case where they're not shipping everything in one package
                    ShipmentEntity shipment = shipments.First();
                    repository.PopulateOrderDetails(shipment);
                    
                    foreach (OrderItemEntity orderItem in shipment.Order.OrderItems)
                    {
                        TokenSuggestion itemSuggestion = new TokenSuggestion(string.Format("{0}, {1} ", orderItem.SKU, orderItem.Quantity), string.Format("All {0} items", orderItem.Name));
                        suggestions.Add(itemSuggestion);
                    }
                }

                // Always add the SKU/Quantity suggestion
                suggestions.Add(defaultSkuQuantitySuggestion);

                return suggestions.ToArray();
            }
            catch (Exception exception)
            {
                string message = string.Format("An error occurred while populating the list of i-parcel token suggestions. {0}", exception.Message);
                throw new iParcelException(message);
            }
        }
    }
}
