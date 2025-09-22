using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Foodloop.Pages.Kunde
{
    public class KundeDashboardModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        public string ArmbandKode { get; set; } = string.Empty;
        public string Efternavn { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            ArmbandKode = HttpContext.Session.GetString("ArmbandKode") ?? "";
            Efternavn = HttpContext.Session.GetString("Efternavn") ?? "";
            Telefon = HttpContext.Session.GetString("Telefon") ?? "";

            if (string.IsNullOrEmpty(ArmbandKode))
            {
                // Ingen aktiv session → send tilbage til login
                return RedirectToPage("/Bod/KundeLogin");
            }

            // Her kan du evt. hente kundens ordre fra databasen
            return Page();
        }
        
        public IActionResult OnPostLogout()
        {
            // ryd sessionen
            HttpContext.Session.Clear();

            // send tilbage til login-siden for kunde
            return RedirectToPage("/Bod/KundeLogin");
        }
    }
}