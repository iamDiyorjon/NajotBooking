using NajotBooking.Api.Models.Seats;
using System.Linq;

namespace NajotBooking.Api.Services.Foundations.Seats
{
    public partial class SeatService
    {
        private IQueryable<Seat> GetEmptySeats()
        {
            IQueryable<Seat> listOfData = storageBroker.SelectAllSeats()
                .OrderBy(u => u.IsBooked ? 0 : 1);

            return listOfData;
        }
    }
}
