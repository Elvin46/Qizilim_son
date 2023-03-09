using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.AppCode.InfraStructure;
using Qizilim.az.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.ColorsModule
{
    public class ColorsRemoveCommand : IJsonRequest
    {
        public int Id { get; set; }

        public class ColorsRemoveCommandHandler : IJsonRequestHandler<ColorsRemoveCommand>
        {
            readonly QizilimDbContext db;
            private readonly IActionContextAccessor ctx;
            public ColorsRemoveCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<CommandJsonResponse> Handle(ColorsRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Colors
                         .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return new CommandJsonResponse(true, "Tapilmadi");
                }

                entity.DeletedById = ctx.GetPrincipalId();
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);

                return new CommandJsonResponse(false, "Qeyd silindi");
            }
        }
    }
}
