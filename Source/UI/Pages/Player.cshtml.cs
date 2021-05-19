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
        public Board Board { get; set; }
        [BindProperty]
        public PlayerTokenColor NameColor { get; set; }
        public string ErrorMessage { get; set; }
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
            return Page();
        }
        public async Task<IActionResult> OnPostAddPlayerAsync(string gameName)
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _ludoData.PostPlayer(gameName, NameColor);
            ErrorMessage = response.Content;
            //Make API call to get board
            Board = await _ludoData.GetGameAsync(gameName);
            if (Board == null)
                return RedirectToPage("Index");
            
            return Page();
        }
        public ActionResult OnPostStartGame(string gameName)
        {
            return RedirectToPage("Game", new {boardName = gameName });
        }
    }
}
