namespace Wiki.Api.Map
{
    public class Layer
    {
        public int minZoom { get; set; }
        public int maxZoom { get; set; }
        public bool tms { get; set; }
        public bool noWrap { get; set; }
    }
}