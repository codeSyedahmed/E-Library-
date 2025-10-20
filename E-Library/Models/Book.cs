using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Models;

public class Book : BaseEntity
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string? Title { get; set; }
	public string? Description { get; set; }

	public string? Image { get; set; }

    public string PdfFilePath { get; set; } // PDF file storage
    //public string Content { get; set; } // HTML content for online reading
    public bool IsAvailableOnline { get; set; } = true;
    public bool AllowDownload { get; set; } = true;

    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<UserToBookMapping>? UserToBook { get; set; }
	public virtual ICollection<Review>? Review { get; set; }
}

