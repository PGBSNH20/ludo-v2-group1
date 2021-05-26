using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Models;

namespace LudoAPI.Models
{
    public class Board
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string? BoardName { get; set; }

        public List<Player> Players { get; set; } = new();

        public List<Square> Squares { get; set; } = new();

        public int PlayerIDLastMadeMove { get; set; } // ID of the player who made the last move in the game
        public string PlayerTurnName { get; set; }
        [NotMapped]
        public Player? LastMadeMove { get; set; } // A player who made the last move in the game
    }
}
