﻿using System;
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
        private readonly List<string> historic;

        /// <summary>
        /// Stores the current pages
        /// </summary>
        private readonly Dictionary<string, Uri> pagesByKey;

        /// <summary>
        /// Backing variable for <see cref="CurrentPageKey"/>
        /// </summary>
        private string currentPageKey;

        /// <summary>
        /// Controls navigation between the pages of the setup wizard
        /// </summary>
        /// <param name="frameName"></param>
        public NavigationService(string frameName = "MainFrame")
        {
            pagesByKey = new Dictionary<string, Uri>();
            historic = new List<string>();
            this.frameName = frameName;

            ConfigurePages();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Contains the key to the current page. Uses INPC.
        /// </summary>
        public string CurrentPageKey
        {
            get { return currentPageKey; }
            private set
            {
                if (currentPageKey != value)
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
        public void ConfigurePage(string pageKey)
        {
            var pagePath = new Uri(String.Join("", new string[] { "../Views/", pageKey, ".xaml" }), UriKind.Relative);

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
            foreach (var value in Enum.GetNames(typeof(T)))
            {
                ConfigurePage(value);
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
                NavigateTo(historic.Last(), null);
            }
        }

        /// <summary>
        /// Navigate to a page defined by enum
        /// </summary>
        /// <param name="navigationPage"></param>
        public void NavigateTo(T navigationPage)
        {
            NavigateTo(navigationPage.ToString(), null);
        }

        /// <summary>
        /// Navigate to page using page name
        /// </summary>
        /// <param name="pageKey"></param>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        /// Navigate to page using page name, passing a parameter
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="parameter"></param>
        public void NavigateTo(string pageKey, object parameter)
        {
            if (pageKey != CurrentPageKey)
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

                OnNavigated(new NavigatedEventArgs(CurrentPageKey));
            }
        }

        /// <summary>
        /// Event that's triggered on navigation
        /// </summary>
        public event EventHandler<NavigatedEventArgs> Navigated;

        /// <summary>
        /// Invoke the navigated event
        /// </summary>
        private void OnNavigated(NavigatedEventArgs e)
        {
            EventHandler<NavigatedEventArgs> handler = Navigated;
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
    public class NavigatedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NavigatedEventArgs(string page)
        {
            NavigatedPage = page;
        }

        /// <summary>
        /// The page we navigated to
        /// </summary>
        public string NavigatedPage { get; }
    }
}