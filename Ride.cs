namespace RideSharingSystem
{
    public enum RideStatus
    {
        Pending,
        Accepted,
        Completed
    }
    public class Ride
    {
        public int Id { get; set; }
        public Passenger Passenger { get; set; }
        public Driver Driver { get; set; }
        public Location PickUpLocation { get; set; }
        public Location DropOffLocation { get; set; }
        public decimal Fare { get; set; }
        public RideStatus Status { get; set; } = RideStatus.Pending;
        public bool IsCompleted { get; set; } = false;

    
    }
}
