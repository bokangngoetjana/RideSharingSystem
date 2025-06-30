using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RideSharingSystem
{
    public class RideManager
    {
        public static string RideFilePath = "rides.txt";

        public static string RidesCSV(Ride ride)
        {
            return $"{ride.Id},{ride.Passenger.Username},{ride.Driver?.Username ?? ""},{ride.PickUpLocation.Name},{ride.DropOffLocation.Name},{ride.Fare},{ride.Status}";
        }

        public static Ride RideFromCsv(string line, List<Passenger> passengers, List<Driver> drivers, List<Location> locations)
        {
            var parts = line.Split(',');

            if (parts.Length < 7)
                throw new InvalidDataException("Invalid ride data line.");

            int id = int.Parse(parts[0]);
            string passengerUsername = parts[1];
            string driverUsername = parts[2];
            string pickupName = parts[3];
            string dropoffName = parts[4];
            decimal fare = decimal.Parse(parts[5]);
            RideStatus status = (RideStatus)Enum.Parse(typeof(RideStatus), parts[6]);

            var passenger = passengers.FirstOrDefault(p => p.Username == passengerUsername);
            var driver = string.IsNullOrEmpty(driverUsername) ? null : drivers.FirstOrDefault(d => d.Username == driverUsername);
            var pickupLoc = locations.FirstOrDefault(l => l.Name == pickupName);
            var dropoffLoc = locations.FirstOrDefault(l => l.Name == dropoffName);

            if (passenger == null)
                throw new Exception($"Passenger '{passengerUsername}' not found.");
            if (pickupLoc == null || dropoffLoc == null)
                throw new Exception($"One or both locations '{pickupName}' or '{dropoffName}' not found.");

            return new Ride
            {
                Id = id,
                Passenger = passenger,
                Driver = driver,
                PickUpLocation = pickupLoc,
                DropOffLocation = dropoffLoc,
                Fare = fare,
                Status = status
            };
        }
        // Load all rides from file, resolving references
        public static List<Ride> LoadAllRides(List<Passenger> passengers, List<Driver> drivers, List<Location> locations)
        {
            var rides = new List<Ride>();

            if (!File.Exists(RideFilePath))
                return rides;

            var lines = File.ReadAllLines(RideFilePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var ride = RideFromCsv(line, passengers, drivers, locations);
                rides.Add(ride);
            }

            return rides;
        }

        // Save all rides back to file (overwrite)
        public static void SaveAllRides(List<Ride> rides)
        {
            var lines = rides.Select(r => RidesCSV(r)).ToArray();
            File.WriteAllLines(RideFilePath, lines);
        }

        // Add a new ride (append)
        public static void AddRide(Ride ride)
        {
            var line = RidesCSV(ride);
            File.AppendAllLines(RideFilePath, new[] { line });
        }
    }
}
