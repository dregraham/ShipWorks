using System;
using System.Reflection;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.UI.Dialogs.AboutShipWorks
{
    /// <summary>
    /// View model for the AboutShipWorksDialog
    /// </summary>
    [Component(SingleInstance = true, RegisterAs = RegistrationType.Self)]
    public class AboutShipWorksViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AboutShipWorksViewModel()
        {
            ShipWorksVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
            BuildDate = AssemblyDateAttribute.Read(Assembly.GetEntryAssembly()).ToLocalTime().ToLongDateString();
            SupportWebsite = "support.shipworks.com";
            SupportWebsiteUri = new Uri($"http://{SupportWebsite}");
            SupportEmail = "support@shipworks.com";
            SupportEmailUri = new Uri($"mailto:{SupportEmail}");
            SupportPhone = "1-800-952-7784";
            PatentInfo = "Covered by and/or for use with U.S. Patents 6,244,763; 6,939,063; 7,216,110; 7,236,956; 7,236,970; 7,458,612; 7,490,065; 7,743,043; 7,818,267; 7,831,524; 7,844,553; 7,882,094; 8,027,926; 8,041,644; 8,103,647;  8,240,579; 8,255,337; 8,374,970; 8,392,391; 8,489,519; 8,626,673; 8,626,674; 8,751,409; 8,762,290; 8,768,857; 8,843,464; and 8,954,355.";
            CopyrightInfo = $"Copyright © 2003 – {DateTime.Now.Year} Interapptive, Inc. All rights reserved. Interapptive, ShipWorks, and the ShipWorks logo are trademarks or registered trademarks of Interapptive, Inc.";
        }
        
        /// <summary>
        /// The version of ShipWorks currently running
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ShipWorksVersion { get; set; }
        
        /// <summary>
        /// The date this version of ShipWorks was built
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string BuildDate { get; set; }
        
        /// <summary>
        /// Support website that is displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SupportWebsite { get; }
        
        /// <summary>
        /// URI to open when support website link is clicked
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Uri SupportWebsiteUri { get; }
        
        /// <summary>
        /// Support email that is displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SupportEmail { get; }
        
        /// <summary>
        /// URI to open when support email link is clicked
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Uri SupportEmailUri { get; }
        
        /// <summary>
        /// Support phone number
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SupportPhone { get; }
        
        /// <summary>
        /// Patent info
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PatentInfo { get; }

        /// <summary>
        /// Copyright info
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CopyrightInfo { get; }
    }
}