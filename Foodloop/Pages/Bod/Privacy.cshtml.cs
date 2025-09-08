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

    public List<Bod> Boder { get; set; } = new();

    public void OnGet()
    {
        // Typisk hentes dette fra database eller service
        Boder = new List<Bod>
        {
            new Bod { Id = 1, Navn = "Bod A", MadType = "Sushimushi", Latitude = 55.6171, Longitude = 12.0796 },
            new Bod { Id = 2, Navn = "Bod B", MadType = "KØD", Latitude = 55.6180, Longitude = 12.0785 },
            new Bod { Id = 3, Navn = "Bod C", MadType = "Vegan", Latitude = 55.6165, Longitude = 12.0802 },
            new Bod { Id = 4, Navn = "Bod D", MadType = "Thai food", Latitude = 55.6177, Longitude = 12.0811 }
        };
    }
}