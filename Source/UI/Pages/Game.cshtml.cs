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
        private readonly ILudoDataAccess _ludoData;
        public GameModel(ILudoDataAccess ludoData)
        {
            _ludoData = ludoData;
        }
        public Board Board { get; set; }
        public async Task<ActionResult> OnGetAsync(string boardName) // Get name from url, make API call to find matching board in database.
        {
            //Make API call to get board here
            Board = await _ludoData.GetGameAsync(boardName);
            if (Board == null)
                return RedirectToPage("Index");

            return Page();
        }
        public ActionResult OnPostNewGame()
        {
            return RedirectToPage("NewGame");
        }
    }
}
