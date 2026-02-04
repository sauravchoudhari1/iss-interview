using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models.Domain
{
        public class Todo
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            [MaxLength(2000)]
            public string? Description { get; set; }

            public bool IsCompleted { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
}
