

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Specifies the behavior when a token is selected in a <see cref="TemplateTokenTextBox"/>.
    /// </summary>
    public enum TokenSelectionMode
    {
        /// <summary>
        /// Replace the current value with the selected token.
        /// </summary>
        Replace,

        /// <summary>
        /// Paste the selected token into the current value.
        /// </summary>
        Paste
    }
}
