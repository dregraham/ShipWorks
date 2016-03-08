using System.ComponentModel;

namespace ShipWorks.ApplicationCore.Licensing
{
    public enum UspsPendingAccountType
    {
        [Description("None")]
        None = 0,

        [Description("Create")]
        Create = 1,

        [Description("Existing")]
        Existing = 2
    }
}