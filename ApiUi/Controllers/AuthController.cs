//This file implements the functionality from the bus layer
//And sending requesting to the API
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
    public AuthController(IUserServices ius) => this._ius = ius;

    [HttpPost("Register")]
    public ActionResult<User> RegisterUser(string email, string password, int roleId)
    {
        User user = _ius.RegisterUser(email, password, roleId);
        return Created("You just registered!", user);
    }
}