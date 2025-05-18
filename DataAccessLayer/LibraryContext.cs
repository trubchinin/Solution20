using Microsoft.EntityFrameworkCore;
using DomainTables;

namespace DataAccessLayer
{
    /// <summary>
    /// Контекст бази даних (Code-First) з гнучким налаштуванням через DbContextOptions
    /// </summary>
    public class LibraryContext : DbContext
    {
        /// <summary>
        /// Рядок з’єднання для файлової SQLite (за замовчуванням)
        /// </summary>
        public static string FileConnectionString { get; set; } = "Data Source=library.db";

        /// <summary>
        /// Конструктор, що приймає налаштування контексту (для InMemory чи іншого провайдера)
        /// </summary>
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Конструктор без параметрів — використовує файл SQLite
        /// </summary>
        public LibraryContext()
            : this(new DbContextOptionsBuilder<LibraryContext>()
                   .UseSqlite(FileConnectionString)
                   .Options)
        {
        }

        public DbSet<Reader> Readers { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Issue> Issues { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Складений ключ для Issue
            modelBuilder.Entity<Issue>()
                .HasKey(i => new { i.ReaderId, i.BookId });

            // Зв’язок Issue → Reader з каскадним видаленням
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Reader)
                .WithMany()
                .HasForeignKey(i => i.ReaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв’язок Issue → Book з каскадним видаленням
            modelBuilder.Entity<Issue>()
                .HasOne(i => i.Book)
                .WithMany()
                .HasForeignKey(i => i.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}