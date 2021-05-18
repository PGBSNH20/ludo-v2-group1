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
        private readonly ILudoDataAccess _ludoData;
        public NewGameModel(ILudoDataAccess ludoData)
        {
            _ludoData = ludoData;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public Board Board { get; set; }

        public async Task<IActionResult> OnPostAsync() // After submitting form, create a board and add it to the database.
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _ludoData.AddBoard(Board.BoardName);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Player", new { gameName = Board.BoardName });
            return Page();
        }
    }
}
