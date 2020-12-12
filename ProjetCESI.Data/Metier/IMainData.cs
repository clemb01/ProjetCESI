using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjetCESI.Data.Metier
{
    public interface IMainData
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}