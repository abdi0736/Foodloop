using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foodloop.Pages.Kunde
{
    public class OrdreBekraeftelseModel : PageModel
    {
        public int OrdreId { get; set; }

        public void OnGet(int id)
        {
            OrdreId = id;
        }
    }
}