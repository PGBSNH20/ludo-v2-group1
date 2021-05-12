using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI.Models;

namespace Ludo.API.Logic
{
    public class GameFactory
    {
        private static List<Token> CreateTokens(TokenColor color, Player player) // Create 4 tokens and routes, and make the first token in the list active.
        {
            int[] route = GetRoute(color);
            var tempList = new List<Token>();
            for (int i = 0; i < 4; i++) // Add tokens, first one is set to be active, rest is inactive.
            {
                tempList.Add(i == 0 ? new Token { Color = color, IsActive = true, PlayerId = player.Id, Route = route }
                : new Token { Color = color, PlayerId = player.Id, Route = route });
            }
            return tempList;
        }

        public static Board CreateBoard(string boardName)  // Create a board, name is used to identify in database, and add 80 squares to the board.
        {
            return new() { BoardName = boardName, Squares = CreateSquares() };
        }

        public static Player NewPlayer(string name, Board board, TokenColor tokenColor) // Create a new player and add the tokens to the player
        {
            var player = new Player { Name = name };
            player.Tokens = CreateTokens(tokenColor, player); // Add tokens yo the player

            switch (tokenColor) // Set starting position
            {
                case TokenColor.Blue:
                    board.Squares[42].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Yellow:
                    board.Squares[28].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Green:
                    board.Squares[14].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Red:
                    board.Squares[0].Occupants.Add(player.Tokens[0]);
                    break;
            }

            return player;
        }

        public static int[] CreateRoute(int delta, int startColor) // Create array with squareIDs that token needs to pass through. Delta and startColor depends on tokens color.
        {
            int[] route = new int[61];
            for (int i = 0; i < 55; i++)
            {
                int index = i + delta;
                if (index <= 55)
                {
                    route[i] = index;
                }
                else
                {
                    route[i] = index - 56;
                }
            }
            for (int i = 55; i < 61; i++)
            {
                route[i] = startColor;
                startColor++;
            }

            return route;
        }

        public static int[] GetRoute(TokenColor color)
        {
            int[] route = new int[61];

            switch (color) // Set the route depending on token color
            {
                case TokenColor.Blue:
                    route = CreateRoute(42, 401);
                    break;

                case TokenColor.Yellow:
                    route = CreateRoute(28, 301);
                    break;

                case TokenColor.Green:
                    route = CreateRoute(14, 201);
                    break;

                case TokenColor.Red:
                    route = CreateRoute(0, 101);
                    break;
            }
            return route;
        }

        public static List<Square> CreateSquares()
        {
            var squares = new List<Square>();
            for (int i = 0; i <= 55; i++)
            {
                squares.Add(new Square { Id = i });
            }

            int k = 1;
            while (k < 5)
            {
                for (int j = 1; j < 7; j++)
                {
                    squares.Add(new Square { Id = 100 * k + j });
                }
                k++;
            }
            return squares;
        }
        public static List<Token> CreateTokens(TokenColor color) // Create 4 tokense and first token in the list active.
        {
            var tempList = new List<Token>();
            for (int i = 0; i < 4; i++) // Add tokens, first one is set to be active, rest is inactive.
            {
                tempList.Add(i == 0 ? new Token { Color = color, IsActive = true } : new Token { Color = color });
            }
            return tempList;
        }
    }
}
