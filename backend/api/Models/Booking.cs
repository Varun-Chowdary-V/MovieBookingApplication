using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Booking
{
    public long Id { get; set; } 

    public long UserId { get; set; }

    public long ShowId { get; set; }

    public DateTime BookingDatetime { get; set; }

    public string Seats { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}
