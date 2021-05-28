using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Tests
{
    public static class MockUserManager
    {
        public static Mock<UserManager<TUser>> UserManager<TUser>(List<TUser> ls) where TUser : class
        {
            List<string> roles = new List<string>() { "Citoyen" };

            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(ls.FirstOrDefault());
            mgr.Setup(x => x.IsEmailConfirmedAsync(It.IsAny<TUser>())).ReturnsAsync(true);
            mgr.Setup(x => x.IsLockedOutAsync(It.IsAny<TUser>())).ReturnsAsync(false);
            mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(true);
            mgr.Setup(x => x.GetRolesAsync(It.IsAny<TUser>())).ReturnsAsync(roles);

            return mgr;
        }
    }
}
