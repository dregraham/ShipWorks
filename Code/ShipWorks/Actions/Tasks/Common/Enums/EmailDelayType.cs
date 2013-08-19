namespace ShipWorks.Actions.Tasks.Common.Enums
{
    /// <summary>
    /// Different time relativities of email delay
    /// </summary>
    public enum EmailDelayType
    {
        TimeMinutes = 0,
        TimeHours = 1,
        TimeDays = 2,
        TimeWeeks = 3,

        DayOfWeek = 4,

        ShipDate = 5
    }
}
