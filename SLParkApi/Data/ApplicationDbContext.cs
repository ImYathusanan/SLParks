using Microsoft.EntityFrameworkCore;
using SLParkApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLParkApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<NationalPark> NationalParks { get; set; }

        public DbSet<Trail> Trails { get; set; }
    }
}
