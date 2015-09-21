using Xunit;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;

namespace ShipWorks.Tests.ApplicationCore.Dashboard.Content
{
    public class DashboardServiceStoppedItemTest
    {
        private DashboardSchedulerServiceStoppedItem testObject;

        [Fact]
        public void Initialize_CanUserDismiss_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.False(testObject.DashboardBar.CanUserDismiss);
        }

        [Fact]
        public void Initialize_PrimaryText_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.Equal("Actions", testObject.DashboardBar.PrimaryText);
        }

        [Fact]
        public void Initialize_SecondaryText_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.Equal("A required ShipWorks action scheduler is not running.", testObject.DashboardBar.SecondaryText);
        }
        
        [Fact]
        public void Initialize_ImageIsNotNull_Test()
        {
            testObject = new DashboardSchedulerServiceStoppedItem();

            testObject.Initialize(new DashboardBar());

            Assert.NotNull(testObject.DashboardBar.Image);
        }
    }
}
