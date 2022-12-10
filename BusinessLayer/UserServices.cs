using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsLayer;
using RepoLayer;

namespace BusinessLayer;

//Interace to implement dependency injection into ApiUi
public interface IUserServices
{
    public User RegisterUser(string email, string password, int roleId);
}

public class UserServices : IUserServices
{
    public User RegisterUser(string email, string password, int roleId)
    {
        return null!;
    }
}