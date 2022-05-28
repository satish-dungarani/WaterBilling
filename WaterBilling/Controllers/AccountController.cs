using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WaterBilling.Models;
using WaterBillingDA;

namespace WaterBilling.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        clsUserMaster _objUserMaster = new clsUserMaster();
        clsCommon _MenuList = new clsCommon();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {

        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    //clsCommonUI._Terminal = "test";
        //    //clsCommonUI._Terminal = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
        //    clsCommonUI._Terminal = System.Net.Dns.GetHostName();
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {


            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe);
                        Session["UserName"] = model.UserName;
                        Session["AccessMenuList"] = _MenuList.get_MenuList(model.UserName);
                        Session["MenuList"] = LoadMenuWithModel(model.UserName);
                        clsCommonUI._User = _objUserMaster.getUserMasterWhere(" and UserName = '" + model.UserName + "'").FirstOrDefault().ID;
                        clsCommonUI._UserRoleId = _objUserMaster.getUserMasterWhere(" and ID = " + clsCommonUI._User).FirstOrDefault().RefRoleID;
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        TempData["Warning"] = "Invalid username or password.";
                        //ModelState.AddModelError("", "Invalid username or password.");
                        //return Json(new { Result = "Warning", msg="Invalid username or password."});
                        //return RedirectToAction("Login", "Account");
                    }
                }
                // If we got this far, something failed, redisplay form
                //return Json(new { Result = "Warning", msg = "Invalid username or password." });
                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //load rights wise menu for display
        public List<MenuMasterModel> LoadMenuWithModel(string pUserName)
        {
            List<MenuMasterModel> _ObjMenu = new List<MenuMasterModel>();
            foreach (var x in _MenuList.get_MenuList(pUserName).OrderBy(x => x.OrderNo))
            {
                _ObjMenu.Add(new MenuMasterModel
                {
                    Id = x.ID,
                    MenuName = x.MenuName,
                    MenuDes = x.MenuDes,
                    IsActive = x.IsActive,
                    ParentManuId = x.ParentMenuID,
                    refMenuGroupId = x.refMenuGroupId,
                    OrderNo = x.OrderNo,
                    ConstrollerName = x.ControllerName,
                    ActionName = x.ActionName,
                    MenuPath = x.MenuPath
                });
            }
            return _ObjMenu;
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool _result = false;
                if (model.RegType.ToString().ToUpper() == "REGISTER")
                {

                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        model.UpdUser = clsCommonUI._User;
                        model.UpdTerminal = clsCommonUI._Terminal;

                        _result = Convert.ToBoolean(_objUserMaster.SetUserRole(model.UserId, model.RefRoleId, model.UserName, model.UpdUser, model.UpdTerminal));


                        if (_result)
                        {

                            TempData["Success"] = "User Successfully Registerd.";
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            TempData["Warning"] = "User already exiest!";
                            return RedirectToAction("Index", "User");
                        }

                    }
                    else
                    {
                        TempData["Warning"] = "User already exiest!";
                        return RedirectToAction("Index", "User");
                    }
                }
                else
                {
                    //User does not have a password so remove any validation errors caused by a missing OldPassword field
                    ModelState state = ModelState["OldPassword"];
                    if (state != null)
                    {
                        state.Errors.Clear();
                    }

                    if (ModelState.IsValid)
                    {
                        var _UserIdentity = await UserManager.FindByNameAsync(model.UserName);
                        if (_UserIdentity != null)
                        {
                            string _HasherPassword = UserManager.PasswordHasher.HashPassword(model.Password);
                            UserStore<ApplicationUser> _Store = new UserStore<ApplicationUser>();
                            await _Store.SetPasswordHashAsync(_UserIdentity, _HasherPassword);
                            IdentityResult Resetresult = await UserManager.UpdateAsync(_UserIdentity);
                            if (Resetresult.Succeeded)
                            {
                                model.UpdUser = clsCommonUI._User;
                                model.UpdTerminal = clsCommonUI._Terminal;

                                _result = Convert.ToBoolean(_objUserMaster.SetUserRole(model.UserId, model.RefRoleId, model.UserName, model.UpdUser, model.UpdTerminal));


                                if (_result)
                                {
                                    TempData["Success"] = "User Password successfully reset.";
                                    return RedirectToAction("Index", "User");
                                }
                                else
                                {
                                    TempData["Warning"] = "You can't reset user password!";
                                    return RedirectToAction("Index", "User");
                                }

                                //return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                            }
                            else
                            {
                                TempData["Warning"] = "You can't reset user password!";
                                return RedirectToAction("Index", "User");
                            }
                        }


                    }
                    return View("Index", "User");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "User");
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        Session.RemoveAll();
                        return RedirectToAction("Login", "Account");
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            //Session.Remove("UserName");
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}