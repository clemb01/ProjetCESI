using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjetCESI.Core;
using ProjetCESI.Metier;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class BaseController : Controller
    {

        private MetierFactory _metierFactory;
        protected MetierFactory MetierFactory 
        { 
            get
            {
                if(_metierFactory == null)
                {
                    _metierFactory = new MetierFactory(UserId);
                }

                return _metierFactory;
            }
        }

        private int _userId;
        public int UserId 
        {
            get
            {
                if (_userId != default(int))
                {
                    string id = UserManager.GetUserId(User);

                    return int.Parse(id);
                }
                else
                    return _userId;
            }
        }

        private User _utilisateur;
        public User Utilisateur
        {
            get
            {
                if (_utilisateur == null)
                {
                    User user = UserManager.GetUserAsync(User).Result;

                    return user;
                }
                else
                    return _utilisateur;
            }
        }

        private List<string> _utilisateurRoles;
        public List<string> UtilisateurRoles 
        { 
            get
            {
                if(_utilisateur != null)
                {
                    _utilisateurRoles = UserManager.GetRolesAsync(Utilisateur).Result.ToList();
                }

                return _utilisateurRoles;
            }
        }


        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IAuthenticationService _authenticationService;
        private IWebHostEnvironment _env;
        private IConfiguration _configuration;
        private ILogger _logger;

        public BaseController()
        {
        }

        public BaseController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

    public UserManager<User> UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;

                return _userManager;
            }
        }

        public SignInManager<User> SignInManager
        {
            get
            {
                if (_signInManager == null)
                    _signInManager = HttpContext.RequestServices.GetService(typeof(SignInManager<User>)) as SignInManager<User>;

                return _signInManager;
            }
        }

        public IAuthenticationService AuthenticationService
        {
            get
            {
                if (_authenticationService == null)
                    _authenticationService = HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;

                return _authenticationService;
            }
        }
        public IWebHostEnvironment HostingEnvironnement
        {
            get
            {
                if (_env == null)
                    _env = HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;

                return _env;
            }
        }
        public IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

                return _configuration;
            }
        }

        public ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = HttpContext.RequestServices.GetService(typeof(ILogger)) as ILogger;

                return _logger;
            }
        }

        public T PrepareModel<T>(T model) where T : BaseViewModel, new()
        {
            model.Basepath = Request.Host.Value;
            model.Path = Request.Path.Value;
            model.QueryString = Request.QueryString.Value;
            model.Action = Request.RouteValues["Action"].ToString() ?? "";
            model.Controller = Request.RouteValues["Controller"].ToString() ?? "";
            model.Area = Request.RouteValues["Area"] != null ? Request.RouteValues["Area"].ToString() : "";
            model.Utilisateur = Utilisateur;

            if(UtilisateurRoles != null)
                model.UtilisateurRole = UtilisateurRoles.FirstOrDefault() != null ? (int)Enum.Parse(typeof(TypeUtilisateur), UtilisateurRoles.FirstOrDefault()) : (int)TypeUtilisateur.Aucun;

            return model;
        }

        public T PrepareModel<T>() where T : BaseViewModel, new()
        {
            return PrepareModel<T>(new T());
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
