using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class CreateArticleController : BaseController
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
