using System;
using System.ComponentModel;

/// <summary>
/// Generic view model for order lookup
/// </summary>
public interface IOrderLookupViewModel : INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Title of the section
    /// </summary>
    string Title { get; }
}