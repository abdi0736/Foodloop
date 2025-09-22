using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Foodloop.Models;
using Microsoft.Data.SqlClient;

namespace Foodloop.Pages.Bod;

public class AdminDashboardModel : PageModel
{
    private readonly string _connectionString =
        "Server=mssql17.unoeuro.com,1433;Database=kunforhustlers_dk_db_test;User Id=kunforhustlers_dk;Password=RmcAfptngeBaxkw6zr5E;TrustServerCertificate=True;Encrypt=True;";

    public List<Models.Bod> Boder { get; set; } = new();

    [BindProperty]
    public Models.Bod NewBod { get; set; } = new();

    [BindProperty]
    public int BodId { get; set; }

    [BindProperty]
    public BodStatus NewStatus { get; set; }

    public void OnGet()
    {
        LoadBoder();
    }

    private void LoadBoder()
    {
        Boder.Clear();
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("SELECT BodId, Navn, Kategori, Lokation, Kontaktinfo, LoginKode, Email, Status FROM Boder", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var statusString = reader.IsDBNull(7) ? "Aaben" : reader.GetString(7);
            if (statusString == "Åben") statusString = "Aaben";

            Boder.Add(new Models.Bod
            {
                BodId = reader.GetInt32(0),
                Navn = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Kategori = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Lokation = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Kontaktinfo = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                LoginKode = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                Email = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                Status = (BodStatus)Enum.Parse(typeof(BodStatus), statusString)
            });
        }
    }

    // Add Bod
    public IActionResult OnPostAddBod()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            @"INSERT INTO Boder (Navn, Lokation, Kontaktinfo, LoginKode, Email, Kategori, Status)
              VALUES (@Navn, @Lokation, @Kontaktinfo, @LoginKode, @Email, @Kategori, @Status)", conn);

        cmd.Parameters.AddWithValue("@Navn", NewBod.Navn ?? "");
        cmd.Parameters.AddWithValue("@Lokation", NewBod.Lokation ?? "");
        cmd.Parameters.AddWithValue("@Kontaktinfo", NewBod.Kontaktinfo ?? "");
        cmd.Parameters.AddWithValue("@LoginKode", NewBod.LoginKode ?? "");
        cmd.Parameters.AddWithValue("@Email", NewBod.Email ?? "");
        cmd.Parameters.AddWithValue("@Kategori", NewBod.Kategori ?? "");
        cmd.Parameters.AddWithValue("@Status", MapStatusToDb(NewBod.Status));

        cmd.ExecuteNonQuery();
        return RedirectToPage();
    }

    // Update Bod
    public IActionResult OnPostUpdateBod()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            @"UPDATE Boder SET 
                Navn=@Navn, Lokation=@Lokation, Kontaktinfo=@Kontaktinfo, LoginKode=@LoginKode, Email=@Email, Kategori=@Kategori, Status=@Status
              WHERE BodId=@BodId", conn);

        cmd.Parameters.AddWithValue("@BodId", BodId);
        cmd.Parameters.AddWithValue("@Navn", NewBod.Navn ?? "");
        cmd.Parameters.AddWithValue("@Lokation", NewBod.Lokation ?? "");
        cmd.Parameters.AddWithValue("@Kontaktinfo", NewBod.Kontaktinfo ?? "");
        cmd.Parameters.AddWithValue("@LoginKode", NewBod.LoginKode ?? "");
        cmd.Parameters.AddWithValue("@Email", NewBod.Email ?? "");
        cmd.Parameters.AddWithValue("@Kategori", NewBod.Kategori ?? "");
        cmd.Parameters.AddWithValue("@Status", MapStatusToDb(NewBod.Status));

        cmd.ExecuteNonQuery();
        return RedirectToPage();
    }

    // Delete Bod
    public IActionResult OnPostDeleteBod() 
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("DELETE FROM Boder WHERE BodId=@BodId", conn);
        cmd.Parameters.AddWithValue("@BodId", BodId);
        cmd.ExecuteNonQuery();

        return RedirectToPage();
    }

    // Change status separat
    public IActionResult OnPostChangeStatus(string NewStatus)
    {
        if (!Enum.TryParse<BodStatus>(NewStatus, out var status))
        {
            status = BodStatus.Aaben; // fallback hvis parsing fejler
        }

        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("UPDATE Boder SET Status=@Status WHERE BodId=@BodId", conn);
        cmd.Parameters.AddWithValue("@Status", MapStatusToDb(status));
        cmd.Parameters.AddWithValue("@BodId", BodId);
        cmd.ExecuteNonQuery();

        return RedirectToPage();
    }


    private string MapStatusToDb(BodStatus status)
    {
        return status switch
        {
            BodStatus.Aaben => "Åben",
            BodStatus.Optaget => "Optaget",
            BodStatus.Lukket => "Lukket",
            _ => "Åben"
        };
    }
}
