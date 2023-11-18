using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime DateCreationMessage { get; set; }

        [ForeignKey("ThemeId")]
        public Theme Theme { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
