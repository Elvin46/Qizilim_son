using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.AppCode.Modules.Advertisement;
using Qizilim.az.Areas.Admin.Models.ViewModels;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class AdvertisementController : Controller
    {
        readonly IMediator mediator;
        readonly QizilimDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly IActionContextAccessor ctx;
        public AdvertisementController(IMediator mediator, QizilimDbContext db,
            IActionContextAccessor ctx,
            IWebHostEnvironment env)
        {
            this.mediator = mediator;
            this.db = db;
            this.ctx = ctx;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var ads = await db.Advertisement.Where(a => a.DeletedById == null).ToListAsync();



            return View(ads);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisementViewModel model)
        {

            if (model?.file == null)
            {
                ctx.AddModelError("Image", "Fayl Sechilmeyib!");
            }

            if (ctx.ModelIsValid())
            {
                string fileExtension = Path.GetExtension(model.file.FileName);

                string name = $"ad-{Guid.NewGuid()}{fileExtension}";
                string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                {
                    await model.file.CopyToAsync(fs);
                }

                var ads = new Advertisement
                {
                    Image = name,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                    CreatedById = ctx.GetPrincipalId()
                };





                await db.Advertisement.AddAsync(ads);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            return null;

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centers = await db.Advertisement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centers == null)
            {
                return NotFound();
            }

            return View(centers);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(AdvertisementRemoveCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool AdvertisementExists(int id)
        {
            return db.Advertisement.Any(e => e.Id == id);
        }
    }
}
