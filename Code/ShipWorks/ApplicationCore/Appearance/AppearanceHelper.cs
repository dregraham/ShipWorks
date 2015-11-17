using System;
using System.Collections.Generic;
using System.Text;
using Divelements.SandRibbon.Rendering;
using System.Drawing;
using System.Reflection;
using ShipWorks.ApplicationCore.Appearance;
using TD.SandDock.Rendering;
using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;
using Interapptive.Shared;
using RibbonOffice2007ColorScheme = Divelements.SandRibbon.Rendering.Office2007ColorScheme;
using DockOffice2007ColorScheme = TD.SandDock.Rendering.Office2007ColorScheme;
using ShipWorks.Filters;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Provides utility functions used for display.
    /// </summary>
    static class AppearanceHelper
    {
        static ConstructorInfo sandRibbonRenderingContextConstructor;

        #region Divelements

        /// <summary>
        /// Create a new instance of a SandRibbon RibbonRenderer
        /// </summary>
        public static RibbonRenderer CreateRibbonRenderer()
        {
            RibbonRenderer renderer = new RibbonRenderer();

            // Apply color scheme
            switch (ShipWorksDisplay.ColorScheme)
            {
                case ColorScheme.Silver:
                    renderer.ColorScheme = RibbonOffice2007ColorScheme.Silver;
                    break;

                case ColorScheme.Black:
                    renderer.ColorScheme = RibbonOffice2007ColorScheme.Vista;
                    break;

                case ColorScheme.Blue:
                default:
                    renderer.ColorScheme = RibbonOffice2007ColorScheme.LunaBlue;
                    break;
            }

            return renderer;
        }

        /// <summary>
        /// Create a grid renderer appropriate for the given target
        /// </summary>
        public static Divelements.SandGrid.Rendering.Office2007Renderer CreateSandGridRenderer(FilterTarget target)
        {
            Divelements.SandGrid.Rendering.Office2007Renderer renderer = new Office2007ShipWorksRenderer();

            renderer.ColorScheme = AppearanceHelper.GetSandGridColorScheme();
            renderer.SelectionFocusedForegroundColor = Color.White;
            renderer.SelectionUnfocusedForegroundColor = Color.White;

            switch (target)
            {
                case FilterTarget.Customers:
                    renderer.SelectionFocusedBackgroundColor = Color.FromArgb(172, 147, 219);
                    renderer.SelectionUnfocusedBackgroundColor = Color.FromArgb(206, 199, 219);
                    break;

                case FilterTarget.Orders:
                    renderer.SelectionFocusedBackgroundColor = Color.FromArgb(100, 140, 231);
                    renderer.SelectionUnfocusedBackgroundColor = Color.FromArgb(190, 203, 231);
                    break;

                default:
                    throw new InvalidOperationException("Unhandled FilterTarget in CreateRenderer");
            }

            return renderer;
        }

        /// <summary>
        /// Get the color scheme corresponding to the user's selected scheme
        /// </summary>
        public static Divelements.SandGrid.Rendering.Office2007ColorScheme GetSandGridColorScheme()
        {
            switch (ShipWorksDisplay.ColorScheme)
            {
                case ColorScheme.Silver: return Divelements.SandGrid.Rendering.Office2007ColorScheme.Silver;
                case ColorScheme.Blue: return Divelements.SandGrid.Rendering.Office2007ColorScheme.Blue;
                case ColorScheme.Black: return Divelements.SandGrid.Rendering.Office2007ColorScheme.Black;
            }

            throw new InvalidOperationException("Unhandled user color scheme.");
        }

        /// <summary>
        /// Create a grid renderer appropriate for the given target
        /// </summary>
        public static Divelements.SandGrid.Rendering.WindowsXPRenderer CreateWindowsRenderer()
        {
            Divelements.SandGrid.Rendering.WindowsXPRenderer renderer = new WindowsXPShipWorksRenderer();

            return renderer;
        }

        /// <summary>
        /// Create a new instance of a SandRibbon RenderingContext
        /// </summary>
        [NDependIgnoreTooManyParams]
        public static RenderingContext CreateRibbonRenderingContext(Graphics g, RibbonRenderer renderer, Font font, bool showMnemonics, Color defaultTextColor, Color disabledTextColor)
        {
            // At the time this was written, the constructor was protected.
            ConstructorInfo constructor = GetRenderingContextConstructor();

            // Create the object using reflection
            RenderingContext context = (RenderingContext) constructor.Invoke(new object[] 
                { 
                    g, renderer, font, false, showMnemonics, defaultTextColor, disabledTextColor
                });

            return context;
        }

        /// <summary>
        /// Get the constructor for the RenderingContext class using reflection, since it was internal to the SandRibbon assembly.
        /// </summary>
        private static ConstructorInfo GetRenderingContextConstructor()
        {
            if (sandRibbonRenderingContextConstructor == null)
            {
                // When this was written, only one constructor
                sandRibbonRenderingContextConstructor = typeof(RenderingContext).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            }

            return sandRibbonRenderingContextConstructor;
        }


        /// <summary>
        /// Creates a new instance of a SandDock renderer based on the current color scheme.
        /// </summary>
        public static Office2007Renderer CreateDockRenderer()
        {
            Office2007Renderer dockRenderer = new Office2007Renderer();

            switch (ShipWorksDisplay.ColorScheme)
            {
                case ColorScheme.Silver:
                    dockRenderer.ColorScheme = DockOffice2007ColorScheme.Silver;
                    break;

                case ColorScheme.Black:
                    dockRenderer.ColorScheme = DockOffice2007ColorScheme.Black;
                    break;

                case ColorScheme.Blue:
                default:
                    dockRenderer.ColorScheme = DockOffice2007ColorScheme.Blue;

                    break;
            }

            dockRenderer.ActiveTitleBarBackground = dockRenderer.InactiveTitleBarBackground;

            return dockRenderer;
        }

        #endregion

        #region Krypton

        /// <summary>
        /// Create the palette to be used for all krypton controls
        /// </summary>
        public static PaletteModeManager GetKryptonPaletteMode()
        {
            switch (ShipWorksDisplay.ColorScheme)
            {
                case ColorScheme.Black:
                    return PaletteModeManager.Office2007Black;

                case ColorScheme.Silver:
                    return PaletteModeManager.Office2007Silver;

                case ColorScheme.Blue:
                default:
                    return PaletteModeManager.Office2007Blue;
            }
        }

        #endregion
    }
}
