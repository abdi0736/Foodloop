namespace Foodloop.Models
{
    public class Ret
    {
        public int MadID { get; set; }
        public int BodID { get; set; }
        public string Navn { get; set; } = string.Empty;
        public string Beskrivelse { get; set; } = string.Empty;
        public decimal Pris { get; set; }

        // Parameterløs constructor til DB-mapping og model binding
        public Ret() { }

        // Brugbar til hurtig oprettelse
        public Ret(int madId, int bodId, decimal pris, string navn, string beskrivelse)
        {
            MadID = madId;
            BodID = bodId;
            Pris = pris;
            Navn = navn;
            Beskrivelse = beskrivelse;
        }

        public override string ToString()
        {
            return $"{nameof(MadID)}: {MadID}, {nameof(BodID)}: {BodID}, {nameof(Navn)}: {Navn}, {nameof(Beskrivelse)}: {Beskrivelse}, {nameof(Pris)}: {Pris}";
        }
    }
}