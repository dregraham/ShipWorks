using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.TypedViewClasses;
using Xunit;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class DisabledIndexTest
    {
        [Fact]
        public void FromList_ReturnsThreeIndexes_WhenTableHasThreeIndexIDs()
        {
            var view = new ShipWorksDisabledDefaultIndexTypedView();
            AddIndex(view, 1, new[] { "Foo" }, new string[0]);
            AddIndex(view, 1, new[] { "ID" }, new[] { "Name" });
            AddIndex(view, 1, new[] { "OrderID", "Location" }, new[] { "Details" });

            var results = DisabledIndex.FromView(view);

            Assert.Equal(3, results.Count());
        }

        private void AddIndex(ShipWorksDisabledDefaultIndexTypedView view, int indexID, string[] columns, string[] included) =>
            columns
                .Select(x => (Name: x, Included: true))
                .Concat(included.Select(x => (Name: x, Included: false)))
                .ForEach((column, index) =>
                {
                    var row = view.NewRow();
                    row[(int) ShipWorksDisabledDefaultIndexesFieldIndex.IndexID] = indexID;
                    row[(int) ShipWorksDisabledDefaultIndexesFieldIndex.ColumnName] = column.Name;
                    row[(int) ShipWorksDisabledDefaultIndexesFieldIndex.IndexColumnId] = index;
                    row[(int) ShipWorksDisabledDefaultIndexesFieldIndex.IsIncluded] = false;
                });
    }
}
