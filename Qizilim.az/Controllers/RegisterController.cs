using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MimeKit;
using Newtonsoft.Json;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using Qizilim.az.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Qizilim.az.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly IMediator mediator;
        private readonly QizilimDbContext db;
        private readonly IActionContextAccessor ctx;
        private readonly SignInManager<QizilimUser> signInManager;
        private readonly UserManager<QizilimUser> userManager;
        public RegisterController(IMediator mediator,
            SignInManager<QizilimUser> signInManager,
            UserManager<QizilimUser> userManager,
            IActionContextAccessor ctx,
            QizilimDbContext db)
        {
            this.mediator = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
            this.ctx = ctx;
        }
        [AllowAnonymous]
        //[Route("/signin.html")]
        public async Task<IActionResult> Register()
        {
            var center = await db.Centers.Where(c => c.DeletedById == null).ToListAsync();
            ViewBag.Center = center;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel registerVM)
        {
            ViewBag.Center = await db.Centers.Where(c => c.DeletedById == null).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await userManager.FindByNameAsync(registerVM.Username);
            if (existUser != null)
            {
                ModelState.AddModelError("Username", "This Username already exist");
                return View();
            }
            if (registerVM.PhoneNumber.Length != 9)
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber Must be 9 characters");
                return View();
            }
            if (!registerVM.isShop)
            {
                var user = new QizilimUser
                {
                    Name = registerVM.Name,
                    Surname = registerVM.Surname,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    UserName = registerVM.Username,
                    EmailConfirmed = false,
                    catdirilma = false,
                    PhoneNumberConfirmed = true,
                    ProfileImg = "default-user-image.jpeg",
                    Status = true,
                    Wallet = null
                };

                Random random = new Random();
                List<int> oldCodes = new List<int>();
                int verificationCode = random.Next(100000, 1000000);
                while (oldCodes.Contains(verificationCode))
                {
                    verificationCode = random.Next(100000, 1000000);
                }
                oldCodes.Add(verificationCode);
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress("EduHome", "codep320@gmail.com"));
                mail.To.Add(new MailboxAddress(user.Name, user.Email));

                mail.Subject = "Verify EduHome Account";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<h2>Your verification code:\"{verificationCode}\"</h2>";
                bodyBuilder.TextBody = "This is some plain text";

                mail.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("elvinva@code.edu.az", "Elvin2003");

                    smtp.Send(mail);
                    smtp.Disconnect(true);
                }

                var result = await userManager.CreateAsync(user, registerVM.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                await userManager.AddToRoleAsync(user, "User");
                var cookie = JsonConvert.SerializeObject(user.Id);
                Response.Cookies.Append("user", cookie);
                var cookieVerify = JsonConvert.SerializeObject(verificationCode);
                Response.Cookies.Append("verificationCode", cookieVerify);
                return RedirectToAction(nameof(VerifyEmail));
            }
            else
            {
                if (registerVM.ShopName == null)
                {
                    ModelState.AddModelError("ShopName", "Magaza adi daxil edilmemisdir");
                    return View();
                }
                if (registerVM.ShopNumber == null)
                {
                    ModelState.AddModelError("ShopNumber", "Magaza nomresi daxil edilmemisdir");
                    return View();
                }
                var center = await db.Centers.Where(x => x.Id == registerVM.CenterId).FirstOrDefaultAsync();
                var user = new QizilimUser
                {
                    Name = registerVM.Name,
                    Surname = registerVM.Surname,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    UserName = registerVM.Username,
                    shopLocation = center.Name,
                    shopName = registerVM.ShopName,
                    shopNumber = registerVM.ShopNumber,
                    catdirilma = false,
                    PhoneNumberConfirmed = true,
                    ProfileImg = "default-shop-image.jpg",
                    Wallet = 0
                };
                Random random = new Random();
                List<int> oldCodes = new List<int>();
                int verificationCode = random.Next(100000, 1000000);
                while (oldCodes.Contains(verificationCode))
                {
                    verificationCode = random.Next(100000, 1000000);
                }
                oldCodes.Add(verificationCode);
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress("EduHome", "codep320@gmail.com"));
                mail.To.Add(new MailboxAddress(registerVM.Name, registerVM.Email));

                mail.Subject = "Verify EduHome Account";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<h2>Your verification code:\"{verificationCode}\"</h2>";
                bodyBuilder.TextBody = "This is some plain text";

                mail.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("elvinva@code.edu.az", "Elvin2003");

                    smtp.Send(mail);
                    smtp.Disconnect(true);
                }

                var result = await userManager.CreateAsync(user, registerVM.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                await userManager.AddToRoleAsync(user, "Shop");
                var cookie = JsonConvert.SerializeObject(user.Id);
                Response.Cookies.Append("user", cookie);
                var cookieVerify = JsonConvert.SerializeObject(verificationCode);
                Response.Cookies.Append("verificationCode", cookieVerify);
                return RedirectToAction(nameof(VerifyEmail));
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(string name, string surname, string username, string email, 
            string phoneNumber, string password, string passwordAgain)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existUser = await userManager.FindByNameAsync(username);
            if (existUser != null)
            {
                ModelState.AddModelError("Username", "This Username already exist");
                return View();
            }
            if (phoneNumber.Length != 9)
            {
                ModelState.AddModelError("PhoneNumber", "PhoneNumber Must be 9 characters");
                return View();
            }
            if (ctx.ModelIsValid())
            {
                var user = new QizilimUser
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    UserName = username,
                    EmailConfirmed = false,
                    catdirilma = false,
                    PhoneNumberConfirmed = true,
                    ProfileImg = "default-user-image.jpeg",
                    Status = true,
                    Wallet = null
                };

                Random random = new Random();
                List<int> oldCodes = new List<int>();
                int verificationCode = random.Next(100000, 1000000);
                while (oldCodes.Contains(verificationCode))
                {
                    verificationCode = random.Next(100000, 1000000);
                }
                oldCodes.Add(verificationCode);
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress("EduHome", "codep320@gmail.com"));
                mail.To.Add(new MailboxAddress(user.Name, user.Email));

                mail.Subject = "Verify EduHome Account";
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<h2>Your verification code:\"{verificationCode}\"</h2>";
                bodyBuilder.TextBody = "This is some plain text";

                mail.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("elvinva@code.edu.az", "Elvin2003");

                    smtp.Send(mail);
                    smtp.Disconnect(true);
                }

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                await userManager.AddToRoleAsync(user, "User");
                var cookie = JsonConvert.SerializeObject(user.Id);
                Response.Cookies.Append("user", cookie);
                var cookieVerify = JsonConvert.SerializeObject(verificationCode);
                Response.Cookies.Append("verificationCode", cookieVerify);

            }

            return Json(new { status = 200 });
        }
        [HttpPost]
        public async Task<IActionResult> RegisterShop(string name, string surname, string username, string email,
               string phoneNumber, string password, string passwordAgain, string selectedLocation, string shopName, string shopPhone)
        {
            if (ctx.ModelIsValid())
            {


                var shop = new QizilimUser
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    UserName = username,
                    shopLocation = selectedLocation,
                    shopName = shopName,
                    shopNumber = shopPhone,
                    catdirilma = false,
                    PhoneNumberConfirmed = true,
                    ProfileImg = "default-shop-image.jpg",
                    Wallet = 0
                };
                var result = await userManager.CreateAsync(shop, password);
                await userManager.AddToRoleAsync(shop, "Shop");
            }

            return Json(new { status = 200 });
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(int? passcode)
        {
            var cookie = Request.Cookies["user"];
            var cookieVerify = Request.Cookies["verificationCode"];
            if (string.IsNullOrEmpty(cookie))
            {
                return Content("Empty");
            }
            var userId = JsonConvert.DeserializeObject<string>(cookie);
            var verificationCode = JsonConvert.DeserializeObject<int>(cookieVerify);
            var user = await userManager.FindByIdAsync(userId);
            if (passcode == verificationCode)
            {
                user.EmailConfirmed = true;
                await db.SaveChangesAsync();
                //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                //IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
                //if (!result.Succeeded)
                //{
                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelError("", error.Description);
                //    }
                //    return View();
                //}
                Response.Cookies.Delete("user");
            }
            if (user.shopName != null)
            {
                return RedirectToAction("verifyRegister", "Account");
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
            
        }
    }
}
