using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingSystem
{
    public class Location
    {
        public string Name { get; set; }
        public Location(string name)
        {
            Name = name;
        }
    }
}
