using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    public interface IConfigurationData
    {
        ConfigurationEntity Fetch();

        void CheckForChangesNeeded();
    }
}