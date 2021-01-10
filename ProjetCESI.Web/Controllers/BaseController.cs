using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
                    _metierFactory = new MetierFactory();
                }

                return _metierFactory;
            }
        }

        private string _userId;
        public string UserId 
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    string id = UserManager.GetUserId(User);

                    return id;
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

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

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

        public T PrepareModel<T>(T model) where T : BaseViewModel, new()
        {
            model.Basepath = Request.Host.Value;
            model.Path = Request.Path.Value;
            model.QueryString = Request.QueryString.Value;
            model.Action = Request.RouteValues["Action"].ToString() ?? "";
            model.Controller = Request.RouteValues["Controller"].ToString() ?? "";
            model.Area = Request.RouteValues["Area"] != null ? Request.RouteValues["Area"].ToString() : "";
            model.Utilisateur = Utilisateur;

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
