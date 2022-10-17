using SB.Server.Common.Managers;

namespace SB.Server.Root.Casinos;

public interface ICasinoManager : IManager
{
    Task<List<ICasino>> GetAllCasinos();
    Task<ICasino> GetCasinoByIdAsync( Guid id );
    //country, state, etc
    Task<bool> UpsertCasino( ICasino casino );

    Task<bool> DeleteCasino( Guid casinoId );

}