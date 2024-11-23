using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplicationLab5.Models;
using WebApplicationLab5.Services;


namespace WebApplicationLab5.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth0Service _auth0Service;

        public AuthController(IAuth0Service auth0Service)
        {
            _auth0Service = auth0Service;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registrationSuccess = await _auth0Service.RegisterUserAsync(user);
                    if (registrationSuccess)
                    {
                        var (accessToken, username, fullname, phone, userEmail) = await _auth0Service.LoginUserAsync(user.Email, user.Password);

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            // ��������� ��'���� ����������� ��� �������� � SignInUser
                            var userInfo = new UserInfo
                            {
                                Username = username,
                                FullName = fullname,
                                Phone = phone,
                                Email = userEmail
                            };

                            // ������������ ��� ���� �������� �����
                            await SignInUser(userInfo);

                            return RedirectToAction("Profile");
                        }
                        else
                        {
                            ModelState.AddModelError("", "�� ������� ������������ ����������� ���� ���������.");
                            return View(user);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "�� ������� ������������ �����������. �������� ������ ���.");
                        return View(user);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "������� ������� ��� ��������� �����������.");
                }
            }
            return View(user);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                // ���� ����������� ����� Auth0
                var (accessToken, username, fullname, phone, userEmail) = await _auth0Service.LoginUserAsync(email, password);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    // ��������� ��'���� ����������� ��� �������� � SignInUser
                    var userInfo = new UserInfo
                    {
                        Username = username,
                        FullName = fullname,
                        Phone = phone,
                        Email = userEmail
                    };

                    // ������������ ��� ���� �������� �����
                    await SignInUser(userInfo);

                    return RedirectToAction("Profile");
                }

                ModelState.AddModelError("", "������ ����������� �� ���� ��� ������ ������� �����������.");
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"������� ������� �� ��� �����: {ex.Message}");
                return View();
            }
        }





        [Authorize]
        public IActionResult Profile()
        {
            var username = User.Identity.Name;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var fullname = User.FindFirstValue("fullname");
            var phone = User.FindFirstValue("phone");

            var model = new UserInfo
            {
                Username = username,
                Email = email,
                FullName = fullname,
                Phone = phone
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task SignInUser(UserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.Username ?? ""),
                new Claim(ClaimTypes.Email, userInfo.Email ?? ""),
                new Claim("fullname", userInfo.FullName ?? ""),
                new Claim("phone", userInfo.Phone ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // ��� ����������� "Remember Me"
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


    }
}
