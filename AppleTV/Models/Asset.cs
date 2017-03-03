using AppleTV.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace AppleTV.Models
{
	public class Asset : ObservableObject
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("url")]
		public Uri URL
		{
			get { return _url; }
			set { SetValue(ref _url, value); }
		}
		private Uri _url;

		[JsonProperty("accessibilityLabel")]
		public string Location { get; set; }

		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public AssetType Type { get; set; }

		[JsonProperty("timeOfDay")]
		[JsonConverter(typeof(StringEnumConverter))]
		public TimeOfDay TimeOfDay { get; set; }
	}
}
