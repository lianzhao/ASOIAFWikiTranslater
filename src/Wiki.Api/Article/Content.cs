namespace Wiki.Api.Article
{
    public class Content
    {
        public string type { get; set; }
        public string text { get; set; }
        public Element[] elements { get; set; }
    }
}