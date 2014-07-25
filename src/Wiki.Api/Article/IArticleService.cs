namespace Wiki.Api.Article
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<ArticleContent> GetContentAsync(int articleId);

        Task<Article> GetArticleAsync(string articleTitle, int abstractLength);

        Task<IEnumerable<Article>> GetArticlesAsync(int abstractLength, params string[] articleTitles);
    }
}