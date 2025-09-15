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

        public void OnGet(string armbandKode)
        {
            if (string.IsNullOrEmpty(armbandKode))
            {
                Message = "Ingen armbåndskode angivet.";
                return;
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT TOP 1 ArmbandKode, Efternavn, Telefon FROM Kunder WHERE ArmbandKode = @ArmbandKode",
                conn
            );
            cmd.Parameters.AddWithValue("@ArmbandKode", armbandKode);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ArmbandKode = reader.GetString(0);
                Efternavn = reader.GetString(1);
                Telefon = reader.GetString(2);
            }
            else
            {
                Message = "Kunde ikke fundet.";
            }
        }
    }
}