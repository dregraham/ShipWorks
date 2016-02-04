using System;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.UI.AttachedProperties
{
    /// <summary>
    /// Allows binding a source URL to a WebBrowser
    /// </summary>
    /// <remarks>
    /// Code inspired by http://stackoverflow.com/questions/263551/databind-the-source-property-of-the-webbrowser-in-wpf
    /// </remarks>
    public static class WebBrowserUtility
    {
        public static readonly DependencyProperty BindableSourceProperty;

        /// <summary>
        /// Initializes the <see cref="WebBrowserUtility"/> class.
        /// </summary>
        static WebBrowserUtility()
        {
            BindableSourceProperty = DependencyProperty.RegisterAttached(
                "BindableSource",
                typeof (Uri),
                typeof (WebBrowserUtility),
                new UIPropertyMetadata(null, BindableSourcePropertyChanged));
        }

        /// <summary>
        /// Gets the bindable source.
        /// </summary>
        public static Uri GetBindableSource(DependencyObject obj)
        {
            return (Uri)obj.GetValue(BindableSourceProperty);
        }

        /// <summary>
        /// Sets the bindable source.
        /// </summary>
        public static void SetBindableSource(DependencyObject obj, Uri value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        /// <summary>
        /// Bindables the source property changed.
        /// </summary>
        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                browser.Source = e.NewValue as Uri;
            }
        }

    }
}
