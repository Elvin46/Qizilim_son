using MediatR;
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
    public class ColorsSingleQuery : IRequest<Color>
    {
        public int Id { get; set; }

        public class ColorsSingleQueryHandler : IRequestHandler<ColorsSingleQuery, Color>
        {
            readonly QizilimDbContext db;
            public ColorsSingleQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<Color> Handle(ColorsSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Colors
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
