using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            // Search for a game named Board.BoardName in the DB
            var boardInDB = await _ludoData.GetBoardAsync(Board.BoardName);
            
            // if there is no game with this name, create it
            if (boardInDB == null)
            {
                var response = await _ludoData.AddBoard(Board.BoardName);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Player", new { gameName = Board.BoardName });
                return Page();
            }
            
            return RedirectToPage("Player", new { gameName = Board.BoardName });
        }
    }
}
