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

        var cmd = new SqlCommand("SELECT BodId, Navn, Kategori, Status FROM Boder", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var statusString = reader.GetString(3);

            // Fix: konverter "Åben" til "Aaben", så Enum.Parse ikke fejler
            if (statusString == "Åben")
                statusString = "Aaben";

            Boder.Add(new Models.Bod
            {
                BodId = reader.GetInt32(0),
                Navn = reader.GetString(1),
                Kategori = reader.GetString(2),
                Status = (BodStatus)Enum.Parse(typeof(BodStatus), statusString)
            });
        }
    }

    public IActionResult OnPostAddBod()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand(
            "INSERT INTO Boder (Navn, Kategori, Status) VALUES (@Navn, @Kategori, @Status)",
            conn
        );
        cmd.Parameters.AddWithValue("@Navn", NewBod.Navn);
        cmd.Parameters.AddWithValue("@Kategori", NewBod.Kategori);
        cmd.Parameters.AddWithValue("@Status", NewBod.Status.ToString());
        cmd.ExecuteNonQuery();

        return RedirectToPage();
    }

    public IActionResult OnPostDeleteBod()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("DELETE FROM Boder WHERE BodId = @BodId", conn);
        cmd.Parameters.AddWithValue("@BodId", BodId);
        cmd.ExecuteNonQuery();

        return RedirectToPage();
    }

    public IActionResult OnPostChangeStatus()
    {
        using var conn = new SqlConnection(_connectionString);
        conn.Open();

        var cmd = new SqlCommand("UPDATE Boder SET Status=@Status WHERE BodId=@BodId", conn);
        cmd.Parameters.AddWithValue("@Status", NewStatus.ToString());
        cmd.Parameters.AddWithValue("@BodId", BodId);
        cmd.ExecuteNonQuery();

        return RedirectToPage();
    }
} 
