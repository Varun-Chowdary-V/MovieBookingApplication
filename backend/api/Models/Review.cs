﻿using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Review
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long UserId { get; set; }

    public decimal? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
