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

        /// <summary>
        /// Creates and loads item entities into the JetOrderEntity based on the JetOrderDetailsResult
        /// </summary>
        public JetOrderItemLoader(IOrderElementFactory orderElementFactory, IJetProductRepository productRepository)
        {
            this.orderElementFactory = orderElementFactory;
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Creates and loads item entities into the JetOrderEntity based on the JetOrderDetailsResult
        /// </summary>
        public void LoadItems(JetOrderEntity orderEntity, JetOrderDetailsResult orderDto)
        {
            foreach (JetOrderItem orderItemDto in orderDto.OrderItems)
            {
                JetOrderItemEntity orderItemEntity = (JetOrderItemEntity) orderElementFactory.CreateItem(orderEntity);

                // Load info from OrderItemDto
                orderItemEntity.Name = orderItemDto.ProductTitle;
                orderItemEntity.Code = orderItemDto.ItemTaxCode;
                orderItemEntity.SKU = orderItemDto.MerchantSku;
                orderItemEntity.MerchantSku = orderItemDto.MerchantSku;
                orderItemEntity.JetOrderItemID = orderItemDto.OrderItemId;

                orderItemEntity.UnitPrice = orderItemDto.ItemPrice.BasePrice;
                orderItemEntity.Quantity = orderItemDto.RequestOrderQuantity;

                // Load info from Fulfillment Node
                orderItemEntity.Location = orderDto.FulfillmentNode;

                JetProduct jetProduct = productRepository.GetProduct(orderItemDto);

                orderItemEntity.Description = jetProduct.ProductDescription;
                if (jetProduct.StandardProductCodes?.StandardProductCodeType == "UPC")
                {
                    orderItemEntity.UPC = jetProduct.StandardProductCodes.StandardProductCode;
                }
                else if(jetProduct.StandardProductCodes?.StandardProductCodeType?.StartsWith("ISBN") ?? false)
                {
                    orderItemEntity.ISBN = jetProduct.StandardProductCodes.StandardProductCode;
                }
                orderItemEntity.Image = jetProduct.MainImageUrl;
                orderItemEntity.Thumbnail = jetProduct.SwatchImageUrl;
                orderItemEntity.Weight = jetProduct.ShippingWeightPounds;
            }
        }
    }
}