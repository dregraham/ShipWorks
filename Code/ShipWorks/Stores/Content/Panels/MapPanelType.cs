using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Enum that controls what type of image the MapPanel gets.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]    
    public enum MapPanelType
    {
        Satellite,
        StreetView
    }
}
