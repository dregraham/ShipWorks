using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Interface for an ApiSettingsRepository
    /// </summary>
    public interface IApiSettingsRepository
    {
        /// <summary>
        /// Save settings
        /// </summary>
        void Save(ApiSettings settings);

        /// <summary>
        /// Load Settings
        /// </summary
        ApiSettings Load();
    }
}
