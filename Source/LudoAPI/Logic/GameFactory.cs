using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ludo.API.Models;
using LudoAPI.Models;

namespace Ludo.API.Logic
{
    public class GameFactory
    {
        public static List<Token> CreateTokens(TokenColor color, Player player) // Create 4 tokens and routes, and make the first token in the list active.
        {
            var tempList = new List<Token>();
            for (int i = 0; i < 4; i++) // Add tokens, first one is set to be active, rest is inactive.
            {
                tempList.Add(i == 0 ? new Token { Color = color, IsActive = true, Route = GetRoute(color) }
                : new Token { Color = color, Route = GetRoute(color) });
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
            player.Tokens = CreateTokens(tokenColor, player); // Add tokens to the player

            switch (tokenColor) // Set starting position
            {
                case TokenColor.Blue:
                    board.Squares[39].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Yellow:
                    board.Squares[26].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Green:
                    board.Squares[13].Occupants.Add(player.Tokens[0]);
                    break;

                case TokenColor.Red:
                    board.Squares[0].Occupants.Add(player.Tokens[0]);
                    break;
            }

            return player;
        }

        public static int[] CreateRoute(int delta, int startColor) // Create array with squareIDs that token needs to pass through. Delta and startColor depends on tokens color.
        {
            int[] route = new int[57];

            for (int i = 0; i < 51; i++)
            {
                int index = i + delta;
                if (index <= 51)
                {
                    route[i] = index;
                }
                else
                {
                    route[i] = index - 52;
                }
            }
            for (int i = 51; i < 57; i++)
            {
                route[i] = startColor;
                startColor++;
            }

            return route;
        }

        public static int[] GetRoute(TokenColor color)
        {
            int[] route = new int[57];

            switch (color) // Set the route depending on token color
            {
                case TokenColor.Blue:
                    route = CreateRoute(39, 401);
                    break;

                case TokenColor.Yellow:
                    route = CreateRoute(26, 301);
                    break;

                case TokenColor.Green:
                    route = CreateRoute(13, 201);
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
            for (int i = 0; i <= 51; i++)
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
    }
}
