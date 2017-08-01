using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Creates and loads item entities into the JetOrderEntity based on the JetOrderDetailsResult
    /// </summary>
    public class JetOrderItemLoader : IJetOrderItemLoader
    {
        private readonly IOrderElementFactory orderElementFactory;
        private readonly IJetProductRepository productRepository;

        public JetOrderItemLoader(IOrderElementFactory orderElementFactory, IJetProductRepository productRepository)
        {
            this.orderElementFactory = orderElementFactory;
            this.productRepository = productRepository;
        }

        public void LoadItems(JetOrderEntity orderEntity, JetOrderDetailsResult orderDto)
        {
            foreach (JetOrderItem orderItemDto in orderDto.OrderItems)
            {
                JetOrderItemEntity orderItemEntity = (JetOrderItemEntity) orderElementFactory.CreateItem(orderEntity);

                // Load info from OrderItemDto
                orderItemEntity.Name = orderItemDto.ProductTitle;
                orderItemEntity.Code = orderItemDto.ItemTaxCode;
                orderItemEntity.SKU = orderItemDto.MerchantSku;
                orderItemEntity.UnitPrice = orderItemDto.ItemPrice.BasePrice;
                orderItemEntity.Quantity = orderItemDto.RequestOrderQuantity;
                orderItemEntity.MerchantSku = orderItemDto.MerchantSku;

                // Load info from Fulfillment Node
                orderItemEntity.Location = orderDto.FulfillmentNode;

                // Get from product repo
                // Description
                // ISBN or UPC
                // Image
                // Thumbnail
                // Weight

            }
        }
    }
}