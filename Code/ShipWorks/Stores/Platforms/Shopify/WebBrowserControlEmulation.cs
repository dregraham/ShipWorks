using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// The .Net WebBrowserControl is supposed to use the latest version of IE installed on the computer.  It may
    /// or may not do this, but we've had issues with adding Shopify stores, where the version of IE is not sent
    /// correctly.  This class modifies the registry to have the control emulate a specific version.
    /// 
    /// It adds the registry settings in the constructor, and then removes them in the destructor.
    /// </summary>
    public sealed class WebBrowserControlEmulation : IDisposable
    {
        /// <summary>
        /// Constructor that also sets the needed registry settings for version emulation.
        /// </summary>
        public WebBrowserControlEmulation()
        {
            SetBrowserFeatureControl();
        }

        /// <summary>
        /// Sets all the emulation registry settings.
        /// 
        /// http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx
        /// </summary>
        private static void SetBrowserFeatureControl()
        {
            // FeatureControl settings are per-process
            string fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode()); // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
        }

        /// <summary>
        /// Sets the individual registry settings for emulation.
        /// </summary>
        private static void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature), RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
                }
            }
            catch
            {
                // If we throw, it's probably a permission issue, or something we can't control
                // so just ignore...we don't want ShipWorks to crash due to this
            }
        }

        /// <summary>
        /// Removes all the emulation registry settings.
        /// 
        /// http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx
        /// </summary>
        private static void RemoveBrowserFeatureControl()
        {
            // FeatureControl settings are per-process
            string fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            RemoveBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName);
            RemoveBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName);
        }

        /// <summary>
        /// Removes the individual registry settings for emulation.
        /// </summary>
        private static void RemoveBrowserFeatureControlKey(string feature, string appName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature), RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key.DeleteValue(appName, false);
                }
            }
            catch
            {
                // If we throw, it's probably a permission issue, or something we can't control
                // so just ignore...we don't want ShipWorks to crash due to this
            }
        }

        /// <summary>
        /// Determines the current version of IE installed, and returns a UInt32 representation of that version.
        /// </summary>
        private static UInt32 GetBrowserEmulationMode()
        {
            UInt32 mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.

            try
            {
                int browserVersion = 10;
                using (RegistryKey ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer", RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.QueryValues))
                {
                    object version = ieKey.GetValue("svcVersion");
                    if (null == version)
                    {
                        version = ieKey.GetValue("Version");
                        if (null == version)
                        {
                            mode = 10000;
                        }
                    }

                    if (!int.TryParse(version.ToString().Split('.')[0], out browserVersion))
                    {
                        mode = 10000;
                    }
                }

                switch (browserVersion)
                {
                    case 7:
                        mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                        break;
                    case 8:
                        mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                        break;
                    case 9:
                        mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                        break;
                    case 10:
                        mode = 10000; // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 mode. Default value for Internet Explorer 10.
                        break;
                    default:
                        mode = 11001; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode. Default value for Internet Explorer 11.
                        break;
                }

            }
            catch
            {
                // Just try the default, IE 10
            }

            return mode;
        }

        /// <summary>
        /// Dispose method, and also removes all emulation registry settings.
        /// </summary>
        public void Dispose()
        {
            RemoveBrowserFeatureControl();
        }
    }
}
