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
            var token11 = new Token { Id = 1, IsActive = true, Color = TokenColor.Red, Steps = 1, PlayerId = 1, SquareID = 1};
            var token12 = new Token { Id = 2, IsActive = true, Color = TokenColor.Red, Steps = 1, PlayerId = 1, SquareID = 1 };
            var token13 = new Token { Id = 3, IsActive = true, Color = TokenColor.Red, Steps = 17, PlayerId = 1, SquareID = 17 };
            var token14 = new Token { Id = 4, IsActive = true, Color = TokenColor.Red, Steps = 18, PlayerId = 1, SquareID = 18 };
            
            var token21 = new Token { Id = 5, IsActive = true, Color = TokenColor.Blue, Steps = 11, PlayerId = 2, SquareID = 50 };
            var token22 = new Token { Id = 6, IsActive = true, Color = TokenColor.Blue, Steps = 25, PlayerId = 2 , SquareID = 12};
            var token23 = new Token { Id = 7, IsActive = false, Color = TokenColor.Blue, Steps = 0, PlayerId = 2, SquareID = 0 };
            var token24 = new Token { Id = 8, IsActive = false, Color = TokenColor.Blue, Steps = 0, PlayerId = 2, SquareID = 0 };
          
            var token31 = new Token { Id = 9, IsActive = true, Color = TokenColor.Green, Steps = 0, PlayerId = 3, SquareID = 13 };
            var token32 = new Token { Id = 10, IsActive = true, Color = TokenColor.Green, Steps = 1, PlayerId = 3, SquareID = 14 };
            var token33 = new Token { Id = 11, IsActive = true, Color = TokenColor.Green, Steps = 2, PlayerId = 3, SquareID = 15 };
            var token34 = new Token { Id = 12, IsActive = true, Color = TokenColor.Green, Steps = 3, PlayerId = 3, SquareID = 16 };
            
            var token41 = new Token { Id = 13, IsActive = true, Color = TokenColor.Yellow, Steps = 51, PlayerId = 4, SquareID = 301 };
            var token42 = new Token { Id = 14, IsActive = true, Color = TokenColor.Yellow, Steps = 52, PlayerId = 4, SquareID = 302 };
            var token43 = new Token { Id = 15, IsActive = true, Color = TokenColor.Yellow, Steps = 53, PlayerId = 4, SquareID = 303 };
            var token44 = new Token { Id = 16, IsActive = true, Color = TokenColor.Yellow, Steps = 54, PlayerId = 4, SquareID = 304 };
            
            var player1 = new Player { Id = 1, Name = "playerRed", BoardId = 1 };
            var player2 = new Player { Id = 2, Name = "playerBlue", BoardId = 1 };
            var player3 = new Player { Id = 3, Name = "playerGreen", BoardId = 1 };
            var player4 = new Player { Id = 4, Name = "playerYellow", BoardId = 1 };
            var game = new Board { Id = 1, BoardName = "game1", PlayerTurnName = "playerRed"};
            modelBuilder.Entity<Token>().HasData(token11, token12, token13, token14,
                                                token21, token22, token23, token24,
                                                token31, token32, token33, token34,
                                                token41, token42, token43, token44);
            modelBuilder.Entity<Player>().HasData(player1, player2, player3, player4);
            modelBuilder.Entity<Board>().HasData(game);
        }
    }
}
