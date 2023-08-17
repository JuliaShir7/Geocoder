using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace Geocoder
{
    public abstract class GeoCoder
    {
        protected abstract string CreateUri(GeoPoint point);
        
        public void GetData(string uri, out string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = "Other";
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            data = reader.ReadToEnd();
        }
        public dynamic FindResult(GeoPoint point)
        {
            string url = CreateUri(point);
            GetData(url, out string data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var obj = js.Deserialize<dynamic>(data);
            return obj;
        }
    }
    public class Yandex : GeoCoder
    { 
        protected override string CreateUri(GeoPoint point)
        {
            string apiKey = "your_api_key";
            string uri = "https://geocode-maps.yandex.ru/1.x/?apikey="+apiKey+"&format=json&geocode=";
            string enduri = "&results=1&lang=ru_RU";
            if (point.Latitude == 0 || point.Longitude == 0)
                uri += point.Country + "+" + point.City + "+" + point.Street + "+" + point.House + "+" + enduri;
            else
                uri +=  point.Longitude.ToString().Replace(',', '.')+"+"+point.Latitude.ToString().Replace(',', '.') +  enduri;
            return uri;
        }
        public void GetCoordinates(GeoPoint point, out double lon, out double lat)
        {
            try
            {
                var obj = FindResult(point);
                var data = obj["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"];
                string[] p = data.ToString().Split();
                lon = Convert.ToDouble(p[0].Replace('.', ','));
                lat = Convert.ToDouble(p[1].Replace('.', ','));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                lat = 0;
                lon = 0;
            }
        }
        public void GetCoordinates(GeoPoint point, out string address)
        {
            try
            {
                var obj = FindResult(point);
                address = obj["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["metaDataProperty"]["GeocoderMetaData"]["text"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                address = "";
            }
        } 
    }
    public class OpenStreetMap : GeoCoder
    {
        protected override string CreateUri(GeoPoint point)
        {
            string uri = "https://nominatim.openstreetmap.org/search/";
            string enduri = "&format=json&limit=1&lang=ru";
            if (point.Latitude == 0 || point.Longitude == 0)
            {
                uri = string.Format(
                    "https://nominatim.openstreetmap.org/search?country={0}&city={1}&street={2}&" + enduri,
                        point.Country, point.City, point.House + " " + point.Street);
            }
            else
            {
                uri = "https://nominatim.openstreetmap.org/reverse?lat=" + point.Latitude.ToString().Replace(',', '.') + "&lon=" + point.Longitude.ToString().Replace(',', '.') + enduri;
            }
            return uri;
        }
        public void GetCoordinates(GeoPoint point, out double lon, out double lat)
        {
            try
            {
                var obj = FindResult(point);
                lat = Convert.ToDouble(obj[0]["lat"].Replace('.', ','));
                lon = Convert.ToDouble(obj[0]["lon"].Replace('.', ','));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                lat = 0;
                lon = 0;
            }
        }
        public void GetCoordinates(GeoPoint point, out string address)
        {
            try
            {
                var obj = FindResult(point);
                address = obj["display_name"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                address = "";
            }
        }
    }
}
