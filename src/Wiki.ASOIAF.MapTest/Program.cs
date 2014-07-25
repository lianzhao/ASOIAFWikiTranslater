using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiki.ASOIAF.MapTest
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Xml.Linq;

    using Wiki.Api;
    using Wiki.Api.Article;
    using Wiki.Api.Map;
    using Wiki.Api.Wikia.Article;
    using Wiki.Api.Wikia.Auth;
    using Wiki.Api.Wikia.Map;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MapTestAsync().Wait();
            }
            catch (Exception ex)
            {
            }
        }

        private static async Task MapTestAsync()
        {
            var mapId = 793;
            var mapService = new FormMapService();
            var articleService = new ApiArticleService();
            var points = await mapService.GetPointsAsync(mapId);
            var articles =
                await
                articleService.GetArticlesAsync(
                    abstractLength: 140,
                    articleTitles: points.Select(p => p.link_title).ToArray());

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler))
            {
                var authService =
                    new FormAuthenticationService(
                        loginUri: "http://zh.asoiaf.wikia.com/wiki/Special:%E7%94%A8%E6%88%B7%E7%99%BB%E5%BD%95",
                        client: client,
                        disposeClient: false);
                await authService.LoginAsync("jiaxingseng", "");
                cookieContainer.SetCookies(new Uri("http://zh.asoiaf.wikia.com"), "wikicitiesToken=6f2369033278234b141870c3d0deb4f7; expires=Wed, 21-Jan-2015 00:45:04 GMT;");
                cookieContainer.SetCookies(new Uri("http://zh.asoiaf.wikia.com"), "wikicitiesUserID=25218722; expires=Wed, 21-Jan-2015 00:45:04 GMT;");
                cookieContainer.SetCookies(new Uri("http://zh.asoiaf.wikia.com"), "wikicitiesUserName=Jiaxingseng; expires=Wed, 21-Jan-2015 00:45:04 GMT;");
                var mapService2 = new FormMapService(client: client, disposeClient: false);
                foreach (var point in points)
                {
                    var article = articles.FirstOrDefault(a => a.title == point.link_title);
                    if (article == null || string.IsNullOrEmpty(article._abstract))
                    {
                        continue;
                    }

                    if (point.description != null && point.description == article._abstract)
                    {
                        continue;
                    }

                    point.description = article._abstract;
                    await mapService2.PostPointAsync(point);
                }
            }
        }
    }
}
