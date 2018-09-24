using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Actions.Tasks.Common;
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
            AddIndex(view, "IX_SomeTable_1", new[] { "Foo" }, new string[0]);
            AddIndex(view, "IX_SomeTable_2", new[] { "ID" }, new[] { "Name" });
            AddIndex(view, "IX_OtherTable_1", new[] { "OrderID", "Location" }, new[] { "Details" });

            var results = DisabledIndex.FromView(view);

            Assert.Equal(3, results.Count());
            Assert.Contains("IX_SomeTable_1", results.Select(x => x.IndexName));
            Assert.Contains("IX_SomeTable_2", results.Select(x => x.IndexName));
            Assert.Contains("IX_OtherTable_1", results.Select(x => x.IndexName));
        }

        [Fact]
        public void FromList_PopulatesIndexes_FromIndexRows()
        {
            var view = new ShipWorksDisabledDefaultIndexTypedView();
            var indexRows = AddIndex(view, "IX_OtherTable_1", new[] { "OrderID", "Location" }, new[] { "Details" },
                x =>
                {
                    x.TableName = "OtherTable";
                    x.EnableIndex = "ENABLE SQL";
                });

            var index = DisabledIndex.FromView(view).Single();

            Assert.Equal("IX_OtherTable_1", index.IndexName);
            Assert.Equal("OtherTable", index.TableName);
            Assert.Equal("ENABLE SQL", index.EnableIndexSql);

            Assert.Equal("OrderID", index.Columns.ElementAt(0).Name);
            Assert.Equal(false, index.Columns.ElementAt(0).IsInclude);

            Assert.Equal("Location", index.Columns.ElementAt(1).Name);
            Assert.Equal(false, index.Columns.ElementAt(1).IsInclude);

            Assert.Equal("Details", index.Columns.ElementAt(2).Name);
            Assert.Equal(true, index.Columns.ElementAt(2).IsInclude);
        }

        [Fact]
        public void FromList_OrdersColumns_WhenColumnsAreNotInCorrectOrder()
        {
            var orders = new Stack<int>();
            orders.Push(1);
            orders.Push(0);
            orders.Push(2);

            var view = new ShipWorksDisabledDefaultIndexTypedView();
            var indexRows = AddIndex(view, "IX_OtherTable_1", new[] { "OrderID", "Location" }, new[] { "Details" }, x => x.IndexColumnId = orders.Pop());

            var index = DisabledIndex.FromView(view).Single();

            Assert.Equal(new[] { "OrderID", "Location", "Details" }, index.Columns.Select(x => x.Name));
        }

        private IEnumerable<DataRow> AddIndex(ShipWorksDisabledDefaultIndexTypedView view, string indexName, string[] columns, string[] included, Action<ShipWorksDisabledDefaultIndexRow> configure = null) =>
            columns
                .Select(x => (Name: x, Included: false))
                .Concat(included.Select(x => (Name: x, Included: true)))
                .Select((column, index) =>
                {
                    var row = (ShipWorksDisabledDefaultIndexRow) view.NewRow();
                    row.IndexName = indexName;
                    row.ColumnName = column.Name;
                    row.IndexColumnId = index;
                    row.IsIncluded = column.Included;
                    configure?.Invoke(row);
                    return row;
                })
                .Do(view.Rows.Add)
            .ToList();
    }
}
