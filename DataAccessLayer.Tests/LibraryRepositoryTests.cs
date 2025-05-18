using Microsoft.Data.Sqlite;   // для ClearAllPools
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using DomainTables;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Tests
{
    [TestClass]
    public class LibraryRepositoryTests
    {
        private LibraryRepository _repo;

        [TestInitialize]
        public void TestInit()
        {
            // Створюємо унікальний In-Memory контекст для кожного тесту
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _repo = new LibraryRepository(options);

            // І створюємо структуру
            using var ctx = new LibraryContext(options);
            ctx.Database.EnsureCreated();
        }

        [TestMethod]
        public void AddGetUpdateDelete_Reader_Crud()
        {
            // Arrange
            var reader = new Reader
            {
                LastName = "Ivanov",
                FirstName = "Ivan",
                Patronymic = "Ivanovich",
                BirthDate = new DateTime(1990, 1, 1),
                Category = "student"
            };

            // Act & Assert: Create + Read
            _repo.AddReader(reader);
            var all = _repo.GetAllReaders();
            Assert.AreEqual(1, all.Count);
            var saved = all.First();
            Assert.AreEqual("Ivanov", saved.LastName);

            // Update
            saved.LastName = "Petrov";
            _repo.UpdateReader(saved);
            var updated = _repo.GetReader(saved.Id);
            Assert.AreEqual("Petrov", updated.LastName);

            // Delete
            _repo.DeleteReader(updated.Id);
            var deleted = _repo.GetReader(updated.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void AddGetUpdateDelete_Book_Crud()
        {
            var book = new Book
            {
                Title = "Test Book",
                Author = "Author A",
                Type = "художня",
                CopyCount = 5
            };

            _repo.AddBook(book);
            var all = _repo.GetAllBooks();
            Assert.AreEqual(1, all.Count);
            var saved = all.First();
            Assert.AreEqual("Test Book", saved.Title);

            saved.Title = "Updated Book";
            _repo.UpdateBook(saved);
            var updated = _repo.GetBook(saved.Id);
            Assert.AreEqual("Updated Book", updated.Title);

            _repo.DeleteBook(updated.Id);
            var deleted = _repo.GetBook(updated.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void AddGetUpdateDelete_Issue_Crud()
        {
            // Спочатку створюємо Reader і Book
            var reader = new Reader
            {
                LastName = "A",
                FirstName = "B",
                Patronymic = "",
                BirthDate = DateTime.Today,
                Category = "student"
            };
            var book = new Book
            {
                Title = "B1",
                Author = "Auth",
                Type = "науково-популярна",
                CopyCount = 1
            };
            _repo.AddReader(reader);
            _repo.AddBook(book);

            // Дізнаємося їхні Id
            var r = _repo.GetAllReaders().First();
            var b = _repo.GetAllBooks().First();

            // Створюємо Issue
            var issue = new Issue
            {
                ReaderId = r.Id,
                BookId = b.Id
            };
            _repo.AddIssue(issue);

            var allIssues = _repo.GetAllIssues();
            Assert.AreEqual(1, allIssues.Count);

            // Оновлюємо ReturnDate
            var toUpdate = _repo.GetIssue(r.Id, b.Id);
            toUpdate.ReturnDate = DateTime.Today;
            _repo.UpdateIssue(toUpdate);
            var updated = _repo.GetIssue(r.Id, b.Id);
            Assert.IsNotNull(updated.ReturnDate);

            // Видаляємо Issue
            _repo.DeleteIssue(r.Id, b.Id);
            var deleted = _repo.GetIssue(r.Id, b.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void CascadeDelete_Reader_Removes_Issues()
        {
            var reader = new Reader
            {
                LastName = "X",
                FirstName = "Y",
                Patronymic = "",
                BirthDate = DateTime.Today,
                Category = "student"
            };
            var book = new Book
            {
                Title = "Z",
                Author = "Auth",
                Type = "художня",
                CopyCount = 1
            };
            _repo.AddReader(reader);
            _repo.AddBook(book);

            var r = _repo.GetAllReaders().First();
            var b = _repo.GetAllBooks().First();

            // Додаємо Issue
            _repo.AddIssue(new Issue { ReaderId = r.Id, BookId = b.Id });

            // Видаляємо Reader
            _repo.DeleteReader(r.Id);

            // Issue автоматично видалений каскадно
            var remaining = _repo.GetAllIssues();
            Assert.AreEqual(0, remaining.Count);
        }
    }
}