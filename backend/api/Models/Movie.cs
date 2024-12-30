using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Movie
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Descr { get; set; }

    public string Lang { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public int Duration { get; set; }

    public string? Poster { get; set; }

    public decimal? Rating { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? Country { get; set; }

    public string? Certificate { get; set; }

    public string? Director { get; set; }

    public string? Producer { get; set; }

    public string? Cast { get; set; }

    public string? TrailerUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
