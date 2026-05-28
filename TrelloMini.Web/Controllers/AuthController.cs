using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using TrelloMini.Models;
using TrelloMini.Web.AppCodes;
using TrelloMini.BusinessLayers;

using System.Threading.Tasks;

namespace TrelloMini.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        // =========================
        // LOGIN GET
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null &&
                User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    "Index",
                    "Task"
                );
            }

            return View();
        }

        // =========================
        // LOGIN POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string hashedPassword =
                    CryptHelper.HashMD5(model.Password);

                var user = _userService.Login(
                    model.Username,
                    hashedPassword
                );

                if (user != null)
                {
                    var userData = new WebUserData
                    {
                        UserID =
                            user.UserID.ToString(),

                        Username =
                            user.Username,

                        DisplayName =
                            user.Username,

                        Roles = new List<string>
                        {
                            WebUserRoles.User
                        }
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userData.CreatePrincipal()
                    );

                    return RedirectToAction(
                        "Index",
                        "Task"
                    );
                }

                ViewBag.Error =
                    "Sai tài khoản hoặc mật khẩu";

                return View(model);
            }
            catch
            {
                ViewBag.Error =
                    "Không thể kết nối database";

                return View(model);
            }
        }

        // =========================
        // REGISTER GET
        // =========================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // REGISTER POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.Password =
                    CryptHelper.HashMD5(
                        model.Password
                    );

                bool result =
                    _userService.Register(model);

                if (result)
                {
                    var userData = new WebUserData
                    {
                        UserID = "0",

                        Username =
                            model.Username,

                        DisplayName =
                            model.Username,

                        Roles = new List<string>
                        {
                            WebUserRoles.User
                        }
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userData.CreatePrincipal()
                    );

                    return RedirectToAction(
                        "Index",
                        "Task"
                    );
                }

                ViewBag.Error =
                    "Đăng ký thất bại";

                return View(model);
            }
            catch
            {
                ViewBag.Error =
                    "Không thể kết nối database";

                return View(model);
            }
        }

        // =========================
        // LOGOUT
        // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                return RedirectToAction(
                    "Login",
                    "Auth"
                );
            }


        }
}

