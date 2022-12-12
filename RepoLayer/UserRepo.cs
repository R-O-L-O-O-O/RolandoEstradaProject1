//This file contains functionality for the database
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using ModelsLayer;
using Microsoft.Extensions.Configuration;

namespace RepoLayer;

public interface IUserRepo
{
    List<User> GetUsers();
    void RegisterUsers(List<User> userDb);
    User UpdateUser(int id, int roleId);
    User UpdateUser(int id, string info);
    User RegisterUser(string email, string password);
    User RegisterUser(string email, string password, int roleid);
    User GetUser(string email);
    User GetUser(int id);
    User LoginUser(string email, string password);
}

public class UserRepo : IUserRepo
{
    const string AzureConnectionString = "Server=tcp:rolo-revature.database.windows.net,1433;Initial Catalog=P1_Database;Persist Security Info=False;User ID=Rolo;Password=AzureKey!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    public List<User> GetUsers()
    {
        List<User> users = new();
            try
            {
                using (SqlConnection connection = new(AzureConnectionString))
                {
                    string getUsersQuery = "SELECT * FROM [dbo].[User]";

                    using (SqlCommand command = new(getUsersQuery, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User(reader.GetInt32(0),reader.GetInt32(1),reader.GetString(2),reader.GetString(3)));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        return users;
    }

    public void RegisterUsers(List<User> userDb)
    {
        string serializedDb = JsonSerializer.Serialize(userDb);
        File.WriteAllText("appsettings.Development.json", serializedDb);
    }

    #region//Put Methods: update role, email, or password
    public User UpdateUser(int id, int roleId)
    {
        using(SqlConnection connection = new(AzureConnectionString))
        {
            string updateUserQuery = "UPDATE [dbo].[User] SET RoleId = @RoleId WHERE Id = @Id";
            SqlCommand command = new(updateUserQuery, connection);
            command.Parameters.AddWithValue("@RoleId", roleId);
            command.Parameters.AddWithValue("@Id", id);

            try{
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected == 1)
                {
                    Console.WriteLine("Update Success");
                    return GetUser(id);
                } else {
                    Console.WriteLine("Update Failure");
                    return null!;
                }
            } catch(Exception e) {
                Console.WriteLine("Update Failure\n" + e.Message);
            }
        }
        return null!;
    }

    public User UpdateUser(int id, string info)
    {
        string regex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        using(SqlConnection connection = new(AzureConnectionString))
        {
            string updateUserQuery;
            if(System.Text.RegularExpressions.Regex.Match(info, regex).Success) {
                // info is an email
                updateUserQuery = "UPDATE [dbo].[User] SET Email = @info WHERE Id = @Id";
            } else {
                // info is a password
                updateUserQuery = "UPDATE [dbo].[User] SET Password = @info WHERE Id = @Id";
            }
            SqlCommand command = new SqlCommand(updateUserQuery, connection);
            command.Parameters.AddWithValue("@info", info);
            command.Parameters.AddWithValue("@Id", id);

            try{
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if(rowsAffected == 1)
                {
                    Console.WriteLine("Update Success");
                    return GetUser(id);
                } else {
                    Console.WriteLine("Update Failure");
                    return null!;
                }
            } catch(Exception e) {
                Console.WriteLine("Update Failure\n" + e.Message);
            }
        }
        return null!;
    }
    #endregion

    #region//Post Methods: create an employee or manager
    //Employee 
    public User RegisterUser(string email, string password)
    {
        using(SqlConnection connection = new(AzureConnectionString))
        {
            string insertUserQuery = "INSERT INTO [dbo].[User] (Email, Password, RoleId) VALUES (@email, @password, @roleId)";
            SqlCommand command = new(insertUserQuery, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@RoleId", 0);
                try {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    Console.WriteLine("Post Success");
                    return GetUser(email);
                }
                else
                {
                    return null!;
                }
            } catch (Exception e) {
                    Console.WriteLine("Insertion Failure\n" + e.Message);
                    return null!;
                }
        }
    }
    //Manager
    public User RegisterUser(string email, string password, int roleid)
    {
        using(SqlConnection connection = new(AzureConnectionString))
        {
            string insertUserQuery = "INSERT INTO [dbo].[User] (Email, Password, RoleId) VALUES (@email, @password, @roleId)";
            SqlCommand command = new(insertUserQuery, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@RoleId", roleid);
                try {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    Console.WriteLine("Post Success");
                    return GetUser(email);
                }
                else
                {
                    return null!;
                }
            } catch (Exception e) {
                    Console.WriteLine("Insertion Failure\n" + e.Message);
                    return null!;
                }
        }
    }
    #endregion

    #region//Get Methods: retrieve unique user by email, id, password
    public User GetUser(string email)
    {
        using SqlConnection connection = new(AzureConnectionString);
        string queryUserByEmail = "SELECT * FROM [dbo].[User] WHERE Email = @email";
        SqlCommand command = new(queryUserByEmail, connection);
        command.Parameters.AddWithValue("@Email", email);
        try
        {
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null!;
                }
                else
                {
                    reader.Read();
                    return new User(
                        (int)reader[0],
                        (int)reader[1],
                        (string)reader[2],
                        (string)reader[3]
                    );
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }

    public User GetUser(int id)
    {
        using SqlConnection connection = new(AzureConnectionString);
        string queryUserById = "SELECT * FROM [dbo].[User] WHERE Id = @id";
        SqlCommand command = new(queryUserById, connection);
        command.Parameters.AddWithValue("@id", id);
        try
        {
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null!;
                }
                else
                {
                    reader.Read();
                    return new User(
                        (int)reader[0],
                        (int)reader[1],
                        (string)reader[2],
                        (string)reader[3]
                    );
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }
    public User LoginUser(string email, string password)
    {
        using SqlConnection connection = new(AzureConnectionString);
        string queryUserByEmail = "SELECT * FROM [dbo].[User] WHERE Email = @email AND Password = @password";
        SqlCommand command = new(queryUserByEmail, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Password", password);
        try
        {
            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return null!;
            }
            else
            {
                reader.Read();
                return new User(
                    (int)reader[0],
                    (int)reader[1],
                    (string)reader[2],
                    (string)reader[3]
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null!;
        }
    }
    #endregion
}