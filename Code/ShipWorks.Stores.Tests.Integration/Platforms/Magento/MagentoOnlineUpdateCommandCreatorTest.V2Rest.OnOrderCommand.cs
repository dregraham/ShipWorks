using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    public partial class MagentoOnlineUpdateCommandCreatorTest
    {
        [Fact]
        public async Task V2Rest_OrderCommand_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 10000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(5.0)),
                10000));
        }

        [Fact]
        public async Task V2Rest_OrderCommand_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, 20000), Times.Never);
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 10000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(3.0)),
                10000));
        }

        [Fact]
        public async Task V2Rest_OrderCommand_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 20000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2.0)),
                20000));
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 30000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(3.0)),
                30000));
        }

        [Fact]
        public async Task V2Rest_OrderCommand_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, 20000), Times.Never);
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 30000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(3.0)),
                30000));
        }

        [Fact]
        public async Task V2Rest_OrderCommand_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);


            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-123" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 10000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(5.0)),
                10000));
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-456" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 50000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(5.0)),
                50000));
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-456" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 60000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(6.0)),
                60000));
        }

        [Fact]
        public async Task V2Rest_OrderCommand_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClientRest.Setup(x => x.UploadShipmentDetails(AnyString, AnyString, 10000))
                .Throws(new MagentoException("Foo"));
            webClientRest.Setup(x => x.UploadShipmentDetails(AnyString, AnyString, 50000))
                .Throws(new MagentoException("Foo"));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-456" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 60000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(6.0)),
                60000));
            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["tracks"][0]["trackNumber"].Value<string>() == "track-789" &&
                    s["tracks"][0]["title"].Value<string>() == "Bar" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 70000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(8.0)),
                70000));
        }
    }
}