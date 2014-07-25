namespace Wiki.Api.Article
{
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<ArticleContent> GetContentAsync(int articleId);

        Task<Article> GetArticleAsync(string articleTitle, int abstractLength);
    }
}