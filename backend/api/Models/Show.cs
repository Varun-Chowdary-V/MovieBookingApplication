using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Show
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long ScreenId { get; set; }

    public DateOnly ShowDate { get; set; }

    public TimeOnly ShowTime { get; set; }

    public String? AvailableSeats { get; set; }

    public decimal TicketFare { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}
