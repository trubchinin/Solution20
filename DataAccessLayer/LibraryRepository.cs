using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DomainTables;

namespace DataAccessLayer
{
    /// <summary>
    /// Репозиторій для CRUD‐операцій над сутностями бібліотеки із гнучким контекстом
    /// </summary>
    public class LibraryRepository
    {
        private readonly DbContextOptions<LibraryContext> _options;

        /// <summary>
        /// Конструктор репозиторію. Якщо options не задані — використовується файлова SQLite.
        /// </summary>
        public LibraryRepository(DbContextOptions<LibraryContext>? options = null)
        {
            _options = options ?? new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite(LibraryContext.FileConnectionString)
                .Options;
        }

        /// <summary>
        /// Створює новий контекст за переданими опціями
        /// </summary>
        private LibraryContext CreateContext() => new LibraryContext(_options);

        // ===== Reader =====

        public void AddReader(Reader reader)
        {
            using var ctx = CreateContext();
            ctx.Readers.Add(reader);
            ctx.SaveChanges();
        }

        public Reader? GetReader(int id)
        {
            using var ctx = CreateContext();
            return ctx.Readers.Find(id);
        }

        public List<Reader> GetAllReaders()
        {
            using var ctx = CreateContext();
            return ctx.Readers.ToList();
        }

        public void UpdateReader(Reader reader)
        {
            using var ctx = CreateContext();
            ctx.Readers.Update(reader);
            ctx.SaveChanges();
        }

        public void DeleteReader(int id)
        {
            using var ctx = CreateContext();
            // 1) Видаляємо всі записи Issue, пов’язані з цим Reader
            var relatedIssues = ctx.Issues
                .Where(i => i.ReaderId == id)
                .ToList();
            if (relatedIssues.Any())
            {
                ctx.Issues.RemoveRange(relatedIssues);
            }

            // 2) Видаляємо самого Reader
            var reader = ctx.Readers.Find(id);
            if (reader != null)
            {
                ctx.Readers.Remove(reader);
            }

            // 3) Зберігаємо всі зміни в одному SaveChanges
            ctx.SaveChanges();
        }

        // ===== Book =====

        public void AddBook(Book book)
        {
            using var ctx = CreateContext();
            ctx.Books.Add(book);
            ctx.SaveChanges();
        }

        public Book? GetBook(int id)
        {
            using var ctx = CreateContext();
            return ctx.Books.Find(id);
        }

        public List<Book> GetAllBooks()
        {
            using var ctx = CreateContext();
            return ctx.Books.ToList();
        }

        public void UpdateBook(Book book)
        {
            using var ctx = CreateContext();
            ctx.Books.Update(book);
            ctx.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            using var ctx = CreateContext();
            // Видаляємо всі записи Issue, пов’язані з цим Book
            var relatedIssues = ctx.Issues
                .Where(i => i.BookId == id)
                .ToList();
            if (relatedIssues.Any())
            {
                ctx.Issues.RemoveRange(relatedIssues);
            }

            // Видаляємо сам Book
            var book = ctx.Books.Find(id);
            if (book != null)
            {
                ctx.Books.Remove(book);
            }

            ctx.SaveChanges();
        }

        // ===== Issue =====

        public void AddIssue(Issue issue)
        {
            using var ctx = CreateContext();
            ctx.Issues.Add(issue);
            ctx.SaveChanges();
        }

        public Issue? GetIssue(int readerId, int bookId)
        {
            using var ctx = CreateContext();
            return ctx.Issues.Find(readerId, bookId);
        }

        public List<Issue> GetAllIssues()
        {
            using var ctx = CreateContext();
            return ctx.Issues.ToList();
        }

        public void UpdateIssue(Issue issue)
        {
            using var ctx = CreateContext();
            ctx.Issues.Update(issue);
            ctx.SaveChanges();
        }

        public void DeleteIssue(int readerId, int bookId)
        {
            using var ctx = CreateContext();
            var entity = ctx.Issues.Find(readerId, bookId);
            if (entity != null)
            {
                ctx.Issues.Remove(entity);
                ctx.SaveChanges();
            }
        }
    }
}