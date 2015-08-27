using System.Web.Mvc;
using System.Web.Security;
using Store.Models;
using UniversalProvidersApplication.Models;
using System.Linq;

namespace UniversalProvidersApplication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        StoreContext db = new StoreContext();

        public ActionResult Login()
        {
            ViewBag.Categories = db.Category.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "catalog");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            ViewBag.Categories = db.Category.ToList();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) ViewBag.Auth = "Авторизован";
            else ViewBag.Auth = "Не авторизован";
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "catalog");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }
            ViewBag.Categories = db.Category.ToList();
            return View(model);
        }
    }
}