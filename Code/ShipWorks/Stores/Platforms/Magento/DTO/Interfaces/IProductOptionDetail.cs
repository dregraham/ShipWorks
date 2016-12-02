namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IProductOptionDetail
    {
        long OptionID { get; set; }
        decimal Price { get; set; }
        string ProductSku { get; set; }
        string Title { get; set; }
    }
}