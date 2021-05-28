using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using ProjetCESI.Core;
using ProjetCESI.Web.Controllers;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ProjetCESI.Web.Tests
{
    public class Account
    {
        [Fact]
        public async Task Login_ReturnAViewResult_WithTheCorrectUserLoggedIn()
        {
            var mockUserManager = MockUserManager.UserManager(Users);

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock
                .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            var controller = new AccountController(mockUserManager.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = serviceProviderMock.Object
                    }
                }
            };

            var result = await controller.Login(new LoginViewModel
            {
                Password = "test",
                Username = "test"
            }, string.Empty);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Accueil", viewResult.ActionName);
            Assert.Equal("Accueil", viewResult.ControllerName);
        }

        public List<User> Users { get; } = new List<User>()
        {
            new User()
            {
                Id = 1,
                Email = "test@test.fr",
                UserName = "test"
            },
            new User()
            {
                Id = 2,
                Email = "test2@test2.fr",
                UserName = "test2"
            }
        };
    }
}
