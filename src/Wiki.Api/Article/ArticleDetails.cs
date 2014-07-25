namespace Wiki.Api.Article
{
    using System.Collections.Generic;

    public class ArticleDetails
    {
        public Dictionary<int, Article> items { get; set; }
        public string basepath { get; set; }
    }
}