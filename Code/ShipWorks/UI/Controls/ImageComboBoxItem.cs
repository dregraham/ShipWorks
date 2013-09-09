using System;
using System.Drawing;
using System.Reflection;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Item for use with the ImageComboBox
    /// </summary>
	public class ImageComboBoxItem
	{
		// Transparent means to inherit the background of the ComboBox
		Color forecolor = Color.FromKnownColor(KnownColor.Transparent);

        string text;
        object value;

        Image image;
        bool bold;

        bool selectable = true;
        bool closeOnSelect = true;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public ImageComboBoxItem()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
		public ImageComboBoxItem(string text) 
		{
            this.text = text;
		}

        /// <summary>
        /// Constructor
        /// </summary>
		public ImageComboBoxItem(string text, Image image)
		{
            this.text = text;
            this.image = image;
		}

        /// <summary>
        /// Constructor
        /// </summary>
		public ImageComboBoxItem(string text, object value, Image image)
		{
            this.text = text;
            this.value = value;
            this.image = image;
		}

        /// <summary>
        /// The item text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

		/// <summary>
		/// The foreground color to use when drawing the text.
		/// </summary>
		public Color ForeColor 
		{
			get 
			{
				return forecolor;
			}
			set
			{
				forecolor = value;
			}
		}

		/// <summary>
		/// The image to be drawn next to the item.  Can be null.
		/// </summary>
		public Image Image 
		{
			get 
			{
				return image;
			}
			set 
			{
                image = value;
			}
		}

		/// <summary>
		/// Indicates if the item text should be drawn in bold.
		/// </summary>
		public bool Bold
		{
			get
			{
				return bold;
			}
			set
			{
                bold = value;
			}
		}

		/// <summary>
		/// A user-controlled value to associate with the item.
		/// </summary>
        [Obfuscation(Exclude = true)]
        public object Value
		{
			get
			{
				return value;
			}
			set
			{
                this.value = value;
			}
		}

        /// <summary>
        /// Indicates if the item can be selected
        /// </summary>
        public bool Selectable
        {
            get
            {
                return selectable;
            }
            set
            {
                selectable = value;
            }
        }

        /// <summary>
        /// Indicates if the DropDown list automatically closes when this item is selected
        /// </summary>
        public bool CloseOnSelect
        {
            get
            {
                return closeOnSelect;
            }
            set
            {
                closeOnSelect = value;
            }
        }
		
		/// <summary>
		/// String representation of the item.
		/// </summary>
		public override string ToString() 
		{
			return Text;
		}
	}
}
