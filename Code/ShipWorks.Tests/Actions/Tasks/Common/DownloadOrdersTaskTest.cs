using System.Linq;
using Xunit;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class DownloadOrdersTaskTest
    {
        private readonly DownloadOrdersTask testObject;

        public DownloadOrdersTaskTest()
        {
            testObject = new DownloadOrdersTask();
        }

        [Fact]
        public void RequiresInput_ReturnsNone_Test()
        {
            Assert.AreEqual(testObject.InputRequirement, ActionTaskInputRequirement.None);
        }

        [Fact]
        public void Initalize_DeserializesXmlTaskSettingsToStoreIDs_Test()
        {
            const string TaskSettings = @"
                    <Settings>
                        <StoreIDs>
                            <Item type=""System.Int64"" value=""12005"" />
                            <Item type=""System.Int64"" value=""5005"" />
                            <Item type=""System.Int64"" value=""8005"" />
                            <Item type=""System.Int64"" value=""11005"" />
                        </StoreIDs>
                    </Settings>";

            testObject.Initialize(TaskSettings);

            Assert.AreEqual(4, testObject.StoreIDs.Count());
            Assert.AreEqual(12005, testObject.StoreIDs.ElementAt(0));
            Assert.AreEqual(5005, testObject.StoreIDs.ElementAt(1));
            Assert.AreEqual(8005, testObject.StoreIDs.ElementAt(2));
            Assert.AreEqual(11005, testObject.StoreIDs.ElementAt(3));
        }
       
    }
}
