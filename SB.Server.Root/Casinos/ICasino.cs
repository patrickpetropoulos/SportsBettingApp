namespace SB.Server.Root.Casinos;

public interface ICasino
{
    Guid Id { get; set; }
    string Name { get; set; }
    string CountryCode { get; set; }

    //TODO move this to its own interface, gotta keep em separated video
    bool BasicDataEquals(object? obj);
}