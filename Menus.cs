using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("5. Logout");
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
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        public static void ViewAvailableRides(Driver driver)
        {
            Console.WriteLine("Feature to list available rides coming soon.");
            Console.ReadLine();
        }

        public static void AcceptRide(Driver driver)
        {
           // List<Ride> rides = RideManager.LoadAllRides(passengers, drivers, locations);
        }

        public static void CompleteRide(Driver driver)
        {
            Console.WriteLine("Feature to complete rides coming soon.");
            Console.ReadLine();
        }

        public static void ViewEarnings(Driver driver)
        {
            Console.WriteLine($"Total Earnings: ${driver.Earnings:F2}");
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
        public Ride RequestRide(Location pickup, Location dropoff)
        {
           
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
            if (passenger.RideHistory.Count == 0)
            {
                Console.WriteLine("No rides found.");
            }
            else
            {
                foreach (var ride in passenger.RideHistory)
                {
                    Console.WriteLine($"Ride ID: {ride.Id}, Driver: {ride.Driver?.Name}, Fare: R{ride.Fare}, Completed: {ride.IsCompleted}");
                }
            }
            Console.ReadLine();
        }
        public static void RateDriver(Passenger passenger)
        {
            Console.WriteLine("Rate driver feature coming soon.");
            Console.ReadLine();
        }
    }
}
