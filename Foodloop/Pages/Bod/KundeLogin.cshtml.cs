using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Foodloop.Pages.Kunde
{
    public class LoginModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        [BindProperty] public string ArmbandKode { get; set; } = string.Empty;
        [BindProperty] public string Adgangskode { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ArmbandKode) || string.IsNullOrEmpty(Adgangskode))
            {
                Message = "Udfyld både armbåndskode og adgangskode.";
                return Page();
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT TOP 1 ArmbandKode, Efternavn, Telefon FROM Kunder WHERE ArmbandKode = @ArmbandKode AND Adgangskode = @Adgangskode",
                conn
            );
            cmd.Parameters.AddWithValue("@ArmbandKode", ArmbandKode);
            cmd.Parameters.AddWithValue("@Adgangskode", Adgangskode);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                // Gem i session
                HttpContext.Session.SetString("ArmbandKode", reader.GetString(0));
                HttpContext.Session.SetString("Efternavn", reader.GetString(1));
                HttpContext.Session.SetString("Telefon", reader.GetString(2));

                return RedirectToPage("/Bod/KundeDashboard");
            }

            Message = "Login mislykkedes – prøv igen.";
            return Page();
        }

        // Logout handler
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Bod/KundeLogin"); // sender tilbage til login
        }
    }
}
