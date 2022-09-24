namespace SB.Server.Root.Casinos;

public interface ICasino
{
    int Id { get; set; }
    string Name { get; set; }
    string CountryCode { get; set; }
}