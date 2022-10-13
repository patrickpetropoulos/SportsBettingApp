using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SB.Server.Common.Managers;
using SB.Server.Root.CasinoGames;
using SB.Server.Root.Casinos;

namespace SB.Server.App.Common;

public class ServerSystem
{
  private Dictionary<string, object> _services;

  private readonly static object _instanceLock = new object();
  private readonly static object _serviceProviderLock = new object();
  private static ServerSystem? _instance;
  private static IServiceProvider? _serviceProvider;

  protected ServerSystem( IServiceProvider services, IConfiguration configuration )
  {
    _services = new Dictionary<string, object>();
    //The trick is both files, the interface and the implemenetation are in the same namespace, I think
    //If using an SQL project, class cannot have the .SQL 
    //Sports Book Manager
    //var sportsBookManager = ActivatorUtilities.CreateInstance( services, Type.GetType( configuration["SportsBookManager:Instance"] ),
    //    new object[] { configuration } ) as ISportsBookManager;
    //SportsBookManager = sportsBookManager;
    //_services.Add( "SportsBookManager", sportsBookManager );


    //In appsettings, its full namespace of file + filename, then comma, then full name of project it is in
    //Need to add them as reference, OBVIOUSLY
    var config = Type.GetType( configuration.GetValue<string>( "Casino:Manager" ) );
    if( config != null )
    {
      var casinoManager = ActivatorUtilities.CreateInstance( services, config, new object[] { configuration } ) as ICasinoManager;
      if( casinoManager != null )
      {
        _services.Add( ManagerNames.CasinoManager, casinoManager );
      }
    }
    config = Type.GetType( configuration.GetValue<string>( "CasinoGame:Manager" ) );
    if( config != null )
    {
      var casinoGameManager = ActivatorUtilities.CreateInstance( services, config, new object[] { configuration } ) as ICasinoGameManager;
      if( casinoGameManager != null )
      {
        _services.Add( ManagerNames.CasinoGameManager, casinoGameManager );
      }
    }
    // config = Type.GetType( configuration["CasinoGame:Manager"] );
    // if( config != null )
    // {
    //   var casinoGameManager = ActivatorUtilities.CreateInstance( services, config, new object[] { configuration } ) as ICasinoGameManager;
    //
    //   if( casinoGameManager != null )
    //   {
    //     _services.Add( "CasinoGameManager", casinoGameManager );
    //   }
    // }

  }
  //public static void CreateInstance()
  //{
  //  lock( _instanceLock )
  //  {
  //    _instance = new ServerSystem();
  //  }
  //}
  public static void CreateInstance( IServiceProvider services, IConfiguration configuration )
  {
    lock( _instanceLock )
    {
      _instance = new ServerSystem( services, configuration );
    }
  }
  public static ServerSystem? Instance
  {
    get
    {
      lock( _instanceLock )
      {
        return _instance;
      }
    }
  }
  public T? Get<T>( string name ) where T : class
  {
    if( !_services.ContainsKey( name ) )
      return null;

    return (T)_services[name];
  }
  public void SetServiceProvider( IServiceProvider serviceProvider )
  {
    lock( _serviceProviderLock )
    {
      _serviceProvider = serviceProvider;
    }
  }
  public void SetLogger( ILoggerFactory loggerFactory )
  {
    Get<ICasinoManager>( ManagerNames.CasinoManager )?.SetLogger( loggerFactory, ManagerNames.CasinoManager );
  }


}