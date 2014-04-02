using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Kata
{
    public class Valet
    {
        internal static string ConnectionString = "Server=localhost;Database=Valet;Integrated Security=SSPI";

        // Table structure is
        //
        //CREATE TABLE [dbo].[ParkedCarInformation](
        //    [TicketNumber] [int] NOT NULL,
        //    [Make] [nvarchar](50) NOT NULL,
        //    [Model] [nvarchar](50) NOT NULL,
        //    [Color] [nvarchar](50) NOT NULL,
        //    [LotRow] [nvarchar](50) NULL,
        //    [LotColumn] [nvarchar](50) NULL
        //)

        public int RecordCarAccepted(string recordString)
        {
            string[] frags = recordString.Split(',');

            string make = frags[0].Trim();
            string model = frags[1].Trim();
            string color = frags[2].Trim();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();

                command.CommandText = "EXEC RecordCarAccepted @make, @model, @color";
                command.Parameters.Add("@make", System.Data.SqlDbType.NVarChar, make.Length).Value = make;
                command.Parameters.Add("@model", System.Data.SqlDbType.NVarChar, model.Length).Value = model;
                command.Parameters.Add("@color", System.Data.SqlDbType.NVarChar, color.Length).Value = color;

                return (int)command.ExecuteScalar();
            }
        }

        public void RecordCarParked(string recordString)
        {
            string[] frags = recordString.Split(',');

            int ticketNumber = int.Parse(frags[0].Trim());
            string lotRow = frags[1].Trim();
            string lotColumn = frags[2].Trim();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();

                command.CommandText = "EXEC RecordCarParked @ticketNumber, @lotRow, @lotColumn";
                command.Parameters.Add("@ticketNumber", System.Data.SqlDbType.Int).Value = ticketNumber;
                command.Parameters.Add("@lotRow", System.Data.SqlDbType.NVarChar, lotRow.Length).Value = lotRow;
                command.Parameters.Add("@lotColumn", System.Data.SqlDbType.NVarChar, lotColumn.Length).Value = lotColumn;

                command.ExecuteNonQuery();
            }
        }

        public string RetrieveParkedCarInformation(int ticketNumber)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();

                command.CommandText = "EXEC RetrieveParkedCarInformation @ticketNumber";
                command.Parameters.Add("@ticketNumber", System.Data.SqlDbType.Int).Value = ticketNumber;

                var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                string make = reader.GetString(0);
                string model = reader.GetString(1);
                string color = reader.GetString(2);
                string lotRow = reader.IsDBNull(3) ? "null" : reader.GetString(3);
                string lotColumn = reader.IsDBNull(4) ? "null" : reader.GetString(4);

                return String.Join(", ", make, model, color, lotRow, lotColumn);
            }
        }

        public void RecordCarReturned(int ticketNumber)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand command = conn.CreateCommand();

                command.CommandText = "DELETE FROM ParkedCarInformation WHERE TicketNumber = " + ticketNumber.ToString();

                command.ExecuteNonQuery();
            }
        }
    }
}
