using System.Collections.Generic;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.Stores.PlatforInterfaces
{
    public interface IOnlineUpdateInstanceCommandsFactory
    {
        List<MenuCommand> Create();
    }
}