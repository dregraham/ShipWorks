using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using System;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests
{
    public class NoteTest
    {
        [Fact]
        public void Add_NoteNotAdded_WhenNoteIsEmpty()
        {
            OrderEntity order = new OrderEntity {IsNew = true};

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, string.Empty, DateTime.Now, NoteVisibility.Internal);

                Assert.Empty(order.Notes);
            }
        }

        [Fact]
        public void Add_NoteNotAdded_WhenDownloadedNoteAlreadyExists()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                string sameText = "blahblahblah";
                var noteWithSameText = new NoteEntity { Text = sameText, Source = (int)NoteSource.Downloaded };
                order.Notes.Add(noteWithSameText);

                Note note = mock.Create<Note>();
                note.Add(order, sameText, DateTime.Now, NoteVisibility.Internal);
                
                // The only note that is there is the one that was added before.
                Assert.Equal(noteWithSameText, order.Notes.Single());
            }
        }

        [Fact]
        public void Add_NoteAdded_WhenNewOrder_AndExistingNoteWithSameText_AndExistingNoteNotDownloaded()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                string sameText = "blahblahblah";
                var noteWithSameText = new NoteEntity { Text = sameText, Source = (int)NoteSource.ShipWorksUser };
                order.Notes.Add(noteWithSameText);

                Note note = mock.Create<Note>();
                note.Add(order, sameText, DateTime.Now, NoteVisibility.Internal);

                // The only note that is there is the one that was added before.
                Assert.Equal(2, order.Notes.Count(n=>n.Text==sameText));
            }
        }

        [Fact]
        public void Add_NoteNotAdded_WhenExistingOrder_AndOrderInRepository()
        {
            OrderEntity order = new OrderEntity { IsNew = false };

            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderRepository> orderRepo = mock.Mock<IOrderRepository>();
                orderRepo.Setup(r => r.ContainsNote(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<NoteSource>()))
                    .Returns(true);

                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Empty(order.Notes);
            }
        }

        [Fact]
        public void Add_NoteAdded_WhenExistingOrder_AndOrderNotInRepository()
        {
            OrderEntity order = new OrderEntity { IsNew = false };

            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderRepository> orderRepo = mock.Mock<IOrderRepository>();
                orderRepo.Setup(r => r.ContainsNote(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<NoteSource>()))
                    .Returns(false);

                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Equal(1, order.Notes.Count);
            }
        }

        [Fact]
        public void Add_OrderRepositoryNotAccessed_WhenOrderIsNew()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderRepository> orderRepo = mock.Mock<IOrderRepository>();

                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                orderRepo.Verify(r=>r.ContainsNote(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<NoteSource>()), Times.Never);
            }
        }

        [Fact]
        public void Add_OrderRepositoryAccessed_WhenOrderIsNotNew()
        {
            OrderEntity order = new OrderEntity { IsNew = false };

            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOrderRepository> orderRepo = mock.Mock<IOrderRepository>();

                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                orderRepo.Verify(r => r.ContainsNote(It.IsAny<OrderEntity>(), It.IsAny<string>(), It.IsAny<NoteSource>()), Times.Once);
            }
        }

        [Fact]
        public void Add_NoteOrderIsOrder()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Equal(order, order.Notes.Single().Order);
            }
        }

        [Fact]
        public void Add_EditedIsParameter()
        {
            OrderEntity order = new OrderEntity { IsNew = true };
            DateTime now = DateTime.Now;

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", now, NoteVisibility.Internal);

                Assert.Equal(now, order.Notes.Single().Edited);
            }
        }

        [Fact]
        public void Add_UserIdIsNull()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Null(order.Notes.Single().UserID);
            }
        }

        [Fact]
        public void Add_SourceIsDownloaded()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Equal((int) NoteSource.Downloaded, order.Notes.Single().Source);
            }
        }

        [Theory]
        [InlineData(NoteVisibility.Internal)]
        [InlineData(NoteVisibility.Public)]
        public void Add_VisibilityIsParameter(NoteVisibility visibility)
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, visibility);

                Assert.Equal((int)visibility, order.Notes.Single().Visibility);
            }
        }

        [Fact]
        public void Add_TextIsParameter()
        {
            OrderEntity order = new OrderEntity { IsNew = true };

            using (var mock = AutoMock.GetLoose())
            {
                Note note = mock.Create<Note>();
                note.Add(order, "blah", DateTime.Now, NoteVisibility.Internal);

                Assert.Equal("blah", order.Notes.Single().Text);
            }
        }

    }
}
