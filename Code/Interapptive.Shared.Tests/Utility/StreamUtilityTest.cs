using Interapptive.Shared.Utility;
using System;
using System.IO;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class StreamUtilityTest
    {
        [Fact]
        public void ConvertToString_ThrowsArgumentException_WhenStreamIsNull()
        {
            Stream testObject = null;
            Assert.Throws<ArgumentNullException>(() => testObject.ConvertToString());
        }

        [Fact]
        public void ConvertToString_ReturnsStringFromStream()
        {
            var testString = "blah";
            string convertedString;
            using (var testObject = GenerateStreamFromString(testString))
            {
                convertedString = testObject.ConvertToString();
            }

            Assert.Equal(testString, convertedString);
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            return stream;
        }
    }
}
