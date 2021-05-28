using Moq;
using ProjetCESI.Core;
using ProjetCESI.Data;
using ProjetCESI.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjetCESI.Tests
{
    public class Admin
    {

        public List<User> Users { get; } = new List<User>()
        {
            new User()
            {
                Id = 1,
                Email = "test@test.fr",
                UserName = "test",
                EmailConfirmed = true
            },
            new User()
            {
                Id = 2,
                Email = "test2@test2.fr",
                UserName = "test2",
                EmailConfirmed = true
            }
        };
    }
}
