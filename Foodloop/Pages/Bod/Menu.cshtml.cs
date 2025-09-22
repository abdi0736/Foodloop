using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Foodloop.Models;
using Foodloop.Helpers;   // <-- skal stå her udenfor namespace

namespace Foodloop.Pages

{
    public class MenuModel : PageModel
    {
        private readonly string _connectionString =
            "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

        public List<Ret> Retter { get; set; } = new();
        public string BodNavn { get; set; } = "";

        public void OnGet(int id) // id = BodId
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmdBod = new SqlCommand("SELECT Navn FROM Boder WHERE BodId=@id", conn);
            cmdBod.Parameters.AddWithValue("@id", id);
            BodNavn = (string?)cmdBod.ExecuteScalar() ?? "";

            var cmd = new SqlCommand("SELECT MadID, Navn, Beskrivelse, Pris FROM Retter WHERE BodID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Retter.Add(new Ret
                {
                    MadID = reader.GetInt32(0),
                    Navn = reader.GetString(1),
                    Beskrivelse = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Pris = reader.GetDecimal(3)
                });
            }
        }

        public IActionResult OnPostAddToCart(int madId, string navn, decimal pris, int bodId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<KurvItem>>("kurv") ?? new List<KurvItem>();

            var existing = cart.FirstOrDefault(x => x.MadID == madId);
            if (existing != null)
                existing.Antal++;
            else
                cart.Add(new KurvItem { MadID = madId, Navn = navn, Pris = pris, Antal = 1, BodID = bodId });

            HttpContext.Session.SetObjectAsJson("kurv", cart);

            return RedirectToPage("/Bod/Kurv");
        }
    }
}
