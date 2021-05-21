using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var token11 = new Token { Id = 1, IsActive = true, Color = TokenColor.Red, Steps = 1, PlayerId = 1, SquareID = 0};
            var token12 = new Token { Id = 2, IsActive = true, Color = TokenColor.Red, Steps = 2, PlayerId = 1, SquareID = 2};
            var token21 = new Token { Id = 3, IsActive = false, Color = TokenColor.Blue, Steps = 0, PlayerId = 2, SquareID = 0 };
            var token22 = new Token { Id = 4, IsActive = true, Color = TokenColor.Blue, Steps = 1, PlayerId = 2 , SquareID = 30};
            var player1 = new Player { Id = 1, Name = "playerRed", BoardId = 1 };
            var player2 = new Player { Id = 2, Name = "playerBlue", BoardId = 1 };
            var game = new Board { Id = 1, BoardName = "game1"};
            modelBuilder.Entity<Token>().HasData(token11, token12, token21, token22);
            modelBuilder.Entity<Player>().HasData(player1, player2);
            modelBuilder.Entity<Board>().HasData(game);
        }
    }
}
