using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using ModelsLayer;

namespace RepoLayer;

public interface ITicketRepo
{
    Ticket SubmitTicket(string guid, string reason, int amount, string description, int userId);
    Ticket GetTicket(string ticketId);
    Ticket UpdateTicket(string ticketId, int statusId);
    List<Ticket> GetTickets(int userId);
    List<Ticket> GetTickets(int userId, int statusId);
    List<Ticket> GetPending(int managerId);
}

public class TicketRepo : ITicketRepo
{
    const string AzureConnectionString = "Server=tcp:rolo-revature.database.windows.net,1433;Initial Catalog=P1_Database;Persist Security Info=False;User ID=Rolo;Password=AzureKey!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    public Ticket UpdateTicket(string ticketId, int statusId) {
        using(SqlConnection connection = new(AzureConnectionString))
        {
            string updateTicketQuery = "UPDATE [dbo].[Ticket] SET StatusId = @statusId WHERE TicketId = @ticketId";
            SqlCommand command = new(updateTicketQuery, connection);
            command.Parameters.AddWithValue("@statusId", statusId);
            command.Parameters.AddWithValue("@ticketId", ticketId);
            try {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected == 1) {
                    Console.WriteLine("Update Success");
                    return GetTicket(ticketId);
                } else {
                    return null!;
                }
            } catch(Exception e) {
                Console.WriteLine("Update Failure\n" + e.Message);
                return null!;
            }
        }
    }
    public Ticket SubmitTicket(string guide, string reason, int amount, string description, int userId)
    {
        using(SqlConnection connection = new(AzureConnectionString)) {
            string insertTicketQuery = "INSERT INTO [dbo].[Ticket] (TicketId, Reason, Amount, Description, StatusId, UserId) VALUES (@guide, @reason, @amount, @description, 0, @userId)";
            SqlCommand command = new(insertTicketQuery, connection);
            command.Parameters.AddWithValue("@guide", guide);
            command.Parameters.AddWithValue("@reason", reason);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@userId", userId);

            try {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected == 1) {
                    Console.WriteLine("Post Success");
                    return GetTicket(guide);
                } else {
                    return null!;
                }
            } catch(Exception e) {
                Console.WriteLine("Insertion Failure\n" + e.Message);
                return null!;
            }
        }
    }
    public Ticket GetTicket(string ticketId)
    {
        using(SqlConnection connection = new(AzureConnectionString)) {
            string queryTicketById = "SELECT * FROM [dbo].[Ticket] WHERE TicketId = @ticketId";
            SqlCommand command = new(queryTicketById, connection);
            command.Parameters.AddWithValue("@ticketId", ticketId);
            try {
                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null!;
                }
                else
                {
                    reader.Read();
                    return new Ticket(
                        (string)reader[0],
                        (string)reader[1],
                        (int)reader[2],
                        (string)reader[3],
                        (int)reader[4],
                        (int)reader[5]
                    );
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return null!;
            }
        }
    }
    public List<Ticket> GetTickets(int userId)
    {
        List<Ticket> userTickets = new();
        using(SqlConnection connection = new(AzureConnectionString)) {
            string queryAllUserTickets = "SELECT * FROM [dbo].[Ticket] WHERE UserId = @userId";
            SqlCommand command = new(queryAllUserTickets, connection);
            command.Parameters.AddWithValue("@userId", userId);

            try {
                connection.Open();

                using(SqlDataReader reader = command.ExecuteReader()) {
                    if(!reader.HasRows) return null!;
                    while(reader.Read()) {
                        Ticket newTicket = new Ticket(
                            (string) reader[0],
                            (string) reader[1],
                            (int) reader[2],
                            (string) reader[3],
                            (int) reader[4],
                            (int) reader[5]
                        );
                        userTickets.Add(newTicket);
                    }
                    Console.WriteLine("GET Success");
                    return userTickets;
                }
            } catch(Exception e) {
                Console.WriteLine("GET error\n" + e.Message);
                return null!;
            }
        }
    }
    public List<Ticket> GetTickets(int userId, int statusId)
    {
        List<Ticket> userTickets = new();
        using(SqlConnection connection = new(AzureConnectionString)) {
            string queryAllUserTickets = "SELECT * FROM [dbo].[Ticket] WHERE UserId = @userId AND StatusId = @statusId";
            SqlCommand command = new(queryAllUserTickets, connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@statusId", statusId);

            try {
                connection.Open();

                using(SqlDataReader reader = command.ExecuteReader()) {
                    if(!reader.HasRows) return null!;
                    while(reader.Read()) {
                        if((int)reader[4] == statusId) {
                            Ticket newTicket = new Ticket(
                                (string) reader[0],
                                (string) reader[1],
                                (int) reader[2],
                                (string) reader[3],
                                (int) reader[4],
                                (int) reader[5]
                            );
                            userTickets.Add(newTicket);
                        }
                    }
                    Console.WriteLine("GET Success");
                    return userTickets;
                }
            } catch(Exception e) {
                Console.WriteLine("GET error\n" + e.Message);
                return null!;
            }
        }
    }
    public List<Ticket> GetPending(int managerId)
    {
        List<Ticket> userTickets = new();
        using(SqlConnection connection = new(AzureConnectionString)) {
            string queryAllUserTickets = "SELECT * FROM [dbo].[Ticket] WHERE StatusId = @statusId";
            SqlCommand command = new(queryAllUserTickets, connection);
            command.Parameters.AddWithValue("@statusId", 0);

            try {
                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader()) {
                    if(!reader.HasRows) return null!;
                    while(reader.Read()) {
                        if((int)reader[4] == 0) {
                            Ticket newTicket = new Ticket(
                                (string) reader[0],
                                (string) reader[1],
                                (int) reader[2],
                                (string) reader[3],
                                (int) reader[4],
                                (int) reader[5]
                            );
                            userTickets.Add(newTicket);
                        }
                    }
                    Console.WriteLine("GET Success");
                    return userTickets;
                }
            } catch(Exception e) {
                Console.WriteLine("GET error\n" + e.Message);
                return null!;
            }
        }
    }
}