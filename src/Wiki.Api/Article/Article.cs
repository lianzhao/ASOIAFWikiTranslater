namespace Wiki.Api.Article
{
    public class Article
    {
        public int id { get; set; }
        public string title { get; set; }
        public int ns { get; set; }
        public string url { get; set; }
        public Revision revision { get; set; }
        public int comments { get; set; }
        public string type { get; set; }
        public string _abstract { get; set; }
        public string thumbnail { get; set; }
        public Original_Dimensions original_dimensions { get; set; }
    }
}