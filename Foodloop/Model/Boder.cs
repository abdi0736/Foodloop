namespace Foodloop.Models
{
    public enum BodStatus
    {
        Aaben,
        Optaget,
        Lukket
    }

    public class Bod
    {
        // Properties
        public int Id { get; set; }
        
        public int BodId { get; set; }
        public string Navn { get; set; } = string.Empty;

        // Kategori kan være en bredere inddeling (fx "Streetfood")
        public string Kategori { get; set; } = string.Empty;

        // MadType mere specifik (fx "Sushi", "Vegan")

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public BodStatus Status { get; set; } = BodStatus.Aaben;

        // Default constructor
        public Bod() { }

        // Parameterized constructor
        public Bod(int id,int bodid, string navn, string kategori, double latitude, double longitude, BodStatus status)
        {
            Id = id;
            Navn = navn;
            Kategori = kategori;
            Latitude = latitude;
            Longitude = longitude;
            Status = status;
            BodId = bodid;

        }
    }
}