using System.ComponentModel.DataAnnotations;

namespace E_Library.Models;

public class Category : BaseEntity
{
	public int Id { get; set; }

	[Required(ErrorMessage = "Category name is required")]
	[StringLength(50)]
	public string? Name { get; set; }

	public virtual ICollection<Book>? Book { get; set; }

}
