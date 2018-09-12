using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Order Archive Filter Regeneration
    /// </summary>
    [Component]
    public class OrderArchiveFilterRegenerator : IOrderArchiveFilterRegenerator
    {
        private readonly IConfigurationData configurationData;
        private readonly ILog log;
        private readonly Func<IFilterHelper> createFilterHelper;
        private readonly Func<ISqlSession> getSqlSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveFilterRegenerator(
            IConfigurationData configurationData,
            Func<IFilterHelper> createFilterHelper,
            Func<ISqlSession> getSqlSession,
            Func<Type, ILog> createLog)
        {
            this.getSqlSession = getSqlSession;
            this.createFilterHelper = createFilterHelper;
            this.configurationData = configurationData;
            log = createLog(GetType());
        }

        /// <summary>
        /// Regenerate filters for an archive database
        /// </summary>
        public void Regenerate()
        {
            IConfigurationEntity configurationEntity = configurationData.FetchReadOnly();

            try
            {
                var document = XDocument.Parse(configurationEntity.ArchivalSettingsXml);
                var regenerationRequiredElement = document.Root
                    .Element("ArchivalSetting")?
                    .Element("NeedsFilterRegeneration");

                regenerationRequiredElement.ToResult()
                    .Map(x => x.Value)
                    .Bind(Functional.ParseBool)
                    .Filter(x => x)
                    .Do(_ => PerformRegeneration(regenerationRequiredElement));
            }
            catch (XmlException ex)
            {
                log.Error("Could not regenerate filters", ex);
            }
            catch (ArgumentNullException ex)
            {
                log.Error("Could not regenerate filters", ex);
            }
        }

        /// <summary>
        /// Perform the regeneration
        /// </summary>
        private void PerformRegeneration(XElement regenerationRequiredElement)
        {
            using (var connection = getSqlSession().OpenConnection())
            {
                createFilterHelper().RegenerateFilters(connection);
            }

            regenerationRequiredElement.SetValue(false);
            configurationData.UpdateConfiguration(x => x.ArchivalSettingsXml = regenerationRequiredElement.Document.ToString());
        }
    }
}
