using Coworking_Booking.Api.Models.Users;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coworking_Booking.Api.Services.Foundations
{
    public interface IUserService
    {
        ValueTask<User> AddUserAsync(User user);
        ValueTask<User> RetrieveUserByIdAsync(Guid userId);
        IQueryable<User> RetrieveAllUsers();
        ValueTask<User> ModifyUserAsync(User user);
        ValueTask<User> RemoveUser(User user);
    }
}
