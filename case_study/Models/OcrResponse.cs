using System.Text.Json.Serialization;

namespace case_study
{
    public class OcrResponse
    {

        [JsonPropertyName("cord")]
        public string Cord { get; set; }

        [JsonPropertyName("desc")]
        public string Desc { get; set; }
    }
}
