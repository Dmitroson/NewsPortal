using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace NewsPortal.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public virtual string Text { get; set; }
        [Required]
        public virtual string UserName { get; set; }
    }
}