using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using Qizilim.az.Models.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        private readonly UserManager<QizilimUser> userManager;
        private readonly QizilimDbContext db;
        public StoreController(UserManager<QizilimUser> userManager, 
            QizilimDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        public async Task<IActionResult> Stores()
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
            var model = new MainViewModel();
            model.QizilimUser = db.Users
               .Where(cp => cp.shopName != null).ToList();
            model.Advertisement = db.Advertisement
                .Where(cp => cp.DeletedById == null).ToList();
            return View(model);
        }
        public async Task<IActionResult> StoreSingle(int id)
        {
            if (User.Identity.Name != null)
            {
                var userAbout = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = userAbout;
                ViewBag.Follow = false;

                if (userAbout.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (userAbout.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }

                var hasFollowersShops = db.FollowerShop.Any<FollowersShops>(u => u.FollowerId == userAbout.Id && u.ShopId == id);

                if (hasFollowersShops)
                {
                    ViewBag.Follow = true;

                }
            }
            ViewBag.Shop = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.ProductEyar = db.ProductEyar.Where(u => u.ProductId == id).ToList();
            ViewBag.Eyar = db.Eyars.Where(u => u.DeletedById == null);


            var model = new MainViewModel();
            model.Products = await db.Products
                .Where(cp => cp.DeletedById == null && cp.CreatedById == id)
                .Include(cp => cp.ProductImages)
                .ThenInclude(cp => cp.Image)
                .Include(cp => cp.ProductEyar)
                .ThenInclude(cp => cp.Eyar)
                .Include(cp => cp.ProductColor)
                .ThenInclude(cp => cp.Color).ToListAsync();
            model.QizilimUser = db.Users
               .Where(cp => cp.shopName != null).ToList();
            model.Advertisement = db.Advertisement
                .Where(cp => cp.DeletedById == null).ToList();
            return View(model);
        }
        public async Task<IActionResult> FollowTheUser(int shopId)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user.Status == null)
            {
                return RedirectToAction("verifyRegister", "account");
            }
            else if (user.Status == false)
            {
                return RedirectToAction("rejectedRegister", "account");
            }
            var shop = await db.Users.FirstOrDefaultAsync(u => u.Id == shopId);
            var hasFollowersShops = db.FollowerShop.Any<FollowersShops>(u => u.FollowerId == user.Id && u.ShopId == shopId);

            if (hasFollowersShops)
            {
                var followersShops = await db.FollowerShop.FirstOrDefaultAsync(u => u.FollowerId == user.Id && u.ShopId == shopId);

                shop.FollowerCount--;
                user.FollowerShops.Remove(followersShops);
            }
            else
            {
                shop.FollowerCount++;
                await db.FollowerShop.AddAsync(new FollowersShops()
                {
                    FollowerId = user.Id,
                    ShopId = shopId,
                }) ;
            }
            
            await db.SaveChangesAsync();
            return Json(new { status = 200 });
        }
        public async Task<IActionResult> ProductSingle(int id)
        {
            var model = new MainViewModel();
            model.Products = await db.Products
                .Where(p => p.Id == id && p.DeletedById == null)
                .Include(p => p.ProductImages).ThenInclude(p => p.Image)
                .Include(p => p.ProductEyar).ThenInclude(p => p.Eyar)
                .Include(p => p.ProductColor).ThenInclude(p => p.Color).ToListAsync();

            model.OxsarProducts = await db.Products.Where(p => p.DeletedById == null)
                .Include(p => p.ProductImages).ThenInclude(p => p.Image)
                .Include(p => p.ProductEyar).ThenInclude(p => p.Eyar)
                .Include(p => p.ProductColor).ThenInclude(p => p.Color).ToListAsync();

            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
            var shop = await db.Users.FirstOrDefaultAsync(u => u.Id == product.CreatedById);
            ViewBag.Shop = shop;

            if (User.Identity.Name != null)
            {
                var nowUser = await userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.nowUser = nowUser;
                if (nowUser.Status == null)
                {
                    return RedirectToAction("verifyRegister", "account");
                }
                else if (nowUser.Status == false)
                {
                    return RedirectToAction("rejectedRegister", "account");
                }
            }

            
            ViewBag.LikedProducts = db.LikedProducts.Where(u => u.ProductId == id).ToList();
            
            return View(model);
        }
    }
}
