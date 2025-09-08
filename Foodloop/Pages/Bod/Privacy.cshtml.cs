using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foodloop.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public List<Models.Bod> Boder { get; set; } = new();

    public void OnGet()
    {
        // Typisk hentes dette fra database eller service
        Boder = new List<Models.Bod>
        {
            new Models.Bod { Id = 1, Navn = "Pizza Palace", Kategori = "Sushimushi", Latitude = 55.6171, Longitude = 12.0796, Status = Models.BodStatus.Aaben },
            new Models.Bod { Id = 2, Navn = "Burger Bar", Kategori = "KØD", Latitude = 55.6180, Longitude = 12.0785, Status = Models.BodStatus.Optaget },
            new Models.Bod { Id = 3, Navn = "Sushi Station", Kategori = "Vegan", Latitude = 55.6165, Longitude = 12.0802, Status = Models.BodStatus.Aaben },
            new Models.Bod { Id = 4, Navn = "Taco Town", Kategori = "Thai food", Latitude = 55.6177, Longitude = 12.0811, Status = Models.BodStatus.Lukket }
        };
    }
}