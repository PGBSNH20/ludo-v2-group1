using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ludo.Web.Pages
{
    public class NewGameModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public Board Board { get; set; }
        [BindProperty]
        public int Players { get; set; } // Remember how many players were set to participate

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            //Add API functions here

            return RedirectToPage($"Game", new {Board.BoardName, Players});
        }
    }
}
