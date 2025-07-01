using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
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

            Console.Write("Enter email: ");
            string username = Console.ReadLine();

            if (!IsValidEmail(username))
            {
                Console.WriteLine("Invalid email format");
                return;
            }
            if(UsernameExists(username))
            {
                Console.WriteLine("Username already exists");
                return ;
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            if (!IsStrongPassword(password))
            {
                Console.WriteLine("Password must be at least 8 characters long and include uppercase, lowercase, digit, and a special character");
                return;
            }

            Console.Write("Enter your full name: ");
            string name = Console.ReadLine();

            string hashedPassword = HashPassword(password);

            string userLine = $"{username},{hashedPassword},{role},{name}";
            //File.AppendAllLines(new[] { userLine }, filePath);
            File.AppendAllLines(filePath, new[] { userLine });

            Console.WriteLine($"{role} successfullly registered!");
        }
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        private static bool IsStrongPassword(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Z]") && // At least one uppercase letter
                   Regex.IsMatch(password, @"[a-z]") && // At least one lowercase letter
                   Regex.IsMatch(password, @"[0-9]") && // At least one digit
                   Regex.IsMatch(password, @"[\W_]");   // At least one special character
        }

        // hashes the password
        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
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
            Console.Write("Enter email: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            string inputHashedPassword = HashPassword(password);

            var lines =File.ReadAllLines(filePath);

            //splits the content inside the file by a comma and stores them as an array of strings
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length != 4) continue;

                string storedUsername = parts[0];
                string storedPassword = parts[1];
                string role = parts[2];
                string name = parts[3];

                password = storedPassword;

                if (username == storedUsername && storedPassword == inputHashedPassword)
                {
                    Console.WriteLine($"Login successful. Welcome {name} ({role})!");

                    if (role == "Passenger")
                    {
                        var passenger =  new Passenger
                        {
                            Username = storedUsername,
                            Password = storedPassword,
                            Name = name
                        };
                        passenger.LoadWalletBalance();
                        passenger.LoadRideHistory();
                        return passenger;
                    }
                    else if (role == "Driver")
                    {
                        var driver =  new Driver
                        {
                            Username = storedUsername,
                            Password = storedPassword,
                            Name = name
                        };
                        driver.LoadEarnings();
                        return driver;
                    }
                }
            }
            Console.WriteLine("Invalid username or password!");
            return null;
        }
    }
}
