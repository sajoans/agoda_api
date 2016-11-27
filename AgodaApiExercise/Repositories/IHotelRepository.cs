using AgodaApiExercise.Core;
using AgodaApiExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgodaApiExercise.Repositories
{
    public interface IHotelRepository
    {
        IEnumerable<Hotel> Get(string cityId, SortOrder? priceSortOrder);
    }
}
