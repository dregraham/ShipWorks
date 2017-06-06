using System.Text;
using Interapptive.Shared.Net;
using Xunit;

namespace Interapptive.Shared.Tests.Net
{
    public class HttpVariableCollectionTest
    {
        private readonly HttpVariableRequestSubmitter requestSubmitter;

        public HttpVariableCollectionTest()
        {
            requestSubmitter = new HttpVariableRequestSubmitter();
            requestSubmitter.Variables.Add("2", "b");
            requestSubmitter.Variables.Add("1", "a");
            requestSubmitter.Variables.Add("4", "d");
            requestSubmitter.Variables.Add("3", "c");
        }

        [Fact]
        public void GetEnumerator_Iterates()
        {
            StringBuilder result = new StringBuilder();

            requestSubmitter.Variables.Sort(v => v.Name);

            using (var enumerator = requestSubmitter.Variables.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    HttpVariable value = enumerator.Current;
                    result.Append($"{value.Name}:{value.Value}|");
                }
            }

            Assert.Equal(@"1:a|2:b|3:c|4:d|", result.ToString());
        }

        [Fact]
        public void AddByNameAndValue_Adds()
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Variables.Add("1", "a");
            submitter.Variables.Add("2", "b");

            Assert.Equal(2, submitter.Variables.Count);
        }

        [Fact]
        public void AddItem_Adds()
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Variables.Add(new HttpVariable("1", "a"));
            submitter.Variables.Add(new HttpVariable("2", "b"));

            Assert.Equal(2, submitter.Variables.Count);
        }

        [Fact]
        public void Indexer_SelectsCorrectName()
        {
            Assert.Equal("b", requestSubmitter.Variables["2"]);
        }

        [Fact]
        public void Remove_Removes()
        {
            int origCount = requestSubmitter.Variables.Count;
            requestSubmitter.Variables.Remove("2");

            Assert.Equal(origCount - 1, requestSubmitter.Variables.Count);
        }

        [Fact]
        public void Sort_Sorts()
        {
            requestSubmitter.Variables.Sort(v => v.Name);

            Assert.Equal(requestSubmitter.Variables[0].Name, "1");
            Assert.Equal(requestSubmitter.Variables[1].Name, "2");
            Assert.Equal(requestSubmitter.Variables[2].Name, "3");
            Assert.Equal(requestSubmitter.Variables[3].Name, "4");
        }
    }
}
