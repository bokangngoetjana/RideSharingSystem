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
        public static string RideFilePath = "ride_history.txt";
        public static int NextRideId = 1;
        public static List<Ride> AllRides { get; set; } = new List<Ride>();

        //the information saved in the ride_history.txt file
        public static string RidesCSV(Ride ride)
        {
            return $"{ride.Id},{ride.Passenger.Username},{ride.Driver?.Username ?? ""},{ride.PickUpLocation.Name},{ride.DropOffLocation.Name},{ride.Fare},{ride.Status},{ride.IsCompleted}";
        }

        /**Parses a CSV line from `ride_history.txt` into a `Ride` object by resolving passenger, driver, 
         * locations, fare, status, and completion status from the provided lists.
         */
        public static Ride RideFromCsv(string line, List<Passenger> passengers, List<Driver> drivers, List<Location> locations)
        {
            var parts = line.Split(',');

            if (parts.Length < 8)
                throw new InvalidDataException("Invalid ride data line.");

            int id = int.Parse(parts[0]);
            string passengerUsername = parts[1];
            string driverUsername = parts[2];
            string pickupName = parts[3];
            string dropoffName = parts[4];
            decimal fare = decimal.Parse(parts[5]);
            RideStatus status = (RideStatus)Enum.Parse(typeof(RideStatus), parts[6]);
            bool isCompleted = bool.Parse(parts[7]);

            var passenger = passengers.FirstOrDefault(p => p.Username == passengerUsername);
            var driver = string.IsNullOrEmpty(driverUsername) ? null : drivers.FirstOrDefault(d => d.Username == driverUsername);
            var pickupLoc = new Location(pickupName);
            var dropoffLoc = new Location(dropoffName);

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
                Status = status,
                IsCompleted = isCompleted,
            };
        }

        // Load all rides from ride_history.txt
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
            if (rides.Any())
            {
                NextRideId = rides.Max(r => r.Id) + 1;
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

        // updates rides if the status changes and IsCompleted is true
        public static void UpdateRideInHistory(Ride updatedRide)
        {
            string filePath = "ride_history.txt";

            if (!File.Exists(filePath)) return;

            var lines = File.ReadAllLines(filePath).ToList();
            for(int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (int.Parse(parts[0]) == updatedRide.Id)
                {
                    lines[i] = $"{updatedRide.Id}," +
                       $"{updatedRide.Passenger.Username}," +
                       $"{updatedRide.Driver?.Username ?? ""}," +
                       $"{updatedRide.PickUpLocation.Name}," +
                       $"{updatedRide.DropOffLocation.Name}," +
                       $"{updatedRide.Fare}," +
                       $"{updatedRide.Status}," +
                       $"{updatedRide.IsCompleted}";
                    break;
                }
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
