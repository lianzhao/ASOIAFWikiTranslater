namespace Wiki.Api.Article
{
    public class Section
    {
        public string title { get; set; }
        public int level { get; set; }
        public Content[] content { get; set; }
        public object[] images { get; set; }
    }
}