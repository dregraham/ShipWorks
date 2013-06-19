using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    [TestClass]
    public class ActionScheduleTest
    {

        //private readonly ActionSchedule testObject = new FakeActionSchedule()
        //{
        //    StartTime = DateTime.Now
        //};

        //[TestMethod]
        //public void Schedule_StartTimeStaysSameAfterSerializeAndDeserailize_Test()
        //{
        //    MemoryStream stream = new MemoryStream();
        //    XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.ASCII);

        //    testObject.SerializeXml(xmlWriter);

        //    xmlWriter.Flush();

        //    stream.Position = 0;

        //    StreamReader streamReader = new StreamReader(stream);
        //    string x = streamReader.ReadToEnd();
            
        //    XPathDocument xpathDocument = new XPathDocument(stream);
        //    XPathNavigator xPathNavigator = xpathDocument.CreateNavigator();

        //    ActionSchedule deserializedAction = new FakeActionSchedule();
        //    deserializedAction.DeserializeXml(xPathNavigator);

        //    Assert.AreEqual(testObject.StartTime, deserializedAction.StartTime);

        //}
    }
}
