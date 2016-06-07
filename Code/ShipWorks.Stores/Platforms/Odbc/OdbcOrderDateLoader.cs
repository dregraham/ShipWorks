using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcOrderDateLoader : IOdbcOrderDetailLoader
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public OdbcOrderDateLoader(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            order.OnlineLastModified = GetSqlCompliantDate(order.OnlineLastModified);

            List<IShipWorksOdbcMappableField> orderDateFields =
                map.FindEntriesBy(OrderFields.OrderDate, false).Select(e => e.ShipWorksField).ToList();

            DateTime? orderDateTime = GetDate(orderDateFields, ShipWorksOdbcMappableField.OrderDateAndTimeDisplayName);
            DateTime? orderDate = GetDate(orderDateFields, ShipWorksOdbcMappableField.OrderDateDisplayName);
            DateTime? orderTime = GetDate(orderDateFields, ShipWorksOdbcMappableField.OrderTimeDisplayName);

            if (orderDateTime.HasValue)
            {
                order.OrderDate = orderDateTime.Value;
            }
            else if (orderDate.HasValue && orderTime.HasValue)
            {
                order.OrderDate = orderDate.Value.Date.Add(orderTime.Value.TimeOfDay);
            }
            else if (orderDate.HasValue)
            {
                order.OrderDate = orderDate.Value;
            }

            // If order date not set, it will be set to now. Else, it will stay the same.
            order.OrderDate = GetSqlCompliantDate(order.OrderDate);
        }

        private DateTime? GetDate(List<IShipWorksOdbcMappableField> orderDateFields, string displayName)
        {
            IShipWorksOdbcMappableField dateField = orderDateFields.FirstOrDefault(f => f.DisplayName == displayName);
            return (DateTime?) dateField?.Value;
        }

        /// <summary>
        /// Sql can't handle dates prior to 1/1/1753
        /// </summary>
        private DateTime GetSqlCompliantDate(DateTime date)
        {
            return date < DateTime.Parse("1/1/1753 12:00:00 AM") ? dateTimeProvider.UtcNow : date;
        }
    }
}
