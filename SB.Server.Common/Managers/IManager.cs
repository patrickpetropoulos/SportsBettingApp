using Microsoft.Extensions.Logging;
namespace SB.Server.Common.Managers;

public interface IManager : IDisposable
{

    void SetLogger( ILoggerFactory loggerFactory, string name );

    //void InitializeConfiguration();

}