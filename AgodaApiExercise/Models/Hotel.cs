using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgodaApiExercise.Models
{
    public class Hotel
    {
        public string City { get; set; }
        public int HotelId { get; set; }
        public string Room { get; set; }
        public double Price { get; set; } 
    }
}