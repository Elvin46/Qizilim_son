using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.EyarModule
{
    public class EyarCreateCommand : IRequest<Eyar>
    {
        public string EyarOlcusu { get; set; }

        public class EyarCreateCommandHandler : IRequestHandler<EyarCreateCommand, Eyar>
        {
            private readonly QizilimDbContext db;
            private readonly IActionContextAccessor ctx;
            public EyarCreateCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Eyar> Handle(EyarCreateCommand request, CancellationToken cancellationToken)
            {
                var eyar = new Eyar();
                eyar.EyarOlcusu = Convert.ToDouble(request.EyarOlcusu);
                eyar.CreatedDate = DateTime.UtcNow.AddHours(4);
                eyar.CreatedById = ctx.GetPrincipalId();

                await db.Eyars.AddAsync(eyar, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return eyar;
            }
        }
    }
}
