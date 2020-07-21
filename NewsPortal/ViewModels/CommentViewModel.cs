using System.ComponentModel.DataAnnotations;

namespace NewsPortal.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }
        
        [Required]
        [Display(Name = "Nickname")]
        public string UserName { get; set; }
    }
}