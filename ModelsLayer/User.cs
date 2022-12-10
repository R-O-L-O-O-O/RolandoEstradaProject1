using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLayer;
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(int id, int roleId, string name, string email, string password)
        {
            this.Id = id;
            this.RoleId = roleId;
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }
    }