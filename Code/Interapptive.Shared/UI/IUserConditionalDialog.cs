namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Dialog that can be dismissed
    /// </summary>
    public interface IUserConditionalDialog : IDialog
    {
        /// <summary>
        /// Should the dialog be hidden from now on
        /// </summary>
        bool DoNotShowAgain { get; }
    }
}
