using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Main
{
    public interface IMainMetier
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
