using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    public class JetStoreSetupControlViewModel : IJetStoreSetupControlViewModel, INotifyPropertyChanged
    {
        private string apiUser;
        private string secret;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        public JetStoreSetupControlViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets or sets the Api User
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ApiUser
        {
            get { return apiUser; }
            set { handler.Set(nameof(ApiUser), ref apiUser, value); }
        }

        /// <summary>
        /// Gets or sets the Secret
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Secret
        {
            get { return secret; }
            set { handler.Set(nameof(Secret), ref secret, value); }
        }

        public void Load(JetStoreEntity store)
        {
            throw new System.NotImplementedException();
        }

        public bool Save(JetStoreEntity store)
        {
            throw new System.NotImplementedException();
        }
    }
}