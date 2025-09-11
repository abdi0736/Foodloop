using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Foodloop.Pages.Bod
{
    public class LoginModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty; // armbåndkode eller loginkode

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                Message = "Udfyld både email og kodeord.";
                return Page();
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // 1) Tjek Kunde (email + armbåndkode)
            var cmdKunde = new SqlCommand(
                "SELECT TOP 1 * FROM Kunder WHERE Email = @Email AND ArmbandKode = @Password",
                conn
            );
            cmdKunde.Parameters.AddWithValue("@Email", Email);
            cmdKunde.Parameters.AddWithValue("@Password", Password);

            using var reader = cmdKunde.ExecuteReader();
            if (reader.Read())
            {
                // Kunde fundet → redirect til KundeDashboard
                return RedirectToPage("/KundeDashboard");
            }
            reader.Close();

            // 2) Tjek Bod (email + loginkode)
            var cmdBod = new SqlCommand(
                "SELECT TOP 1 * FROM Boder WHERE Email = @Email AND LoginKode = @Password",
                conn
            );
            cmdBod.Parameters.AddWithValue("@Email", Email);
            cmdBod.Parameters.AddWithValue("@Password", Password);

            using var reader2 = cmdBod.ExecuteReader();
            if (reader2.Read())
            {
                // Bod fundet → redirect til AdminDashboard
                return RedirectToPage("/AdminDashboard");
            }

            // 3) Ingen match
            Message = "Login mislykkedes – prøv igen.";
            return Page();
        }
    }
}
