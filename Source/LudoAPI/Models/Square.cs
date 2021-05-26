using System.Collections.Generic;
using LudoAPI.Models;

namespace Ludo.API.Models
{
    public class Square
    {
        public int Id { get; set; } // Can be 0-55, 101-106, 201-206, 301-306, 401-406
        public int Index { get; set; }
        public int BoardId { get; set; }
        public List<SquareOccupant> Occupants { get; set; } = new(); // A list with tokens that are on this square.
    }

    public class SquareOccupant
    {
        public int Id { get; set; }
        public int SquareId { get; set; }
        public Token Occupant { get; set; }
    }
}