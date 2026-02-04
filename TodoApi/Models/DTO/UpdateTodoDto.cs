using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTO
{
    public class UpdateTodoDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
