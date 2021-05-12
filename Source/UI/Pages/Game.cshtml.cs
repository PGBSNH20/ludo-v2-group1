using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ludo.Web.Pages
{
    public class GameModel : PageModel
    {
        public string BoardName { get; set; }
        public int Players { get; set; }
        public void OnGet(string boardName, int players)
        {
            BoardName = boardName;
            Players = players;
            //Make API call to get board here
        }
    }
}
