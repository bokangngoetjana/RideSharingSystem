using System;
using System.IO;
using System.Linq;
using RideSharingSystem;
using System.Xml.Linq;

namespace RideSharingSystem
{
    public static class UserManager
    {
        private static string filePath = "users.txt";

        public static void Register()
        {
            Console.WriteLine("Are you a: \n 1. Driver or 2.Passenger?");
            string roleSelection = Console.ReadLine();

            string role = roleSelection;

            switch (role)
            {
                case "1":
                    role = "Driver";
                    break;
                case "2":
                    role = "Passenger";
                    break;
                default:
                    Console.WriteLine("Please select a valid role");
                    break;
            };

            if(role == null)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            if(UsernameExists(username))
            {
                Console.WriteLine("Username already exists");
                return ;
            }
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Console.Write("Enter your full name: ");
            string name = Console.ReadLine();

            string userLine = $"{username},{password},{role},{name}";
            //File.AppendAllLines(new[] { userLine }, filePath);
            File.AppendAllLines(filePath, new[] { userLine });

            Console.WriteLine($"{role} successfullly registered!");
        }
        private static bool UsernameExists(string username)
        {
            if(!File.Exists(filePath)) return false;

            return File.ReadAllLines(filePath)
                .Any(line => line.Split(',')[0] == username);
        }
        public static User Login()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No users registered yet.");
                return null;
            }
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            var lines =File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 4) continue;

                string storedUsername = parts[0];
                string storedPassword = parts[1];
                string role = parts[2];
                string name = parts[3];

                if (username == storedUsername && password == storedPassword)
                {
                    Console.WriteLine($"Login successful. Welcome {name} ({role})!");

                    if (role == "Passenger")
                    {
                        return new Passenger
                        {
                            Username = storedUsername,
                            Password = storedPassword,
                            Name = name
                        };
                    }
                    else if (role == "Driver")
                    {
                        return new Driver
                        {
                            Username = storedUsername,
                            Password = storedPassword,
                            Name = name
                        };
                    }
                }
            }
            Console.WriteLine("Invalid username or password!");
            return null;
        }
    }
}
