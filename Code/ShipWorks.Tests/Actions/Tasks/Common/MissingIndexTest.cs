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
            Assert.Equal("OtherTable", index.TableName);

            Assert.Equal("OrderID", index.Columns.ElementAt(0).Name);
            Assert.Equal(false, index.Columns.ElementAt(0).IsInclude);

            Assert.Equal("Location", index.Columns.ElementAt(1).Name);
            Assert.Equal(false, index.Columns.ElementAt(1).IsInclude);

            Assert.Equal("Details", index.Columns.ElementAt(2).Name);
            Assert.Equal(true, index.Columns.ElementAt(2).IsInclude);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsNull_WhenNoDisabledIndexesExist()
        {
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false) });

            var result = testObject.FindBestMatchingIndex(Enumerable.Empty<DisabledIndex>());

            Assert.Null(result);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsNull_WhenDisabledIndexesDoNotApplyToTable()
        {
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false) });

            var result = testObject.FindBestMatchingIndex(new[] { new DisabledIndex("IX_A", "Other", string.Empty, new[] { new IndexColumn("Bar", false) }) });

            Assert.Null(result);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsNull_IndexHasNoMatchingColumns()
        {
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false) });

            var result = testObject.FindBestMatchingIndex(new[] { new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Other", false) }) });

            Assert.Null(result);
        }

        [Theory]
        [InlineData("Foo", "Bar", "Foo", "Bar")]
        [InlineData("Foo", "Bar", "FOO", "BAR")]
        public void FindBestMatchingIndex_ReturnsIndex_WhenColumnsMatch(string disabledTable, string disabledColumn, string missingTable, string missingColumn)
        {
            var expectedIndex = new DisabledIndex("IX_A", disabledTable, string.Empty, new[] { new IndexColumn(disabledColumn, false) });
            var testObject = new MissingIndex(1, missingTable, new[] { new IndexColumn(missingColumn, false) });

            var result = testObject.FindBestMatchingIndex(new[] { expectedIndex });

            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsIndexWithMostColumnMatches_WhenMultipleIndexesQualify()
        {
            var otherIndex1 = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false) });
            var expectedIndex = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false), new IndexColumn("Baz", false) });
            var otherIndex2 = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false) });
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false), new IndexColumn("Baz", false) });

            var result = testObject.FindBestMatchingIndex(new[] { otherIndex1, expectedIndex, otherIndex2 });

            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsIndexWithFewestColumns_WhenMultipleIndexesHaveSameMatchCounts()
        {
            var otherIndex = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false), new IndexColumn("Baz", true) });
            var expectedIndex = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false) });
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false) });

            var result = testObject.FindBestMatchingIndex(new[] { expectedIndex, otherIndex });

            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void FindBestMatchingIndex_ReturnsFirstIndex_WhenMultipleIndexesHaveSameMatchCounts()
        {
            var expectedIndex = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false) });
            var otherIndex = new DisabledIndex("IX_A", "Foo", string.Empty, new[] { new IndexColumn("Bar", false) });
            var testObject = new MissingIndex(1, "Foo", new[] { new IndexColumn("Bar", false), new IndexColumn("Baz", false) });

            var result = testObject.FindBestMatchingIndex(new[] { expectedIndex, otherIndex });

            Assert.Equal(expectedIndex, result);
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
                .Do(view.Rows.Add)
                .ToList();
    }
}
