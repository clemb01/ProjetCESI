using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class RessourceController : BaseController
    {
        [HttpGet]
        public IActionResult Ressource(string name)
        {


            return View();
        }
    }
}
