using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace StocksApplication.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [Length(5, 100, ErrorMessage = "Title must from 5 to 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(0, ErrorMessage = "Content must have at least 1 character")]
        public string Content { get; set; } = string.Empty;
    }
}
