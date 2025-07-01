using System.Collections.Generic;

namespace RideSharingSystem.Interfaces
{
    public interface IRideable
    {
       List<Ride> RideHistory { get; set; }
    }
}
