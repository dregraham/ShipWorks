namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Convert weight to and from a string
    /// </summary>
    public interface IWeightConverter
    {
        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        string FormatWeight(double weight);

        /// <summary>
        /// Format the given weight based on the specified display format.
        /// </summary>
        string FormatWeight(double weight, WeightDisplayFormat defaultDisplayFormat);

        /// <summary>
        /// Convert from weight String (lbs and oz) to Double
        /// </summary>
        double? ParseWeight(string value);
    }
}