using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ludo.Web.Pages
{
    public class PlayerModel : PageModel
    {
        private readonly ILudoDataAccess _ludoData;
        [BindProperty]
        public Board Board { get; set; }
        [BindProperty]
        public List<Player> Players { get; set; }
        [BindProperty]
        public PlayerTokenColor nameColor { get; set; }
        public PlayerModel(ILudoDataAccess ludoData)
        {
            _ludoData = ludoData;
        }
        public async Task<ActionResult> OnGetAsync(string gameName) // Get name from url, make API call to find matching board in database.
        {
            //Make API call to get board here
            Board = await _ludoData.GetGameAsync(gameName);

            if (Board == null)
                return RedirectToPage("Index");
            Players = Board.Players;
            return Page();
        }
        public async Task<IActionResult> OnPostAddPlayerAsync(string gameName)
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _ludoData.AddPlayer(gameName, nameColor);
            string url = Url.Page($"Player", OnGetAsync(gameName));
            return Redirect(url);
        }
        public ActionResult OnPostStartGame(string gameName)
        {
            return RedirectToPage("Game", new {boardName = gameName });
        }
    }
}
