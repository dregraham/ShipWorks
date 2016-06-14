using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)] 
    public class ThreeDCartQuestion
    {
        public int OrderID { get; set; }

        public int QuestionID { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionAnswer { get; set; }

        public string QuestionType { get; set; }

        public int QuestionCheckoutStep { get; set; }

        public int QuestionSorting { get; set; }

        public int QuestionDiscountGroup { get; set; }
    }
}