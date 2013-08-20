namespace ShipWorks.Actions.Tasks.Common.Enums
{
    /// <summary>
    /// Defines how many requests are made
    /// </summary>
    public enum WebRequestCardinality
    {
        SingleRequest = 0,

        OneRequestPerFilterResult = 1,

        OneRequestPerTemplateResult = 2
    }
}