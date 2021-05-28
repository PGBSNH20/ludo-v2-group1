using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Models;
using LudoAPI.Models;

namespace Ludo.API.Logic
{
    public static class Movement
    {
        public static string Move(Board board, Player player, Token token, int dice)
        {
            string push = "";
            // If token is on the base
            if (!token.IsActive)
            {
                if (dice != 6)
                {
                    return "You can't move this token out of the base because the dice didn't hit 6!";
                }

                var startSquareForThisToken = board.Squares.First(s => s.Id == token.Route[0]); // A square from which the token starts moving on the board
                int numberOfOccupants = startSquareForThisToken.Occupants.Count; // Number of tokens that are already on the start square for this token
                if (numberOfOccupants == 2)
                {
                    return "Start square is blocked!";
                }
                if (numberOfOccupants == 1 && startSquareForThisToken.Occupants[0].Color != token.Color) // If one opponents token is on the start square - push the opponents token to its base
                {
                    Push(startSquareForThisToken);
                    push = "Push! ";
                }

                token.IsActive = true; // Token is in play
                startSquareForThisToken.Occupants.Add(token);
                return (push + "You made a move!");
            }
            // If token is out of the base
            List<Square> shortRoute = GetShortRoute(board, dice, token); // A list with squares between a square where token is now and a square where the token should go.
            bool isShortRouteBlocked = IsBlocked(shortRoute, token);

            if (isShortRouteBlocked) return "Route is blocked!";

            var currentSquare = board.Squares.Single(s => s.Id == token.Route[token.Steps]);
            if (token.Steps + dice >= token.Route.Length - 1)
            {
                if (token.Steps + dice == token.Route.Length - 1)
                {
                    currentSquare.Occupants.Remove(token);
                    player.Tokens.Remove(token);
                    return player.Tokens.Count == 0 ? "Win!" : "Token at the finish!";
                }

                return "Token moves to the finish square only with an exact roll.";
            }
            var nextSquare = board.Squares.Single(s => s.Id == token.Route[token.Steps + dice]);

            if (nextSquare.Occupants.Count == 1 && nextSquare.Occupants[0].Color != token.Color) // If one opponents token is on the square where the token lands - push the opponents token to its base
            {
                Push(nextSquare);
                push = "Push! ";
            }
            token.Steps += dice; // Update tokens position
            token.SquareId = token.Route[token.Steps];
            nextSquare.Occupants.Add(token);
            currentSquare.Occupants.Remove(token);
            return (push + "You made a move!");
        }
        private static void Push(Square square) // Move token from square "square" to the tokens base
        {
            Token occupant = square.Occupants[0];
            occupant.IsActive = false;
            occupant.Steps = 0;
            occupant.SquareId = occupant.Route[0];
            square.Occupants.Remove(occupant);
        }

        private static List<Square> GetShortRoute(Board board, int dice, Token token) // Return a list with squares between a square where token is now and a square where the token should go. The list depends on tokens route, dice and current position.
        {
            var shortRoute = new List<Square>();
            int i = 1;
            while ((token.Steps + i) < token.Route.Length && i <= dice) // Condition ((Steps + i) < Route.Length) needs if token is near finish.
            {
                int squareID = token.Route[token.Steps + i];
                Square s = board.Squares.Single(el => el.Id == squareID);
                shortRoute.Add(s);
                i++;
            }
            return shortRoute;
        }

        private static bool IsBlocked(List<Square> shortRoute, Token token)
        {
            Square endSquare = shortRoute.Last();
            if (endSquare.Occupants.Count == 2)
            {
                return true;
            }

            foreach (Square s in shortRoute)
            {
                if (s.Occupants.Count == 2 && s.Occupants[0].Color != token.Color)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
