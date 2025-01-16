using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DawidKrolikiewiczProj.Models;

namespace DawidKrolikiewiczProj.Data
{
    public class DawidKrolikiewiczProjContext : DbContext
    {
        public DawidKrolikiewiczProjContext (DbContextOptions<DawidKrolikiewiczProjContext> options)
            : base(options)
        {
        }

        public DbSet<DawidKrolikiewiczProj.Models.Posel> Posel { get; set; } = default!;
    }
}
