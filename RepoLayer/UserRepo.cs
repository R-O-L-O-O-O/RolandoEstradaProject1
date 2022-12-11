//This file contains functionality for the database
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using ModelsLayer;

namespace RepoLayer;

public interface IUserRepo
{
    List<User> GetUsers();
    void RegisterUsers(List<User> userDb);
    User RegisterUser(string email, string password);
    User RegisterUser(string email, string password, int roleid);
}

public class UserRepo : IUserRepo
{
    public List<User> GetUsers()
    {
        if (File.Exists("UserDatabase.json"))
        {
            return JsonSerializer.Deserialize<List<User>>(File.ReadAllText("UserDatabase.json"))!;
        }
        else
        {
            return new List<User>();
        }
    }

    public void RegisterUsers(List<User> userDb)
    {
        string serializedDb = JsonSerializer.Serialize(userDb);
        File.WriteAllText("UserDatabase.json", serializedDb);
    }

    public User RegisterUser(string email, string password)
    {
        string conString = File.ReadAllText("../../ConString.txt");
        using(SqlConnection connection = new SqlConnection(conString))
        {
            return null!;
        }
    }

    public User RegisterUser(string email, string password, int roleid)
    {
        string conString = File.ReadAllText("../../ConString.txt");
        using(SqlConnection connection = new SqlConnection(conString))
        {
            return null!;
        }
    }
}