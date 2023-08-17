using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Geocoder
{
    public class GeoPoint
    {
        string country;
        string city;
        string street;
        int house;
        double latitude;
        double longitude;
        public string Country => country.Trim();
        public string City => city.Trim();
        public string Street => street.Trim();
        public int House => house;
        public double Latitude => latitude;
        public double Longitude => longitude;
        public GeoPoint(string country, string city, string street, int house) 
        { 
            this.country = country;
            this.city = city;
            this.street = street;
            this.house = house;
        }
        public GeoPoint(double latitude, double longitude) 
        { 
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public double GetDistanceTo(GeoPoint geopoint)
        {
            var distance = 0;
            return distance;
        }
    }
}
