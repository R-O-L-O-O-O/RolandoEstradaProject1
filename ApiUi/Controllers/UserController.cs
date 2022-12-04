using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;

namespace ApiUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        static public List<User> Users = new List<User> //List array to hold user objs
        {
            new User { Id = 1, Name = "John David Vernon"},
            new User { Id = 2, Name = "Garrett McCluney"},
            new User { Id = 3, Name = "Devin Goss"},
            new User { Id = 4, Name = "Barbara Gonzales"},
        };

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(Users);
        }

        [HttpGet("{id}")]
        public ActionResult<List<User>> GetUser(int id)
        {
            var user = Users.Find(x => x.Id == id);
            if (user == null)
                return NotFound("User Not Found");
                return Ok(user);
        }

        [HttpPost]
        public ActionResult<List<User>> CreateUser(User user)
        {
            Users.Add(user);
            return Ok(Users);
        }
        
        [HttpPut]
        public ActionResult<List<User>> UpdateUser(User updatedUser)
        {
            var user = Users.Find(x => x.Id == updatedUser.Id);
            if (user == null)
                return NotFound("User Not Found");

            user.Name = updatedUser.Name;

            return Ok(Users);

        }

        [HttpDelete("{id}")]
        public ActionResult<List<User>> Delete(int id)
        {
            var user = Users.Find(x => x.Id == id);
            if (user == null)
                return NotFound("User Not Found");

                Users.Remove(user);
                return Ok(user);
        }

    }
}