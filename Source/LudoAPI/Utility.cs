using LudoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ludo.API
{
    public class Utility
    {
        public static TokenColor ColorFromStringToEnum(string selectedColorString)
        {
            var colorsArrayEnum = Enum.GetValues(typeof(TokenColor)); // Array of colors
            return colorsArrayEnum.Cast<TokenColor>().FirstOrDefault(c => c.ToString().ToLower() == selectedColorString.ToLower()); // Return the color that matches input color
        }
    }
}
