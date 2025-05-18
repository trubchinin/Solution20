using System;

namespace DomainTables
{
    /// <summary>
    /// Сутність «Видача»: зв’язок читача та книги
    /// </summary>
    public class Issue
    {
        // Ідентифікатор читача (FK)
        public int ReaderId { get; set; }

        // Ідентифікатор книги (FK)
        public int BookId { get; set; }

        // Дата видачі, за замовчуванням – поточна дата
        public DateTime IssueDate { get; set; } = DateTime.Now;

        // Дата повернення (nullable)
        public DateTime? ReturnDate { get; set; }

        // Навігаційні властивості
        public Reader Reader { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}