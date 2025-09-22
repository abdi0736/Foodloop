namespace Foodloop.Models;

public class Ordre
{
    public int OrdreID { get; set; }       // Primærnøgle
    public int KundeID { get; set; }       // Fremmednøgle til Kunde
    public DateTime Dato { get; set; } = DateTime.Now; // Ordredato

    // Liste af retter i ordren
    public List<Ret> Retter { get; set; } = new();

    // Total pris beregnet fra retterne
    public decimal TotalPris => BeregnTotalPris();

    private decimal BeregnTotalPris()
    {
        decimal total = 0;
        foreach (var ret in Retter)
        {
            total += ret.Pris;
        }
        return total;
    }

    public Ordre(int ordreId, int kundeId, DateTime dato)
    {
        OrdreID = ordreId;
        KundeID = kundeId;
        Dato = dato;
        
    }
}