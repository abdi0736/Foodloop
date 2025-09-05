using Microsoft.AspNetCore.Mvc.RazorPages;
using Foodloop.Models;

namespace Foodloop.Pages.Boder;

public class IndexModel : PageModel
{
    public List<Models.Bod> Boder { get; set; } = new();

    public void OnGet()
    {
        Boder = new List<Models.Bod>
        {
            new Models.Bod(1, "Pizza Palace", "Italiensk", BodStatus.Aaben),
            new Models.Bod(2, "Burger Bar", "Amerikansk", BodStatus.Optaget),
            new Models.Bod(3, "Taco Town", "Mexicansk", BodStatus.Aaben),
            new Models.Bod(4, "Sushi Station", "Japansk", BodStatus.Aaben),
            new Models.Bod(5, "Kaffe Kiosk", "Kaffe & Te", BodStatus.Lukket),
            new Models.Bod(6, "Grillmester", "Grill", BodStatus.Aaben)
        };
    }
}