using System.ComponentModel.DataAnnotations;

namespace E_Library.Models;

public class Category : BaseEntity
{
	public int Id { get; set; }

	[Required]
	public string? Name { get; set; }

	public virtual ICollection<BookToCategoryMapping>? BookCategories { get; set; }

}
