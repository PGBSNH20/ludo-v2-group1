using LudoAPI;
using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public class PlayerRepo : IPlayerRepo
    {
        private readonly LudoContext _context;
        public PlayerRepo(LudoContext context)
        {
            _context = context;
        }
        public async Task<Task> AddPlayer(string playerName, string gameName, string color)
        {
            try
            {
                // Validate game name
                gameName = gameName.ToLower();
                var board = _context.Board.FirstOrDefault(b => b.BoardName == gameName);
                if (board == null) return Task.FromException(new ArgumentException("There is no such game."));
                
                var players = _context.Player.Where(p => p.BoardId == board.Id).Include(t => t.Tokens).ToList();
                // Validate number of players
                if (players.Count() >= 4) return Task.FromException(new ArgumentException("Exceeded the number of players"));
                // Validate tokens color
                TokenColor selectedColor = Utility.ColorFromStringToEnum(color);
                if (HasThisColor(selectedColor, players)) return Task.FromException(new ArgumentException("This color has already been chosen by another player"));
                
                var player = new Player();
                player.Name = playerName;
                player.Tokens = Logic.GameFactory.CreateTokens(selectedColor);
                board.Players.Add(player);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch
            {
                return Task.FromException(new ArgumentException("Bad request"));
            }

        }
        private static bool HasThisColor(TokenColor color, List<Player> players)
        {
            foreach (Player p in players)
            {
                foreach (Token t in p.Tokens)
                {
                    if (t.Color == color)
                        return true;
                }
            }
            return false;
        }
    }
}
