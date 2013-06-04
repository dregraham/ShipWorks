using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Interapptive.Shared.Utility;
using ShipWorks.UI;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using System.IO;
using log4net;
using System.Xml.Linq;
using System.Diagnostics;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Maintains the window size of the print preview window
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class BrowserWindowPosition
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BrowserWindowPosition));

        static string statefile = Path.Combine(Path.GetDirectoryName(WindowStateSaver.StateFile), "printpreview.xml");

        Rectangle parentBounds = new Rectangle();
        string browserWidth;
        string browserHeight;

        /// <summary>
        /// The bounds of the parent window of the preview window.  Used to calculate a centered position.
        /// </summary>
        internal Rectangle ParentBounds
        {
            get { return parentBounds; }
            set { parentBounds = value; }
        }

        /// <summary>
        /// Load the saved window position settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public void Load()
        {
            if (!File.Exists(statefile))
            {
                return;
            }

            try
            {
                XElement root = XElement.Load(statefile);
                browserWidth = (string) root.Element("Width");
                browserHeight = (string) root.Element("Height");
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to read browser state file: " + ex.Message);
                log.Error("Failed to read browser state file: " + ex.Message);
            }
        }

        /// <summary>
        /// Save the window position settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public void Save(string width, string height)
        {
            browserWidth = width;
            browserHeight = height;

            try
            {
                XElement root = new XElement("ShipWorks",
                    new XElement("Width", width),
                    new XElement("Height", height));

                root.Save(statefile);
            }
            catch (IOException ex)
            {
                Debug.Fail("Failed to save browser state file: " + ex.Message);
                log.Error("Failed to save browser state file: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.Fail("Failed to save browser state file: " + ex.Message);
                log.Error("Failed to save browser state file: " + ex.Message);

                throw;
            }
        }

        /// <summary>
        /// The top screen coordinate of the window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Top
        {
            get 
            {
                int height = parentBounds.Height;

                if (Height.Contains("px"))
                {
                    int.TryParse(Height.Replace("px", ""), out height);
                }

                string top = string.Format("{0}px", parentBounds.Top + (parentBounds.Height / 2) - (height / 2) );

                return top;
            }
        }
        
        /// <summary>
        /// The left screen coordinate of the window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Left
        {
            get 
            {
                int width = parentBounds.Width;

                if (Width.Contains("px"))
                {
                    int.TryParse(Width.Replace("px", ""), out width);
                }

                string left = string.Format("{0}px", parentBounds.Left + (parentBounds.Width - width) / 2);

                return left;
            }
        }

        /// <summary>
        /// The width of the browser preview window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Width
        {
            get 
            {
                if (!string.IsNullOrEmpty(browserWidth))
                {
                    return browserWidth;
                }
                else
                {
                    return string.Format("{0}px", Math.Max(100, parentBounds.Width - 75));
                }
            }
        }

        /// <summary>
        /// The height of the browser preview window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Height
        {
            get 
            {
                if (!string.IsNullOrEmpty(browserHeight))
                {
                    return browserHeight;
                }
                else
                {
                    return string.Format("{0}px", Math.Max(100, parentBounds.Height - 75));
                }
            }
        }
    }
}
