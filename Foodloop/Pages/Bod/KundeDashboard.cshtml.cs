using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Foodloop.Models;

namespace Foodloop.Pages.Kunde
{
    public class KundeDashboardModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        public string ArmbandKode { get; set; } = string.Empty;
        public string Efternavn { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;

        public List<KundeOrdre> Ordrer { get; set; } = new();

        public IActionResult OnGet()
        {
            // Tjek session
            ArmbandKode = HttpContext.Session.GetString("ArmbandKode") ?? "";
            Efternavn = HttpContext.Session.GetString("Efternavn") ?? "";
            Telefon = HttpContext.Session.GetString("Telefon") ?? "";
            int? kundeId = HttpContext.Session.GetInt32("KundeID");

            if (kundeId == null)
            {
                return RedirectToPage("/Kunde/Login");
            }

            // Hent ordrer + ordrelinjer fra DB
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(@"
                SELECT o.OrdreID, o.Dato, 
                       r.Navn AS RetNavn, ol.Antal, ol.Pris
                FROM Ordrer o
                JOIN OrdreLinje ol ON o.OrdreID = ol.OrdreID
                JOIN Retter r ON ol.MadID = r.MadID
                WHERE o.KundeID = @kundeId
                ORDER BY o.Dato DESC, o.OrdreID DESC", conn);

            cmd.Parameters.AddWithValue("@kundeId", kundeId);

            using var reader = cmd.ExecuteReader();
            var ordreDict = new Dictionary<int, KundeOrdre>();

            while (reader.Read())
            {
                int ordreId = reader.GetInt32(0);
                DateTime dato = reader.GetDateTime(1);

                if (!ordreDict.ContainsKey(ordreId))
                {
                    ordreDict[ordreId] = new KundeOrdre
                    {
                        OrdreID = ordreId,
                        Dato = dato,
                        Linjer = new List<KundeOrdreLinje>()
                    };
                }

                ordreDict[ordreId].Linjer.Add(new KundeOrdreLinje
                {
                    RetNavn = reader.GetString(2),
                    Antal = reader.GetInt32(3),
                    Pris = reader.GetDecimal(4)
                });
            }

            Ordrer = ordreDict.Values.ToList();

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Bod/KundeLogin");
        }
    }

    public class KundeOrdre
    {
        public int OrdreID { get; set; }
        public DateTime Dato { get; set; }
        public List<KundeOrdreLinje> Linjer { get; set; } = new();
    }

    public class KundeOrdreLinje
    {
        public string RetNavn { get; set; } = "";
        public int Antal { get; set; }
        public decimal Pris { get; set; }
    }
}
