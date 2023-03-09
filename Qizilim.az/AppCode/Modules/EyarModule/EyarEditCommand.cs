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

namespace Qizilim.az.AppCode.Modules.EyarModule
{
    public class EyarEditCommand : IRequest<Eyar>
    {
        public int Id { get; set; }
        public string EyarOlcusu { get; set; }


        public class EyarEditCommandHandler : IRequestHandler<EyarEditCommand, Eyar>
        {
            readonly QizilimDbContext db;
            readonly IActionContextAccessor ctx;
            public EyarEditCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Eyar> Handle(EyarEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Eyars
                        .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.EyarOlcusu = Convert.ToDouble(request.EyarOlcusu);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
