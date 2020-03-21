using Library.Controllers;
using Library.Entities;
using Library.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace XUnitTestProject1.Controllers
{
    public class UsersUnitTests
    {
        [Fact]
        public async Task GetUsers_200Ok()
        {
            //AAA
            //Arrange
            var m = new Mock<IUserRepository>();
            ICollection<User> users = new List<User>
            {
                new User{IdUser=1, Name="kowalski", Email="kowalski@wp.pl"},
                new User{IdUser=2, Name="kowalski2", Email="kowalski2@wp.pl"},
                new User{IdUser=3, Name="kowalski3", Email="kowalski3@wp.pl"}
            };
            m.Setup(c => c.GetUsers()).Returns(Task.FromResult(users));
            var controller = new UsersController(m.Object);

            //Act
            var result = await controller.GetUsers();

            //Assert
            Assert.True(result is OkObjectResult);
            var r = result as OkObjectResult;
            Assert.True((r.Value as ICollection<User>).Count == 3);
            Assert.True((r.Value as ICollection<User>).ElementAt(0).Name == "kowalski");
        } 
    }
}
