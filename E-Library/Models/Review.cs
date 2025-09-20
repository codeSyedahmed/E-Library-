using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Models;

public class Review : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Rating { get; set;}
	public int BookId { get; set; }
	public Book Book { get; set; }
	public string UserId { get; set; }
	public ApplicationUser ApplicationUser { get; set; }
}
