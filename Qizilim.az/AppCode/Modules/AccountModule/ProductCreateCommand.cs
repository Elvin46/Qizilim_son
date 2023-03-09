using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Color;
using static Qizilim.az.Models.Entities.Images;

namespace Qizilim.az.AppCode.Modules.AccountModule
{
    public class ProductCreateCommand : IRequest<Products>
    {
        public string Name { get; set; }
        public string Kateqoriya { get; set; }
        public string Nov { get; set; }
        public string aboutProduct { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public bool HasDiamond { get; set; }
        public double CountDiamond { get; set; }
        public IFormFile[] file { get; set; }
       
        public int[] EyarIds { get; set; }
        public int[] ColorIds { get; set; }
        public int[] ImageIds { get; set; }

        public ICollection<ProductKateqoriya> CategoriesCloud { get; set; }
        public ICollection<ProductEyar> EyarsCloud { get; set; }
        public ICollection<ProductColors> ColorsCloud { get; set; }



        public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Products>
        {
            private readonly QizilimDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;
            public ProductCreateCommandHandler(QizilimDbContext db,
                IWebHostEnvironment env,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Products> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
            {
                //if (request?.file == null)
                //{
                //    ctx.AddModelError("BlogPhoto", "Fayl Sechilmeyib!");
                //}

                //if (ctx.ModelIsValid())
                //{
                //    string fileExtension = Path.GetExtension(request.file.FileName);

                //    string name = $"blog-{Guid.NewGuid()}{fileExtension}";
                //    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "photouploads", "images", name);

                //    using (FileStream fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                //    {
                //        await request.file.CopyToAsync(fs, cancellationToken);
                //    }


                    var product = new Products
                    {
                        Name = request.Name,
                        Kateqoriya = request.Kateqoriya,
                        Nov = request.Nov,
                        aboutProduct = request.aboutProduct,
                        Price = request.Price,
                        Weight = request.Weight,
                        HasDiamond = request.HasDiamond,
                        CountDiamond = request.CountDiamond,
                        CreatedDate = DateTime.UtcNow.AddHours(4),
                        CreatedById = ctx.GetPrincipalId()
                    };

                    await db.AddAsync(product, cancellationToken);
                    int affected = await db.SaveChangesAsync(cancellationToken);


                //if (affected > 0 && request.ImageIds != null && request.ImageIds.Length > 0)
                //{
                //    foreach (var item in request.ImageIds)
                //    {
                //        await db.ProductImage.AddAsync(new ProductImage
                //        {
                //            ProductId = product.Id,
                //            ImageId = item
                //        }, cancellationToken);
                //    }

                //    await db.SaveChangesAsync(cancellationToken);
                //}


                if (affected > 0 && request.EyarIds != null && request.EyarIds.Length > 0)
                {
                    foreach (var item in request.EyarIds)
                    {
                        await db.ProductEyar.AddAsync(new ProductEyar
                        {
                            ProductId = product.Id,
                            EyarId = item
                        }, cancellationToken);
                    }

                    await db.SaveChangesAsync(cancellationToken);
                }

                if (affected > 0 && request.ColorIds != null && request.ColorIds.Length > 0)
                {
                    foreach (var item in request.ColorIds)
                    {
                        await db.ProductColors.AddAsync(new ProductColors
                        {
                            ProductId = product.Id,
                            ColorId = item
                        }, cancellationToken);
                    }

                    await db.SaveChangesAsync(cancellationToken);
                }

                return product;
                //}

            }
        }
    }
}
