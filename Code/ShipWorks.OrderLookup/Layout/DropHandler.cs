using System.Reflection;
using GongSolutions.Wpf.DragDrop;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.OrderLookup.Layout
{
    /// <summary>
    /// Drop Handler for Order Lookup Mode
    /// </summary>
    [Component]
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public class DropHandler : DefaultDropHandler
    {
    }
}