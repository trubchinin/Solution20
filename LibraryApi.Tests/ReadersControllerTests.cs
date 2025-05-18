using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainTables;
using LibraryApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibraryApi.Tests
{
    [TestClass]
    public class ReadersControllerTests
    {
        private WebApplicationFactory<Program> _factory;

        [TestInitialize]
        public void Init()
        {
            // Генеруємо єдину назву для InMemory БД у цьому тесті
            var dbName = Guid.NewGuid().ToString();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureServices(services =>
                    {
                        // Видаляємо всі попередні реєстрації LibraryContext/EF Core
                        var descriptors = services
                            .Where(d =>
                                d.ServiceType == typeof(DbContextOptions<LibraryContext>) ||
                                d.ImplementationType == typeof(LibraryContext))
                            .ToList();
                        foreach (var d in descriptors)
                            services.Remove(d);

                        // Реєструємо контекст з єдиною InMemory назвою
                        services.AddDbContext<LibraryContext>(options =>
                            options.UseInMemoryDatabase(dbName));

                        // Репозиторій
                        services.AddScoped<LibraryRepository>();
                    });
                });

            // Створюємо таблиці в цій єдиній InMemory БД
            using var scope = _factory.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<LibraryContext>();
            ctx.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task CreateGetUpdateDelete_Reader_Workflow()
        {
            var client = _factory.CreateClient();

            // POST
            var newReader = new Reader
            {
                LastName = "Test",
                FirstName = "Api",
                Patronymic = "User",
                BirthDate = new DateTime(2000, 1, 1),
                Category = "student"
            };
            var postResponse = await client.PostAsJsonAsync("/api/readers", newReader);
            Assert.AreEqual(HttpStatusCode.Created, postResponse.StatusCode);

            // Отримуємо створений об’єкт
            var created = await postResponse.Content.ReadFromJsonAsync<Reader>();
            Assert.IsNotNull(created);
            Assert.AreEqual("Test", created.LastName);

            // GET all
            var all = await client.GetFromJsonAsync<Reader[]>("/api/readers");
            Assert.AreEqual(1, all.Length);

            // PUT
            created.LastName = "Updated";
            var putResponse = await client.PutAsJsonAsync($"/api/readers/{created.Id}", created);
            Assert.AreEqual(HttpStatusCode.NoContent, putResponse.StatusCode);

            // GET by id
            var getResponse = await client.GetAsync($"/api/readers/{created.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<Reader>();
            Assert.AreEqual("Updated", updated.LastName);

            // DELETE
            var deleteResponse = await client.DeleteAsync($"/api/readers/{created.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Перевіряємо NotFound
            var notFound = await client.GetAsync($"/api/readers/{created.Id}");
            Assert.AreEqual(HttpStatusCode.NotFound, notFound.StatusCode);
        }
    }
}