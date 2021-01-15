using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class StatistiquesController : BaseController
    {
        public IActionResult Statistiques()
        {
            return View();
        }
    }
}
