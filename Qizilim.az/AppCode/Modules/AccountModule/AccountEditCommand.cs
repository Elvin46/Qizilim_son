using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.AccountModule
{
    public class AccountEditCommand : IRequest<QizilimUser>
    {
        public int Id { get; set; }
        public string? shopName { get; set; }
        public string? Email { get; set; }
        public string? aboutShop { get; set; }
        public string? shopNumber { get; set; }
        public string? instagramLink { get; set; }
        public string? tiktokLink { get; set; }
        public double? whatsappNumber { get; set; }
        public string? shopLocation { get; set; }
        public bool catdirilma { get; set; }
        public string ProfileImg { get; set; }
        public IFormFile file { get; set; }

        public class AccountEditCommandHandler : IRequestHandler<AccountEditCommand, QizilimUser>
        {
            private readonly QizilimDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public AccountEditCommandHandler(QizilimDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<QizilimUser> Handle(AccountEditCommand request, CancellationToken cancellationToken)
            {
                if (request.file == null && string.IsNullOrEmpty(request.ProfileImg))
                {
                    ctx.AddModelError("ProfileImg", "Fayl Sechilmeyib!");
                }

                if (true)
                {
                    var entity = await db.Users
                      .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                    if (entity == null)
                    {
                        return null;
                    }

                    if (entity.ProfileImg == null)
                    {
                        string fileExtension = Path.GetExtension(request.file.FileName);

                        string name = $"user-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                        using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            await request.file.CopyToAsync(fs, cancellationToken);
                        }

                        entity.ProfileImg = name;
                    }
                    else
                    {
                        string OldFileName = entity.ProfileImg;

                        if (request.file != null)
                        {
                            string fileExtension = Path.GetExtension(request.file.FileName);
                            string name = $"user-{Guid.NewGuid()}{fileExtension}";
                            string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);
                            using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                            {
                                await request.file.CopyToAsync(fs, cancellationToken);
                            }
                            entity.ProfileImg = name;

                            string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", OldFileName);

                            if (System.IO.File.Exists(physicalPathOld))
                            {
                                System.IO.File.Delete(physicalPathOld);
                            }

                        }
                    }


                    entity.shopName = request.shopName;
                    entity.shopLocation = request.shopLocation;
                    entity.shopNumber = request.shopNumber;
                    entity.aboutShop = request.aboutShop;
                    entity.whatsappNumber = request.whatsappNumber;
                    entity.instagramLink = request.instagramLink;
                    entity.tiktokLink = request.tiktokLink;
                    entity.catdirilma = request.catdirilma;
                    await db.SaveChangesAsync(cancellationToken);

                    return entity;
                }

                return null;
            }
        }
    }
}
