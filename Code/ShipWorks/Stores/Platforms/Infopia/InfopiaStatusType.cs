namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Status codes returned by Infopia
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
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
