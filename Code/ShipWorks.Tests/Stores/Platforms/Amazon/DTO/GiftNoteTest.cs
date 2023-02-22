using System;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.Amazon.DTO
{
    public class GiftNoteTest
    {
        [Fact]
        public void FromOrderSourceNote_CorrectlyParses()
        {
            var testNote = new OrderSourceNote()
            {
                Type = OrderSourceNoteType.GiftMessage,
                Text = "Gift Details:\nASIN: B01MAWHUZR\nSKU: W-PLD\nOrderItemId: 09316160083634\nMessage: Enjoy your gift!\nFrom Susan Feero\n\n"
            };

            var result = GiftNote.FromOrderSourceNote(testNote);

            Assert.Equal("B01MAWHUZR", result.ASIN);
            Assert.Equal("W-PLD", result.SKU);
            Assert.Equal("09316160083634", result.OrderItemId);
            Assert.Equal("Enjoy your gift!\nFrom Susan Feero", result.Message);
            Assert.Equal(0, result.Fee);
        }

        [Fact]
        public void FromOrderSourceNote_SetsNoteAsMessage_WhenNothingToParse()
        {
            var testNote = new OrderSourceNote()
            {
                Type = OrderSourceNoteType.GiftMessage,
                Text = "foo\nbar"
            };

            var result = GiftNote.FromOrderSourceNote(testNote);
            
            Assert.Null(result.OrderItemId);
            Assert.Equal(testNote.Text, result.Message);
        }
    }
}
