namespace Wiki.Api.Wikia.Article
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Wiki.Api.Article;

    public class ApiArticleService : WebServiceBase, IArticleService
    {
        private const string BaseUri = "http://zh.asoiaf.wikia.com/api/v1/Articles";

        private const int MaxQueryArticleTitles = 10;

        public ApiArticleService()
        {
        }

        public ApiArticleService(HttpClient client, bool disposeClient)
            : base(client, disposeClient)
        {
        }

        public async Task<ArticleContent> GetContentAsync(int articleId)
        {
            var uri = string.Format("{0}/AsSimpleJson?id={1}", BaseUri, articleId.ToString(CultureInfo.InvariantCulture));
            var response = await this.client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<ArticleContent>(json);
            return root;
        }

        public async Task<Article> GetArticleAsync(string articleTitle, int abstractLength)
        {
            var articles = await this.GetArticlesAsync(abstractLength, articleTitle);
            return articles.FirstOrDefault();
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(int abstractLength, params string[] articleTitles)
        {
            if (articleTitles.Length > MaxQueryArticleTitles)
            {
                var groups =
                    articleTitles.Select((title, i) => new { Title = title, Index = i })
                        .GroupBy(x => x.Index / MaxQueryArticleTitles, x => x.Title);
                var rv = Enumerable.Empty<Article>();
                foreach (var @group in groups)
                {
                    rv = rv.Concat(await this.GetArticlesAsync(abstractLength, group.ToArray()));
                }

                return rv;
            }

            var uri = string.Format("{0}/Details?titles={1}", BaseUri, string.Join(",", articleTitles));
            if (abstractLength >= 0)
            {
                uri = string.Format("{0}&abstract={1}", uri, abstractLength.ToString(CultureInfo.InvariantCulture));
            }
            var response = await this.client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<ArticleDetails>(json);
            return root.items.Values;
        }
    }
}