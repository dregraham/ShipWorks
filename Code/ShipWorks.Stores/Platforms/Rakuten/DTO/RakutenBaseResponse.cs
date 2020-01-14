using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
	/// <summary>
	/// Response returned by Rakuten when searching for orders
	/// </summary>
	[Obfuscation(Exclude = true)]
	public class RakutenBaseResponse
	{
		/// <summary>
		/// The list of errors encountered, if any
		/// </summary>
		[JsonProperty("errors")]
		public RakutenErrors Errors { get; set; }
	}
}
