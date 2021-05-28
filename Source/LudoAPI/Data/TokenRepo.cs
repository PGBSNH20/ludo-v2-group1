using Ludo.API.Logic;
using Ludo.API.Models;
using LudoAPI;
using LudoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API.Data
{
    public class TokenRepo: ITokenRepo
    {
        private readonly LudoContext _context;
        public TokenRepo(LudoContext context)
        {
            _context = context;
        }
        public async Task<string> MoveToken(int tokenId, int diceNumber)
        {
            Token token = await _context.Token.Where(t => t.Id == tokenId).SingleAsync();
            int playerToMoveId = token.PlayerId;
            Player playerToMove = await _context.Player.Where(p => p.Id == playerToMoveId).SingleAsync();
            int boardId = playerToMove.BoardId;
            Board board = await _context.Board.Include(b => b.Players).ThenInclude(p => p.Tokens).Where(b => b.Id == boardId).FirstAsync();
            board.Squares = GameFactory.CreateSquares();
            foreach (var player in board.Players)
            {
                TokenColor color = player.Tokens[0].Color;
                int[] route = GameFactory.GetRoute(color);  // Set the route by finding the color of the loaded token(s).

                foreach (var t in player.Tokens)
                {
                    t.Route = route; // Assign route to each token
                    if (t.IsActive)
                    {
                        var SquareId = route[t.Steps];
                        Square square = board.Squares.Single(s => s.Id == SquareId);
                        square.Occupants.Add(t);
                    }
                }
            }

            var result = Movement.Move(board, playerToMove, token, diceNumber);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
