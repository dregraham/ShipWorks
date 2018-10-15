using System;
using System.ComponentModel;

/// <summary>
/// Generic view model for order lookup
/// </summary>
public interface IOrderLookupViewModel : INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Is the section expanded
    /// </summary>
    bool Expanded { get; set; }

    /// <summary>
    /// Title of the section
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Is the section visible
    /// </summary>
    bool Visible { get; }
}