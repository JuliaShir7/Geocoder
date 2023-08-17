using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocoder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GeoPoint point1 = new GeoPoint("Россия", "Москва", "Ленина", 13);
            GeoPoint point2 = new GeoPoint("Россия", "Москва", "Тверская", 19);
            GeoPoint point3 = new GeoPoint(55.7655663, 37.6031899062193);
            OpenStreetMap gc = new OpenStreetMap();
            Yandex yandex = new Yandex();
            yandex.GetCoordinates(point2, out double lat, out double lon);
            Console.WriteLine(lon.ToString() + " , " + lat.ToString());
            yandex.GetCoordinates(point3, out string address);
            Console.WriteLine(address);
            gc.GetCoordinates(point2, out lat, out lon);
            Console.WriteLine(lon.ToString() + " , " + lat.ToString());
            gc.GetCoordinates(point3, out address);
            Console.WriteLine(address);
            Console.ReadLine();
        }
    }
}
