namespace Wiki.Api.Article
{
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class WikiaApiArticleService : WebServiceBase, IArticleService
    {
        private const string BaseUri = "http://zh.asoiaf.wikia.com/api/v1/Articles";

        public WikiaApiArticleService()
        {
        }

        public WikiaApiArticleService(HttpClient client, bool disposeClient)
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
            var uri = string.Format("{0}/Details?titles={1}", BaseUri, articleTitle);
            if (abstractLength >= 0)
            {
                uri = string.Format("{0}&abstract={1}", uri, abstractLength.ToString(CultureInfo.InvariantCulture));
            }

            var response = await this.client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<ArticleDetails>(json);
            return root.items.Values.FirstOrDefault();
        }
    }
}