namespace Wiki.Api.Map
{
    using System;

    public class Map
    {
        public int id { get; set; }
        public string title { get; set; }
        public string city_title { get; set; }
        public string city_url { get; set; }
        public int locked { get; set; }
        public DateTime updated_on { get; set; }
        public int tile_set_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int min_zoom { get; set; }
        public int max_zoom { get; set; }
        public int status { get; set; }
        public string attribution { get; set; }
        public string subdomains { get; set; }
        public Point[] points { get; set; }
        public Type[] types { get; set; }
        public int catchAllCategoryId { get; set; }
        public Layer layer { get; set; }
        public string imagesPath { get; set; }
        public int zoom { get; set; }
        public string gaUser { get; set; }
        public string pathTemplate { get; set; }
        public Boundaries boundaries { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int defaultZoomForRealMap { get; set; }
        public I18n i18n { get; set; }
        public string iframeSrc { get; set; }
    }
}