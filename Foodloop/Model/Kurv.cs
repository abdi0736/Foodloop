namespace Foodloop.Models
{
    public class KurvItem
    {
        public int MadID { get; set; }
        public int BodID { get; set; }
        public string Navn { get; set; } = string.Empty;
        public decimal Pris { get; set; }
        public int Antal { get; set; }

        public decimal TotalPris => Pris * Antal;

        public KurvItem() { }

        public KurvItem(int madId, int bodId, string navn, decimal pris, int antal = 1)
        {
            MadID = madId;
            BodID = bodId;
            Navn = navn;
            Pris = pris;
            Antal = antal;
        }

        public override string ToString()
        {
            return $"{Antal} x {Navn} á {Pris} kr. (Total: {TotalPris} kr.)";
        }
    }
}