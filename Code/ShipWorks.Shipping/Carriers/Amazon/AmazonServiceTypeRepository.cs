using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public class AmazonServiceTypeRepository : IAmazonServiceTypeRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public AmazonServiceTypeRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        public List<AmazonServiceTypeEntity> Get()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<AmazonServiceTypeEntity> query = new QueryFactory().AmazonServiceType;

                IEntityCollection2 types = sqlAdapter.FetchQueryAsync(query).Result;
                return types.OfType<AmazonServiceTypeEntity>().ToList();
            }
        }

        public AmazonServiceTypeEntity CreateNewService(string name, string description)
        {
            throw new NotImplementedException();
        }
    }
}
