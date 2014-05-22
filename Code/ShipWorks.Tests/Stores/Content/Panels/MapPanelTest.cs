using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Content.Panels;

namespace ShipWorks.Tests.Stores.Content.Panels
{
    [TestClass]
    public class MapPanelTest
    {
        [TestMethod]
        public void GetPicturesize_ReturnsSameSize_WhenLessThan640x640()
        {
            Size size = new Size(600,500);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.AreEqual(size.Width, pictureSize.Width);
            Assert.AreEqual(size.Height, pictureSize.Height);
        }

        [TestMethod]
        public void GetPicturesize_Returns640x320_WhenLessThan2000x1000()
        {
            Size size = new Size(2000, 1000);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.AreEqual(640, pictureSize.Width);
            Assert.AreEqual(320, pictureSize.Height);
        }

        [TestMethod]
        public void GetPicturesize_Returns320x640_WhenLessThan1000x2000()
        {
            Size size = new Size(1000, 2000);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.AreEqual(320, pictureSize.Width);
            Assert.AreEqual(640, pictureSize.Height);
        }
    }
}
