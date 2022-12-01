using ShipWorks.Stores.Platforms.Etsy;

namespace ShipWorks.Data.Model.EntityClasses
{
	/// <summary>
	/// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
	/// </summary>
	public partial class EtsyStoreEntity : IEtsyCredentials
	{
		/// <summary>
		/// Amazon auth token
		/// </summary>
		string IEtsyCredentials.AuthToken
		{
			get { return AuthToken; }
			set
			{
				// No setter needed for this implementation
			}
		}

		/// <summary>
		/// Amazon merchant ID
		/// </summary>
		string IEtsyCredentials.MerchantID
		{
			get { return MerchantID; }
			set
			{
				// No setter needed for this implementation
			}
		}

	}
}