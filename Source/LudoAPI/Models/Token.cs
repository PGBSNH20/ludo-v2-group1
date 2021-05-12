using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Logic;

namespace LudoAPI.Models
{
    public class Token
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public bool IsActive { get; set; } // False - token is on the base; true - token is in play; if a token has reached finish it removes from the game
        public int Steps { get; set; } // The number of squares that token has passed. Steps kan be from 0 to 60
        public TokenColor Color { get; set; }

        [NotMapped]
        public int[] Route { get; set; } // Array with squareIDs that token needs to pass through. Route depends on tokens color.

        public string Move(Board board, Player player, int dice) // Move token, return a string with result information
        {
            string push = "";
            // If token is on the base
            if (!IsActive)
            {
                if (dice != 6)
                {
                    return "You can't move this token out of the base because the dice didn't hit 6!";
                }

                var startSquareForThisToken = board.Squares.First(s => s.Id == Route[0]); // A square from which the token starts moving on the board
                int numberOfOccupants = startSquareForThisToken.Occupants.Count; // Number of tokens that are already on the start square for this token
                if (numberOfOccupants == 2)
                {
                    return "Start square is blocked!";
                }
                if (numberOfOccupants == 1 && startSquareForThisToken.Occupants[0].Color != this.Color) // If one opponents token is on the start square - push the opponents token to its base
                {
                    Push(startSquareForThisToken);
                    push = "Push! ";
                }

                this.IsActive = true; // Token is in play
                startSquareForThisToken.Occupants.Add(this);
                return (push + "You made a move!");
            }
            // If token is out of the base
            List<Square> shortRoute = GetShortRoute(board, dice); // A list with squares between a square where token is now and a square where the token should go.
            bool isShortRouteBlocked = IsBlocked(shortRoute);

            if (isShortRouteBlocked) return "Route is blocked!";

            var currentSquare = board.Squares.Single(s => s.Id == Route[Steps]);
            if (Steps + dice >= Route.Length - 1)
            {
                if (Steps + dice == Route.Length - 1)
                {
                    currentSquare.Occupants.Remove(this);
                    player.Tokens.Remove(this);
                    return player.Tokens.Count == 0 ? "Win!" : "Token at the finish!";
                }

                return "Token moves to the home triangle only with an exact roll.";
            }
            var nextSquare = board.Squares.Single(s => s.Id == Route[Steps + dice]);

            if (nextSquare.Occupants.Count == 1 && nextSquare.Occupants[0].Color != Color) // If one opponents token is on the square where the token lands - push the opponents token to its base
            {
                Push(nextSquare);
                push = "Push! ";
            }
            Steps += dice; // Update tokens position
            nextSquare.Occupants.Add(this);
            currentSquare.Occupants.Remove(this);
            return (push + "You made a move!");
        }

        private static void Push(Square square) // Move token from square "square" to the tokens base
        {
            Token occupant = square.Occupants[0];
            occupant.IsActive = false;
            occupant.Steps = 0;
            square.Occupants.Remove(occupant);
        }

        private List<Square> GetShortRoute(Board board, int dice) // Return a list with squares between a square where token is now and a square where the token should go. The list depends on tokens route, dice and current position.
        {
            var shortRoute = new List<Square>();
            int i = 1;
            while ((Steps + i) < Route.Length && i <= dice) // Condition ((Steps + i) < Route.Length) needs if token is near finish.
            {
                int squareID = Route[Steps + i];
                Square s = board.Squares.Single(el => el.Id == squareID);
                shortRoute.Add(s);
                i++;
            }
            return shortRoute;
        }

        private bool IsBlocked(List<Square> shortRoute)
        {
            Square endSquare = shortRoute.Last();
            if (endSquare.Occupants.Count == 2)
            {
                return true;
            }

            foreach (Square s in shortRoute)
            {
                if (s.Occupants.Count == 2 && s.Occupants[0].Color != this.Color)
                {
                    return true;
                }
            }
            return false;
        }
    }
}