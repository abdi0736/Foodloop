using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Foodloop.Pages.Bod
{
    public class OpretModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        [BindProperty]
        public string ArmbandKode { get; set; } = string.Empty;

        [BindProperty]
        public string Efternavn { get; set; } = string.Empty;

        [BindProperty]
        public string Telefon { get; set; } = string.Empty;

        [BindProperty]
        public string Adgangskode { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ArmbandKode) || string.IsNullOrEmpty(Efternavn) ||
                string.IsNullOrEmpty(Telefon) || string.IsNullOrEmpty(Adgangskode))
            {
                Message = "Alle felter skal udfyldes.";
                return Page();
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // find næste KundeID
            var getMaxIdCmd = new SqlCommand("SELECT ISNULL(MAX(KundeID), 0) + 1 FROM Kunder", conn);
            int nextId = (int)getMaxIdCmd.ExecuteScalar();

            var cmd = new SqlCommand(
                "INSERT INTO Kunder (KundeID, ArmbandKode, Efternavn, Telefon, Adgangskode) " +
                "VALUES (@KundeID, @ArmbandKode, @Efternavn, @Telefon, @Adgangskode)",
                conn
            );
            cmd.Parameters.AddWithValue("@KundeID", nextId);
            cmd.Parameters.AddWithValue("@ArmbandKode", ArmbandKode);
            cmd.Parameters.AddWithValue("@Efternavn", Efternavn);
            cmd.Parameters.AddWithValue("@Telefon", Telefon);
            cmd.Parameters.AddWithValue("@Adgangskode", Adgangskode);

            try
            {
                cmd.ExecuteNonQuery();
                Message = "Kunde oprettet succesfuldt!";
                return RedirectToPage("/Kunde/Login");
            }
            catch (SqlException ex)
            {
                Message = "Fejl under oprettelse: " + ex.Message;
                return Page();
            }
        }
    }
}
