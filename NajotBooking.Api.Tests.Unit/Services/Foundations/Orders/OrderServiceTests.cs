// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using Moq;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Services.Foundations.Orders;
using Tynamix.ObjectFiller;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IOrderService orderService;

        public OrderServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.orderService = new OrderService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Order CreateRandomOrder() =>
            CreateOrderFiller().Create();

        private static Filler<Order> CreateOrderFiller()
        {
            var filler = new Filler<Order>();
            DateTimeOffset dates = GetRandomDateTime();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
