namespace E_Library.Models;

public class BookToCategoryMapping : BaseEntity
{
    public int Id { get; set; }
    public int BookId { get; set; }
	public Book Book { get; set; }

	public int CategoryId { get; set; }
	public Category Category { get; set; }
}
