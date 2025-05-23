using System.ComponentModel.DataAnnotations;
using TaskerFullStackApp.Client.Models;
using TaskerFullStackApp.Data;

namespace TaskerFullStackApp.Models
{
    public class DbTaskerItem : TaskerItem
    {
        // Foreign key to the user who created the task
        [Required]
        public string? UserId { get; set; }

        // Navigation property to the user who created the task
        public virtual ApplicationUser? User { get; set; } // Navigation property to the user who created the task
    }
   
}
