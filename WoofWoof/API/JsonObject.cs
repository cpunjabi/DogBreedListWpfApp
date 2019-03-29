using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoofWoof.Models
{
	public class ListJsonObject
	{
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }

		[JsonProperty(PropertyName = "message")]
		public List<string> Breeds { get; set; }
	}

	public class ImageJsonObject
	{
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }

		[JsonProperty(PropertyName = "message")]
		public string ImageUrl { get; set; }
	}
}