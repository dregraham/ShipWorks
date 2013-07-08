using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.ApplicationCore.Dashboard.Content
{
    [TestClass]
    public class DashboardServiceStoppedItemTest
    {
        private DashboardServiceStoppedItem testObject;

        [TestMethod]
        public void Initialize_CanUserDismiss_Test()
        {
            testObject = new DashboardServiceStoppedItem(new List<WindowsServiceEntity>());

            testObject.Initialize(new DashboardBar());

            Assert.IsFalse(testObject.DashboardBar.CanUserDismiss);
        }

        [TestMethod]
        public void Initialize_PrimaryText_Test()
        {
            testObject = new DashboardServiceStoppedItem(new List<WindowsServiceEntity>());

            testObject.Initialize(new DashboardBar());

            Assert.AreEqual("Schedulers", testObject.DashboardBar.PrimaryText);
        }

        [TestMethod]
        public void Initialize_SecondaryText_WhenMoreThanOneSchedulers_Test()
        {
            List<WindowsServiceEntity> schedulerEntities = new List<WindowsServiceEntity>
            {
                new WindowsServiceEntity(),
                new WindowsServiceEntity(),
                new WindowsServiceEntity()
            };
                
            testObject = new DashboardServiceStoppedItem(schedulerEntities);

            testObject.Initialize(new DashboardBar());

            Assert.AreEqual("There are 3 required ShipWorks scheduling services not running.", testObject.DashboardBar.SecondaryText);
        }

        [TestMethod]
        public void Initialize_SecondaryText_WhenOneSchedulerTest()
        {
            List<WindowsServiceEntity> schedulerEntities = new List<WindowsServiceEntity>
            {
                new WindowsServiceEntity()
            };

            testObject = new DashboardServiceStoppedItem(schedulerEntities);

            testObject.Initialize(new DashboardBar());

            Assert.AreEqual("There is 1 required ShipWorks scheduling service not running.", testObject.DashboardBar.SecondaryText);
        }

        [TestMethod]
        public void Initialize_ImageIsNotNull_Test()
        {
            testObject = new DashboardServiceStoppedItem(new List<WindowsServiceEntity>());

            testObject.Initialize(new DashboardBar());

            Assert.IsNotNull(testObject.DashboardBar.Image);
        }
    }
}
