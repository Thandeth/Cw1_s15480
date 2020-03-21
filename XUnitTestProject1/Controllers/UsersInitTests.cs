using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace XUnitTestProject1.Controllers
{
    public class UsersInitTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;


        public UsersInitTests()
        {
            _server = ServerFactory.GetServerInstance();
            _client = _server.CreateClient();


            using (var scope = _server.Host.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();

                _db.User.Add(new User
                {
                    Email = "jd@pja.edu.pl",
                    Name = "Daniel",
                    Surname = "Jabłoński",
                    Login = "jd",
                    Password = "ASNDKWQOJRJOP!JO@JOP"
                });

                _db.SaveChanges();
            }
        }


        [Fact]
        public async Task GetUsers_200Ok()
        {
            //Arrange i Act
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            // using (var scope = _server.Host.Services.CreateScope())
            // {
            //     var _db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            //     Assert.True(_db.User.Any());
            // }

            Assert.True(users.Count() == 1);
            Assert.True(users.ElementAt(0).Login == "jd");
        }

        [Fact]
        public async Task GetIdUsers_200Ok()
        {
            var httpResponse = await _client.GetAsync($"{_client.BaseAddress.AbsoluteUri}api/users/1");

            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<User>(content);

            Assert.True(users.Name == "Daniel");
        }


        [Fact]
        public async Task PostUsers_200Ok()
        {
            var newUser = new User { Email = "hielo@pj.pl", Name = "Adam", Surname = "Szwonder", Login = "as", Password = "jd1234" };

            var serializedUser = JsonConvert.SerializeObject(newUser);

            var payload = new StringContent(serializedUser, Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync($"{_client.BaseAddress.AbsoluteUri}api/users", payload);
            httpResponse.EnsureSuccessStatusCode();

            var responseString = await httpResponse.Content.ReadAsStringAsync();

            Assert.Contains("Adam", responseString);

        }
        [Fact]
        public async Task PostBookBorrow_200Ok()
        {
            var newBorrow = new BookBorrow
            {
                IdUser = 1,
                IdBook = 1,
                Comments = "Wypozyczona dla Daniela"
            };
            var serializedBookBorrow = JsonConvert.SerializeObject(newBorrow);
            var payload = new StringContent(serializedBookBorrow, Encoding.UTF8, "application/json");

            var httpResponse = await _client.PostAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows", payload);
            httpResponse.EnsureSuccessStatusCode();

            var responseString = await httpResponse.Content.ReadAsStringAsync();

            Assert.Contains("Wypozyczona dla Daniela", responseString);
        }

        [Fact]
        public async Task PutBookBorrow_200Ok()
        {

            var newBorrow = new BookBorrow
            {
                IdUser = 1,
                IdBook = 1,
                Comments = "Wypozyczona dla Daniela"
            };
            var serializedBookBorrow = JsonConvert.SerializeObject(newBorrow);
            var payload = new StringContent(serializedBookBorrow, Encoding.UTF8, "application/json");

            var httpResponse = await _client.PutAsync($"{_client.BaseAddress.AbsoluteUri}api/book-borrows/1", payload);
            httpResponse.EnsureSuccessStatusCode();

        }

    }
}
