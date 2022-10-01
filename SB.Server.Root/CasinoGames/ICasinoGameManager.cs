using SB.Server.Common.Managers;

namespace SB.Server.Root.CasinoGames;

public interface ICasinoGameManager : IManager
{
    Task<List<ICasinoGame>> GetAllCasinoGames();
    //country, state, etc
    Task<bool> UpsertCasinoGame(ICasinoGame casinoGame);

    Task<bool> DeleteCasinoGame(Guid casinoId);

}