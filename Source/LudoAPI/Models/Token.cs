using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Logic;
using Ludo.API.Models;

namespace LudoAPI.Models
{
    public class Token
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public bool IsActive { get; set; } // False - token is on the base; true - token is in play; if a token has reached finish it removes from the game
        public int Steps { get; set; } // The number of squares that token has passed. Steps kan be from 0 to 56
        public int SquareID { get; set; }
        public TokenColor Color { get; set; }
        public List<Route> Route { get; set; } // Array with squareIDs that token needs to pass through. Route depends on tokens color.
    }
}