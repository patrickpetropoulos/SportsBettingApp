namespace SB.Server.Common.SQL;

public static class StoredProcedures
{
    #region Casinos

    public const string Casino_SelectAllCasinos = "SelectAllCasinos";
    public const string Casino_UpsertCasino = "UpsertCasino";
    public const string Casino_DeleteCasino = "DeleteCasino";

    #endregion

    #region CasinoGames

    public const string CasinoGames_SelectAllCasinoGames = "SelectAllCasinoGames";
    public const string CasinoGames_UpsertCasinoGames = "UpsertCasinoGame";
    public const string CasinoGames_DeleteCasinoGame = "DeleteCasinoGame";

    #endregion

}