using System;

namespace DomainTables
{
    /// <summary>
    /// Сутність «Читач»
    /// </summary>
    public class Reader
    {
        // Первинний ключ, автоінкремент
        public int Id { get; set; }

        // Прізвище читача
        public string LastName { get; set; } = null!;

        // Ім'я читача
        public string FirstName { get; set; } = null!;

        // По батькові
        public string Patronymic { get; set; } = null!;

        // Дата народження
        public DateTime BirthDate { get; set; }

        // Дата оформлення, за замовчуванням — поточна
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Категорія читача: школяр, студент, аспірант, робочий, службовець
        public string Category { get; set; } = null!;
    }
}