using System;

namespace Ludo.API.Logic
{
    public class Dice
    {
        public static int RollDice()
        {
            Random rnd = new();
            int roll = rnd.Next(1, 7);
            return roll;
        }
    }
}
