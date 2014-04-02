using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kata;
using System.Data.SqlClient;
using Xunit;
using Xunit.Extensions;

namespace ValetTests
{
    public class ValetTests
    {
        public ValetTests()
        {
            using (SqlConnection conn = new SqlConnection(Valet.ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();

                command.CommandText = "EXEC ClearTheLot";

                command.ExecuteNonQuery();
            }
        }

        [Fact]
        public void A_car_can_be_accepted()
        {
            ANewValet().RecordCarAccepted(ANewCarRecord());
        }

        [Fact]
        public void When_a_car_is_accepted_a_ticket_number_is_generated()
        {
            int ticketNumber = ANewValet().RecordCarAccepted(ANewCarRecord());
        }

        [Fact]
        public void Each_accepted_car_has_a_different_ticket_number()
        {
            Valet valet = ANewValet();
            int ticketNumber1 = valet.RecordCarAccepted(ANewCarRecord());
            int ticketNumber2 = valet.RecordCarAccepted(ANewCarRecord());

            ticketNumber1.Should().NotBe(ticketNumber2);
        }

        [Fact]
        public void A_car_parked_record_can_be_entered()
        {
            Valet valet = ANewValet();

            int ticketNumber = valet.RecordCarAccepted(ANewCarRecord());

            valet.RecordCarParked(
                String.Join(", ", ticketNumber.ToString(), "A", "1"));
        }

        [Theory]
        [InlineData("Honda, Civic, Blue", "Honda, Civic, Blue, null, null")]
        [InlineData("Honda, Civic, Green", "Honda, Civic, Green, null, null")]
        public void When_a_ticket_number_is_entered_the_car_information_is_returned(string carRecord, string expectedRecord)
        {
            Valet valet = ANewValet();

            int ticketNumber = valet.RecordCarAccepted(carRecord);

            valet.RetrieveParkedCarInformation(ticketNumber).ShouldBeEquivalentTo(expectedRecord);
        }

        [Fact]
        public void A_car_can_be_returned()
        {
            Valet valet = ANewValet();

            int ticketNumber = valet.RecordCarAccepted(ANewCarRecord());

            valet.RecordCarReturned(ticketNumber);
        }

        static Valet ANewValet()
        {
            return new Valet();
        }

        static string ANewCarRecord()
        {
            return "Honda, Civic, Blue";
        }
    }
}
