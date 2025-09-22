using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Foodloop.Models;

namespace Foodloop.Pages
{
    public class SamletModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        public List<Models.Bod> Boder { get; set; } = new();

        public void OnGet() 
        {
            Boder.Clear();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT BodId, Navn, Kategori, Status, Lokation, Latitude, Longitude FROM Boder", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var statusString = reader.IsDBNull(3) ? "Aaben" : reader.GetString(3);
                if (statusString == "Åben") statusString = "Aaben";

                if (!Enum.TryParse<BodStatus>(statusString, out var status))
                    status = BodStatus.Aaben;

                Boder.Add(new Models.Bod
                {
                    BodId = reader.GetInt32(0),
                    Navn = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Kategori = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Status = status,
                    Lokation = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Latitude = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Longitude = reader.IsDBNull(6) ? 0 : reader.GetDouble(6)
                });
            }
        }
    }
}