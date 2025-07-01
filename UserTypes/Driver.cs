using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RideSharingSystem
{
    public class Driver: User
    {
        public bool IsAvailable { get; set; } = true;
        public decimal Earnings { get; set; } = 0;
        public List<Ride> AcceptedRides { get; set; } = new List<Ride>();
        public List<int> Ratings { get; set; } = new List<int>();
        public bool IsFlaggedForReview { get; set; } = false;
        public Driver()
        {
            Role = "Driver";
        }
        public double GetAverageRating()
        {
            if(Ratings.Count == 0)
                return 0;
            return Ratings.Average();
        }
        public void SaveEarnings()
        {
            const string driverDataFile = "driver_data.txt";
            var lines = File.Exists(driverDataFile)
                ? File.ReadAllLines(driverDataFile).ToList() 
                : new List<string>();

            lines.RemoveAll(l => l.StartsWith($"{Username},"));
            lines.Add($"{Username}, {Earnings}");
            File.WriteAllLines(driverDataFile, lines);
        }
        public static List<Driver> LoadAllDrivers()
        {
            string filePath = "users.txt";
            var drivers = new List<Driver>();

            if (!File.Exists(filePath)) return drivers;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 4) continue;

                string username = parts[0];
                string password = parts[1];
                string role = parts[2];
                string name = parts[3];

                if (role == "Driver")
                {
                    var driver = new Driver
                    {
                        Username = username,
                        Password = password,
                        Role = role,
                        Name = name
                    };
                    driver.LoadEarnings();
                    drivers.Add(driver);
                }
            }

            return drivers;
        }

        public void LoadEarnings()
        {
            const string driverDataFile = "driver_data.txt";
            if (!File.Exists(driverDataFile)) return;

            var match = File.ReadAllLines(driverDataFile)
                .FirstOrDefault(line => line.StartsWith($"{Username},"));

            if (match != null)
            {
                var parts = match.Split(',');
                if (decimal.TryParse(parts[1], out decimal earned))
                {
                    Earnings = earned;
                }
            }
        }
    }
}
