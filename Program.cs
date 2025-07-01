using System;
using System.Collections.Generic;

namespace RideSharingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Passenger> passengers = Passenger.LoadAllPassengers();
            List<Driver> drivers = Driver.LoadAllDrivers();
            // Initial loading of data (locations, users)
            List<Location> locations = new List<Location>
            {
                new Location("Pretoria"),
                new Location("Johannesburg"),
                new Location("Gqeberha"),
                new Location("Durban"),
                new Location("Cape Town")
            };
            while (true)
            {
                Console.WriteLine("\n=== Ride-Sharing App ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        UserManager.Register();
                        break;
                    case "2":
                        var user = UserManager.Login();
                        if(user != null)
                        {
                            RideManager.AllRides = RideManager.LoadAllRides(passengers, drivers, locations);
                            if (user is Passenger passenger)
                            {
                                Menus.PassengerMenu(passenger);
                            }
                            else if (user is Driver driver)
                            {
                                Menus.DriverMenu(driver);
                            }
                        }
                       
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }
     
    }
}
