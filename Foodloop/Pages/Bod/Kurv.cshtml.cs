using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Foodloop.Models;

namespace Foodloop.Pages.Kunde;
using Foodloop.Helpers;

    public class KurvModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        public List<KurvItem> Kurv { get; set; } = new();
        public decimal TotalPris => Kurv.Sum(x => x.Pris * x.Antal);

        public void OnGet()
        {
            Kurv = HttpContext.Session.GetObjectFromJson<List<KurvItem>>("kurv") ?? new List<KurvItem>();
        }

        public IActionResult OnPostBestil()
        {
            int? kundeId = HttpContext.Session.GetInt32("KundeID");
            if (kundeId == null)
            {
                return RedirectToPage("/Kunde/Login");
            }

            Kurv = HttpContext.Session.GetObjectFromJson<List<KurvItem>>("kurv") ?? new List<KurvItem>();
            if (!Kurv.Any())
            {
                TempData["Message"] = "Din kurv er tom.";
                return RedirectToPage();
            }

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            // 1. Indsæt ordre
            var cmdOrdre = new SqlCommand(
                "INSERT INTO Ordrer (KundeID, Dato) VALUES (@KundeID, GETDATE()); SELECT SCOPE_IDENTITY();",
                conn);
            cmdOrdre.Parameters.AddWithValue("@KundeID", kundeId);

            int ordreId = Convert.ToInt32(cmdOrdre.ExecuteScalar());

            // 2. Indsæt ordrelinjer
            foreach (var item in Kurv)
            {
                var cmdLinje = new SqlCommand(
                    "INSERT INTO OrdreLinje (OrdreID, MadID, Antal, Pris) VALUES (@OrdreID, @MadID, @Antal, @Pris)",
                    conn);

                cmdLinje.Parameters.AddWithValue("@OrdreID", ordreId);
                cmdLinje.Parameters.AddWithValue("@MadID", item.MadID);
                cmdLinje.Parameters.AddWithValue("@Antal", item.Antal);
                cmdLinje.Parameters.AddWithValue("@Pris", item.Pris);

                cmdLinje.ExecuteNonQuery();
            }

            // 3. Ryd kurv
            HttpContext.Session.Remove("kurv");

            return RedirectToPage("/Bod/OrdreBekraeftelse", new { id = ordreId });
        }
    }

    public class KurvItem
    {
        public int MadID { get; set; }
        public string Navn { get; set; } = "";
        public decimal Pris { get; set; }
        public int Antal { get; set; }
        public int BodID { get; set; }
    }

