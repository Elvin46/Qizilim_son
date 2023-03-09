using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.ColorsModule
{
    public class ColorsEditCommand : IRequest<Color>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public class ColorsEditCommandHandler : IRequestHandler<ColorsEditCommand, Color>
        {
            readonly QizilimDbContext db;
            readonly IActionContextAccessor ctx;
            public ColorsEditCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Color> Handle(ColorsEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Colors
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
