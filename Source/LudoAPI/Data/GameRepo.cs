using LudoAPI;
using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public class GameRepo :IGameRepo
    {
        private readonly LudoContext _context;

        public GameRepo(LudoContext context)
        {
            _context = context;
        }
        public async Task<Board> GetGameByName(string name)
        {
            try
            {
                return await _context.Board
                                     .Include(b => b.Players)
                                     .ThenInclude(p => p.Tokens)
                                     .Where(b => b.BoardName == name)
                                     .FirstAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
