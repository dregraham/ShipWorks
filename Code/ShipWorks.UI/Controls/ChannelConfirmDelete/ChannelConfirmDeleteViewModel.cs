using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelConfirmDeleteViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IStoreManager storeManager;
        private string message;
        private string intro;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storeType"></param>
        public ChannelConfirmDeleteViewModel(IStoreManager storeManager)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.storeManager = storeManager;
        }

        public void Load(StoreTypeCode storeType)
        {

        }
        
        /// <summary>
        /// The intro message to display to the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Intro
        {
            get { return intro; }
            set { handler.Set(nameof(Intro), ref intro, value); }
        }
        
        /// <summary>
        /// The delete message to display to the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message
        {
            get { return message; }
            set { handler.Set(nameof(Message), ref message, value); }
        }
    }
}
