using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.KateqoriyasModule
{
    public class KateqoriyasEditCommand : IRequest<Kateqoriya>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public class KateqoriyasEditCommandHandler : IRequestHandler<KateqoriyasEditCommand, Kateqoriya>
        {
            readonly QizilimDbContext db;
            readonly IActionContextAccessor ctx;
            public KateqoriyasEditCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Kateqoriya> Handle(KateqoriyasEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Kateqoriya
                        .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.Name = request.Name;
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
