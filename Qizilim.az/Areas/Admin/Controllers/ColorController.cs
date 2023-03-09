using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Modules.ColorsModule;
using Qizilim.az.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class ColorController : Controller
    {
        readonly IMediator mediator;
        readonly QizilimDbContext db;
        public ColorController(IMediator mediator, QizilimDbContext db)
        {
            this.mediator = mediator;
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await mediator.Send(new ColorsAllQuery());
            return View(data);
        }


        public async Task<IActionResult> Details(ColorsSingleQuery query)
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
        public async Task<IActionResult> Create(ColorsCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }


        public async Task<IActionResult> Edit(ColorsSingleQuery query)
        {
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }

            var command = new ColorsEditCommand();
            command.Name = entity.Name;
            return View(command);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ColorsEditCommand command)
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

            var centers = await db.Colors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centers == null)
            {
                return NotFound();
            }

            return View(centers);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(ColorsRemoveCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool ColorsExists(int id)
        {
            return db.Colors.Any(e => e.Id == id);
        }
    }
}
