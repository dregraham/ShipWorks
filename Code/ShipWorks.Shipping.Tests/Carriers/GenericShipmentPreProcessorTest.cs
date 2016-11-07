using System;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers
{
    public class GenericShipmentPreProcessorTest : IDisposable
    {
        readonly AutoMock mock;
        readonly GenericShipmentPreProcessor testObject;

        public GenericShipmentPreProcessorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<GenericShipmentPreProcessor>();
        }

        public class WhenInitiallyReadyToShip : GenericShipmentPreProcessorTest
        {
            ShipmentEntity shipment;

            public WhenInitiallyReadyToShip()
            {
                shipment = Create.Shipment(new OrderEntity()).AsOther().Build();

                var retriever = mock.CreateMock<ICarrierAccountRetriever>(m =>
                    m.Setup(x => x.AccountsReadOnly).Returns(new[] { mock.Create<ICarrierAccount>() }));
                mock.Mock<ICarrierAccountRetrieverFactory>()
                    .Setup(x => x.Create(ShipmentTypeCode.Other)).Returns(retriever.Object);
                mock.Mock<IShippingSettings>()
                    .Setup(x => x.IsConfigured(ShipmentTypeCode.Other)).Returns(true);
            }

            [Fact]
            public void Run_DelegatesToEnsureShipmentLoaded_WhenCarrierIsReadyToShip()
            {
                testObject.Run(shipment, null, null);

                mock.Mock<IShippingManager>()
                    .Verify(x => x.EnsureShipmentLoaded(shipment));
            }

            [Fact]
            public void Run_ReturnsShipment_WhenCarrierIsReadyToShip()
            {
                var results = testObject.Run(shipment, null, null);

                Assert.Equal(new[] { shipment }, results);
            }
        }

        public class WhenNotInitiallyReadyToShipAndCancelsDialog : GenericShipmentPreProcessorTest
        {
            ShipmentEntity shipment;

            public WhenNotInitiallyReadyToShipAndCancelsDialog()
            {
                shipment = Create.Shipment(new OrderEntity()).AsOther().Build();
            }

            [Fact]
            public void Run_DelegatesToShipmentTypeManager()
            {
                testObject.Run(shipment, null, null);

                mock.Mock<IShipmentTypeManager>()
                    .Verify(x => x.Get(shipment));
            }

            [Fact]
            public void Run_ShowWarning_WhenRegistrationIsNotAllowed()
            {
                testObject.Run(shipment, null, null);

                mock.Mock<IMessageHelper>()
                    .Verify(x => x.ShowWarning(It.IsAny<string>()));
            }

            [Fact]
            public void Run_ReturnsNull_WhenRegistrationIsNotAllowed()
            {
                var result = testObject.Run(shipment, null, null);

                Assert.Null(result);
            }

            [Fact]
            public void Run_CreatesDialog_WhenRegistrationIsAllowed()
            {
                mock.WithShipmentTypeFromShipmentManager(s => s.SetupGet(x => x.IsAccountRegistrationAllowed).Returns(true));
                mock.Mock<IMessageHelper>()
                    .Setup(x => x.ShowDialog(It.IsAny<Func<IForm>>()))
                    .Callback<Func<IForm>>(x => x());

                testObject.Run(shipment, null, null);

                mock.Mock<IShipmentTypeSetupWizardFactory>()
                    .Verify(x => x.Create(ShipmentTypeCode.Other));
            }

            [Fact]
            public void Run_ReturnsNull_WhenDialogIsCanceled()
            {
                mock.WithShipmentTypeFromShipmentManager(s => s.SetupGet(x => x.IsAccountRegistrationAllowed).Returns(true));
                mock.Mock<IMessageHelper>()
                    .Setup(x => x.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.Cancel);

                var result = testObject.Run(shipment, null, null);

                Assert.Null(result);
            }
        }

        public class WhenNotInitiallyReadyToShipAndAcceptsDialog : GenericShipmentPreProcessorTest
        {
            ShipmentEntity shipment;
            Mock<ICarrierAccount> account;

            public WhenNotInitiallyReadyToShipAndAcceptsDialog()
            {
                shipment = Create.Shipment(new OrderEntity()).AsOther().Build();
                account = mock.CreateMock<ICarrierAccount>();

                mock.WithShipmentTypeFromShipmentManager(s => s.SetupGet(x => x.IsAccountRegistrationAllowed).Returns(true));
                mock.Mock<IMessageHelper>()
                    .Setup(x => x.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

                var retriever = mock.CreateMock<ICarrierAccountRetriever>();
                mock.Mock<ICarrierAccountRetrieverFactory>()
                    .Setup(x => x.Create(ShipmentTypeCode.Other)).Returns(retriever.Object);

                mock.Mock<IShippingSettings>()
                    .Setup(x => x.CheckForChangesNeeded())
                    .Callback(() =>
                    {
                        retriever.Setup(x => x.AccountsReadOnly).Returns(new[] { account.Object });
                        mock.Mock<IShippingSettings>()
                            .Setup(x => x.IsConfigured(ShipmentTypeCode.Other)).Returns(true);
                    });
            }

            [Fact]
            public void Run_MarksCarrierAsConfigured_WhenDialogIsFinished()
            {
                testObject.Run(shipment, null, null);

                mock.Mock<IShippingSettings>()
                    .Verify(x => x.MarkAsConfigured(ShipmentTypeCode.Other));
            }

            [Fact]
            public void Run_DelegatesToShippingManagerToLoadShipment_WhenDialogIsFinished()
            {
                testObject.Run(shipment, null, null);

                mock.Mock<IShippingManager>().Verify(x => x.RefreshShipment(shipment));
                mock.Mock<IShippingManager>().Verify(x => x.EnsureShipmentLoaded(shipment));
            }

            [Fact]
            public void Run_ExecutesCallback_WhenDialogIsFinished()
            {
                bool wasCalled = false;

                testObject.Run(shipment, null, () => wasCalled = true);

                Assert.True(wasCalled);
            }

            [Fact]
            public void Run_AppliesAccountToShipment_WhenDialogIsFinished()
            {
                testObject.Run(shipment, null, null);

                account.Verify(x => x.ApplyTo(shipment));
            }

            [Fact]
            public void Run_SavesToDatabase_WhenDialogIsFinished()
            {
                var sqlAdapter = mock.CreateMock<ISqlAdapter>();
                mock.Mock<ISqlAdapterFactory>()
                    .Setup(x => x.CreateTransacted()).Returns(sqlAdapter.Object);

                testObject.Run(shipment, null, null);

                sqlAdapter.Verify(x => x.SaveAndRefetch(shipment));
                sqlAdapter.Verify(x => x.Commit());
            }

            [Fact]
            public void Run_ReturnsOriginalShipment_WhenDialogIsFinished()
            {
                var result = testObject.Run(shipment, null, null);

                Assert.Equal(new[] { shipment }, result);
            }
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
