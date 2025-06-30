using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RideSharingSystem.Interfaces;

namespace RideSharingSystem
{
    public class Passenger: User, IRideable, IPayable
    {
        public decimal WalletBalance { get; set; }
        public List<Ride> RideHistory { get; set; } = new List<Ride>();
        public Passenger()
        {
            Role = "Passenger";
        }
    }
}
