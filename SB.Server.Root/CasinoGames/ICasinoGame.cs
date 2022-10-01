namespace SB.Server.Root.CasinoGames;

public interface ICasinoGame
{
    Guid Id { get; set; }
    string Name { get; set; }
    bool HasSubType { get; set; }

    //TODO move this to its own interface, gotta keep em separated video
    bool BasicDataEquals(object? obj);
}