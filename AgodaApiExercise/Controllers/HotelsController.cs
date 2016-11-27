using AgodaApiExercise.Core;
using AgodaApiExercise.Core.RateLimit;
using AgodaApiExercise.Models;
using AgodaApiExercise.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgodaApiExercise.Controllers
{
    [RoutePrefix("api/hotels")]
    public class HotelsController : ApiController
    {
        private IHotelRepository _hotelRepository;

        public HotelsController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        [HttpGet]
        [Route("city/{cityId}")]
        public IEnumerable<Hotel> GetHotelsByCity(string cityId, string pricesort = null)
        {
            if (pricesort == null)
            {
                return _hotelRepository.Get(cityId, null);
            }
            return _hotelRepository.Get(cityId, (SortOrder)Enum.Parse(typeof(SortOrder), pricesort.ToUpper()));
        }

    }
}
