using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ludo.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string NameOfGame { get; set; }
        public void OnGet()
        {

        }

        public ActionResult OnPostStartGame()
        {
            return RedirectToPage("Game", new { boardName = NameOfGame });
        }
    }
}
