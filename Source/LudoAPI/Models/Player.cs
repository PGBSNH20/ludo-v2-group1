using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LudoAPI.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public List<Token> Tokens { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
    public class PlayerTokenColor
    {
        public string PlayerName { get; set; }
        public string TokenColor { get; set; }
    }
}
