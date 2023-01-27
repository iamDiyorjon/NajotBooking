// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Coworking_Booking.Api.Models.Orders;

namespace Coworking_Booking.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public async ValueTask<Order> InsertOrderAsync(Order order) =>
            await InsertAsync(order);
    }
}
