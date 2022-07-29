using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Models
{
    public class Location
    {
        [FromQuery]
        [Required, Range(-90.0, 90.0)]        
        public double? Lat { get; set; }

        [FromQuery, Required, Range(-180.0, 180.0)] 
        public double? Lon { get; set; }

        public Location()
        {

        }
        public Location(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }
}