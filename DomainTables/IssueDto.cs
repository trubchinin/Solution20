namespace DomainTables
{
    /// <summary>
    /// DTO для створення/оновлення видачі без навігаційних властивостей
    /// </summary>
    public class IssueDto
    {
        public int ReaderId { get; set; }
        public int BookId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}