using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Domain.Enums
{
    public enum RentalStatus
    {
        Pending = 1,
        Confirmed = 2,
        Active = 3,
        Completed = 4,
        Cancelled = 5
    }
}
