using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api.Models;

public partial class MovieBookingDbContext : DbContext
{
    public MovieBookingDbContext()
    {
    }

    public MovieBookingDbContext(DbContextOptions<MovieBookingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }


    public virtual DbSet<Show> Shows { get; set; }


    public virtual DbSet<Theatre> Theatres { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {

            entity.ToTable("bookings");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.BookingDatetime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("booking_datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Seats)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seats");
            entity.Property(e => e.ShowId).HasColumnName("show_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

        });

        modelBuilder.Entity<Movie>(entity =>
        {

            entity.ToTable("movies");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Cast)
                .HasColumnType("text")
                .HasColumnName("cast");
            entity.Property(e => e.Certificate)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("certificate");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Descr)
                .HasColumnType("text")
                .HasColumnName("descr");
            entity.Property(e => e.Director)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("director");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Genre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genre");
            entity.Property(e => e.Lang)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lang");
            entity.Property(e => e.Poster)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("poster");
            entity.Property(e => e.Producer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("producer");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("rating");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TrailerUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trailer_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Review>(entity =>
        {

            entity.ToTable("reviews");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

        });

        modelBuilder.Entity<Screen>(entity =>
        {

            entity.ToTable("screens");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("screen_name");
            entity.Property(e => e.TheatreId).HasColumnName("theatre_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");

        });


        modelBuilder.Entity<Show>(entity =>
        {

            entity.ToTable("shows");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.AvailableSeats).HasColumnName("available_seats");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");
            entity.Property(e => e.ShowDate).HasColumnName("show_date");
            entity.Property(e => e.ShowTime).HasColumnName("show_time");
            entity.Property(e => e.TicketFare)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ticket_fare");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");

        });

        

        modelBuilder.Entity<Theatre>(entity =>
        {

            entity.ToTable("theatres");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Pincode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pincode");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fname");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lname");
            entity.Property(e => e.PasswordHashed)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hashed");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("customer")
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
