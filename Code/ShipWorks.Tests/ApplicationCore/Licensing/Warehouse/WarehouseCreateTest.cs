using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Warehouse
{
    public class WarehouseCreateTest : IDisposable
    {
        private AutoMock mock;

        public WarehouseCreateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var restResponse = mock.Mock<IRestResponse>();
            restResponse.Setup(r => r.Content).Returns("{\"id\" : \"foo\"}");

            mock.Mock<IWarehouseRequestClient>()
                .Setup(c => c.MakeRequest(It.IsAny<IRestRequest>(), "CreateDefaultWarehouse"))
                .ReturnsAsync(GenericResult.FromSuccess(restResponse.Object));
        }

        [Fact]
        public async Task Create_DelegatesRequestCreation_ToWarehouseRequestFactory()
        {

            var testObject = mock.Create<WarehouseCreate>();
            Details warehouse = new Details();
            await testObject.Create(warehouse);

            mock.Mock<IWarehouseRequestFactory>().Verify(
                f => f.Create(WarehouseEndpoints.Warehouses,
                    Method.POST,
                    It.Is<ShipWorks.ApplicationCore.Licensing.Warehouse.DTO.Warehouse>(x => x.details == warehouse)));
        }

        [Fact]
        public async Task Create_ReturnsWarehouseID()
        {
            var testObject = mock.Create<WarehouseCreate>();
            var result = await testObject.Create(new Details());

            Assert.True(result.Success);
            Assert.Equal("foo", result.Value);
        }

        [Fact]
        public async Task Create_ReturnsFailure_IfRequestFails()
        {
            mock.Mock<IWarehouseRequestClient>()
                .Setup(c => c.MakeRequest(It.IsAny<IRestRequest>(), "CreateDefaultWarehouse"))
                .ReturnsAsync(GenericResult.FromError<IRestResponse>("Blah"));

            var testObject = mock.Create<WarehouseCreate>();
            var result = await testObject.Create(new Details());

            Assert.True(result.Failure);
            Assert.Equal("Blah", result.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
