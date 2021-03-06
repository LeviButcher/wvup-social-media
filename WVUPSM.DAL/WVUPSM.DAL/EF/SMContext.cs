﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.EF
{
    /// <summary>
    ///     WVUP Social Media Connection setup object
    /// </summary>
    public class SMContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        ///    Connection string used for Development
        /// </summary>
        protected string connection = @"Server=(localdb)\mssqllocaldb;Database=WVUPSM;Trusted_connection=True;MultipleActiveResultSets=true;";

        /// <summary>
        ///     Table of <see cref="User"/> in Database
        /// </summary>
        public DbSet<User> UserAccounts { get; set; }

        /// <summary>
        ///     Table of <see cref="Follow"/>in Database
        /// </summary>
        public DbSet<Follow> Follows { get; set; }

        /// <summary>
        ///     Table of <see cref="Post"/> in Database
        /// </summary>
        public DbSet<Post> Posts { get; set; }
        /// <summary>
        ///     Table of <see cref="Group"/> in Database
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        ///     Table of <see cref="UserGroup"/> in Database
        /// </summary>
        public DbSet<UserGroup> UserGroups { get; set; }

        /// <summary>
        ///     Table of <see cref="Comment"/> in Database
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        ///     Table of <see cref="Message"/> in Database
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        ///     Table of <see cref="File"/> in Database
        /// </summary>
        public DbSet<File> Files { get; set; }

        /// <summary>
        ///     Table of <see cref="Tag"/> in Database
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        ///     Table of <see cref="Notification"/> in Database
        /// </summary>
        public DbSet<Notification> Notifications {get;set;}


        /// <summary>
        ///     Database Context
        /// </summary>
        public SMContext()
        {

        }

        /// <summary>
        ///     Overloaded constructer for providing Database construction options.
        /// </summary>
        /// <param name="options"></param>
        public SMContext(DbContextOptions options) : base (options)
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception)
            {
                //Book says do something intelligent here
            }
        }

        /// <summary>
        ///     Called upon setup of Database, make sure Database is good to go
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
            }
        }

        /// <summary>
        ///     Hook method open creation of database, setup custom sql properties here
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>(entity =>
           {
               entity.Property(e => e.DateCreated)
               .HasDefaultValueSql("getdate()");
           });

            builder.Entity<Group>(entity =>
           {
               entity.Property(e => e.DateCreated)
               .HasDefaultValueSql("getdate()");
           });

            builder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("getdate()");
            });

            builder.Entity<Message>(entity =>
            {
                entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("getdate()");
            });

            builder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("getdate()");
            });

            builder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Name)
                .IsUnique();
            });

            builder.Entity<UserTag>().HasKey(key => new { key.UserId, key.TagId });

            builder.Entity<UserGroup>().HasKey(key => new { key.UserId, key.GroupId});

            builder.Entity<UserGroup>()
                .HasOne(e => e.User)
                .WithMany(e => e.Groups)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>().HasKey(key => new { key.UserId, key.FollowId });

            builder.Entity<Follow>()
                .HasOne(e => e.User)
                .WithMany(e => e.Following)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasKey(key => new { key.Id, key.ReceiverId, key.SenderId});

            builder.Entity<Message>()
               .HasOne(e => e.Recipient)
               .WithMany(e => e.RecievedMessages)
               .HasForeignKey(e => e.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(e => e.InteractingUser)
                .WithMany(e => e.Interactions)
                .HasForeignKey(e => e.InteractingUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
               .HasOne(e => e.User)
               .WithMany(e => e.Notifications)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Notification>()
                .HasOne(e => e.Comment)
                .WithOne(e => e.InvolvedNotification)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(builder);
        }
    }
}
