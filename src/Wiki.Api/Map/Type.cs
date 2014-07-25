namespace Wiki.Api.Map
{
    public class Type
    {
        public int id { get; set; }
        public int parent_poi_category_id { get; set; }
        public int map_id { get; set; }
        public string name { get; set; }
        public object marker { get; set; }
        public int status { get; set; }
    }
}