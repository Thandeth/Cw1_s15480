namespace Library.Models.DTO
{
    public class BookBorrowDto
    {
        public int IdUser { get; set; }
        public int IdBook { get; set; }
        public string Comments { get; set; }
    }
}
