using System.Web.Mvc;
using Demo.UI.Models;
using WebMatrix.WebData;
using System.Web.Security;
using Demo.ApplicationLogic;
using Raven.Client;
using Raven.Client.Document;
using StructureMap.Attributes;
using Demo.Storage.Repositories;

namespace Demo.UI.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IUserService userService;

        public AccountController(IUserRepository userRepository, IUserService userService)
        {
            this.userRepository = userRepository;
            this.userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }
            return this.View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LogInUserModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            //var canSignIn = this.usersBusinessLogic.CanSignIn(model.Login, model.Password);
            //if (!canSignIn)
            //{
            //    this.ModelState.AddModelError("", ValidationMessages.IncorrectCredential);
            //    return this.View(model);
            //}

            FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
            string url = FormsAuthentication.GetRedirectUrl(model.Login, model.RememberMe);

            return this.Redirect(url);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.Session.Abandon();
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Login", "Account");
        }
     
    }
}
