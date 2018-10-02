using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WVUPSM.Models.Entities;


namespace WVUPSM.DAL.EF
{
    public class SMContextProduction : SMContext
    {
        public SMContextProduction()
        {
            connection = @"Server=wvupsmdb;Database=WVUPSMPro;User=sa;Password=Develop@90";
        }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
            }
        }
    }
}
