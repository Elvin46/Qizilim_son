using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Modules.EyarModule;
using Qizilim.az.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class EyarController : Controller
    {
        readonly IMediator mediator;
        readonly QizilimDbContext db;
        public EyarController(IMediator mediator, QizilimDbContext db)
        {
            this.mediator = mediator;
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await mediator.Send(new EyarAllQuery());
            return View(data);
        }

        public async Task<IActionResult> Details(EyarSingleQuery query)
        {
            var entity = await mediator.Send(query);

            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EyarCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }



        public async Task<IActionResult> Edit(EyarSingleQuery query)
        {
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }

            var command = new EyarEditCommand();
            command.EyarOlcusu = Convert.ToString(entity.EyarOlcusu);
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EyarEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centers = await db.Eyars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centers == null)
            {
                return NotFound();
            }

            return View(centers);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EyarRemoveCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool EyarExists(int id)
        {
            return db.Eyars.Any(e => e.Id == id);
        }
    }
}
