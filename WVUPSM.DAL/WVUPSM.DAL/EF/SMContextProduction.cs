using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WVUPSM.Models.Entities;


namespace WVUPSM.DAL.EF
{
    /// <summary>
    ///     WVUP Social Media Connection Production setup object
    /// </summary>
    public class SMContextProduction : SMContext
    {
        /// <summary>
        ///     Default Constructor
        /// </summary>
        public SMContextProduction()
        {
           connection = @"Server=wvupsmdb;Database=WVUPSMPro;User=sa;Password=Develop@90";
        }

        /// <summary>
        ///     Overloaded constructer for providing Database construction options.
        /// </summary>
        /// <param name="options"></param>
        public SMContextProduction(DbContextOptions options) : base(options)
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
    }
}
