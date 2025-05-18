using System;

namespace DomainTables
{
    /// <summary>
    /// Сутність «Книга»
    /// </summary>
    public class Book
    {
        // Первинний ключ, автоінкремент
        public int Id { get; set; }

        // Назва книги
        public string Title { get; set; } = null!;

        // Автор книги
        public string Author { get; set; } = null!;

        // Тип: навчальна, художня, науково-популярна
        public string Type { get; set; } = null!;

        // Кількість примірників
        public int CopyCount { get; set; }
    }
}