using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Model.TypedViewClasses;
using Xunit;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class MissingIndexTest
    {
        [Fact]
        public void FromList_ReturnsThreeIndexes_WhenTableHasThreeIndexIDs()
        {
            var view = new ShipWorksMissingIndexRequestsTypedView();
            AddIndex(view, 1, "SomeTable", new[] { "Foo" }, new string[0]);
            AddIndex(view, 2, "SomeTable", new[] { "ID" }, new[] { "Name" });
            AddIndex(view, 3, "OtherTable", new[] { "OrderID", "Location" }, new[] { "Details" });

            var results = MissingIndex.FromView(view);

            Assert.Equal(3, results.Count());
            Assert.Contains(1, results.Select(x => x.GroupHandle));
            Assert.Contains(2, results.Select(x => x.GroupHandle));
            Assert.Contains(3, results.Select(x => x.GroupHandle));
        }

        [Fact]
        public void FromList_PopulatesIndexes_FromIndexRows()
        {
            var view = new ShipWorksMissingIndexRequestsTypedView();
            var indexRows = AddIndex(view, 1, "OtherTable", new[] { "OrderID", "Location" }, new[] { "Details" });

            var index = MissingIndex.FromView(view).Single();

            Assert.Equal(1, index.GroupHandle);
            Assert.Equal("OtherTable", index.Table);

            Assert.Equal("OrderID", index.Columns.ElementAt(0).Name);
            Assert.Equal(false, index.Columns.ElementAt(0).IsInclude);

            Assert.Equal("Location", index.Columns.ElementAt(1).Name);
            Assert.Equal(false, index.Columns.ElementAt(1).IsInclude);

            Assert.Equal("Details", index.Columns.ElementAt(2).Name);
            Assert.Equal(true, index.Columns.ElementAt(2).IsInclude);
        }

        private IEnumerable<DataRow> AddIndex(ShipWorksMissingIndexRequestsTypedView view, int groupHandle, string tableName, string[] columns, string[] included) =>
            columns
                .Select(x => (Name: x, Included: "EQUALITY"))
                .Concat(included.Select(x => (Name: x, Included: "INCLUDE")))
                .Select((column, index) =>
                {
                    var row = (ShipWorksMissingIndexRequestsRow) view.NewRow();
                    row.TableName = tableName;
                    row.GroupHandle = groupHandle;
                    row.ColumnName = column.Name;
                    row.ColumnUsage = column.Included;
                    return row;
                })
                .ForEach(view.Rows.Add);
    }
}
