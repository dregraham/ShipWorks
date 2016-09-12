using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Factory for creating and returning an IPrinterSetting object
    /// </summary>
    public static class PrinterSettingFactory
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PrinterSettingFactory));
        private static readonly LruCache<string, IPrinterSetting> printerSettingsCache = new LruCache<string, IPrinterSetting>(100, TimeSpan.FromMinutes(20));
        private static readonly LruCache<string, PrinterConfigurationContext.PrinterSettingsProxy> printerSettingsProxyCache = new LruCache<string, PrinterConfigurationContext.PrinterSettingsProxy>(100, TimeSpan.FromMinutes(20));
        private const string DefaultPrinterKey = "DEFAULT";

        /// <summary>
        /// Get an IPrinterSetting by printer name.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        public static IPrinterSetting GetPrinterSettings(string printerName)
        {
            using (new LoggedStopwatch(log, $"PrinterSettingFactory GetPrinterSettings({printerName})"))
            {
                string printerNameInvariant = printerName.ToUpperInvariant();

                IPrinterSetting printerSetting = printerSettingsCache[printerNameInvariant];
                if (printerSetting != null)
                {
                    return printerSetting;
                }

                PrinterSettings printerSettings = new PrinterSettings {PrinterName = printerName};

                printerSetting = new PrinterSetting(printerSettings);
                printerSettingsCache[printerNameInvariant] = printerSetting;

                return printerSetting;
            }
        }

        /// <summary>
        /// Get the currently selected default printer IPrinterSetting.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        public static IPrinterSetting GetDefaultPrinterSettings()
        {
            using (new LoggedStopwatch(log, "PrinterSettingFactory GetDefaultPrinterSettings()"))
            {
                IPrinterSetting printerSetting = printerSettingsCache[DefaultPrinterKey];
                if (printerSetting != null)
                {
                    return printerSetting;
                }

                // PrinterSettings() will return the default printer.
                printerSetting = new PrinterSetting(new PrinterSettings());
                printerSettingsCache[DefaultPrinterKey] = printerSetting;

                return printerSetting;
            }
        }

        /// <summary>
        /// Get an PrinterSettingsProxy by printer name.
        /// If the requested printer name has been cached, it will be returned.  
        /// Otherwise a new one will be created, cached and returned.
        /// </summary>
        public static PrinterConfigurationContext.PrinterSettingsProxy GetPrinterSettingsProxy(string printerName)
        {
            using (new LoggedStopwatch(log, $"PrinterSettingFactory GetPrinterSettingsProxy({printerName})"))
            {
                string printerNameInvariant = printerName.ToUpperInvariant();
                PrinterConfigurationContext.PrinterSettingsProxy printerSettingsProxy = printerSettingsProxyCache[printerNameInvariant];

                if (printerSettingsProxy != null && printerSettingsProxy.PrinterSettings != null)
                {
                    return printerSettingsProxy;
                }

                printerSettingsProxy = new PrinterConfigurationContext.PrinterSettingsProxy(GetPrinterSettings(printerName));
                printerSettingsProxyCache[printerNameInvariant] = printerSettingsProxy;

                return printerSettingsProxy;
            }
        }
    }
}
