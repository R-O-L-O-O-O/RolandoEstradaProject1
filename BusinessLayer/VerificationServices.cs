//This file focuses verifying emails, passwords, and roles
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ModelsLayer;
using RepoLayer;

namespace BusinessLayer;

public interface IVerificationServices
{
    public bool VerifyEmail(string email);
    public bool VerifyPassword(string password);
    public bool VerifyRole(int roleid);
    public bool VerifyRegistration(string email, string password);
    public bool VerifyRegistration(string email, string password, int roleid);
    public bool IsEmployee(int id);
    public bool IsManager(int id);
    public bool IsPassword(int id, string oldPassword);
}

public class VerificationServices : IVerificationServices
{
    private readonly IUserRepo _iur;
    public VerificationServices(IUserRepo iur) => this._iur = iur;

    public bool VerifyRegistration(string email, string password) => VerifyEmail(email) && VerifyPassword(password);
    public bool VerifyRegistration(string email, string password, int roleid) => VerifyEmail(email) && VerifyPassword(password) && VerifyRole(roleid);

    public bool VerifyEmail(string email)
    {
        string regex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        return Regex.Match(email, regex).Success;
    }
    public bool VerifyPassword(string password) => Regex.Match(password, @"^([0-9a-zA-Z]{6,})$").Success;

    public bool VerifyRole(int roleId) => roleId >= 0 && roleId <= 1;
    public bool IsEmployee(int id)
    {
        if(_iur.GetUser(id) is null) return false;
        else return true;
    }

    public bool IsManager(int id)
    {
        User tmp = _iur.GetUser(id);
        if(tmp is null || tmp.RoleId == 0) return false;
        else return true;
    }

    public bool IsPassword(int id, string oldPassword)
    {
        User tmp = _iur.GetUser(id);
        if(!((tmp.Password).Equals(oldPassword))) return false;
        else return true;
    }
}