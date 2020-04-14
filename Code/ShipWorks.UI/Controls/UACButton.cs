using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Interapptive.Shared.Win32;
using Image = System.Windows.Controls.Image;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Button that places the UAC shield to the left of the content
    /// </summary>
    public class UACButton : Button
    {
        public static readonly DependencyProperty ShowShieldProperty =
            DependencyProperty.Register("ShowShield", typeof(bool), typeof(UACButton),
                                        new PropertyMetadata(default(bool)));

        /// <summary>
        /// Whether or not the UAC Shield should show
        /// </summary>
        public bool ShowShield
        {
            get => (bool) GetValue(ShowShieldProperty);
            set => SetValue(ShowShieldProperty, value);
        }

        /// <summary>
        /// When the template is applied, get the uac icon and add it
        /// </summary>
        public override void OnApplyTemplate()
        {
            Image image = (Image) Template.FindName("icon", this);
            if (image != null)
            {
                try
                {
                    image.Source = GetUACShieldIcon();
                }
                catch (Exception)
                {
                    // Just eat the exception
                }
            }
        }

        /// <summary>
        /// Get the UAC Shield icon from windows.
        /// </summary>
        /// <remarks>
        /// We're doing this instead of just adding a png or svg because this icon is different between versions of
        /// windows. We also can't use the native method SetShield because it does not work with WPF.
        /// </remarks>
        private BitmapSource GetUACShieldIcon()
        {
            NativeMethods.SHSTOCKICONINFO iconInfo = new NativeMethods.SHSTOCKICONINFO();
            iconInfo.cbSize = (uint) Marshal.SizeOf<NativeMethods.SHSTOCKICONINFO>();

            Marshal.ThrowExceptionForHR(NativeMethods.SHGetStockIconInfo(
                                            NativeMethods.SHSTOCKICONID.SIID_SHIELD,
                                            NativeMethods.SHGSI.SHGSI_SMALLICON | NativeMethods.SHGSI.SHGSI_ICON,
                                            ref iconInfo));

            IntPtr iconHandle = iconInfo.hIcon != IntPtr.Zero ? iconInfo.hIcon : SystemIcons.Shield.Handle;

            return Imaging.CreateBitmapSourceFromHIcon(iconHandle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
