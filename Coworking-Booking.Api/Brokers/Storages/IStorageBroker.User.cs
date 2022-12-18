using Coworking_Booking.Api.Models.Users;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<User> InsertUserAsync(User user);
    }
}
