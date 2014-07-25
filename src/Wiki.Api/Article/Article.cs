namespace Wiki.Api.Article
{
    using Newtonsoft.Json;

    public class Article
    {
        public int id { get; set; }
        public string title { get; set; }
        public int ns { get; set; }
        public string url { get; set; }
        public Revision revision { get; set; }
        public int comments { get; set; }
        public string type { get; set; }

        [JsonProperty(PropertyName = "abstract")]
        public string _abstract { get; set; }
        public string thumbnail { get; set; }
        public Original_Dimensions original_dimensions { get; set; }
    }
}