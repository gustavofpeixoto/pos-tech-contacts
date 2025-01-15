﻿using Microsoft.EntityFrameworkCore;
using PosTech.Contacts.ApplicationCore.Entities;
using System.Reflection;

namespace PosTech.Contacts.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Ddd> Ddds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
