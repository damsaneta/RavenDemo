using System.Web.Mvc;
using Demo.UI.Models;
using WebMatrix.WebData;
using System.Web.Security;
using Raven.Client;
using Raven.Client.Document;
using StructureMap.Attributes;

namespace Demo.UI.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
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
