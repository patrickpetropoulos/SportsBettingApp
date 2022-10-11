using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SB.Server.Common.Managers;
using SB.Server.Common.SQL;
using System.Data;

namespace SB.Server.Root.CasinoGames.SQL;

public class SqlCasinoGameManager : Manager, ICasinoGameManager
{
    private string _connectionString;
    public SqlCasinoGameManager( IConfiguration configuration ) : base( configuration )
    {

    }
    protected override void InitializeConfiguration()
    {
        _connectionString = Configuration.GetConnectionString( "Default" );
    }
    public override void SetLogger( ILoggerFactory loggerFactory, string name )
    {
        base.SetLogger( loggerFactory, name );
    }

    public async Task<string> GetCasinos()
    {
        await Task.Delay( 1000 );
        return "Hello from SqlCasinoManager";
    }

    public async Task<List<ICasinoGame>> GetAllCasinoGames()
    {
        return await DatabaseUtilities.ExecuteAsync<List<ICasinoGame>>( _connectionString,
                  async ( c ) => await SelectAllCasinoGames( c ) );
    }

    public async Task<bool> UpsertCasinoGame( ICasinoGame casinoGame )
    {
        return await DatabaseUtilities.ExecuteAsync<bool>( _connectionString,
          async ( c ) => await UpsertCasinoGame( c, casinoGame ) );
    }

    public async Task<bool> DeleteCasinoGame( Guid casinoId )
    {
        //what to do with int??? move this logic up, have an insert call???
        return await DatabaseUtilities.ExecuteAsync<bool>( _connectionString,
          async ( c ) => await DeleteCasinoGame( c, casinoId ) );
    }

    //Access Helper Functions
    public void ReadCasino( ICasinoGame casinoGame, SqlDataReader sqlDataReader )
    {
        casinoGame.Id = DatabaseUtilities.GetGuid( sqlDataReader, "Id", 0 ) ?? Guid.Empty;
        casinoGame.Name = DatabaseUtilities.GetString( sqlDataReader, "Name", 1 );
        casinoGame.HasSubType = DatabaseUtilities.GetBoolean( sqlDataReader, "HasSubType", 2 ) ?? false;
    }

    //Database Access

    public async Task<List<ICasinoGame>> SelectAllCasinoGames( SqlConnection sqlConnection )
    {
        var casinoGames = new List<ICasinoGame>();
        try
        {
            using( var sqlCmd = new SqlCommand( StoredProcedures.CasinoGames_SelectAllCasinoGames, sqlConnection )
            {
                CommandType = CommandType.StoredProcedure
            } )
            {
                using( var sqlReader = await sqlCmd.ExecuteReaderAsync() )
                {
                    while( sqlReader.Read() )
                    {
                        var casinoGame = new CasinoGame();
                        ReadCasino( casinoGame, sqlReader );
                        casinoGames.Add( casinoGame );
                    }
                }
            }
            return casinoGames;
        }
        catch( Exception ex )
        {
            _log.LogError( ex, StoredProcedures.Casino_SelectAllCasinos );
            throw;
        }
    }

    public async Task<bool> UpsertCasinoGame( SqlConnection sqlConnection, ICasinoGame casinoGame )
    {
        try
        {
            //TODO move these out to file
            await using var sqlCmd = new SqlCommand( StoredProcedures.CasinoGames_UpsertCasinoGames, sqlConnection )
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add( new SqlParameter( "Id ", casinoGame.Id ) );
            sqlCmd.Parameters.Add( new SqlParameter( "Name ", casinoGame.Name ) );
            sqlCmd.Parameters.Add( new SqlParameter( "HasSubType ", casinoGame.HasSubType ) );


            await sqlCmd.ExecuteNonQueryAsync();
            return true;
        }
        catch( SqlException e )
        {
            //TODO update name
            //_log.LogError( e, StoredProcedures.Casino_UpsertCasino );
            throw;
        }
    }

    public async Task<bool> DeleteCasinoGame( SqlConnection sqlConnection, Guid casinoId )
    {
        try
        {
            //TODO move these out to file
            using( var sqlCmd = new SqlCommand( StoredProcedures.CasinoGames_DeleteCasinoGame, sqlConnection )
            {
                CommandType = CommandType.StoredProcedure
            } )
            {
                sqlCmd.Parameters.Add( new SqlParameter( "Id ", casinoId ) );


                await sqlCmd.ExecuteNonQueryAsync();
                return true;
            }
        }
        catch( SqlException e )
        {
            //TODO update name
            _log.LogError( e, StoredProcedures.Casino_DeleteCasino );
            throw;
        }
    }



}