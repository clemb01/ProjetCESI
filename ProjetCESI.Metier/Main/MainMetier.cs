using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetCESI.Data.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Main
{
    public class MainMetier : MetierBase<Core.Main, MainData>, IMainMetier
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration) => DataClass.ConfigureServices(services, configuration);
    }
}
