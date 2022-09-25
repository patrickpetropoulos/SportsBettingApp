using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SB.Server.Common.SQL;
using SB.Server.Common.Managers;

namespace SB.Server.Root.Casinos.SQL;

public class SqlCasinoManager : Manager, ICasinoManager
{
  private string _connectionString;
  public SqlCasinoManager( IConfiguration configuration ) : base( configuration )
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

  public async Task<List<ICasino>> GetAllCasinos()
  {
    return await DatabaseUtilities.ExecuteAsync<List<ICasino>>( _connectionString,
              async ( c ) => await SelectAllCasinos( c ) );
  }

  public async Task UpsertCasino( ICasino casino )
  {
    if( casino.Id <= 0 )
    {
      //what to do with int??? move this logic up, have an insert call???
      var result = await DatabaseUtilities.ExecuteAsync<int>( _connectionString,
              async ( c ) => await InsertCasino( c, casino ) );
      casino.Id = result;
    }
    else
    {
      var result = await DatabaseUtilities.ExecuteAsync<bool>( _connectionString,
        async ( c ) => await UpdateCasino( c, casino ) );
    }
  }
  
  public async Task<bool> DeleteCasino( int casinoId )
  {
    if( casinoId > 0 )
    {
      //what to do with int??? move this logic up, have an insert call???
      return await DatabaseUtilities.ExecuteAsync<bool>( _connectionString,
        async ( c ) => await DeleteCasino( c, casinoId ) );
    }
    else
    {
      throw new Exception();
    }
  }

  //Access Helper Functions
  public void ReadCasino( ICasino casino, SqlDataReader sqlDataReader )
  {
    casino.Id = DatabaseUtilities.GetInt32( sqlDataReader, "Id", 0 ) ?? 0;
    casino.Name = DatabaseUtilities.GetString( sqlDataReader, "Name", 1 );
    casino.CountryCode = DatabaseUtilities.GetString( sqlDataReader, "CountryCode", 2 );
  }
  
  //Database Access

  public async Task<List<ICasino>> SelectAllCasinos( SqlConnection sqlConnection )
  {
    var casinos = new List<ICasino>();
    try
    {
      using( var sqlCmd = new SqlCommand( StoredProcedures.Casino_SelectAllCasinos, sqlConnection )
      {
        CommandType = CommandType.StoredProcedure
      } )
      {
        using( var sqlReader = await sqlCmd.ExecuteReaderAsync() )
        {
          while( sqlReader.Read() )
          {
            var casino = new Casino();
            ReadCasino( casino, sqlReader );
            casinos.Add( casino );
          }
        }
      }
      return casinos;
    }
    catch( Exception ex )
    {
      _log.LogError( ex, StoredProcedures.Casino_SelectAllCasinos );
      throw;
    }
  }

  public async Task<int> InsertCasino( SqlConnection sqlConnection, ICasino casino )
  {
    try
    {
      //TODO move these out to file
      using( var sqlCmd = new SqlCommand( StoredProcedures.Casino_InsertCasino, sqlConnection )
      {
        CommandType = CommandType.StoredProcedure
      } )
      {
        sqlCmd.Parameters.Add( new SqlParameter( "Name ", casino.Name ) );
        sqlCmd.Parameters.Add( new SqlParameter( "CountryCode ", casino.CountryCode ) );

        sqlCmd.Parameters.Add( "@id", SqlDbType.Int ).Direction = ParameterDirection.Output;

        await sqlCmd.ExecuteNonQueryAsync();
        string id = sqlCmd.Parameters["@id"].Value.ToString();
        return int.Parse( id );
      }
    }
    catch( SqlException e )
    {
      //TODO update name
      _log.LogError( e, StoredProcedures.Casino_InsertCasino );
      throw;
    }
  }
  
  public async Task<bool> UpdateCasino( SqlConnection sqlConnection, ICasino casino )
  {
    try
    {
      //TODO move these out to file
      using( var sqlCmd = new SqlCommand( StoredProcedures.Casino_UpdateCasino, sqlConnection )
            {
              CommandType = CommandType.StoredProcedure
            } )
      {
        sqlCmd.Parameters.Add( new SqlParameter( "Id ", casino.Id ) );
        sqlCmd.Parameters.Add( new SqlParameter( "Name ", casino.Name ) );
        sqlCmd.Parameters.Add( new SqlParameter( "CountryCode ", casino.CountryCode ) );


        await sqlCmd.ExecuteNonQueryAsync();
        return true;
      }
    }
    catch( SqlException e )
    {
      //TODO update name
      _log.LogError( e, StoredProcedures.Casino_UpdateCasino );
      throw;
    }
  }
  
  public async Task<bool> DeleteCasino( SqlConnection sqlConnection, int casinoId )
  {
    try
    {
      //TODO move these out to file
      using( var sqlCmd = new SqlCommand( StoredProcedures.Casino_DeleteCasino, sqlConnection )
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