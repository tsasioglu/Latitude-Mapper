namespace GoogleMapping.Controllers
{
    public class MapDetails
    {
        public double MinLat          { get; set; }
        public double MaxLat          { get; set; }
        public double MinLong         { get; set; }
        public double MaxLong         { get; set; }
        public double CenterLongitude { get; set; }
        public double CenterLatitude  { get; set; }
        public int    ZoomLevel       { get; set; }
    }
}