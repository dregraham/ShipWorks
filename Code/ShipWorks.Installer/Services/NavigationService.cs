using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShipWorks.Installer.Services
{
    public class NavigationService<T> : INavigationService<T>, INotifyPropertyChanged where T : Enum
    {
        /// <summary>
        /// The xaml x:Name used by the frame the NavigationService will use for pages.
        /// </summary>
        private readonly string frameName;

        /// <summary>
        /// Stores previous page history
        /// </summary>
        private readonly List<T> historic;

        /// <summary>
        /// Stores the current pages
        /// </summary>
        private readonly Dictionary<T, Uri> pagesByKey;

        /// <summary>
        /// Backing variable for <see cref="CurrentPageKey"/>
        /// </summary>
        private T currentPageKey;

        /// <summary>
        /// Controls navigation between the pages of the setup wizard
        /// </summary>
        /// <param name="frameName"></param>
        public NavigationService(string frameName = "MainFrame")
        {
            pagesByKey = new Dictionary<T, Uri>();
            historic = new List<T>();
            this.frameName = frameName;

            ConfigurePages();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Contains the key to the current page. Uses INPC.
        /// </summary>
        public T CurrentPageKey
        {
            get { return currentPageKey; }
            private set
            {
                if (!EqualityComparer<T>.Default.Equals(currentPageKey, value))
                {
                    currentPageKey = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Page Parameter
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// Add or alter a page by providing the name of the page as a string and path.
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="pagePath">
        /// Assumes Views are in ../Views folder and follow MVVM Naming convention Name.xaml when null.
        /// </param>
        public void ConfigurePage(T pageKey)
        {
            var pagePath = new Uri(String.Join("", new string[] { "../Views/", Enum.GetName(typeof(T), pageKey), ".xaml" }), UriKind.Relative);

            lock (pagesByKey)
            {
                if (pagesByKey.ContainsKey(pageKey))
                {
                    pagesByKey[pageKey] = pagePath;
                }
                else
                {
                    pagesByKey.Add(pageKey, pagePath);
                }
            }
        }

        /// <summary>
        /// Add or alter a page providing only the enum as T. Assumes Views are in ../Views folder
        /// and follow MVVM Naming convention Name.xaml
        /// </summary>
        public void ConfigurePages()
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                ConfigurePage((T)value);
            }
        }

        /// <summary>
        /// Add or alter a page
        /// </summary>
        public void GoBack()
        {
            if (historic.Count > 1)
            {
                historic.RemoveAt(historic.Count - 1);
                //Stash the new last page then remove it since
                //Navigate to will add it again so we don't get duplicate
                //pages in historic
                var lastPage = historic.Last();
                historic.RemoveAt(historic.Count - 1);
                NavigateTo(lastPage);
            }
        }

        /// <summary>
        /// Navigate to a page defined by enum
        /// </summary>
        /// <param name="navigationPage"></param>
        public void NavigateTo(T navigationPage)
        {
            NavigateTo(navigationPage, null);
        }
 
        /// <summary>
        /// Navigate to page using page name, passing a parameter
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        public void NavigateTo(T pageKey, object parameter)
        {
            if (!EqualityComparer<T>.Default.Equals(CurrentPageKey, pageKey))
            {
                lock (pagesByKey)
                {
                    if (!pagesByKey.ContainsKey(pageKey))
                    {
                        throw new ArgumentException(string.Format("No such page: {0} ", pageKey), nameof(pageKey));
                    }
                    var frame = GetDescendantFromName(Application.Current.MainWindow, frameName) as Frame;

                    if (frame != null)
                    {
                        frame.Source = pagesByKey[pageKey];
                    }
                    Parameter = parameter;
                    historic.Add(pageKey);
                    CurrentPageKey = pageKey;
                }

                OnNavigated(new NavigatedEventArgs<T>(CurrentPageKey));
            }
        }

        /// <summary>
        /// Event that's triggered on navigation
        /// </summary>
        public event EventHandler<NavigatedEventArgs<T>> Navigated;

        /// <summary>
        /// Invoke the navigated event
        /// </summary>
        private void OnNavigated(NavigatedEventArgs<T> e)
        {
            EventHandler<NavigatedEventArgs<T>> handler = Navigated;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the correct Frame
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// INPC
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Event args for the Navigated event
    /// </summary>
    public class NavigatedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NavigatedEventArgs(T page)
        {
            NavigatedPage = page;
        }

        /// <summary>
        /// The page we navigated to
        /// </summary>
        public T NavigatedPage { get; }
    }
}