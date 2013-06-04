using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Status codes returned by Infopia
    /// </summary>
    enum InfopiaStatusType
    {
        Error = -100,
        InsertSuccess = 100,
        InvalidUserToken = -1,
        SearchPartialSuccess = 201,
        SearchSuccess = 200,
        SettingsHeadersSuccess = 300,
        SettingsMasterHeaderTypesSuccess = 400,
        UpdateSuccess = 101
    }
}
