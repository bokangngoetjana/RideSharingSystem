using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RideSharingSystem.Interfaces;

namespace RideSharingSystem
{
    public class Passenger: User, IRideable, IPayable
    {
        public decimal WalletBalance { get; set; } 
        public List<Ride> RideHistory { get; set; } = new List<Ride>();

        private static string passengerData = "passenger_data.txt";
        public Passenger()
        {
            Role = "Passenger";
        }
        public static List<Passenger> LoadAllPassengers()
        {
            string filePath = "users.txt";
            var passengers = new List<Passenger>();

            if (!File.Exists(filePath)) return passengers;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 4) continue;

                string username = parts[0];
                string password = parts[1];
                string role = parts[2];
                string name = parts[3];

                if (role == "Passenger")
                {
                    var passenger = new Passenger
                    {
                        Username = username,
                        Password = password,
                        Role = role,
                        Name = name
                    };
                    passenger.LoadWalletBalance();
                    passenger.LoadRideHistory();
                    passengers.Add(passenger);
                }
            }

            return passengers;
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
                Id = RideManager.NextRideId++,
                Passenger = this,
                PickUpLocation = pickup,
                DropOffLocation = dropoff,
                Fare = fare,
                Status = RideStatus.Pending
            };
            RideHistory.Add(ride);
            SaveRideHistory();
            Console.WriteLine($"Ride requested from {pickup.Name} to {dropoff.Name} for a fare of R{fare:F2}");
            return ride;
        }
        private decimal CalculateFare(Location pickup, Location dropoff)
        {
            return 10 + (pickup.Name.Length + dropoff.Name.Length) * 2; 
        }
        public void SaveWalletBalance()
        {
            var lines = File.Exists(passengerData)
                ? File.ReadAllLines(passengerData).ToList()
                : new List<string>();

            lines.RemoveAll(l => l.StartsWith($"{Username},"));
            lines.Add($"{Username},{WalletBalance}");

            File.WriteAllLines(passengerData, lines);
        }
        public void LoadWalletBalance()
        {
            if (!File.Exists(passengerData)) return;

            var match = File.ReadAllLines(passengerData)
                .FirstOrDefault(line => line.StartsWith($"{Username},"));

            if (match != null)
            {
                var parts = match.Split(',');
                if (decimal.TryParse(parts[1], out decimal balance))
                {
                    WalletBalance = balance;
                }
            }
        }
        public void SaveRideHistory()
        {
            var lines = RideHistory.Select(ride =>
                $"{ride.Id},{Username},{ride.Driver?.Username},{ride.PickUpLocation.Name},{ride.DropOffLocation.Name},{ride.Fare},{ride.Status},{ride.IsCompleted}");

            File.AppendAllLines("ride_history.txt", lines);
        }
        public void LoadRideHistory()
        {
            if (!File.Exists("ride_history.txt")) return;

            var rides = File.ReadAllLines("ride_history.txt")
                .Where(line => line.Contains($",{Username},"))
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new Ride
                    {
                        Id = int.Parse(parts[0]),
                        Passenger = this,
                        Driver = new Driver { Username = parts[2] },
                        PickUpLocation = new Location(parts[3]),
                        DropOffLocation = new Location(parts[4]),
                        Fare = decimal.Parse(parts[5]),
                        Status = (RideStatus)Enum.Parse(typeof(RideStatus), parts[6]),
                        IsCompleted = bool.Parse(parts[7])
                    };
                }).ToList();

            RideHistory = rides;
        }

    }
}
