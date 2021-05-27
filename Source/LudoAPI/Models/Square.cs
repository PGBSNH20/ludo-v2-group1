using System.Collections.Generic;
using LudoAPI.Models;

namespace Ludo.API.Models
{
    public class Square
    {
        public int Id { get; set; }
        public List<Token> Occupants { get; set; } = new(); // A list with tokens that are on this square.
    }
}