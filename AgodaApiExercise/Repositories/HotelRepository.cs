using AgodaApiExercise.Core;
using AgodaApiExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgodaApiExercise.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private ICsvLoader _csvLoader;
        private string _dataSroucePath;

        public HotelRepository(ICsvLoader csvLoader, string dataPath = null)
        {
            _csvLoader = csvLoader;
            _dataSroucePath = dataPath ?? HttpContext.Current.Request.MapPath("~\\App_Data\\hotels.csv");
        }

        public IEnumerable<Hotel> Get(string cityId, SortOrder? priceSortOrder)
        {
            var hotelsLines = _csvLoader.ReadLines(_dataSroucePath);

            IEnumerable<Hotel> hotels =
            from hotelsLine in hotelsLines
            let hotelsLineItems = hotelsLine.Split(',')
            where hotelsLineItems[0] == cityId
            select new Hotel()
            {
                City = hotelsLineItems[0],
                HotelId = int.Parse(hotelsLineItems[1]),
                Room = hotelsLineItems[2],
                Price = double.Parse(hotelsLineItems[3])
            };

            if (priceSortOrder != null && priceSortOrder == SortOrder.DESC)
            {
                return hotels.ToList().OrderByDescending(hotel => hotel.Price);
            }
            else if (priceSortOrder != null && priceSortOrder == SortOrder.ASC)
            {
                return hotels.ToList().OrderBy(hotel => hotel.Price);
            }
            return hotels.ToList();
        }
    }
}