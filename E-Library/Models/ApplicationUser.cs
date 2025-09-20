using Microsoft.AspNetCore.Identity;

namespace E_Library.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    
    public virtual ICollection<UserToBookMapping> UserToBook { get; set;}
	public virtual ICollection<Review> Review { get; set; }
}
