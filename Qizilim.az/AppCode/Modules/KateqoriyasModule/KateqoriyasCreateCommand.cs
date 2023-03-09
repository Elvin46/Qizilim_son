using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.KateqoriyasModule
{
    public class KateqoriyasCreateCommand : IRequest<Kateqoriya>
    {
        public string Name { get; set; }

        public class KateqoriyasCreateCommandHandler : IRequestHandler<KateqoriyasCreateCommand, Kateqoriya>
        {
            private readonly QizilimDbContext db;
            private readonly IActionContextAccessor ctx;
            public KateqoriyasCreateCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Kateqoriya> Handle(KateqoriyasCreateCommand request, CancellationToken cancellationToken)
            {
                var category = new Kateqoriya();
                category.Name = request.Name;
                category.CreatedDate = DateTime.UtcNow.AddHours(4);
                category.CreatedById = ctx.GetPrincipalId();

                await db.Kateqoriya.AddAsync(category, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return category;
            }
        }
    }
}
