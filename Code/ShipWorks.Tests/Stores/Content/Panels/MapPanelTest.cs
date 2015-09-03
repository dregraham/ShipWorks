using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Stores.Content.Panels;

namespace ShipWorks.Tests.Stores.Content.Panels
{
    public class MapPanelTest
    {
        [Fact]
        public void GetPicturesize_ReturnsSameSize_WhenLessThan640x640()
        {
            Size size = new Size(600,500);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.Equal(size.Width, pictureSize.Width);
            Assert.Equal(size.Height, pictureSize.Height);
        }

        [Fact]
        public void GetPicturesize_Returns640x320_WhenLessThan2000x1000()
        {
            Size size = new Size(2000, 1000);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.Equal(640, pictureSize.Width);
            Assert.Equal(320, pictureSize.Height);
        }

        [Fact]
        public void GetPicturesize_Returns320x640_WhenLessThan1000x2000()
        {
            Size size = new Size(1000, 2000);
            Size pictureSize = MapPanel.GetPictureSize(size);

            Assert.Equal(320, pictureSize.Width);
            Assert.Equal(640, pictureSize.Height);
        }
    }
}
