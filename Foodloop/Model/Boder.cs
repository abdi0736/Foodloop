namespace Foodloop.Models;

public enum BodStatus
{
    Aaben,
    Optaget,
    Lukket
}

public class Bod
{
    // Felter
    private int _id;
    private string _navn;
    private string _kategori;
    private BodStatus _status;

    // Properties
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Navn
    {
        get { return _navn; }
        set { _navn = value; }
    }

    public string Kategori
    {
        get { return _kategori; }
        set { _kategori = value; }
    }

    public BodStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    // Default constructor
    public Bod()
    {
        _id = 0;
        _navn = "";
        _kategori = "";
        _status = BodStatus.Aaben;
    }

    // Parameterized constructor
    public Bod(int id, string navn, string kategori, BodStatus status)
    {
        _id = id;
        _navn = navn;
        _kategori = kategori;
        _status = status;
    }
}