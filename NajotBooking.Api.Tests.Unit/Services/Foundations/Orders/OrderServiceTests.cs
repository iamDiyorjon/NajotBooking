// ---------------------------------------------------------------
// Copyright (c) Coalition of the THE STANDART SHARPISTS
// Free To Use to Book Places in Coworking Zones
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using NajotBooking.Api.Brokers.Loggings;
using NajotBooking.Api.Brokers.Storages;
using NajotBooking.Api.Models.Orders;
using NajotBooking.Api.Services.Foundations.Orders;
using Tynamix.ObjectFiller;
using Xeptions;

namespace NajotBooking.Api.Tests.Unit.Services.Foundations.Orders
{
    public partial class OrderServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOrderService orderService;

        public OrderServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.orderService = new OrderService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<Exception, bool>> SameExpressionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static DateTimeOffset GetBeforeRandomDateTime(DateTimeOffset dates) =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch, latestDate: dates.UtcDateTime).GetValue();

        private static DateTimeOffset GetAfterRandomDateTime(DateTimeOffset dates) =>
            new DateTimeRange(earliestDate: dates.UtcDateTime,
                latestDate: dates.UtcDateTime.AddHours(GetRandomNumber())).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 1, max: 10000).GetValue();

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
