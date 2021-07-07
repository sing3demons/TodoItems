using System.ComponentModel.DataAnnotations;

namespace TodoItems.Models.DTOs
{
    public class CreateItemDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string Desc { get; init; }

    }
}