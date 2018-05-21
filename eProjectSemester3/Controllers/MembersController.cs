using eProjectSemester3.Application.Services;
using eProjectSemester3.Application.UnitOfWork;
using eProjectSemester3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eProjectSemester3.Controllers
{
    public class MembersController : Controller
    {
        public readonly MembershipService _membershipSevice;
        public readonly UnitOfWorkManager _unitOfWorkManager;

        public MembersController(MembershipService membershipSevice, UnitOfWorkManager unitOfWorkManager)
        {
            _membershipSevice = membershipSevice;
            _unitOfWorkManager = unitOfWorkManager;
        }


        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Login & Logout
        [AllowAnonymous]
        public ActionResult Login()
        {
            LogOnViewModel viewModel = new LogOnViewModel();

            var returnUrl = Request["ReturnUrl"];
            if (!string.IsNullOrEmpty(returnUrl))
            {
                viewModel.ReturnUrl = returnUrl;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LogOnViewModel model)
        {
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var username = model.UserName;
                var password = model.Password;

                try
                {
                    if (ModelState.IsValid)
                    {
                        if (_membershipSevice.ValidateUser(username, password, System.Web.Security.Membership.MaxInvalidPasswordAttempts))
                        {
                            var user = _membershipSevice.GetUser(username);
                            if (user.IsApproved && !user.IsLockedOut && !user.IsBanned)
                            {
                                // Set last login date
                                FormsAuthentication.SetAuthCookie(username, model.RememberMe);
                                user.LastLoginDate = DateTime.UtcNow;

                                // Redirect old page
                                if (Url.IsLocalUrl(model.ReturnUrl) && model.ReturnUrl.Length > 1 && model.ReturnUrl.StartsWith("/")
                                    && !model.ReturnUrl.StartsWith("//") && !model.ReturnUrl.StartsWith("/\\"))
                                {
                                    return Redirect(model.ReturnUrl);
                                }

                                return RedirectToAction("Index", "Home", new { area = string.Empty });
                            }
                        }
                        else
                        {
                            // get here Login failed, check the login status
                            var loginStatus = _membershipSevice.LastLoginStatus;

                            switch (loginStatus)
                            {
                                case LoginAttemptStatus.UserNotFound:
                                case LoginAttemptStatus.PasswordIncorrect:
                                    ModelState.AddModelError(string.Empty,"Password Incorrect");
                                    break;

                                case LoginAttemptStatus.PasswordAttemptsExceeded:
                                    ModelState.AddModelError(string.Empty, "Password Attempts Exceeded");
                                    break;

                                case LoginAttemptStatus.UserLockedOut:
                                    ModelState.AddModelError(string.Empty,"User Locked Out");
                                    break;

                                case LoginAttemptStatus.Banned:
                                    ModelState.AddModelError(string.Empty,"NowBanned");
                                    break;

                                case LoginAttemptStatus.UserNotApproved:
                                    ModelState.AddModelError(string.Empty,"User Not Approved");
                                    //user = MembershipService.GetUser(username);
                                    //SendEmailConfirmationEmail(user);
                                    break;

                                default:
                                    ModelState.AddModelError(string.Empty, "Logon Generic");
                                    break;
                            }
                        }
                    }
                }
                //catch
                //{
                //    LoggingService.Error(ex);
                //}
                finally
                {
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        //LoggingService.Error(ex);
                    }

                }

                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult LogOut()
        {
            using (_unitOfWorkManager.NewUnitOfWork())
            {
                System.Web.Security.FormsAuthentication.SignOut();

                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }
        }

        #endregion
    }
}