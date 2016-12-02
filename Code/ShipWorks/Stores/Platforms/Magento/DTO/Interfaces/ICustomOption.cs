namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface ICustomOption
    {
        IExtensionAttributes ExtensionAttributes { get; set; }
        long OptionID { get; set; }
        string OptionValue { get; set; }
    }
}