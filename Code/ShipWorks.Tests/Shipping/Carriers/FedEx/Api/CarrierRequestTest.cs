using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class CarrierRequestTest
    {
        // A little unique here in that we need a class to test/expose a protected method on the 
        // abstract class (CarrierRequest) that we're ultimately interested in testing
        public class CarrierRequestToTestProtectedMethod : CarrierRequest
        {
            public CarrierRequestToTestProtectedMethod(IEnumerable<ICarrierRequestManipulator> manipulators, ShipmentEntity shipmentEntity)
                : base(manipulators, shipmentEntity)
            { }
            
            //  A public method that just defers to the protected method we want to test
            // on the CarrierRequest base class
            public new void ApplyManipulators()
            {
                base.ApplyManipulators();
            }

            public override IEntity2 CarrierAccountEntity
            {
                get { throw new NotImplementedException(); }
            }

            public override ICarrierResponse Submit()
            {
                // not important for this test
                throw new NotImplementedException();
            }
        }

        // Mocking a couple of manipulators to use for our test
        Mock<ICarrierRequestManipulator> firstManipulator;
        Mock<ICarrierRequestManipulator> secondManipulator;
        
        // We're going to use the inline class to test our object; again this is only due to the 
        // uniqueness of the situation of testing an protected method on an abstract class; normally
        // we wouldn't be testing a protected method, but doing so here allows us to not have to 
        // test that the appy manipulators method actually uses all of the manipulators provided.
        CarrierRequestToTestProtectedMethod testObject;

        [TestInitialize]
        public void Initialize()
        {
            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            // create our test object with the mocked manipulators
            testObject = new CarrierRequestToTestProtectedMethod(manipulators, new ShipmentEntity());
        }

        [Fact]
        public void ApplyManipulators_DelegatesToAllManipulators_Test()
        {
            testObject.ApplyManipulators();

            // Check that each manipulator was called
            firstManipulator.Verify(m => m.Manipulate(It.IsAny<CarrierRequestToTestProtectedMethod>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(It.IsAny<CarrierRequestToTestProtectedMethod>()), Times.Once());
        }
    }
}
