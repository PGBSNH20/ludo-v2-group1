using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Logic;
using Ludo.API.Models;
using LudoAPI;
using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;

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
                var board = await _context.Board.Where(b => b.BoardName == gameName).FirstAsync();
                if (board == null) return Task.FromException(new ArgumentException("There is no such game."));
                board.Squares = GameFactory.CreateSquares();

                var players = _context.Player.Where(p => p.BoardId == board.Id).Include(t => t.Tokens).ToList();

                // Validate number of players
                if (players.Count() >= 4) return Task.FromException(new ArgumentException("Exceeded the number of players"));

                //Validate player name
                playerName = playerName.Trim();
                string sameNameInDB = players.Select(p => p.Name).FirstOrDefault(name => name.ToLower() == playerName.ToLower());
                if (string.IsNullOrEmpty(playerName) || !string.IsNullOrEmpty(sameNameInDB) || string.IsNullOrWhiteSpace(playerName))
                    return Task.FromException(new ArgumentException("Choose a different name"));

                // Validate tokens color
                TokenColor selectedColor = Utility.ColorFromStringToEnum(color);
                if (HasThisColor(selectedColor, players)) return Task.FromException(new ArgumentException("This color has already been chosen by another player"));

                var player = GameFactory.NewPlayer(playerName, board, selectedColor);
                foreach (Token t in player.Tokens)
                {
                    t.Route = GameFactory.GetRoute(selectedColor);
                    t.SquareId = t.Route[t.Steps];
                }

                board.Players.Add(player);
                board.PlayerTurnName = player.Name;
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(new ArgumentException(e.Message));
            }

        }
        public async Task<List<Player>> GetPlayers(string gameName)
        {
            return await _context.Board.Where(n => n.BoardName == gameName).Include(p => p.Players).SelectMany(p => p.Players).ToListAsync();
        }
        public async Task<string> GetPlayerTurn(string gameName)
        {
            Board board = await _context.Board.FirstOrDefaultAsync(n => n.BoardName == gameName);
            if (board == null) return "";
            return board.PlayerTurnName;
        }
        public async Task<Task> AddPlayerTurnName(string gameName, string playerName)
        {
            Board board = await _context.Board.Where(n => n.BoardName == gameName).Include(p => p.Players).FirstOrDefaultAsync();
            if (board == null) return Task.FromException(new ArgumentException());
            var list = board.Players;
            int index = list.FindIndex(x => x.Name.ToLower().Equals(playerName.ToLower()));
            if (index == -1) return Task.FromException(new ArgumentException());
            if (index + 1 > list.Count - 1)
                index = -1;
            board.PlayerTurnName = list[index + 1].Name;
            await _context.SaveChangesAsync();
            return Task.CompletedTask;
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
