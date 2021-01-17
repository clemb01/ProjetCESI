using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class TableauDeBordController : BaseController
    {
        [Route("TableauDeBord")]
        public IActionResult TableauDeBord(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            return View();
        }
    }
}
