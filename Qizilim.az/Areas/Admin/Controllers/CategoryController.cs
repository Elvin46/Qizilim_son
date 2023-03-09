using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Modules.KateqoriyasModule;
using Qizilim.az.Models.DataContext;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class CategoryController : Controller
    {
        readonly IMediator mediator;
        readonly QizilimDbContext db;
        public CategoryController(IMediator mediator, QizilimDbContext db)
        {
            this.mediator = mediator;
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await mediator.Send(new KateqoriyasAllQuery());
            return View(data);
        }


        public async Task<IActionResult> Details(KateqoriyasSingleQuery query)
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
        public async Task<IActionResult> Create(KateqoriyasCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }


        public async Task<IActionResult> Edit(KateqoriyasSingleQuery query)
        {
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }

            var command = new KateqoriyasEditCommand();
            command.Name = entity.Name;
            return View(command);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KateqoriyasEditCommand command)
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

            var centers = await db.Kateqoriya
                .FirstOrDefaultAsync(m => m.Id == id);
            if (centers == null)
            {
                return NotFound();
            }

            return View(centers);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(KateqoriyasRemoveCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        private bool KateqoriyasExists(int id)
        {
            return db.Colors.Any(e => e.Id == id);
        }
    }
}
