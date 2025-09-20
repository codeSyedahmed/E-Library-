namespace E_Library.Models;

public class UserToBookMapping : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
