using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using Qizilim.az.Models.Entities.Membreship;
using Qizilim.az.Models.Entities.ViewModels;
using Qizilim.az.Models.FormModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Products;

namespace Qizilim.az.Controllers
{
    
    public class HomePageController : Controller
    {
        private readonly QizilimDbContext db;
        private readonly IMediator mediator;
        private readonly UserManager<QizilimUser> userManager; 
        public HomePageController(QizilimDbContext db,
            UserManager<QizilimUser> userManager,
            IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
            this.userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
                var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
                var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
                var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
                ViewBag.Colors = colors;
                ViewBag.Categories = categories;
                ViewBag.Eyars = eyars;
                ViewBag.Centers = centers;
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
            model.Products = await db.Products
                .Where(cp => cp.DeletedById == null)
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
            model.Centers = centers;

            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> VipProducts()
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
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
            model.Products = await db.Products
               .Where(cp => cp.DeletedById == null && cp.PremiumProduct == true)
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
        public async Task<IActionResult> Liked()
        {
            var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
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

            var likedProducts = db.LikedProducts.Where(lp => lp.UserId == userAbout.Id).ToList();
            ViewBag.LikedProducts = likedProducts;
            
            var model = new MainViewModel();
            model.Products = await db.Products
               .Where(cp => cp.DeletedById == null)
               .Include(cp => cp.ProductImages)
               .ThenInclude(cp => cp.Image)
               .Include(cp => cp.ProductEyar)
               .ThenInclude(cp => cp.Eyar)
               .Include(cp => cp.ProductColor)
               .ThenInclude(cp => cp.Color)
               .Include(cp=>cp.LikedProducts).ToListAsync();
            model.QizilimUser = db.Users
               .Where(cp => cp.shopName != null).ToList();
            model.Advertisement = db.Advertisement
                .Where(cp => cp.DeletedById == null).ToList();
            return View(model);
        }
        public async Task<IActionResult> toLike(int productId)
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

            var likedProducts = db.LikedProducts.Where(lp => lp.UserId == userAbout.Id).ToList();

            foreach (var item in likedProducts)
            {
                if (item.ProductId == productId)
                {
                    var productliked = db.LikedProducts.FirstOrDefault(lp => lp.ProductId == productId);
                    db.LikedProducts.Remove(productliked);
                    await db.SaveChangesAsync();

                    return Json(new { status = 200 });
                }
            }

            await db.LikedProducts.AddAsync(new LikedProduct
            {
                UserId = userAbout.Id,
                ProductId = productId
            });

            await db.SaveChangesAsync();

            return Json(new { status = 200 });
        }
        [AllowAnonymous]
        public async Task<IActionResult> searchProduct(MainViewModel filterProduct)
        {
        var colors = await db.Colors.Where(x => x.DeletedById == null).ToListAsync();
            var categories = await db.Kateqoriya.Where(x => x.DeletedById == null).ToListAsync();
            var eyars = await db.Eyars.Where(x => x.DeletedById == null).ToListAsync();
            var centers = await db.Centers.Where(x => x.DeletedById == null).ToListAsync();
            ViewBag.Colors = colors;
            ViewBag.Categories = categories;
            ViewBag.Eyars = eyars;
            ViewBag.Centers = centers;
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
            var productFilter = await db.Products.Where(p => p.DeletedById == null).Include(cp => cp.ProductImages)
                .ThenInclude(cp => cp.Image)
                .Include(cp => cp.ProductEyar)
                .ThenInclude(cp => cp.Eyar)
                .Include(cp => cp.ProductColor)
                .ThenInclude(cp => cp.Color).ToListAsync();
            var product = new Products { };
            List<Products> products = new List<Products>();

            if (filterProduct.EyarId != null)
            {
                var ProductEyar = await db.ProductEyar.Where(x => x.EyarId == filterProduct.EyarId).ToListAsync();
                foreach (var item in ProductEyar)
                {
                    product = await db.Products.Where(x=>x.Id == item.ProductId).FirstOrDefaultAsync();
                    products.Add(product);
                }
                productFilter = products;
            }
            if (filterProduct.ColorId != null)
            {
                products.RemoveAll(x=>x.DeletedById==null);
                var ProductColor = await db.ProductColors.Where(x => x.ColorId == filterProduct.ColorId).ToListAsync();
                foreach (var item in ProductColor)
                {
                    product = await db.Products.Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                    products.Add(product);
                }
                productFilter = products;
                
            }
            if (filterProduct.CategoryId != null)
            {
                var category = await db.Kateqoriya.Where(x=>x.Id == filterProduct.CategoryId).FirstOrDefaultAsync();
                productFilter = productFilter.Where(pf => pf.Kateqoriya == category.Name).ToList();

            }
            if (filterProduct.hasDiamond != null)
            {
                productFilter = productFilter.Where(pf => pf.HasDiamond == filterProduct.hasDiamond).ToList();
            }
            if (filterProduct.hasDelivery != null)
            {
                productFilter = productFilter.Where(pf => pf.Delivery == filterProduct.hasDelivery).ToList();
            }
            if (filterProduct.CenterId != null)
            {
                products.RemoveAll(x => x.DeletedById == null);
                var center = await db.Centers.Where(x => x.Id == filterProduct.CenterId).FirstOrDefaultAsync();
                var users = db.Users.Where(u => u.shopLocation == center.Name).ToList();
                foreach (var item in users)
                {
                    var x = productFilter.Where(p => p.CreatedById == item.Id).ToList();
                    foreach (var x1 in x)
                    {
                        products.Add(x1);

                    }
                }
                productFilter = products;
            }
            if (filterProduct.minWeight !=null || filterProduct.maxWeight != null)
            {
                if (filterProduct.minWeight == null)
                {
                    productFilter = productFilter.Where(pf => pf.Weight <= filterProduct.maxWeight).ToList();

                }
                else if (filterProduct.maxWeight == null)
                {
                    productFilter = productFilter.Where(pf => pf.Weight >= filterProduct.minWeight).ToList();

                }
                else
                {
                    productFilter = productFilter.Where(pf => pf.Weight >= filterProduct.minWeight && pf.Weight <= filterProduct.maxWeight).ToList();

                }
            }
            if (filterProduct.minPrice != null || filterProduct.maxPrice != null)
            {
                if (filterProduct.minPrice == null)
                {
                    productFilter = productFilter.Where(pf => pf.Price <= filterProduct.maxPrice).ToList();

                }
                else if (filterProduct.maxPrice == null)
                {
                    productFilter = productFilter.Where(pf => pf.Price >= filterProduct.minPrice).ToList();

                }
                else
                {
                    productFilter = productFilter.Where(pf => pf.Weight >= filterProduct.minPrice && pf.Weight <= filterProduct.maxPrice).ToList();

                }
            }

            ViewBag.Shops = await db.Users.Where(u => u.shopName != null).ToListAsync();
            var model = new MainViewModel();
            model.Products = productFilter;
            model.QizilimUser = db.Users
               .Where(cp => cp.shopName != null).ToList();
            model.Advertisement = db.Advertisement
                .Where(cp => cp.DeletedById == null).ToList();
            model.CategoryId = filterProduct.CategoryId;
            model.EyarId = filterProduct.EyarId;
            model.ColorId = filterProduct.ColorId;
            model.CenterId = filterProduct.CenterId;
            model.hasDiamond = filterProduct.hasDiamond;
            model.hasDelivery = filterProduct.hasDelivery;
            model.minWeight = filterProduct.minWeight;
            model.maxWeight = filterProduct.maxWeight;
            model.minPrice = filterProduct.minPrice;
            model.maxPrice = filterProduct.maxPrice;
            return View(model);
        }

    }
}
