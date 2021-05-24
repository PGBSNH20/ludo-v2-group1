using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI.Models;

namespace Ludo.API.Logic
{
    public static class Movement
    {
        public static string Move(Board board, Player player, int dice)
        {
            string push = "";
            // If token is on the base
            if (!player.Tokens[0].IsActive)
            {
                if (dice != 6)
                {
                    return "You can't move this token out of the base because the dice didn't hit 6!";
                }

                var startSquareForThisToken =
                    board.Squares.First(s =>
                        s.Id == player.Tokens[0].Route[0]); // A square from which the token starts moving on the board
                int numberOfOccupants =
                    startSquareForThisToken.Occupants
                        .Count; // Number of tokens that are already on the start square for this token
                if (numberOfOccupants == 2)
                {
                    return "Start square is blocked!";
                }

                if (numberOfOccupants == 1 && startSquareForThisToken.Occupants[0].Color != player.Tokens[0].Color) // If one opponents token is on the start square - push the opponents token to its base
                {
                    Push(startSquareForThisToken);
                    push = "Push! ";
                }
            }
            List<Square> shortRoute = GetShortRoute(board, player, dice); // A list with squares between a square where token is now and a square where the token should go.
            bool isShortRouteBlocked = IsBlocked(shortRoute, player);

            if (isShortRouteBlocked) return "Route is blocked!";

            var currentSquare = board.Squares.Single(s => s.Id == player.Tokens[0].Route[player.Tokens[0].Steps]);
            if (player.Tokens[0].Steps + dice >= player.Tokens[0].Route.Length - 1)
            {
                if (player.Tokens[0].Steps + dice == player.Tokens[0].Route.Length - 1)
                {
                    currentSquare.Occupants.Remove(player.Tokens[0]);
                    player.Tokens.Remove(player.Tokens[0]);
                    return player.Tokens.Count == 0 ? "Win!" : "Token at the finish!";
                }

                return "Token moves to the home triangle only with an exact roll.";
            }
            var nextSquare = board.Squares.Single(s => s.Id == player.Tokens[0].Route[player.Tokens[0].Steps + dice]);

            if (nextSquare.Occupants.Count == 1 && nextSquare.Occupants[0].Color != player.Tokens[0].Color) // If one opponents token is on the square where the token lands - push the opponents token to its base
            {
                Push(nextSquare);
                push = "Push! ";
            }
            player.Tokens[0].Steps += dice; // Update tokens position
            nextSquare.Occupants.Add(player.Tokens[0]);
            currentSquare.Occupants.Remove(player.Tokens[0]);
            return (push + "You made a move!");
        }
        private static void Push(Square square) // Move token from square "square" to the tokens base
        {
            Token occupant = square.Occupants[0];
            occupant.IsActive = false;
            occupant.Steps = 0;
            square.Occupants.Remove(occupant);
        }

        private static List<Square> GetShortRoute(Board board, Player player, int dice) // Return a list with squares between a square where token is now and a square where the token should go. The list depends on tokens route, dice and current position.
        {
            var shortRoute = new List<Square>();
            int i = 1;
            while (player.Tokens[0].Steps + i < player.Tokens[0].Route.Length && i <= dice) // Condition ((Steps + i) < Route.Length) needs if token is near finish.
            {
                int squareID = player.Tokens[0].Route[player.Tokens[0].Steps + i];
                Square s = board.Squares.Single(el => el.Id == squareID);
                shortRoute.Add(s);
                i++;
            }
            return shortRoute;
        }

        private static bool IsBlocked(List<Square> shortRoute, Player player)
        {
            Square endSquare = shortRoute.Last();
            if (endSquare.Occupants.Count == 2)
            {
                return true;
            }

            foreach (Square s in shortRoute)
            {
                if (s.Occupants.Count == 2 && s.Occupants[0].Color != player.Tokens[0].Color)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
