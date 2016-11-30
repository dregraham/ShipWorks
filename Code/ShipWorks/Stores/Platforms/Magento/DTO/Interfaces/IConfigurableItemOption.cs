namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IConfigurableItemOption
    {
        string OptionId { get; set; }
        int OptionValue { get; set; }
        IExtensionAttributes ExtensionAttributes { get; set; }
    }
}