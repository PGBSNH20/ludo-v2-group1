using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Models;

namespace LudoAPI
{
    public class LudoContext : DbContext
    {
        public LudoContext(DbContextOptions<LudoContext> options) : base(options)
        {
        }
        public DbSet<Board> Board { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Token> Token { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) // for tests
        {

        }
    }
}
