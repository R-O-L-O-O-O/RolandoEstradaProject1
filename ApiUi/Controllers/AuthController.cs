//This file implements the functionality from the bus layer
//And sending requests to the API
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using ModelsLayer;

namespace ApiUi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserServices _ius;
    private readonly ITicketService _its;
    public AuthController(IUserServices ius, ITicketService its)
    {
        this._ius = ius;
        this._its = its;
    }

    [HttpPost("Register")]
    public ActionResult<User> RegisterUser(string email, string password)
    {
        User newUser = _ius.RegisterUser(email, password);
        return Created("path/to/db", newUser);
    }

    [HttpPost("Register Manager")]
    public ActionResult<User> RegisterUser(string email, string password, int roleid)
    {
        User newUser = _ius.RegisterUser(email, password, roleid);
        return Created("path/to/db", newUser);
    }

    [HttpGet("Login")]
    public ActionResult<User> LoginUser(string email, string password)
    {
        User user = _ius.LoginUser(email, password);
        return Created("path/to/db", user);
    }

    [HttpPut("Change Password")]
    public ActionResult<User> EditUser(int targetId, string oldPassword, string newPassword)
    {
        User user = _ius.EditUser(targetId, oldPassword, newPassword);
        return Created("path/", user);
    }

    [HttpPut("Change Email")]
    public ActionResult<User> EditUser(int targetId, string newEmail)
    {
        User user = _ius.EditUser(targetId, newEmail);
        return Created("path/", user);
    }

    [HttpPut("Change Role")]
    public ActionResult<User> EditUser(int managerId, int targetId, int newRoleId)
    {
        User user = _ius.EditUser(managerId, targetId, newRoleId);
        return Created("path/", user);
    }

    [HttpGet("Tickets")]
    public ActionResult<List<Ticket>> UserTickets(int userId) {
        List<Ticket> userTickets = _its.GetUserTickets(userId);
        return Created("path/", userTickets);
    }

    [HttpGet("Ticket Status")]
    public ActionResult<List<Ticket>> UserTickets(int userId, int status) {
        List<Ticket> userTickets = _its.GetUserTickets(userId, status);
        return Created("path/", userTickets);
    }
}