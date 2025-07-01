using System.Collections.Generic;
using System.Linq;

namespace RideSharingSystem
{
    public class Driver: User
    {
        public bool IsAvailable { get; set; } = true;
        public decimal Earnings { get; set; } = 0;
        public List<Ride> AcceptedRides { get; set; } = new List<Ride>();
        public List<int> Ratings { get; set; } = new List<int>();

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
    }
}
