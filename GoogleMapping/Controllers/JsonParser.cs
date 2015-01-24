using System;
using System.Linq;
using GoogleMapping.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Net;
using System.IO;

namespace GoogleMapping.Controllers
{
    public class JsonParser
    {
        private const string GoogleApiKey = "<GOOGLE_API_KEY>";

        public static Image Parse(string locationData, MapDetails map)
        {
            LocationsJson locations = JsonConvert.DeserializeObject<LocationsJson>(locationData);
            if (!locations.Locations.Any())
                throw new ImageProcessingException("No location data found");
            
            Image image = GetImage(map.CenterLongitude, map.CenterLatitude, map.ZoomLevel);
            Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;                       

            int width  = image.Width;
            int height = image.Height;

            Pen pen = new Pen(Color.Red, 2F)
            {
                MiterLimit = 2F,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel,
            };
            
            Location previousLocation = locations.Locations[0];
            foreach (Location location in locations.Locations)
            {
                var x1 = Scale(previousLocation.Longitude, map.MinLong, map.MaxLong, width);
                var y1 = Scale(previousLocation.Latitude, map.MinLat, map.MaxLat, height);

                var x2 = Scale(location.Longitude, map.MinLong, map.MaxLong, width);
                var y2 = Scale(location.Latitude, map.MinLat, map.MaxLat, height);

                if (EuclideanDistance(x1, y1, x2, y2) <= 200)
                    graphics.DrawLine(pen, x1, height - y1, x2, height - y2);                    
                                
                previousLocation = location;
            }

            graphics.Dispose();
            return image;
        }

        private static Image GetImage(double longitude, double latitude, int zoom)
        {
            var url = String.Format("http://maps.googleapis.com/maps/api/staticmap?&size=640x640&key={0}&scale=2&format=jpg&center={1},{2}&zoom={3}", GoogleApiKey, latitude, longitude, zoom);
            
            byte[] map;
            using (WebClient wc = new WebClient())
            {
                map = wc.DownloadData(url);                
            }

            using (MemoryStream ms = new MemoryStream(map)) 
            {
                return Image.FromStream(ms);
            }
        }

        // For debugging        
        private static void PrintInfo(Location location, int x1, int y1, int x2, int y2, double euclidDist)
        {
            Console.WriteLine("Processing location {0}", location);
            Console.WriteLine("Points generated x1: {0}, y1: {1}, x2: {2}, y2: {3}. Euclid: {4}", x1, y1, x2, y2, euclidDist);
            Console.ReadLine();
        }

        private static int Scale(double latOrLong, double minLatOrLong, double maxLatOrLong, int heightOrWidth) 
        {
            if (latOrLong < minLatOrLong || latOrLong > maxLatOrLong)
                return 0;
            
            double scaleFactor = (latOrLong - minLatOrLong) / (maxLatOrLong - minLatOrLong);
            return  Convert.ToInt32(scaleFactor * heightOrWidth);
        }

        private static double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    
    }
}