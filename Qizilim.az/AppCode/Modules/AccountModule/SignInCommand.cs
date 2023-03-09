using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.AccountModule
{
    public class SigninCommand : IRequest<QizilimUser>
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public QizilimUser User { get; set; }

        public class SigninCommandHandler : IRequestHandler<SigninCommand, QizilimUser>
        {
            private readonly UserManager<QizilimUser> userManager;
            private readonly SignInManager<QizilimUser> signInManager;
            private readonly IActionContextAccessor ctx;
            private readonly QizilimDbContext db;

            public SigninCommandHandler(UserManager<QizilimUser> userManager,
                SignInManager<QizilimUser> signInManager,
                IActionContextAccessor ctx,
                QizilimDbContext db)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.ctx = ctx;
                this.db = db;
            }
            public async Task<QizilimUser> Handle(SigninCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.ModelIsValid())
                {
                    return null;
                }

                var user = await userManager.FindByNameAsync(request.Username);

                if (user == null)
                {
                    ctx.AddModelError("UserName", "Istifadeci adi ve ya shifre sehvdir!");
                    return null;
                }

                


                var signinResult = await signInManager.PasswordSignInAsync(user, request.Password, true, false);

                if (signinResult.IsLockedOut)
                {
                    ctx.AddModelError("UserName", "Hesabiniz muveqqeti olaraq bloklanib");
                    return null;
                }
                else if (signinResult.IsNotAllowed)
                {
                    ctx.AddModelError("UserName", "Girish huququnuz yoxdur!");
                    return null;
                }


                if (!signinResult.Succeeded)
                {
                    ctx.AddModelError("UserName", "Istifadeci adi ve ya shifre yanlishdir!");
                    return null;
                }


                return user;
            }
        }
    }
}
