using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class User //TODO: Implement email + password
    {
        public int Id { get; set; } //ID to represent user information

        public string Role { get; set; } = string.Empty; //According to project, any new users become employees

        public string Name { get; set; } = string.Empty;
        

        
    }
}