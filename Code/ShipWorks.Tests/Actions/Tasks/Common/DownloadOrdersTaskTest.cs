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
            Assert.Equal(testObject.InputRequirement, ActionTaskInputRequirement.None);
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

            Assert.Equal(4, testObject.StoreIDs.Count());
            Assert.Equal(12005, testObject.StoreIDs.ElementAt(0));
            Assert.Equal(5005, testObject.StoreIDs.ElementAt(1));
            Assert.Equal(8005, testObject.StoreIDs.ElementAt(2));
            Assert.Equal(11005, testObject.StoreIDs.ElementAt(3));
        }
       
    }
}
