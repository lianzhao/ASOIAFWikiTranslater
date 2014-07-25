namespace Wiki.Api.Wikia.Map
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Wiki.Api.Map;

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

        public async Task PostPointAsync(Point point)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("name", point.name);
            dict.Add("link_title", point.link_title);
            dict.Add("poi_category_id", point.poi_category_id.ToString(CultureInfo.InvariantCulture));
            dict.Add("description", point.description);
            dict.Add("id", point.id.ToString(CultureInfo.InvariantCulture));
            dict.Add("mapId", point.map_id.ToString(CultureInfo.InvariantCulture));
            dict.Add("lat", point.lat.ToString(CultureInfo.InvariantCulture));
            dict.Add("lon", point.lon.ToString(CultureInfo.InvariantCulture));
            dict.Add("imageUrl", point.photo);
            var response = await client.PostAsync(
                "http://zh.asoiaf.wikia.com/wikia.php?controller=WikiaInteractiveMapsPoi&method=editPoi&format=json",
                new FormUrlEncodedContent(dict));
            response.EnsureSuccessStatusCode();
        }
    }
}