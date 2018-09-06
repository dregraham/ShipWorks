using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using TechTalk.SpecFlow;
using Xunit;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Core.Specs.Testing
{
    [Binding]
    public class AddItemUpdateRollupSteps
    {
        private readonly DataContext context;
        private OrderEntity order;

        public AddItemUpdateRollupSteps(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });
        }

        [Given(@"the order has items")]
        public void GivenAnOrderWithItems(Table table) =>
            AddItemsFromTable(table, Modify.Order(order));

        [When(@"a new item is added to an order")]
        public void WhenANewItemIsAddedToAnOrder(Table table)
        {
            AddItemsFromTable(table, Modify.Order(order));
        }

        [Given(@"an order")]
        public void GivenAnOrder() =>
            order = Create.Order(context.Store, context.Customer).Save();

        [Given(@"the order has notes")]
        public void GivenTheOrderHasNotes(Table table) =>
            AddNotesFromTable(table, Modify.Order(order));

        [When(@"a new note is added to the order")]
        public void WhenANewNoteIsAddedToTheOrder(Table table) =>
            AddNotesFromTable(table, Modify.Order(order));

        [Then(@"note count rollup shows (.*)")]
        public void ThenNoteCountRollupShows(int count) =>
            Assert.Equal(count, order.RollupNoteCount);

        [Then(@"item quantity rollup shows (.*)")]
        public void ThenItemQuantityRollupShows(int quantity) =>
            Assert.Equal(quantity, order.RollupItemQuantity);

        [Then(@"item count rollup shows (.*)")]
        public void ThenItemCountRollupShows(int count) =>
            Assert.Equal(count, order.RollupItemCount);

        private void AddItemsFromTable(Table table, OrderEntityBuilder<OrderEntity> orderBuilder)
        {
            table.Rows
                .Select(row => row["Quantity"])
                .Select(ParseInt)
                .Select(x => x.Match(
                            i => i,
                            ex => throw new InvalidOperationException("Could not create a quantity from row")))
                .ForEach(quantity => orderBuilder.WithItem(item => item.Set(x => x.Quantity = quantity)));

            order = orderBuilder.Save();
        }

        private void AddNotesFromTable(Table table, OrderEntityBuilder<OrderEntity> orderBuilder)
        {
            table.Rows
                .Select(row => row["Text"])
                .ForEach(text => orderBuilder.WithNote(note => note.Set(x => x.Text = text)));

            order = orderBuilder.Save();
        }
    }
}
