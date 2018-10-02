using System.Collections.Generic;

namespace ShipWorks.OrderLookup.FieldManager
{
    public interface IOrderLookupFieldLayoutDefaults
    {
        IEnumerable<SectionLayout> GetDefaults();
    }
}