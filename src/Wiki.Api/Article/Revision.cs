namespace Wiki.Api.Article
{
    public class Revision
    {
        public int id { get; set; }
        public string user { get; set; }
        public int user_id { get; set; }
        public string timestamp { get; set; }
    }
}