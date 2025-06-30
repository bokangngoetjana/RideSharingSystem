using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingSystem.Interfaces
{
    public interface IPayable
    {
        decimal WalletBalance { get; set; }
    }
}
