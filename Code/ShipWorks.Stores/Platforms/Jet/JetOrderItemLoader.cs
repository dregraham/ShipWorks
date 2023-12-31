﻿using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Creates and loads item entities into the JetOrderEntity based on the JetOrderDetailsResult
    /// </summary>
    [Component]
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
        public void LoadItems(JetOrderEntity orderEntity, JetOrderDetailsResult orderDto, JetStoreEntity store)
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

                JetProduct jetProduct = productRepository.GetProduct(orderItemDto, store);

                orderItemEntity.Description = jetProduct.ProductDescription;

                orderItemEntity.UPC = jetProduct.StandardProductCodes?
                                          .FirstOrDefault(c => c.StandardProductCodeType == "UPC")?
                                          .StandardProductCode ?? string.Empty;

                orderItemEntity.ISBN = jetProduct.StandardProductCodes?
                                           .FirstOrDefault(c => c.StandardProductCodeType.StartsWith("ISBN"))?
                                           .StandardProductCode ?? string.Empty;

                orderItemEntity.Image = jetProduct.MainImageUrl;
                orderItemEntity.Thumbnail = jetProduct.SwatchImageUrl;
                orderItemEntity.Weight = jetProduct.ShippingWeightPounds;
            }
        }
    }
}