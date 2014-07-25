namespace Wiki.Api.Map
{
    using System;

    public class Point
    {
        public int id { get; set; }
        public string name { get; set; }
        public int poi_category_id { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string link_title { get; set; }
        public string photo { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public DateTime? created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? updated_on { get; set; }
        public string updated_by { get; set; }
        public int map_id { get; set; }
    }
}