using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RideSharingSystem
{
    public class Menus
    {
        List<Passenger> passengers;
        List<Driver> drivers;
        List<Location> locations;

        public Menus(List<Passenger> passengers, List<Driver> drivers, List<Location> locations)
        {
            this.passengers = passengers;
            this.drivers = drivers;
            this.locations = locations;
        }

        //Driver menu
        public static void DriverMenu(Driver driver)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Welcome, {driver.Name} (Driver) ===");
                Console.WriteLine("1. View Available Ride Requests");
                Console.WriteLine("2. Accept a Ride");
                Console.WriteLine("3. Complete a Ride");
                Console.WriteLine("4. View Earnings");
                Console.WriteLine("5. View Completed Rides");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAvailableRides(driver);
                        break;
                    case "2":
                        AcceptRide(driver);
                        break;
                    case "3":
                        CompleteRide(driver);
                        break;
                    case "4":
                        ViewEarnings(driver);
                        break;
                    case "5":
                        ViewCompletedRides(driver);
                        break;
                    case "6":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        public static void ViewCompletedRides(Driver driver)
        {
            try
            {
                var completedRides = RideManager.AllRides
                    .Where(r => r.Driver != null && r.Driver.Username == driver.Username && r.IsCompleted)
                    .ToList();

                if (!completedRides.Any())
                {
                    Console.WriteLine("You have no completed rides.");
                }
                else
                {
                    Console.WriteLine("=== Completed Rides ===");
                    foreach (var ride in completedRides)
                    {
                        Console.WriteLine($"Ride ID: {ride.Id}, Passenger: {ride.Passenger.Name}, Pickup: {ride.PickUpLocation.Name}, Dropoff: {ride.DropOffLocation.Name}, Fare: R{ride.Fare:F2}, Status: {ride.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching completed rides.");
                Console.WriteLine($"Details: {ex.Message}");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static void ViewAvailableRides(Driver driver)
        {
            var pendingRides = RideManager.AllRides
                .Where(r => r.Status == RideStatus.Pending)
                .ToList();

            if (!pendingRides.Any())
            {
                Console.WriteLine("No available ride requests.");
            }
            else
            {
                foreach(var ride in pendingRides)
                {
                    Console.WriteLine($"Ride ID: {ride.Id}, Passenger: {ride.Passenger.Name}, Pick-up: {ride.PickUpLocation.Name}, Drop-off: {ride.DropOffLocation.Name}, Fare: R{ride.Fare:F2}");
                }
            }
                Console.ReadLine();
        }

        public static void AcceptRide(Driver driver)
        {
            var pendingRides = RideManager.AllRides
                .Where(ride => ride.Status == RideStatus.Pending)
                .ToList();

            if (!pendingRides.Any())
            {
                Console.WriteLine("No rides to accept.");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Ride ID to accept: ");
            if (int.TryParse(Console.ReadLine(), out int rideId))
            {
                var ride = pendingRides.FirstOrDefault(r => r.Id == rideId);
                if (ride != null)
                {
                    ride.Status = RideStatus.Accepted;
                    ride.Driver = driver;

                    //Deduct fare from passenger's wallet balance after accepting ride
                    ride.Passenger.WalletBalance -= ride.Fare;
                    ride.Passenger.SaveWalletBalance();

                    driver.Earnings += ride.Fare;
                    driver.SaveEarnings();
                    RideManager.UpdateRideInHistory(ride);
                    Console.WriteLine("Ride accepted.");
                }
                else
                {
                    Console.WriteLine("Ride not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Ride ID.");
            }
            Console.ReadLine();
        }

        public static void CompleteRide(Driver driver)
        {
            var acceptedRides = RideManager.AllRides
                    .Where(r => r.Driver == driver && r.Status == RideStatus.Accepted)
                    .ToList();

            if (!acceptedRides.Any())
            {
                Console.WriteLine("No rides to complete.");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Ride ID to complete: ");
            if (int.TryParse(Console.ReadLine(), out int rideId))
            {
                var ride = acceptedRides.FirstOrDefault(r => r.Id == rideId);
                if (ride != null)
                {
                    ride.Status = RideStatus.Completed;
                    ride.IsCompleted = true;

                    driver.SaveEarnings();
                    RideManager.UpdateRideInHistory (ride);
                    Console.WriteLine("Ride completed.");
                }
                else
                {
                    Console.WriteLine("Ride not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Ride ID");
            }
                
            Console.ReadLine();
        }

        public static void ViewEarnings(Driver driver)
        {
            Console.WriteLine($"Total Earnings: R{driver.Earnings:F2}");
            Console.ReadLine();
        }

        //Passenger menu
        public static void PassengerMenu(Passenger passenger)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Welcome, {passenger.Name} (Passenger) ===");
                Console.WriteLine("1. Request a Ride");
                Console.WriteLine("2. View Wallet Balance");
                Console.WriteLine("3. Add Funds to Wallet");
                Console.WriteLine("4. View Ride History");
                Console.WriteLine("5. Rate a Driver");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RequestRide(passenger);
                        break;
                    case "2":
                        ViewWallet(passenger);
                        break;
                    case "3":
                        AddFunds(passenger);
                        break;
                    case "4":
                        ViewRideHistory(passenger);
                        break;
                    case "5":
                        RateDriver(passenger);
                        break;
                    case "6":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        public static void RequestRide(Passenger passenger)
        {
            Console.Write("Enter pickup location: ");
            string pickupName = Console.ReadLine();

            Console.Write("Enter dropoff location: ");
            string dropoffName = Console.ReadLine();

            var pickup = new Location(pickupName);
            var dropoff = new Location(dropoffName);

            var ride = passenger.RequestRide(pickup, dropoff);

            if (ride != null)
            {
                RideManager.AllRides.Add(ride);
            }

            Console.ReadLine();
        }

        public static void ViewWallet(Passenger passenger)
        {
            Console.WriteLine($"Your wallet balance: R{passenger.WalletBalance:F2}");
            Console.ReadLine();
        }
        public static void AddFunds(Passenger passenger)
        {
            Console.Write("Enter amount to add: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                passenger.WalletBalance += amount;
                Console.WriteLine($"Added R{amount:F2} to wallet.");
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
            Console.ReadLine();
        }
        public static void ViewRideHistory(Passenger passenger)
        {
            const string RideHistoryPath = "ride_history.txt";

            if (!File.Exists(RideHistoryPath))
            {
                Console.WriteLine("No ride history found.");
                Console.ReadLine();
                return;
            }

            var lines = File.ReadAllLines(RideHistoryPath)
                .Where(line => line.Split(',')[1] == passenger.Username)
                .ToList();

            if (!lines.Any())
            {
                Console.WriteLine("No rides found.");
            }
            else
            {
                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    string driver = string.IsNullOrEmpty(parts[2]) ? "N/A" : parts[2];
                    string pickup = parts[3];
                    string dropoff = parts[4];
                    string fare = parts[5];
                    string status = parts[6];
                    string completed = parts[7];

                    Console.WriteLine($"Ride ID: {parts[0]}, Driver: {driver}, Pickup: {pickup}, Dropoff: {dropoff}, Fare: R{fare}, Status: {status}, Completed: {completed}");
                }
            }

            Console.ReadLine();
        }

        public static void RateDriver(Passenger passenger)
        {
            var completedRides = passenger.RideHistory
               .Where(r => r.IsCompleted && r.Driver != null)
               .ToList();

            if (!completedRides.Any())
            {
                Console.WriteLine("No completed rides available to rate.");
                Console.ReadLine();
                return;
            }

            foreach (var ride in completedRides)
            {
                Console.WriteLine($"Ride ID: {ride.Id}, Driver: {ride.Driver.Username}");
            }
            Console.Write("Enter Ride ID to rate: ");
            if (int.TryParse(Console.ReadLine(), out int rideId))
            {
                var ride = completedRides.FirstOrDefault(r => r.Id == rideId);
                if (ride != null)
                {
                    Console.Write($"Rate driver {ride.Driver.Username} (1–5): ");
                    string rating = Console.ReadLine();
                    if (int.TryParse(rating, out int score) && score >= 1 && score <= 5)
                    {
                        ride.Driver.Ratings.Add(score);

                        double avg = ride.Driver.GetAverageRating();
                        if (avg < 3)
                        {
                            ride.Driver.IsFlaggedForReview = true;
                            Console.WriteLine($"Driver {ride.Driver.Name} has been flagged for review due to low ratings.");
                        }
                        Console.WriteLine($"Thank you! You rated {ride.Driver.Name} a {score}/5.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid rating. Must be a number between 1 and 5.");
                    }

                    Console.WriteLine($"Thank you! You rated {ride.Driver.Username} a {rating}/5.");
                }

            }

            Console.ReadLine();
        }
    }
}
