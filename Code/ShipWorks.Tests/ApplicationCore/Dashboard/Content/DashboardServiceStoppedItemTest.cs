using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.Tests.ApplicationCore.Dashboard.Content
{
    [TestClass]
    public class DashboardServiceStoppedItemTest
    {
        private DashboardSchedulerServiceStoppedItem testObject;

        [TestMethod]
        public void Initialize_CanUserDismiss_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.IsFalse(testObject.DashboardBar.CanUserDismiss);
        }

        [TestMethod]
        public void Initialize_PrimaryText_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.AreEqual("Actions", testObject.DashboardBar.PrimaryText);
        }

        [TestMethod]
        public void Initialize_SecondaryText_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.AreEqual("A required ShipWorks action scheduler is not running.", testObject.DashboardBar.SecondaryText);
        }
        
        [TestMethod]
        public void Initialize_ImageIsNotNull_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.IsNotNull(testObject.DashboardBar.Image);
        }
    }
}
