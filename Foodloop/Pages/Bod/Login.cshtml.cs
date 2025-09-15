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
        public string Password { get; set; } = string.Empty; // LoginKode for bod

        public string Message { get; set; } = string.Empty;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                Message = "Udfyld både email og kodeord.";
                return Page();
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // Tjek Bod (Email + LoginKode)
            var cmdBod = new SqlCommand(
                "SELECT TOP 1 * FROM Boder WHERE Email = @Email AND LoginKode = @Password",
                conn
            );
            cmdBod.Parameters.AddWithValue("@Email", Email);
            cmdBod.Parameters.AddWithValue("@Password", Password);

            using var reader = cmdBod.ExecuteReader();
            if (reader.Read())
            {
                // Bod fundet → redirect til AdminDashboard
                return RedirectToPage("/Bod/AdminDashboard");
            }

            // Ingen match
            Message = "Login mislykkedes – prøv igen.";
            return Page();
        }
    }
}