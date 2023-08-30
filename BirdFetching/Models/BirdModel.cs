using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BirdFetching.Models
{
    public class BirdModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

       /* [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }*/
    }

    public class BirdApiResponse 
    {
        [JsonPropertyName("data")]
        public List<BirdModel> Data { get; set; }
    }

    public class SingleBirdResponse
    {
        [JsonPropertyName("data")]
        public BirdModel Data { get; set; }
    }
}
