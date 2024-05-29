using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagerApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsCompleted { get; set; }
        [AllowNull]
        public DateTime? DeadLine { get; set; }
    }
}
