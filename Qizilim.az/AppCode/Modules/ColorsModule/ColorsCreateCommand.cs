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

namespace Qizilim.az.AppCode.Modules.ColorsModule
{
    public class ColorsCreateCommand : IRequest<Color>
    {
        public string Name { get; set; }

        public class ColorsCreateCommandHandler : IRequestHandler<ColorsCreateCommand, Color>
        {
            private readonly QizilimDbContext db;
            private readonly IActionContextAccessor ctx;
            public ColorsCreateCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Color> Handle(ColorsCreateCommand request, CancellationToken cancellationToken)
            {
                var color = new Color();
                color.Name = request.Name;
                color.CreatedDate = DateTime.UtcNow.AddHours(4);
                color.CreatedById = ctx.GetPrincipalId();

                await db.Colors.AddAsync(color, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return color;
            }
        }
    }
}
