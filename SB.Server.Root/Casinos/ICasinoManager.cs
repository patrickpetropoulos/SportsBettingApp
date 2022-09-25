using SB.Server.Common.Managers;

namespace SB.Server.Root.Casinos;

public interface ICasinoManager : IManager
{
    Task<List<ICasino>> GetAllCasinos();
    //country, state, etc
    Task UpsertCasino(ICasino casino);

    Task<bool> DeleteCasino(int casinoId);

}