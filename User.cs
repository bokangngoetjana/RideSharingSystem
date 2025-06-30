using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingSystem
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; protected set; } 
        public string Username { get; set; }
       // public List<int> Ratings { get; set; }
    }
}
