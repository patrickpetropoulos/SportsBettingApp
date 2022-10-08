using SB.Server.Root.Base;

namespace SB.Server.Root.CasinoGames;

public interface ICasinoGame : IEntity
{
    Guid Id { get; set; }
    string Name { get; set; }
    bool HasSubType { get; set; }
}