using System;

namespace GoogleMapping.Models
{
    class Location
    {
        public long TimestampMs { get; set; } // e.g. 1374446256604
        public long LatitudeE7  { get; set; }  // e.g. 515015731
        public long LongitudeE7 { get; set; } // e.g. -78520
        public int  Accuracy    { get; set; }     // e.g. 753

        public double Latitude
        {
            get
            {
                return LatitudeE7 / 10000000D;
            }
        }

        public double Longitude
        {
            get
            {
                return LongitudeE7 / 10000000D;
            }
        }

        public override string ToString()
        {
            return String.Format("Long: {0}, Lat: {1}", Longitude, Latitude);
        }
    }
}
