//This file defines the data fields for the User object used throughout the proj
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLayer;
public class User
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string ?Email { get; set; }
    public string ?Password { get; set; }

    //Constructor for a employee
    public User(int id, string email, string password)
    {
        this.Id = id;
        this.RoleId = 0;
        this.Email = email;
        this.Password = password;
    }

    //Constructor for a manager
    public User(int id, int roleid, string email, string password)
    {
        this.Id = id;
        this.RoleId = roleid;
        this.Email = email;
        this.Password = password;
    }
    public User(){}
}