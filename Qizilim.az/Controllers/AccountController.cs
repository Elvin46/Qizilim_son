using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.AppCode.Modules;
using Qizilim.az.AppCode.Modules.AccountModule;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using Qizilim.az.Models.Entities.Membreship;
using Qizilim.az.Models.Entities.ViewModels;
using Qizilim.az.Models.FormModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Color;
using static Qizilim.az.Models.Entities.Images;

namespace Qizilim.az.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly QizilimDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly IActionContextAccessor ctx;
        private readonly SignInManager<QizilimUser> signInManager;
        private readonly UserManager<QizilimUser> userManager;
        public AccountController(IMediator mediator,
            IActionContextAccessor ctx,
            SignInManager<QizilimUser> signInManager,
            UserManager<QizilimUser> userManager,
            IWebHostEnvironment env,
            QizilimDbContext db)
        {
            this.mediator = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = db;
            this.ctx = ctx;
            this.env = env;
        }

        [AllowAnonymous]
        //[Route("/signin.html")]
        public async Task<IActionResult> Signin()
        {
            ViewBag.Center = await db.Centers.Where(c => c.DeletedById == null).ToListAsync();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("/signin.html")]
        public async Task<IActionResult> Signin(SigninCommand command)
        {
            var centers = await db.Centers.Where(c => c.DeletedById == null).ToListAsync();
            ViewBag.Center = centers;
            var response = await mediator.Send(command);

            if (!ModelState.IsValid)
            {
                return View(command);
            }

            var user = await db.Users.Where(u => u.UserName == command.Username && u.EmailConfirmed == true).FirstOrDefaultAsync();
            
            if (user.EmailConfirmed == false)
            {
                return View(command);
            }
            if (user.Status == null)
            {
                return RedirectToAction("verifyRegister", "Account");
            }
            else if (user.Status == false)
            {
                return RedirectToAction("rejectedRegister", "Account");
            }

            if (User.IsInRole("Superadmin") || User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "admin");
            }

            if (User.IsInRole("Shop"))
            {
                return RedirectToAction("Index", "shop");
            }

            var redirectUrl = Request.Query["ReturnUrl"];

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                return Redirect(redirectUrl);
            }
            
            
            return RedirectToAction("Index", "homepage");
        }
        [AllowAnonymous]
        [Route("/signin.html")]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Homepage");
        }
        public async Task<IActionResult> myStore()
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = userAbout;


            if (userAbout.Status == null)
            {
                return RedirectToAction("verifyRegister", "account");
            }
            else if (userAbout.Status == false)
            {
                return RedirectToAction("rejectedRegister", "account");
            }

            ViewBag.ProductEyar = db.ProductEyar.Where(u => u.ProductId == userAbout.Id).ToList();
            ViewBag.Eyar = db.Eyars.Where(u => u.DeletedById == null);

            ViewBag.Shops = db.Users.Where(u => u.shopName != null).ToList();

            ViewBag.LikedProducts = db.LikedProducts.Where(lp => lp.UserId == userAbout.Id);
            var followedShops = await db.FollowerShop.Where(x => x.FollowerId == userAbout.Id).ToListAsync();
            List<QizilimUser> shops = new List<QizilimUser>();
            foreach (var item in followedShops)
            {
                var shop = await db.Users.Where(x => x.Id == item.ShopId).FirstOrDefaultAsync();
                shops.Add(shop);
            }
            var model = new MainViewModel();
            model.Products = await db.Products
                .Where(cp => cp.DeletedById == null && cp.CreatedById == userAbout.Id)
                .Include(cp => cp.ProductImages)
                .ThenInclude(cp => cp.Image)
                .Include(cp => cp.ProductEyar)
                .ThenInclude(cp => cp.Eyar)
                .Include(cp => cp.ProductColor)
                .ThenInclude(cp => cp.Color).ToListAsync();
            model.QizilimUser = db.Users
               .Where(cp => cp.Id == userAbout.Id).ToList();
            model.Shops = shops;
            model.Advertisement = db.Advertisement
                .Where(cp => cp.DeletedById == null).ToList();
            return View(model);
        }
        public async Task<IActionResult> editShopAccount(int? id)
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
            var entity = await db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            var userAbout = db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            ViewBag.userAbout = userAbout;

            var model = new editAccountViewModel();
            model.Email = entity.Email;
            model.aboutShop = entity.aboutShop;
            model.shopLocation = entity.shopLocation;
            model.shopName = entity.shopName;
            model.shopNumber = entity.shopNumber;
            model.instagramLink = entity.instagramLink;
            model.tiktokLink = entity.tiktokLink;
            model.whatsappNumber = entity.whatsappNumber;
            model.catdirilma = entity.catdirilma;
            model.ProfileImg = entity.ProfileImg;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editShopAccount(editAccountViewModel model)
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
            var entity = await db.Users.Where(u => u.Id == model.Id).FirstOrDefaultAsync();


            if (model?.file == null)
            {
                ctx.AddModelError("ProfileImg", "Fayl Sechilmeyib!");
            }

            if (ctx.ModelIsValid())
            {
                if (entity.ProfileImg == null)
                {
                    string fileExtension = Path.GetExtension(model.file.FileName);

                    string name = $"shop-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "Shopimages", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.file.CopyToAsync(fs);
                    }
                    entity.ProfileImg = name;
                }
                else if (entity.ProfileImg != null)
                {
                    string OldFileName = entity.ProfileImg;

                    string fileExtension = Path.GetExtension(model.file.FileName);

                    string name = $"shop-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "Shopimages", name);

                    using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await model.file.CopyToAsync(fs);
                    }
                    entity.ProfileImg = name;

                    string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "Shopimages", OldFileName);

                    if (System.IO.File.Exists(physicalPathOld))
                    {
                        System.IO.File.Delete(physicalPathOld);
                    }
                }

                entity.shopName = model.shopName;
                entity.Email = model.Email;
                entity.aboutShop = model.aboutShop;
                entity.instagramLink = model.instagramLink;
                entity.tiktokLink = model.tiktokLink;
                entity.whatsappNumber = model.whatsappNumber;
                entity.shopLocation = model.shopLocation;
                entity.catdirilma = model.catdirilma;
                await db.SaveChangesAsync();
                return RedirectToAction("myStore", "Account");

            }

            return NotFound();

        }
        public async Task<IActionResult> detailUser()
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
            var userNow = await userManager.FindByNameAsync(User.Identity.Name);

            var entity = await db.Users.Where(u => u.Id == userNow.Id).FirstOrDefaultAsync();
            ViewBag.User = entity;


            return View();
        }
        public async Task<IActionResult> addProduct()
        {
            ViewBag.Eyyar = await db.Eyars.Where(k => k.DeletedById == null).ToListAsync();
            ViewBag.Colors = await db.Colors.Where(k => k.DeletedById == null).ToListAsync();
            ViewBag.Kateqoriya = await db.Kateqoriya.Where(k => k.DeletedById == null).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addProduct(addingProductViewModel model)
        {

            ViewBag.Eyyar = await db.Eyars.Where(k => k.DeletedById == null).ToListAsync();
            ViewBag.Colors = await db.Colors.Where(k => k.DeletedById == null).ToListAsync();
            ViewBag.Kateqoriya = await db.Kateqoriya.Where(k => k.DeletedById == null).ToListAsync();

            if (model?.files == null)
            {
                ctx.AddModelError("files", "Fayl Sechilmeyib!");
            }
            if (!ctx.ModelIsValid())
            {
                return View(model);
            }
            var product = new Products();
            product.Name = model.Name;
            product.aboutProduct = model.aboutProduct;
            product.Price = model.Price;
            product.Weight = model.Weight;
            product.HasDiamond = model.HasDiamond;
            product.CountDiamond = model.CountDiamond;
            product.Kateqoriya = model.Kateqoriya;
            product.CreatedDate = DateTime.UtcNow.AddHours(4);
            product.CreatedById = ctx.GetPrincipalId();

            await db.Products.AddAsync(product);
            int affected = await db.SaveChangesAsync();

            if (affected > 0 && model.EyarOlcusuId != null)
            {
                await db.ProductEyar.AddAsync(new ProductEyar
                {
                    ProductId = product.Id,
                    EyarId = model.EyarOlcusuId
                });

                await db.SaveChangesAsync();
            }
            if (affected > 0 && model.ColorsId != null)
            {

                await db.ProductColors.AddAsync(new ProductColors
                {
                    ProductId = product.Id,
                    ColorId = model.ColorsId
                });
                await db.SaveChangesAsync();
            }


            if (affected > 0 && model.files != null && model.files.Length > 0)
            {
                foreach (var images in model.files)
                {

                    string fileExtension = Path.GetExtension(images.FileName);

                    string name = $"product-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "ProductImages", name);

                    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await images.CopyToAsync(fs);
                    }

                    var mainClassImg = new Images();
                    mainClassImg.Path = name;
                    mainClassImg.CreatedDate = DateTime.UtcNow.AddHours(4);
                    mainClassImg.CreatedById = ctx.GetPrincipalId();
                    await db.Images.AddAsync(mainClassImg);
                    var result = await db.SaveChangesAsync();

                    if (result > 0)
                    {
                        await db.ProductImage.AddAsync(new ProductImage
                        {
                            ProductId = product.Id,
                            ImageId = mainClassImg.Id
                        });
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("myStore", "Account");
        }
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (User.Identity.Name != null)
            {
                var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = userAbout;

                if (userAbout.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (userAbout.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }
            }
            if (id == null)
            {
                return NotFound();
            }

            var centers = await db.Centers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centers == null)
            {
                return NotFound();
            }

            return View(centers);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductRemoveCommand command)
        {
            if (User.Identity.Name != null)
            {
                var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = userAbout;

                if (userAbout.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (userAbout.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }
            }
            var response = await mediator.Send(command);

            return Json(response);
        }
        public async Task<IActionResult> PremiumProduct(int? productId, int dayPremium)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout;
            

                if (userAbout.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (userAbout.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }

            var products = await db.Products.FirstOrDefaultAsync(lp => lp.Id == productId);

            if (products.PremiumProduct)
            {
                return Json(new { status = 100 });
            }
            else if(userAbout.Wallet > dayPremium)
            {
                products.PremiumProduct = true;
                products.PremiumStartDate = DateTime.UtcNow.AddHours(4);
                products.PremiumEndDate = DateTime.UtcNow.AddDays(dayPremium).AddHours(4);
                userAbout.Wallet = userAbout.Wallet - dayPremium;
            }
            else if(userAbout.Wallet < dayPremium)
            {
                return Json(new { status = 300 });
            }

            await db.SaveChangesAsync();

            return Json(new { status = 200 });
        }
        public async Task<IActionResult> FirstProduct(int? productId)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.User = userAbout; 
                

                if (userAbout.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (userAbout.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }



            var products = await db.Products.FirstOrDefaultAsync(lp => lp.Id == productId);

            if (products.IreliCekilmisProduct)
            {
                return Json(new { status = 100 });
            }
            else if(userAbout.Wallet >= 1)
            {
                products.IreliCekilmisProduct = true;
                products.IreliStartDate = DateTime.UtcNow.AddHours(4);
                products.IreliEndDate = DateTime.UtcNow.AddDays(1).AddHours(4);
                userAbout.Wallet = userAbout.Wallet - 1;
            }
            else if (userAbout.Wallet < 1)
            {
                return Json(new { status = 300 });
            }

            await db.SaveChangesAsync();

            return Json(new { status = 200 });
        }
        [AllowAnonymous]
        public async Task<IActionResult> verifyRegister()
        {
            if (User.Identity.Name != null)
            {
                var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = userAbout;

                if (userAbout.Status == true)
                {
                    return RedirectToAction("index", "homepage");
                }
            }
            return View();
        }
        public async Task<IActionResult> rejectedRegister()
        {
            if (User.Identity.Name != null)
            {
                var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = userAbout;

                
            }
            return View();
        }
        public async Task<IActionResult> addWallet(int countWallet)
        {
            var userAbout = await userManager.FindByNameAsync(User.Identity.Name);

            userAbout.Wallet += countWallet;
            await db.SaveChangesAsync();

            return Json(new { status = 200 });
        }
    }
}
