using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelsLayer;
using RepoLayer;

namespace BusinessLayer;

//Interface to implement dependency injection into ApiUi
public interface IUserServices
{
    public User RegisterUser(string email, string password);
    public User RegisterUser(string email, string password, int roleid);
    public User LoginUser(string email, string password);

    //public User EditUser(int id, string oldPassword, string newPassword);
    //public User EditUser(int id, string email);
    // public User EditUser(int managerId, int userId, int roleId);
}

public class UserServices : IUserServices
{
    private readonly IUserRepo _iur;
    private readonly IVerificationServices _ivs;
    public UserServices(IUserRepo iur)
    {
        this._iur = iur;
        this._ivs = new VerificationServices(_iur);
    }

    public User LoginUser(string email, string password) => _iur.LoginUser(email, password); //Login Constructor

    #region //Registration Methods
    public User RegisterUser(string email, string password)
    {
        if(!_ivs.VerifyRegistration(email, password))
            return null!;
        return _iur.RegisterUser(email, password);
    }
    public User RegisterUser(string email, string password, int roleid)
    {
        if(!_ivs.VerifyRegistration(email, password, roleid))
            return null!;
        return _iur.RegisterUser(email, password, roleid );
    }
    #endregion

    #region//Edit User Methods
    // public User EditUser(int id, string oldPassword, string newPassword)
    // {
    //     if(!_ivs.IsEmployee(id) || !_ivs.VerifyPassword(newPassword) || !_ivs.IsPassword(id, oldPassword))
    //     {
    //         Console.WriteLine("Invalid userId, invalid new password, or passwords don't match");
    //         return null!;
    //     }
    //     return _iur.UpdateUser(id, newPassword);
    // }
    // public User EditUser(int id, string email)
    // {
    //     if(!_ivs.IsEmployee(id) || !_ivs.VerifyEmail(email))
    //     {
    //         Console.WriteLine("Invalid userId, or invalid email");
    //         return null!;
    //     }
    //     return _iur.UpdateUser(id, email);
    // }
    // public User EditUser(int managerId, int userId, int roleId)
    // {
    //     if(!_ivs.IsEmployee(id) || !_ivs.VerifyEmail(email))
    //     {
    //         Console.WriteLine("Invalid userId, or invalid email");
    //         return null!;
    //     }
    //     return _iur.UpdateUser(id, email);
    // }

    #endregion
}