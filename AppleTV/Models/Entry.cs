using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppleTV.Models
{
	public class Entry : ObservableObject
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("assets")]
		public IReadOnlyCollection<Asset> Assets { get; set; }
	}
}
