using System;
using System.Collections.Generic;
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

        public Ride RequestRide(Location pickup, Location dropoff)
        {
            decimal fare = CalculateFare(pickup, dropoff);

            if(WalletBalance < fare)
            {
                Console.WriteLine("Insufficient wallet balance to request a ride.");
                return null;
            }
            var ride = new Ride
            {
                Id = RideHistory.Count + 1,
                Passenger = this,
                PickUpLocation = pickup,
                DropOffLocation = dropoff,
                Fare = fare,
                Status = RideStatus.Pending
            };
            RideHistory.Add(ride);
            Console.WriteLine($"Ride requested from {pickup.Name} to {dropoff.Name} for a fare of R{fare:F2}");
            return ride;
        }
        private decimal CalculateFare(Location pickup, Location dropoff)
        {
            return 10 + (pickup.Name.Length + dropoff.Name.Length) * 2; 
        }
    }
}
