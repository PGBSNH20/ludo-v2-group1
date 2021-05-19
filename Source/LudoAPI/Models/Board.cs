using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LudoAPI.Models
{
    public class Board
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string? BoardName { get; set; }

        public List<Player> Players { get; set; } = new();

        [NotMapped]
        public List<Square> Squares { get; set; } = new();

        public int PlayerIDLastMadeMove { get; set; } // ID of the player who made the last move in the game
        public string PlayerTurnName { get; set; }

        [NotMapped]
        public Player? LastMadeMove { get; set; } // A player who made the last move in the game
    }
    public class Square
    {
        public int Id { get; set; } // Can be 0-55, 101-106, 201-206, 301-306, 401-406
        public List<Token> Occupants { get; set; } = new(); // A list with tokens that are on this square.
    }
}
