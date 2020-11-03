using System;
using System.Collections.Generic;
using System.Text;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Manager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Parts> Parts { get; set; }
        public DbSet<PartType> PartType { get; set; }
        public DbSet<UserParts> UserParts { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
