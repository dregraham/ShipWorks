﻿using System;
using System.Windows;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI
{
    /// <summary>
    /// Interface for WebBrowserFactory
    /// </summary>
    public interface IWebBrowserFactory
    {
        /// <summary>
        /// Creates an IDialog with the given URI and title
        /// </summary>
        IDialog Create(Uri uri, string title, IWin32Window owner, Size size);
    }
}