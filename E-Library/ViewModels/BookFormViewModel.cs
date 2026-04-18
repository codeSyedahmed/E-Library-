using E_Library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Library.ViewModels
{
    public class BookFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? CategoryName { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? Image { get; set; }

        public IFormFile? PdfFile { get; set; }

        public string? PdfFilePath { get; set; }

        public bool IsAvailableOnline { get; set; } = true;

        public bool AllowDownload { get; set; } = true;

        [Required(ErrorMessage = "Please select at least one category")]
        public int? CategoryId { get; set; }

        public List<SelectListItem>? CategoryList { get; set; }
    }
}
