namespace Wiki.Api.Map
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class FormMapService : WebServiceBase, IMapService
    {
        public FormMapService()
        {
        }

        public FormMapService(HttpClient client, bool disposeClient)
            : base(client, disposeClient)
        {
        }

        public async Task<IEnumerable<Point>> GetPointsAsync(int mapId)
        {
            var baseUri = "http://maps.wikia-services.com/api/v1/render";
            var uri = string.Format("{0}/{1}", baseUri, mapId.ToString(CultureInfo.InvariantCulture));
            var response = await this.client.GetAsync(uri);
            var html = await response.Content.ReadAsStringAsync();
            var start = html.IndexOf("window.mapSetup");
            var tmp = html.Substring(start, html.Length - start);
            var end = tmp.IndexOf("</script>");
            tmp = tmp.Substring(0, end);
            tmp = tmp.Split('=')[1].Trim();
            var json = tmp.Substring(0, tmp.Length - 1);
            var map = JsonConvert.DeserializeObject<Api.Map.Map>(json);

            return map.points;
        }
    }
}