using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Screen
{
    public long Id { get; set; }

    public long TheatreId { get; set; }

    public string ScreenName { get; set; } = null!;

    public int Capacity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}
